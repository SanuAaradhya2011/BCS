using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CAB.IECChannel.ReadOut;
using System.Threading;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Contracts;
using CABAppControl;
using CABEntity;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CAB.IECChannel.Programming;

namespace CAB.IECChannel.ReadOut
{
    public class ReadConfigurations : ReadBase
    {
        int commandreTry = 3;
        //private IECLocalCommunication communications;
        string MDConfigReadCommand = string.Empty;
        public ReadConfigurations()
        {
            command = Command.GetInstance();
            //communications = ChannelManager.GetChannel() as IECLocalCommunication;
        }
        # region old implementation
        //private bool ReadkvarSelection(MeterConfigurationConfigSection configSection, ref string data)
        //{
        //    string kvarSelectionReadCommand = string.Empty;
        //    try
        //    {
        //        communications.OutBuffer = string.Empty;
        //        kvarSelectionReadCommand = configSection.ReadCommand;
        //        communications.Command = kvarSelectionReadCommand;
        //        communications.CommandID = 2;
        //        communications.ReadFlag = false;
        //        communications.IsDataReceived = false;
        //        communications.OutBuffer = string.Empty;
        //        communications.CurrentTime = DateTime.Now;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.DelayExecution();
        //        do
        //        {
        //            if (communications.Timeout())
        //            {
        //                this.StatusMessage = "Timeout!";
        //                Application.DoEvents();
        //                break;
        //            }
        //            if (IsAborted)
        //            {
        //                this.StatusMessage = "User Aborted.";
        //                Application.DoEvents();
        //                return false;
        //            }
        //        } while (true);
        //        if (communications.ReadFlag)
        //        {
        //            communications.DelayExecution();
        //            data = communications.OutBuffer;
        //            if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
        //                data = string.Empty;
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        this.StatusMessage = "Failure in MD with IP Readout";
        //        return false;
        //    }
        //}

        //private bool ReadMDWithIP(MeterConfigurationConfigSection configSection,ref string data)
        //{
        //    try
        //    {
        //        communications.OutBuffer = string.Empty;
        //        MDConfigReadCommand = configSection.ReadCommand;
        //        communications.Command = MDConfigReadCommand;
        //        communications.CommandID = 2;
        //        communications.ReadFlag = false;
        //        communications.IsDataReceived = false;
        //        communications.OutBuffer = string.Empty;
        //        communications.CurrentTime = DateTime.Now;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.DelayExecution();
        //        do
        //        {
        //            if (communications.Timeout())
        //            {
        //                this.StatusMessage = "Timeout!";
        //                Application.DoEvents();
        //                break;
        //            }
        //            if (IsAborted)
        //            {
        //                this.StatusMessage = "User Aborted.";
        //                Application.DoEvents();
        //                return false;
        //            }
        //        } while (true);
        //        if (communications.ReadFlag)
        //        {
        //            communications.DelayExecution();
        //            data = communications.OutBuffer;
        //            if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
        //                data = string.Empty;
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        this.StatusMessage = "Failure in MD with IP Readout";
        //        return false;
        //    }
        //}

        # endregion

        private int[] GetFutureTODAddresses()
        {
            int[] futureTODAddress = new int[] 
            {
                784, 785, 786, 787, 788, 789,790, 800, 801, 802, 803, 804, 805,  806,816,
                817, 818, 819, 820, 821, 822,832, 833, 834, 835, 836, 837,838, 848, 849,    
                850, 851, 852, 853, 854, 855, 856, 857,864 
            };
            return futureTODAddress;
        }

        private int[] GetCurrentTODAddresses()
        {
            int[] currentTODAddress = new int[] 
            {
                1040, 1041,1042,1043,1044,1045,1046,
                1056,1057,1058,1059,1060,1061,1062,
                1072,1073,1074,1075,1076,1077,1078,
                1088,1089,1090,1091,1092,1093,1094,
                1104,1105,1106,1107,1108,1109,1110,1111,1112,1113,1114
            };
            return currentTODAddress;
        }

        public List<string> GetTODConfigurationReadCommandSP(TODConfigurationReadCommandSP todReadCommand)
        {
            List<string> touCommands = new List<string>();
            touCommands.Add(todReadCommand.R4D401);
            touCommands.Add(todReadCommand.R4D402);
            touCommands.Add(todReadCommand.R4D403);
            touCommands.Add(todReadCommand.R4D404);
            touCommands.Add(todReadCommand.R4D400);
            return touCommands;
        }


        public List<string> GetPushReadCommandSP(TODConfigurationPushParameterRead pushReadCommand)
        {
            List<string> touCommands = new List<string>();
            touCommands.Add(pushReadCommand.R1);
            touCommands.Add(pushReadCommand.R2);
            touCommands.Add(pushReadCommand.R3);
            touCommands.Add(pushReadCommand.R4);
            return touCommands;
        }


        public List<string> GetPushWriteCommandSP(TODConfigurationPushParameterWrite pushWriteCommand)
        {
            List<string> touCommands = new List<string>();
            touCommands.Add(pushWriteCommand.W1);
            touCommands.Add(pushWriteCommand.W2);
            touCommands.Add(pushWriteCommand.W3);
            touCommands.Add(pushWriteCommand.W4);
            return touCommands;
        }


        public List<string> GetScrollReadCommandSP(TODConfigurationScrollParameterRead scrollReadCommand)
        {
            List<string> touCommands = new List<string>();
            touCommands.Add(scrollReadCommand.R1);
            touCommands.Add(scrollReadCommand.R2);
            touCommands.Add(scrollReadCommand.R3);
            touCommands.Add(scrollReadCommand.R4);
            return touCommands;
        }


        public List<string> GetScrollWriteCommandSP(TODConfigurationScrollParameterWrite scrollWriteCommand)
        {
            List<string> touCommands = new List<string>();
            touCommands.Add(scrollWriteCommand.W1);
            touCommands.Add(scrollWriteCommand.W2);
            touCommands.Add(scrollWriteCommand.W3);
            touCommands.Add(scrollWriteCommand.W4);
            return touCommands;
        }


        public List<string> GetHighReadCommandSP(TODConfigurationHighParameterRead highReadCommand)
        {
            List<string> touCommands = new List<string>();
            touCommands.Add(highReadCommand.R1);
            touCommands.Add(highReadCommand.R2);
            touCommands.Add(highReadCommand.R3);
            touCommands.Add(highReadCommand.R4);
            return touCommands;
        }


        public List<string> GetHighWriteCommandSP(TODConfigurationHighParameterWrite highWriteCommand)
        {
            List<string> touCommands = new List<string>();
            touCommands.Add(highWriteCommand.W1);
            touCommands.Add(highWriteCommand.W2);
            touCommands.Add(highWriteCommand.W3);
            touCommands.Add(highWriteCommand.W4);
            return touCommands;
        }





        private List<string> GetFutureTODCommands(TODConfigurationFutureReadCommand todReadCommand)
        {
            List<string> touCommands = new List<string>();
            //TODConfigurationFutureReadCommand todReadCommand = new TODConfigurationFutureReadCommand();
            touCommands.Add(todReadCommand.S1D1);
            touCommands.Add(todReadCommand.S1D2);
            touCommands.Add(todReadCommand.S1D3);
            touCommands.Add(todReadCommand.S1D4);
            touCommands.Add(todReadCommand.S1D5);
            touCommands.Add(todReadCommand.S1D6);
            touCommands.Add(todReadCommand.S1AcDate);
            touCommands.Add(todReadCommand.S2D1);
            touCommands.Add(todReadCommand.S2D2);
            touCommands.Add(todReadCommand.S2D3);
            touCommands.Add(todReadCommand.S2D4);
            touCommands.Add(todReadCommand.S2D5);
            touCommands.Add(todReadCommand.S2D6);
            touCommands.Add(todReadCommand.S2AcDate);
            touCommands.Add(todReadCommand.S3D1);
            touCommands.Add(todReadCommand.S3D2);
            touCommands.Add(todReadCommand.S3D3);
            touCommands.Add(todReadCommand.S3D4);
            touCommands.Add(todReadCommand.S3D5);
            touCommands.Add(todReadCommand.S3D6);
            touCommands.Add(todReadCommand.S3AcDate);
            touCommands.Add(todReadCommand.S4D1);
            touCommands.Add(todReadCommand.S4D2);
            touCommands.Add(todReadCommand.S4D3);
            touCommands.Add(todReadCommand.S4D4);
            touCommands.Add(todReadCommand.S4D5);
            touCommands.Add(todReadCommand.S4D6);
            touCommands.Add(todReadCommand.S4AcDate);
            touCommands.Add(todReadCommand.H1);
            touCommands.Add(todReadCommand.H2);
            touCommands.Add(todReadCommand.H3);
            touCommands.Add(todReadCommand.H4);
            touCommands.Add(todReadCommand.H5);
            touCommands.Add(todReadCommand.H6);
            touCommands.Add(todReadCommand.H7);
            touCommands.Add(todReadCommand.H8);
            touCommands.Add(todReadCommand.H9);
            touCommands.Add(todReadCommand.H10);
            touCommands.Add(todReadCommand.HAcDate);

            return touCommands;
        }

        public List<string> GetCurrentTODCommands(TODConfigurationCurrentReadCommand todReadCommand)//2 march 2012 function access modifier changed from private to public to access it from TOUInformation..
        {
            List<string> touCommands = new List<string>();
            //TODConfigurationCurrentReadCommand todReadCommand = new TODConfigurationCurrentReadCommand();

            touCommands.Add(todReadCommand.S1D1);
            touCommands.Add(todReadCommand.S1D2);
            touCommands.Add(todReadCommand.S1D3);
            touCommands.Add(todReadCommand.S1D4);
            touCommands.Add(todReadCommand.S1D5);
            touCommands.Add(todReadCommand.S1D6);
            touCommands.Add(todReadCommand.S1AcDate);
            touCommands.Add(todReadCommand.S2D1);
            touCommands.Add(todReadCommand.S2D2);
            touCommands.Add(todReadCommand.S2D3);
            touCommands.Add(todReadCommand.S2D4);
            touCommands.Add(todReadCommand.S2D5);
            touCommands.Add(todReadCommand.S2D6);
            touCommands.Add(todReadCommand.S2AcDate);
            touCommands.Add(todReadCommand.S3D1);
            touCommands.Add(todReadCommand.S3D2);
            touCommands.Add(todReadCommand.S3D3);
            touCommands.Add(todReadCommand.S3D4);
            touCommands.Add(todReadCommand.S3D5);
            touCommands.Add(todReadCommand.S3D6);
            touCommands.Add(todReadCommand.S3AcDate);
            touCommands.Add(todReadCommand.S4D1);
            touCommands.Add(todReadCommand.S4D2);
            touCommands.Add(todReadCommand.S4D3);
            touCommands.Add(todReadCommand.S4D4);
            touCommands.Add(todReadCommand.S4D5);
            touCommands.Add(todReadCommand.S4D6);
            touCommands.Add(todReadCommand.S4AcDate);
            touCommands.Add(todReadCommand.H1);
            touCommands.Add(todReadCommand.H2);
            touCommands.Add(todReadCommand.H3);
            touCommands.Add(todReadCommand.H4);
            touCommands.Add(todReadCommand.H5);
            touCommands.Add(todReadCommand.H6);
            touCommands.Add(todReadCommand.H7);
            touCommands.Add(todReadCommand.H8);
            touCommands.Add(todReadCommand.H9);
            touCommands.Add(todReadCommand.H10);
            touCommands.Add(todReadCommand.HAcDate);

            return touCommands;
        }

        private bool ReadTODCommands(ref string currTOU, ref string futureTOU,ref string statusMsg)
        {
            try
            {
                string strReadCommand = string.Empty;
                string responseTOU = String.Empty;
                statusMsg = "";
                this.StatusMessage = "Reading TOD data. Please wait...";
                Application.DoEvents();
                communications.OutBuffer = string.Empty;
                //MeterConfiguration meterConfiguration = null;
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader( AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                communications.DelayExecution();

                foreach (string touCommand in GetCurrentTODCommands(todConfiguration.CurrentReadCommand))
                {
                    communications.Command = touCommand.Substring(1, touCommand.Length - 2);
                    communications.CommandID = 2;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    communications.SendCommand();
                    communications.DelayExecution();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = "Timeout!";
                            statusMsg = this.StatusMessage;
                            currTOU = "";
                            futureTOU = "";
                            return false;
                        }
                    } while (!communications.ReadFlag);

                    responseTOU = communications.OutBuffer;
                    responseTOU = responseTOU.Substring(responseTOU.IndexOf("("));
                    if (ReadoutCommon.CalculateBcc(responseTOU, responseTOU.Length - 2, responseTOU.Substring(responseTOU.Length - 1, 1)))
                    {
                        currTOU += communications.OutBuffer;
                    }
                    else
                    {
                        this.StatusMessage = "Invalid TOU Data.";
                        return false;
                    }
                }

                foreach (string touCommand in GetFutureTODCommands(todConfiguration.FutureReadCommand))
                {
                    communications.Command = touCommand.Substring(1, touCommand.Length - 2);
                    communications.CommandID = 2;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    communications.SendCommand();
                    communications.DelayExecution();
                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = "Timeout!";
                            statusMsg = this.StatusMessage;
                            return false;
                        }
                    } while (!communications.ReadFlag);

                    responseTOU = communications.OutBuffer;
                    responseTOU = responseTOU.Substring(responseTOU.IndexOf("("));
                    if (ReadoutCommon.CalculateBcc(responseTOU, responseTOU.Length - 2, responseTOU.Substring(responseTOU.Length - 1, 1)))
                    {
                        futureTOU += communications.OutBuffer;
                    }
                    else
                    {
                        this.StatusMessage = "Invalid TOU Data.";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool ReadTODCommandsSP(ref string currTOU, ref string futureTOU, ref string statusMsg)
        {
            try
            {
                int imaxcmdcounter = 6;
                string strReadCommand = string.Empty;
                string responseTOU = String.Empty;
                statusMsg = "";
                this.StatusMessage = "Reading TOD data. Please wait...";
                Application.DoEvents();
                communications.OutBuffer = string.Empty;
                //MeterConfiguration meterConfiguration = null;
                TODConfiguration todConfiguration = null;
                XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                communications.DelayExecution();

                if (!ConnectToIECMeter()) return false;

                if (communications.ResponseSignOn.Contains("LGC110A")
                    || communications.ResponseSignOn.Contains("LGC110B")
                    ) imaxcmdcounter = 10;

                foreach (string touCommand in GetTODConfigurationReadCommandSP(todConfiguration.ReadCommandSP))
                {
                    communications.Command = touCommand.Substring(1, touCommand.Length - 2);
                    communications.CommandID = 2;
                    communications.ReadFlag = false;
                    communications.IsDataReceived = false;
                    communications.OutBuffer = string.Empty;
                    communications.CurrentTime = DateTime.Now;
                    Thread.Sleep(100);
                    communications.SendCommand();
                    communications.DelayExecution();

                    if (touCommand.Contains("340244343030"))
                        imaxcmdcounter = 4;

                    do
                    {
                        if (communications.Timeout())
                        {
                            this.StatusMessage = "Timeout!";
                            statusMsg = this.StatusMessage;
                            currTOU = "";
                            futureTOU = "";
                            return false;
                        }
                    } while (!communications.ReadFlag);

                    responseTOU = communications.OutBuffer;
                    responseTOU = responseTOU.Substring(responseTOU.IndexOf("("));

                    if (ReadoutCommon.CalculateBcc(responseTOU, responseTOU.Length - 2, responseTOU.Substring(responseTOU.Length - 1, 1)))
                    {
                        currTOU += communications.OutBuffer.Remove(communications.OutBuffer.Length - 2, 2);
                    }

                    for (int icmd = 0; icmd < imaxcmdcounter - 1; icmd++)
                    {
                        communications.Command = "06";
                        communications.CommandID = 2;
                        communications.ReadFlag = false;
                        communications.IsDataReceived = false;
                        communications.OutBuffer = string.Empty;
                        communications.CurrentTime = DateTime.Now;
                        Thread.Sleep(100);
                        communications.SendCommand();
                        communications.DelayExecution();
                        Thread.Sleep(100);

                        do
                        {
                            if (communications.Timeout())
                            {
                                this.StatusMessage = "Timeout!";
                                statusMsg = this.StatusMessage;
                                currTOU = "";
                                futureTOU = "";
                                return false;
                            }
                        } while (!communications.ReadFlag);

                        responseTOU = communications.OutBuffer;
                        responseTOU = responseTOU.Substring(responseTOU.IndexOf("("));

                        if (ReadoutCommon.CalculateBcc(responseTOU, responseTOU.Length - 2, responseTOU.Substring(responseTOU.Length - 1, 1)))
                        {
                            currTOU += communications.OutBuffer.Remove(communications.OutBuffer.Length - 2, 2);
                        }
                    }

                    currTOU += "\x04";
                }

                currTOU += "\x03";

                return true;

            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                // Close port and break command is added after read TOD
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
        }

        private bool ReadCommands(MeterConfigurationConfigSection configSection, ref string data)
        {
            string strReadCommand = string.Empty;
            //List<string> touCommands = new List<string>();
            //string responseTOU = String.Empty;
            try
            {
                communications.OutBuffer = string.Empty;
                strReadCommand = configSection.ReadCommand;

                string calculatedBcc = ReadoutCommon.ReturnBcc(strReadCommand.Substring(2, strReadCommand.Length - 5));
                strReadCommand = strReadCommand.Replace(ReadoutConstant.BCC, calculatedBcc);

                communications.Command = strReadCommand;
                communications.CommandID = 2;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.OutBuffer = string.Empty;

                communications.CurrentTime = DateTime.Now;
                communications.SendCommand();
                communications.DelayExecution();
                //communications.DelayExecution();

                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout!";
                        Application.DoEvents();
                        break;
                    }
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                        return false;
                    }
                } while (true);
                if (communications.ReadFlag)
                {
                    communications.DelayExecution();
                    data = communications.OutBuffer;
                    if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
                        data = string.Empty;
                }
                return true;
            }
            catch (Exception)
            {
                this.StatusMessage = "Failure in " + configSection.Name+ " Readout";
                return false;
            }
        }




        public string HandshakeCommands(bool IsManufactureSpecificReadCmd)
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;
            string passwordCommand = string.Empty;

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port";
                    return null;
                }

                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Sign-On failure";
                    return null;
                }
                communications.DelayExecution();
                if (!IsManufactureSpecificReadCmd)
                {
                    communications.Command = "063035310D0A";
                    communications.OutBuffer = string.Empty;
                    communications.CommandID = 2;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = "Sign-On failure";
                            return null;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    communications.CurrentTime = DateTime.Now;

                    communications.OutBuffer = string.Empty;

                    //Meter Id changed from "11111111" to "00000000"
                    passwordCommand = "0150310228"+ProgrammingCommon.GetASCIIValue("00000000") +  "2903Bcc";

                    string calculatedBcc = ReadoutCommon.ReturnBcc(passwordCommand.Substring(2, passwordCommand.Length - 5));
                    passwordCommand = passwordCommand.Replace(ReadoutConstant.BCC, calculatedBcc);

                    communications.Command = passwordCommand;
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
                            return null;
                        }
                    } while (communications.OutBuffer.Length < 1);

                    if (communications.OutBuffer.Length >= 1)
                    {
                        if (communications.OutBuffer.Length == 2)
                            charACK = Convert.ToChar(communications.OutBuffer.Substring(1, 1));
                        else
                            charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                        if (charACK != 6)
                        {
                            this.StatusMessage = "Access Denied";
                            return null;
                        }
                    }
                }
                else
                {
                    communications.Command = "063035360D0A";
                    communications.OutBuffer = string.Empty;
                    communications.CommandID = 2;
                    communications.SendCommand();
                    communications.DelayExecution();
                    Thread.Sleep(200);
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = "Sign-On failure";
                            return null;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    communications.CurrentTime = DateTime.Now;

                    communications.OutBuffer = string.Empty;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return communications.ResponseSignOn + "|" + data;
        }


        public void BreakCommunication()
        {
            communications.Command = command.BreakCommand;
            communications.SendCommand();
            communications.DelayExecution();
            if(communications.ComPort.IsOpen)
                communications.ClosePort();
        }
        
        public string ReadMeterConfigurations(MeterConfigurationConfigSection configSection,ref string statusMsg)
        {
            string data = string.Empty;
            string currTOD = string.Empty;
            string futureTOD = string.Empty;
            statusMsg = "";
            try
            {
                //Read Command

                if (configSection.Name.ToLower() == DisplayParameter.PushMode.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data))
                        if(data!="")this.StatusMessage = "PUSH Mode Parameters readout Successful";
                }

                if (configSection.Name.ToLower() == DisplayParameter.ScrollMode.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data))
                    { if (data != "")this.StatusMessage = "SCROLL Mode Parameters readout Successful"; }
                }

                if (configSection.Name.ToLower() == DisplayParameter.HighResolutionMode.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data))
                    { if (data != "")this.StatusMessage = "High Resolution Mode Parameters readout Successful"; }
                }

                if (configSection.Name.ToLower() == DisplayParameter.DisplayTimeouts.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data))
                    { if (data != "")this.StatusMessage = "Display Timeout Parameters readout Successful"; }
                }

                if (configSection.Name.ToLower() == ConfigurationParameter.MDWithIP.ToString().ToLower())
                {
                    //if (ReadMDWithIP(configSection, ref data)) { this.StatusMessage = "MD with IP readout Successful"; }
                    if (ReadCommands(configSection, ref data))
                    { this.StatusMessage = "MD with IP readout Successful"; }
                }


                if (configSection.Name.ToLower() == ConfigurationParameter.KVAHSelection.ToString().ToLower())
                {
                    //if (ReadkvarSelection(configSection, ref data)) { this.StatusMessage = "kvar Selection readout Successful"; }
                    if (ReadCommands(configSection, ref data)) { this.StatusMessage = "kvah Selection readout Successful"; }
                }


                if (configSection.Name.ToLower() == ConfigurationParameter.DisplayParameters.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data)) { this.StatusMessage = "Display Parameters readout Successful"; }
                }


                if (configSection.Name.ToLower() == ConfigurationParameter.TOD.ToString().ToLower())
                {
                    if (ReadTODCommands(ref currTOD, ref futureTOD,ref statusMsg))
                    {
                        this.StatusMessage = "TOD readout Successful";
                    }


                    return communications.ResponseSignOn + "|" + "/TU//" + currTOD + "/FU//" + futureTOD;
                }
                // Read TOD data for 1P IEC meters
                if (configSection.Name.ToLower() == ConfigurationParameter.TODSP.ToString().ToLower())
                {
                    if (ReadTODCommandsSP(ref currTOD, ref futureTOD, ref statusMsg))
                    {
                        this.StatusMessage = "TOD readout Successful";
                    }


                    return currTOD;
                }

                if (configSection.Name.ToLower() == "RTC".ToLower())
                {
                    if (ReadCommands(configSection, ref data)) { this.StatusMessage = "RTC readout Successful"; }
                }


                if (configSection.Name.ToLower() == ConfigurationParameter.DailyLog.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data)) { this.StatusMessage = "Daily Log readout Successful"; }
                }
                // Billing Reset Parameters	
                if (configSection.Name.ToLower() == ConfigurationParameter.ModeOfBilling.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data)) { this.StatusMessage = "Mode of Billing readout Successful"; }
                }
                if (configSection.Name.ToLower() == ConfigurationParameter.BillingPeriod.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data)) { this.StatusMessage = "Billing Period readout Successful"; }
                }
                if (configSection.Name.ToLower() == ConfigurationParameter.ResetLockOutDays.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data)) { this.StatusMessage = "Reset lock out days readout Successful"; }
                }
                if (configSection.Name.ToLower() == ConfigurationParameter.LockUnlockRS232.ToString().ToLower())
                {
                    if (ReadCommands(configSection, ref data))
                    {
                        this.StatusMessage = "RS232Lock readout Successful";
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            statusMsg = this.StatusMessage;
            return communications.ResponseSignOn + "|" + data;
        }

        public string ReadMeterConfigurations_Old(MeterConfigurationConfigSection configSection)
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;
            string passwordCommand = string.Empty;
            string MDConfigReadCommand = string.Empty;

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port";
                    return null;
                }

                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Sign-On failure";
                    return null;
                }
                communications.DelayExecution();
                communications.Command = "063035310D0A";
                communications.OutBuffer = string.Empty;
                communications.CommandID = 2;
                communications.SendCommand();
                communications.DelayExecution();
                Thread.Sleep(200);
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = "Sign-On failure";
                        return null;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;

                communications.OutBuffer = string.Empty;
                passwordCommand = "015031022831313131313131314D2903Bcc";
                //passwordCommand = "01503102283030303030303030482903Bcc";

                string calculatedBcc = ReadoutCommon.ReturnBcc(passwordCommand.Substring(2, passwordCommand.Length - 5));
                passwordCommand = passwordCommand.Replace(ReadoutConstant.BCC, calculatedBcc);

                communications.Command = passwordCommand;
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
                        return null;
                    }
                } while (communications.OutBuffer.Length < 1);

                if (communications.OutBuffer.Length >= 1)
                {
                    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                    if (charACK != 6)
                    {
                        this.StatusMessage = "Access Denied";
                        return null;
                    }
                }

                //Read Command

                communications.OutBuffer = string.Empty;
                MDConfigReadCommand = configSection.ReadCommand;
                calculatedBcc = ReadoutCommon.ReturnBcc(MDConfigReadCommand.Substring(2, MDConfigReadCommand.Length - 5));
                MDConfigReadCommand = MDConfigReadCommand.Replace(ReadoutConstant.BCC, calculatedBcc);

                communications.Command = MDConfigReadCommand;
                communications.CommandID = 2;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.OutBuffer = string.Empty;
                communications.CurrentTime = DateTime.Now;


                communications.SendCommand();
                communications.DelayExecution();
                //communications.DelayExecution();
                //communications.DelayExecution();
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout!";
                        Application.DoEvents();
                        break;
                    }
                    if (IsAborted)
                    {
                        this.StatusMessage = "User Aborted.";
                        Application.DoEvents();
                        return string.Empty;
                    }
                } while (true);
                if (communications.ReadFlag)
                {
                    communications.DelayExecution();
                    data = communications.OutBuffer;
                    if (!ReadoutCommon.CalculateBcc(data.Substring(1), data.Length - 3, data.Substring(data.Length - 1, 1)))
                        data = string.Empty;
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
            return data;
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
