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
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using CAB.IECChannel;
using CAB.IECChannel.Programming;

namespace IEC.CAB.CHANNEL.Programming
{
    public class TOUInformationSP : ReadBase 
    {
        public int HighLoadThreshold {get;set;}
        public int LowLoadThreshold { get; set; }
        public int TransformerRating { get; set; }
        public int DailyParamsValue { get; set; }
        private DateTime rtcDateTime;

        string MeterPwdCommand = "0150310228PWD2903BCC";         

        public TOUInformationSP()
        {
            command = Command.GetInstance();
        }
        //********************Configuration Reading ***************************

        // Added GetTOUSP,GetRTC,IECSignOn,GetBillingType,DemandIntegrationPeriod for Configuration read with single sign on

        public string GetTOUSP(Dictionary<string, Dictionary<int, string>> dicTOUSP)
        {
            IsSignOnFailure = false;
            string data = string.Empty;

            string responseTOU = string.Empty;
            List<string> touCommands = new List<string>();
            StringBuilder touBuilder = new StringBuilder();
            try
            {
               TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                ReadConfigurations readConfig = new ReadConfigurations();
              
                foreach (string touCommand in readConfig.GetTODConfigurationReadCommandSP(todConfiguration.ReadCommandSP))
                {
                    communications.Command = touCommand.Substring(1, touCommand.Length - 2);
                    communications.CommandID = 2;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    Thread.Sleep(200);
                   
                    communications.SendCommand();
                    //communications.DelayExecution();
                    communications.CurrentTime = DateTime.Now;
                    string KeyCommand = communications.Command;
                    int IndexIterator = 0;
                    Dictionary<int,string> dicTOUCmd = new Dictionary<int,string>();
                                      
                    do
                    {
                        //communications.CurrentTime = DateTime.Now;
                        if (communications.Timeout())
                        {
                            //this.StatusMessage = MessageConstant.GetText("M000040");
                            Application.DoEvents();
                            this.StatusMessage = "Timeout.";
                            //data = string.Empty;
                            IsSignOnFailure = false;
                            break;
                        }
                        if (communications.OutBuffer.Contains("\x15"))
                        {
                            this.StatusMessage = "Access Denied.";
                            Application.DoEvents();
                            data = string.Empty;
                            IsSignOnFailure = false;
                            break;
                        }

                        if (communications.OutBuffer.Contains("\x06"))
                        {
                            this.StatusMessage = "Read Completed";
                            Application.DoEvents();
                            data += communications.OutBuffer;
                            IsSignOnFailure = true;
                            break;
                        }
                        
                        if (communications.OutBuffer.Contains("\x04"))
                        {
                           
                            Application.DoEvents();
                            dicTOUCmd.Add(IndexIterator, communications.OutBuffer);
                            data += communications.OutBuffer;
                            IsSignOnFailure = true;

                            //Send Acknowledgement
                            communications.Command = "06";
                            communications.CommandID = 2;
                            communications.ReadFlag = false;
                            communications.IsDataReceived = false;
                            communications.OutBuffer = string.Empty;
                            Thread.Sleep(200); 
                            communications.SendCommand();
                            communications.CurrentTime = DateTime.Now;
                            IndexIterator++;
                                                                                                     
                            
                        }
                        if (communications.OutBuffer.Contains("\x03"))
                        {
                            dicTOUCmd.Add(IndexIterator, communications.OutBuffer);
                            //this.StatusMessage = "Read Completed";
                            Application.DoEvents();                                                    
                            data += communications.OutBuffer;
                            IsSignOnFailure = true;                                                  
                            break;
                        }                                               

                        
                        if (IsAborted)
                        {
                            this.StatusMessage = "User Aborted.";
                            Application.DoEvents();
                            data = string.Empty;
                            IsSignOnFailure = false;
                            break; // Story - 354382 - During parameter written should break when user aborted
                        }
                    } while (true);                                            
                    dicTOUSP.Add(KeyCommand, dicTOUCmd);
                    
                }
            }           
            catch (Exception ex)
            {
                new CABException(ex);
            }
            finally
            {
                
            }
            return data;
        }

        public string GetRTC(ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = "";
            try
            {
                data = ReceiveDataCommand(command.ReadRTCCommandForSP, 2, "\x03", communications);
                if (data.Contains("Error"))statusMsg = data;
                else 
                {
                    statusMsg = "Read Completed";
                    Application.DoEvents();
                    data += communications.OutBuffer;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                
            }
            return data;
        }

        //public bool IECSignOn(IECChannelBase communications)
        //{
        //    try
        //    {
        //        string statusMsg = "";
        //        if (ConfigInfo.GetLocalMode().Equals("Optical"))
        //            communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());

        //        if (!communications.OpenPort())
        //        {
        //            statusMsg = MessageConstant.GetText("M000038");
        //            Application.DoEvents();
        //            return false;
        //        }

        //        communications.CurrentTime = DateTime.Now;
        //        if (!communications.SignOnForSPhaseIEC())
        //        {
        //            IsSignOnFailure = true;
        //            communications.DelayExecution();
        //            communications.ClosePort();
        //            return false;
        //        }

        //       // communications.DelayExecution();

        //        communications.TotalReadBytes = 0;
        //        string passwordData = string.Empty;
        //        communications.ReadFlag = false;
        //        communications.Command = command.DTMDailySurveyCommand_A;
        //        communications.CommandID = 2;
        //        communications.OutBuffer = string.Empty;
        //        communications.CurrentTime = DateTime.Now;
        //        if (!communications.SendCommand())
        //            return false;

        //        Thread.Sleep(200);
        //        Application.DoEvents();
        //        int index = communications.ResponseSignOn.IndexOf("/");
        //        communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
        //        //Thread.Sleep(200);
        //        Application.DoEvents();
        //        do
        //        {
        //            if (IsAborted)
        //            {
        //                // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
        //                Application.DoEvents();
        //                statusMsg = "User Aborted.";
        //                Application.DoEvents();
        //                return false;
        //            }

        //            if (communications.Timeout())
        //            {
        //                this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040"); 
        //                return false;
        //            }

        //            if (communications.ReadFlag)
        //            {
        //                Thread.Sleep(200);
        //                Application.DoEvents();
        //                passwordData = communications.OutBuffer;
        //                break;
        //            }

        //        } while (true);

        //        if (passwordData != string.Empty)
        //        {
        //            string commandPassword = AlgorithemicSignOn(passwordData);
        //           // communications.DelayExecution();

        //            communications.ReadFlag = false;
        //            communications.Command = commandPassword;
        //            communications.CommandID = 3;
        //            communications.OutBuffer = "";
        //            communications.IsDataReceived = false;
        //            communications.SendCommand();

        //            //Thread.Sleep(200);
        //            communications.CurrentTime = DateTime.Now;
        //            do
        //            {
        //                if (communications.ReadFlag) break;
        //                if (IsAborted)
        //                {
        //                    statusMsg = "User Aborted.";
        //                    Application.DoEvents();
        //                    return false;
        //                }
        //                if (communications.Timeout()) break;
        //            } while (true);

        //            if (communications.OutBuffer.Length >= 0)
        //            {
        //                if (communications.OutBuffer.Contains("\x15"))
        //                {
        //                    this.StatusMessage = "Access Denied.";
        //                    Application.DoEvents();
        //                    return false;
        //                }
        //                if (communications.OutBuffer.Contains("\x06"))
        //                    return true;
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //}

        public bool IECSignOn(IECChannelBase communications, string meterPassword)
        {
            try
            {
                string statusMsg = "";
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());

                if (!communications.OpenPort())
                {
                    statusMsg = MessageConstant.GetText("M000038");
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
                 Thread.Sleep(200); //ecostar & star light for not communicating sleep change from 1ms to 200ms
               //---------------Baud rate switch Command ----------------
                communications.TotalReadBytes = 0;
                string passwordData = string.Empty;
                communications.ReadFlag = false;
                communications.Command = command.DTMDailySurveyCommand_A;
                communications.CommandID = 2;
                communications.OutBuffer = string.Empty;
                communications.CurrentTime = DateTime.Now;
                if (!communications.SendCommand())
                    return false;

                 Thread.Sleep(200);
                Application.DoEvents();
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;
                Application.DoEvents();
                do
                {
                    if (IsAborted)
                    {
                        Application.DoEvents();
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                    }
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Time Out!";
                        break;
                    }
                    if (communications.ReadFlag)
                    {
                        Thread.Sleep(200);
                        Application.DoEvents();
                        passwordData = communications.OutBuffer;
                        break;
                    }

                } while (true);

                if (passwordData != string.Empty)
                {
                    string commandPassword = "";
                    if (meterPassword != null && meterPassword != string.Empty)
                    {
                     commandPassword = communications.Command = command.PasswordCommandRTC_SP;
                     commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(meterPassword));
                     commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(2, commandPassword.Length - 5)));
                     meterPassword = "";
                    }
                    else
                    {
                        commandPassword = AlgorithemicSignOn(passwordData);
                    }  
                    string teaAlgoresponse = ReceiveDataCommand(commandPassword, 3, "\x06", communications);
                    if (teaAlgoresponse.Contains("Error")) this.StatusMessage = teaAlgoresponse;
                    else return true;
                }
               
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public string ReceiveDataCommand(string commandData, int commandID, string responseByte, IECChannelBase communications)
        {
            communications.ReadFlag = false;
            communications.Command = commandData;
            communications.CommandID = commandID;
            communications.OutBuffer = "";
            communications.IsDataReceived = false;
            communications.SendCommand();
            communications.CurrentTime = DateTime.Now;
            string recData = "";
            Application.DoEvents();
            do
            {
                if (IsAborted)
                {
                    // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                    Application.DoEvents();
                     this.StatusMessage = "User Aborted.";
                    Application.DoEvents();
                    return "Error : " + "User Aborted."; 
                }

                if (communications.Timeout())
                {
                    this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040"); 
                    return "Error : " + "Time Out!";
                }

                if (communications.ReadFlag)
                {
                    Thread.Sleep(200);
                    Application.DoEvents();
                    recData = communications.OutBuffer;
                    break;
                }

            } while (true);
            if (recData.Length >= 0)
            {
                if (recData.Contains("\x15"))
                {
                    this.StatusMessage = "Access Denied.";
                    Application.DoEvents();
                    return "Error : " + "Access Denied.";
                }
                else if (recData.ToUpperInvariant().Contains("ERR")) return recData;
                else if (recData.Contains(responseByte)) return recData;
                else return "Error : " + recData;
                   
            }
            return "Error : " + "Invalid Response";
        }

        public string GetBillingType(ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                //communications.Command = "0152310233433441283136290361";
                // TPDDL Starlight 1P IEC Command for Billing Type 0152310234303030283036290361 //User Story 478249. Starlight Billing Type Command implemented
                if (communications.ResponseSignOn.Contains("LGC110N05"))
                {
                    communications.Command = "0152310234303030283036290361";
                }
                else
                {
                    //communications.Command = "0152310233433441283136290361";
                    communications.Command = "0152310233433441283036290360";
                }        

                data = ReceiveDataCommand(communications.Command , 2, "\x03", communications);
                if (data.Contains("Error")) statusMsg = data;
                else { /* Do Noting, Message is displayed in final called methods*/}
            }
            catch (Exception)
            {
            }
            
            return data;
        }

        public string DemandIntegrationPeriod(ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                string cmd = "01523102333046332830312903BCC";
                cmd = cmd.Replace("BCC", ReadoutCommon.ReturnBcc(cmd.Substring(2, cmd.Length - 5)));
                data = ReceiveDataCommand(cmd, 2, "\x03", communications);
                if (data.Contains("Error")) statusMsg = data;
                else data = communications.OutBuffer;
                
            }
            catch (Exception)
            {
            }
            
            return data;

        }

        public string KVAHSelection(ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                //string cmd = "01523102333046332830312903BCC";
                string cmd = "01523102333331462830372903BCC";
                cmd = cmd.Replace("BCC", ReadoutCommon.ReturnBcc(cmd.Substring(2, cmd.Length - 5)));
                data = ReceiveDataCommand(cmd, 2, "\x03", communications);
                if (data.Contains("Error")) statusMsg = data;
                else data = communications.OutBuffer;

            }
            catch (Exception)
            {
            }

            return data;

        }



        //********************Configuration Writting ***************************

        // Added for configuration write GetTOUSP,GetRTC,IECSignOn,GetBillingType,DemandIntegrationPeriod,TamperReset for with single sign on


        public bool ResetMagneticTamperSP()
        {

            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                communications.DelayExecution();
                communications.CurrentTime = DateTime.Now;
                communications.ReadFlag = false;


                string commandBillingReset = command.TamperIconReset_SP;
                commandBillingReset = commandBillingReset.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandBillingReset.Substring(2, commandBillingReset.Length - 5)));
                communications.Command = commandBillingReset;
                communications.CommandID = 2;
                communications.OutBuffer = "";
                communications.IsDataReceived = false;
                communications.SendCommand();
                data = "";
                Thread.Sleep(5000);
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.OutBuffer.Contains("\x15"))
                    {
                        this.StatusMessage = "Access Denied.";
                        Application.DoEvents();
                        data = string.Empty;
                        IsSignOnFailure = false;
                        break;
                    }
                    if (communications.OutBuffer.Contains("\x06"))
                    {
                        Application.DoEvents();
                        data += communications.OutBuffer;
                        IsSignOnFailure = true;
                        break;
                    }
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                        data = string.Empty;
                        IsSignOnFailure = false;
                        break; // Story - 354382 - During parameter written should break when user aborted
                    }
                    if (communications.Timeout())
                    {
                        //this.StatusMessage = MessageConstant.GetText("M000040");
                        //Application.DoEvents();
                        IsSignOnFailure = false;
                        break;
                    }
                } while (true);

            }
            catch (Exception)
            {
            }
            finally
            {

            }
            return IsSignOnFailure;
        }      

        public bool SetRTC(string meterPassword)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                communications.DelayExecution();
                communications.CurrentTime = DateTime.Now;
                communications.ReadFlag = false;

                string commandUpdateRTC = communications.Command = command.CommandUpdateRTC_SP;
                commandUpdateRTC = commandUpdateRTC.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValueSP(RTCDateTime));
                commandUpdateRTC = commandUpdateRTC.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandUpdateRTC.Substring(2, commandUpdateRTC.Length - 5)));
                communications.Command = commandUpdateRTC;
                communications.CommandID = 2;
                communications.OutBuffer = "";
                communications.IsDataReceived = false;
                communications.SendCommand();
                data = "";
                Thread.Sleep(100);
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.OutBuffer.Contains("\x15"))
                    {
                        this.StatusMessage = "Access Denied.";
                        Application.DoEvents();
                        data = string.Empty;
                        IsSignOnFailure = false;
                        break;
                    }
                    if (communications.OutBuffer.Contains("\x06"))
                    {
                        Application.DoEvents();
                        data += communications.OutBuffer;
                        IsSignOnFailure = true;
                        break;
                    }
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                        data = string.Empty;
                        IsSignOnFailure = false;
                        break; // Story - 354382 - During parameter written should break when user aborted
                    }
                    if (communications.Timeout())
                    {
                        //this.StatusMessage = MessageConstant.GetText("M000040");
                        //Application.DoEvents();
                        IsSignOnFailure = false;
                        break;
                    }
                } while (true);
                
            }
            catch (Exception)
            {
            }
            finally
            {
                
            }
            return IsSignOnFailure;
        }

        public bool SetTOU(List<string> touCommands)
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = true;
            string responseTOU = string.Empty;
            StringBuilder touBuilder = new StringBuilder();
            try
            {
               
                // this.StatusMessage = "Programing TOU data in the meter. Please wait...";
                Application.DoEvents();
                communications.OutBuffer = string.Empty;
                foreach (string touCommand in touCommands)
                {
                    //rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(string.Concat(HighLoadThreshold.ToString(),LowLoadThreshold.ToString(),TransformerRating.ToString())));
                    //calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                    string calculatedBcc = ReadoutCommon.ReturnBcc(touCommand.Substring(2));
                    //rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                    communications.Command = string.Concat(touCommand, calculatedBcc);
                    communications.CommandID = 2;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    communications.SendCommand();
                    communications.DelayExecution();
                    //User story taskID: 490975
                    Thread.Sleep(100);

                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = "Timeout Error.";
                            flag = false;
                            break;
                        }
                    } while (communications.OutBuffer.Length < 1);

                    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                    if (charACK != 6)
                    {
                        this.StatusMessage = "Access Denied";
                        flag = false;
                        break;
                    }
                                                                         
                }

            }
            catch (Exception ex)
            {
                new CABException(ex);
            }
            finally
            {
                
            }
            return flag;
        }

        public bool ResetBilling()
        {
            
            string data = string.Empty;
            try
            {
                string commandBillingReset = command.BillingEnergyReset_SP;
                commandBillingReset = commandBillingReset.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandBillingReset.Substring(2, commandBillingReset.Length - 5)));
                communications.Command = commandBillingReset;

                data = ReceiveDataCommand(commandBillingReset, 3, "\x06", communications);
                if (data.Contains("Error")) this.StatusMessage = data;
                else return true;
            }
            catch (Exception)
            {
            }
            return false;
          
        }

        public bool ResetDIP(string DemandInterval)
        {
            int tempcommandTimeout = communications.CommandTimeout;
            int tempInterChatracterDelay = communications.CommandTimeout;
            string data = string.Empty;
            try
            {
                string commandDIPReset = command.DIPReset_SP;
                commandDIPReset = commandDIPReset.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(DemandInterval));
                commandDIPReset = commandDIPReset.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandDIPReset.Substring(2, commandDIPReset.Length - 5)));
                communications.Command = commandDIPReset;
                communications.CommandTimeout = 30000;
                communications.InterChatracterDelay = 30000;
                data = ReceiveDataCommand(communications.Command, 3, "\x06", communications);
                if (data.Contains("Error")) this.StatusMessage = data;
                else { return true; }
             }
            catch (Exception)
            {
            }
            finally
            {
                communications.CommandTimeout = tempcommandTimeout;
                communications.InterChatracterDelay = tempInterChatracterDelay;
            }
            return false;
        }

        public bool WriteKVAHSelection(string tamperData)
        {

            string data = string.Empty;
            try
            {

                string commandKvahSelection = command.TamperConfig_SP;
                commandKvahSelection = commandKvahSelection.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(tamperData));
                commandKvahSelection = commandKvahSelection.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandKvahSelection.Substring(2, commandKvahSelection.Length - 5)));

                communications.Command = commandKvahSelection;

                data = ReceiveDataCommand(commandKvahSelection, 3, "\x06", communications);
                if (data.Contains("Error")) this.StatusMessage = data;
                else return true;
            }
            catch (Exception ex)
            {
            }
            return false;

        }

        public bool UpdateBillingType(string DemandInterval)
        {

           string data = string.Empty;
            try
            {

                communications.DelayExecution();
                communications.CurrentTime = DateTime.Now;
                communications.ReadFlag = false;

                //string commandBillingType = command.BillingType_SP;
                string commandBillingType = string.Empty;
                // TPDDL Starlight 1P IEC Command for Billing Type//User Story 478249. Starlight Billing Type Command implemented
                if (communications.ResponseSignOn.Contains("LGC110N05"))
                {
                    commandBillingType = command.StarLightBillingType_SP;
                }
                else
                {
                    commandBillingType = command.BillingType_SP;
                }

                commandBillingType = commandBillingType.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(DemandInterval));
                commandBillingType = commandBillingType.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandBillingType.Substring(2, commandBillingType.Length - 5)));
                communications.Command = commandBillingType;
                
                data = ReceiveDataCommand(communications.Command, 3, "\x06", communications);
                if (data.Contains("Error")) this.StatusMessage = data;
                else { return true; }
                 
            }
            catch (Exception)
            {
            }
            finally
            {
                
            }
            return false;

        }


        public DateTime RTCDateTime
        {
            get { return rtcDateTime; }
            set { rtcDateTime = value; }
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


        private List<string> GetTOUCommands(string commandTOU)
        {
            const string regexTOU = @"(\x22([\w\W]*?)\x22)";
            List<string> TOUCommands = new List<string>();
            MatchCollection matches = Regex.Matches(commandTOU, regexTOU, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                TOUCommands.Add(groups[0].Value);
            }
            return TOUCommands;
        }

        
        public string GetTOUFileContent(string filePath)
        {
            string fileContent = string.Empty;
            int index = 0;
            int intsec = 34;
            int intb = 0;
            string strTemp = string.Empty;
            string[] readData = new string[40];
            string[] finalData = new string[40];

            StreamReader streamReader = File.OpenText(filePath);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            if (fileStream.Length <= 0)
                return string.Empty;

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                while (sr.Peek() >= 0)
                {
                    readData[index++] = sr.ReadLine();
                }
            }
            index = 0;

            while (intb < 38)
            {
                if (intb == 6 || intb == 13 || intb == 20 || intb == 27)
                {
                    finalData[intb] = readData[intsec];
                    intsec++;
                }
                else
                {
                    finalData[intb] = readData[index];
                    index++;
                }
                intb++;
                if (intb == 38)
                    finalData[intb] = readData[intb];

            }
            index = 0;
            while (index < 39)
            {
                strTemp = finalData[index];
                strTemp = strTemp.Replace("28", "(");
                strTemp = strTemp.Replace("29", ")");
                strTemp = strTemp.Replace(" ", "");
                strTemp = strTemp.Substring(strTemp.IndexOf("("), ((strTemp.IndexOf(")") + 1) - strTemp.IndexOf("(")));
                intb = 1;
                fileContent += "(";
                while (intb < strTemp.Length - 1)
                {
                    string tempcontent = (Convert.ToInt16(strTemp.Substring(intb, 2)) - 30).ToString();
                    fileContent += tempcontent;
                    intb += 2;
                }
                fileContent += ")";
                index++;
            }
            fileStream.Close();
            streamReader.Close();
            return fileContent;
        }

        public string GetPushDisplayParameterSP(Dictionary<string, string> dicData, ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = string.Empty;            
            try
            {
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                ReadConfigurations readConfig = new ReadConfigurations();

                foreach (string touCommand in readConfig.GetPushReadCommandSP(todConfiguration.PushParameterRead))
                {
                    if (touCommand.Length> 5)
                    {
                        string command = touCommand.Substring(1, touCommand.Length - 2);
                        data = ReceiveDataCommand(command, 2, "\x03", communications);
                        if (data.Contains("Error"))
                        {
                            statusMsg = data;
                            break;
                        }
                        else
                        {
                            dicData.Add(touCommand, communications.OutBuffer);
                            Thread.Sleep(50);
                        }
                    }
                }                
            }
            catch (Exception)
            {
            }
            finally
            {

            }
            return data;    
        }




        public string GetScrollDisplayParameterSP(Dictionary<string, string> dicData, ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = "";
            try
            {
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                ReadConfigurations readConfig = new ReadConfigurations();

                foreach (string touCommand in readConfig.GetScrollReadCommandSP(todConfiguration.ScrollParameterRead))
                {
                    if (touCommand.Length > 5)
                    {
                        string command = touCommand.Substring(1, touCommand.Length - 2);
                        data = ReceiveDataCommand(command, 2, "\x03", communications);
                        if (data.Contains("Error"))
                        {
                            statusMsg = data;
                            break;
                        }
                        else
                        {
                            dicData.Add(touCommand, communications.OutBuffer);
                            Thread.Sleep(50);
                        }
                    }
                }                
            }
            catch (Exception)
            {
            }
            finally
            {

            }
            return data;    
        }




        public string GetHighDisplayParameterSP(Dictionary<string, string> dicData, ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = "";
            try
            {
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                ReadConfigurations readConfig = new ReadConfigurations();

                foreach (string touCommand in readConfig.GetHighReadCommandSP(todConfiguration.HighParameterRead))
                {
                    if (touCommand.Length > 5)
                    {
                        string command = touCommand.Substring(1, touCommand.Length - 2);
                        data = ReceiveDataCommand(command, 2, "\x03", communications);
                        if (data.Contains("Error"))
                        {
                            statusMsg = data;
                            break;
                        }
                        else
                        {
                            dicData.Add(touCommand, communications.OutBuffer);
                            Thread.Sleep(50);
                        }
                    }
                }                
            }
            catch (Exception)
            {
            }
            finally
            {

            }
            return data;
        }



        public void SetPushParameter(List<string> SelectListPush)
        {            
            string ResData = "";
            try
            {
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                ReadConfigurations readConfig = new ReadConfigurations();
                int iter = 0;
                foreach (string touCommand in readConfig.GetPushWriteCommandSP(todConfiguration.PushParameterWrite))
                {
                    if (touCommand.Length > 5)
                    {
                        string command = touCommand;
                        string Data = StrToHex(SelectListPush[iter++]);
                        command = command.Replace(ReadoutConstant.DATA, Data);
                        command = command.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(command.Substring(2, command.Length - 5)));
                        ResData = ReceiveDataCommand(command, 3, "\x06", communications);
                        if (ResData.Contains("Error"))
                        {
                            this.StatusMessage = ResData;
                            break;
                        }                     
                    }
                }
               
            }
            catch (Exception)
            {
            }
            finally
            {

            }            
        }

        public void SetScrollParameter(List<string> SelectListScroll)
        {
            string ResData = "";
            try
            {
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                ReadConfigurations readConfig = new ReadConfigurations();
                int iter = 0;
                foreach (string touCommand in readConfig.GetScrollWriteCommandSP(todConfiguration.ScrollParameterWrite))
                {
                    if (touCommand.Length > 5)
                    {
                        string command = touCommand;
                        string Data = StrToHex(SelectListScroll[iter++]);
                        command = command.Replace(ReadoutConstant.DATA, Data);
                        command = command.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(command.Substring(2, command.Length - 5)));
                        ResData = ReceiveDataCommand(command, 3, "\x06", communications);
                        if (ResData.Contains("Error"))
                        {
                            this.StatusMessage = ResData;
                            break;
                        }                       
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {

            }            
        }

        public void SetHighParameter(List<string> SelectListHigh)
        {
            string ResData = "";
            try
            {
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                ReadConfigurations readConfig = new ReadConfigurations();
                int iter = 0;
                foreach (string touCommand in readConfig.GetHighWriteCommandSP(todConfiguration.HighParameterWrite))
                {
                    if (touCommand.Length > 5)
                    {
                        string command = touCommand;
                        string Data = StrToHex(SelectListHigh[iter++]);
                        command = command.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(Data));
                        command = command.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(command.Substring(2, command.Length - 5)));
                        ResData = ReceiveDataCommand(command, 3, "\x06", communications);
                        if (ResData.Contains("Error"))
                        {
                            this.StatusMessage = ResData;
                            break;
                        }                       
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {

            }            
        }
    }
}




