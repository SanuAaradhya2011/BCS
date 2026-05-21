using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECChannel.ReadOut;
using System.Threading;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Contracts;
using IEC.CAB.AppControl;

namespace CAB.IECChannel.Programming
{
    public class WriteConfigurations : ReadBase
    {
        private IECLocalCommunication communications;
        //string MDConfigWriteCommand = string.Empty;
        string configWriteCommand = string.Empty;
        private string data;
        // Added for daily log
        public bool DailyLogFlag = false;
        string configDeleteCommand = string.Empty;
        public WriteConfigurations()
        {
            data = String.Empty;
            command = Command.GetInstance();
            communications = ChannelManager.GetChannel() as IECLocalCommunication;
        }

        public string HandshakeCommands(string meterPassword, bool IsManufactureSpecifcReadCmd)
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
                    BreakCommunication();
                    this.StatusMessage = "Error in opening port";
                    return null;
                }

                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    BreakCommunication();
                    this.StatusMessage = "Sign-On failure";
                    return null;
                }
                communications.DelayExecution();                
                communications.OutBuffer = string.Empty;
                communications.CommandID = 2;
                if (IsManufactureSpecifcReadCmd)
                    communications.Command = "063035360D0A";//";//"063035310D0A";
                else
                    communications.Command = "063035310D0A";
                communications.SendCommand();

                communications.DelayExecution();
                ////////////////////////////////////////
                communications.DelayExecution();
                communications.DelayExecution();
                ////////////////////////////////////////////
                Thread.Sleep(200);
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        BreakCommunication();
                        this.StatusMessage = "Sign-On failure";
                        return null;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                if (!IsManufactureSpecifcReadCmd)
                {
                    communications.CurrentTime = DateTime.Now;

                    communications.OutBuffer = string.Empty;
                    //Meter ID changed from "11111111" to "00000000"
                    passwordCommand = "0150310228" + ProgrammingCommon.GetASCIIValue(meterPassword)+ "4D2903Bcc";

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
                            BreakCommunication();
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
                            BreakCommunication();
                            this.StatusMessage = "Access Denied";
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                BreakCommunication();
                return null;
            }
            return communications.ResponseSignOn + "|" + data;
        }

        public void BreakCommunication()
        {
            communications.Command = command.BreakCommand;
            communications.SendCommand();
            communications.DelayExecution();
            if (communications.ComPort.IsOpen)
            communications.ClosePort();
        }
        # region old implementation
        //private bool WriteMDWithIP(MeterConfigurationConfigSection configSection, ref string data)
        //{
        //    communications.OutBuffer = string.Empty;
        //    MDConfigWriteCommand = configSection.WriteCommand;
        //    MDConfigWriteCommand = MDConfigWriteCommand.Replace(ReadoutConstant.DATA, data);
        //    string calBcc = ReadoutCommon.ReturnBcc(MDConfigWriteCommand.Substring(2, MDConfigWriteCommand.IndexOf("Bcc") - 2));

        //    MDConfigWriteCommand = MDConfigWriteCommand.Replace(ReadoutConstant.BCC, calBcc);
        //    communications.Command = MDConfigWriteCommand;
        //    communications.CommandID = 3;
        //    communications.ReadFlag = false;
        //    communications.IsDataReceived = false;
        //    communications.OutBuffer = string.Empty;
        //    communications.CurrentTime = DateTime.Now;
        //    communications.SendCommand();
        //    do
        //    {
        //        if (communications.Timeout())
        //        {
        //            this.StatusMessage = "Timeout!";
        //            Application.DoEvents();
        //            return false;
        //        }
        //    } while (communications.OutBuffer.Length < 1);
        //    return true;
        //}

        //private bool WritekvarSelection(MeterConfigurationConfigSection configSection, ref string data)
        //{
        //    string kvarSelectionWriteCommand = String.Empty;
        //    try
        //    {
        //        communications.OutBuffer = string.Empty;
        //        kvarSelectionWriteCommand = configSection.WriteCommand;
        //        kvarSelectionWriteCommand = kvarSelectionWriteCommand.Replace(ReadoutConstant.DATA, data);
        //        string calBcc = ReadoutCommon.ReturnBcc(kvarSelectionWriteCommand.Substring(2, kvarSelectionWriteCommand.IndexOf("Bcc") - 2));

        //        kvarSelectionWriteCommand = kvarSelectionWriteCommand.Replace(ReadoutConstant.BCC, calBcc);
        //        communications.Command = kvarSelectionWriteCommand;
        //        communications.CommandID = 3;
        //        communications.ReadFlag = false;
        //        communications.IsDataReceived = false;
        //        communications.OutBuffer = string.Empty;
        //        communications.CurrentTime = DateTime.Now;
        //        communications.SendCommand();
        //        do
        //        {
        //            if (communications.Timeout())
        //            {
        //                this.StatusMessage = "Timeout!";
        //                Application.DoEvents();
        //                return false;
        //            }
        //        } while (communications.OutBuffer.Length < 1);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        # endregion

        public void WriteTODConfigurations(List<string> todCommands)
        {
            this.StatusMessage = "Writing TOU data. Please wait...";
            Application.DoEvents();
            communications.OutBuffer = string.Empty;
            foreach (string todCommand in todCommands)
            {
                string calculatedBcc = ReadoutCommon.ReturnBcc(todCommand.Substring(2));
                communications.Command = string.Concat(todCommand, calculatedBcc);
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
                        this.StatusMessage = "Timeout Error.";
                    }
                } while (communications.OutBuffer.Length < 1);

                char charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                if (charACK != 6)
                {
                    this.StatusMessage = "Access Denied";
                }
            }
        }

        private int WriteConfiguration(string meterPwd, MeterConfigurationConfigSection configSection, bool flag_Bcc, ref string data)
        {
            configWriteCommand = configSection.WriteCommand;
            configWriteCommand = configWriteCommand.Replace(ReadoutConstant.DATA, data);
            string calBcc = string.Empty;
            configWriteCommand = configWriteCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(meterPwd));
            if (flag_Bcc)
                calBcc = ReadoutCommon.ReturnBcc(configWriteCommand.Substring(0, configWriteCommand.IndexOf("Bcc")));
            else
                calBcc = ReadoutCommon.ReturnBcc(configWriteCommand.Substring(2, configWriteCommand.IndexOf("Bcc") - 2));

            configWriteCommand = configWriteCommand.Replace(ReadoutConstant.BCC, calBcc);
           
            communications.Command = configWriteCommand;
            communications.CommandID = 3;
            communications.ReadFlag = false;
            communications.IsDataReceived = false;
            communications.OutBuffer = string.Empty;
            communications.CurrentTime = DateTime.Now;
            communications.SendCommand();
            communications.DelayExecution();
            communications.DelayExecution();
            communications.DelayExecution();
            //Thread.Sleep(200);
            communications.CurrentTime = DateTime.Now;
            do
            {
                if (communications.Timeout())
                {
                    this.StatusMessage = "Timeout!";
                    Application.DoEvents();
                    return 0;
                }
            } while (communications.OutBuffer.Length < 1);
            //Code block added on 1 Sept by Vivek to Check NAK value for Billing Reset.
            if (communications.OutBuffer.Length >= 1)
            {
                char charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                if (charACK != 6)
                {
                    this.StatusMessage = "Access Denied";
                    return 2;
                }
            }
            return 1;
        }

        public string WriteMeterConfigurations(string meterPwd, MeterConfigurationConfigSection configSection, string data, bool isManufactureSpecific, ref string msgResult)
        {
            try
            {
                //Write Command
                int resWriteConfig = WriteConfiguration(meterPwd, configSection, isManufactureSpecific, ref data);
                if (resWriteConfig == 1)
                {
                    //msgResult = configSection.Name + " programming successful"; 
                    Application.DoEvents();
                    if (configSection.Name == "DailyLog")
                    {
                        DailyLogFlag = true;
                    }
                    return "Success";
                }
                else
                {
                    if (resWriteConfig == 0 || resWriteConfig == 2)
                    {
                        msgResult = this.StatusMessage;
                        //msgResult = "Failure in programming " + configSection.Name;
                        Application.DoEvents();
                        return null;
                    }
                    //else if (resWriteConfig == 2)
                    //{
                    //    msgResult = configSection.Name + " : Negative response from the meter";
                    //    Application.DoEvents();
                    //    return null;
                    //}
                }
                //if (configSection.Name == "DailyLog")
                //{
                //    DailyLogFlag = true;
                //}
                return null;
            }
            catch (Exception ex)
            {
                BreakCommunication();
                msgResult = StatusMessage = configSection.Name + " : Negative response from the meter";
                Application.DoEvents();
                return null;
            }
        }
        // For Deleting Daily log data
        #region 

        public bool WriteMeterCongigurationsDailyLogDelete(MeterConfigurationConfigSection configSection, string meterPwd)//Changed on 9th march 2012 as per validation report. The function now has a password string as second parameter
        {
            if (configSection.Name == "DailyLog")
            {

                WriteConfigurationDailyLog(configSection, meterPwd);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// This function deletes the daily log data from the meter.
        /// </summary>
        /// <param name="configSection"></param>
        /// <param name="meterPwd"></param>
        /// <returns>acknowledgement from the meter</returns>
        private bool WriteConfigurationDailyLog(MeterConfigurationConfigSection configSection, string meterPwd)//Changed on 9th march 2012 as per validation report. The function now has a password string as second parameter
        {
            configDeleteCommand = configSection.DeleteCommand;
            string calBcc = string.Empty;
           // following line added on 9th march 2012 as per validation report
            configDeleteCommand = configDeleteCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(meterPwd));
            calBcc = ReadoutCommon.ReturnBcc(configDeleteCommand.Substring(0, configDeleteCommand.IndexOf("Bcc")));
            configDeleteCommand = configDeleteCommand.Replace(ReadoutConstant.BCC, calBcc);
            communications.Command = configDeleteCommand;
            communications.CommandID = 3;
            communications.ReadFlag = false;
            communications.IsDataReceived = false;
            communications.OutBuffer = string.Empty;
            communications.CurrentTime = DateTime.Now;
            communications.SendCommand();
            communications.DelayExecution();
            communications.DelayExecution();
            do
            {
                if (communications.Timeout())
                {
                    this.StatusMessage = "Timeout!";
                    Application.DoEvents();
                    return false;
                }
            } while (communications.OutBuffer.Length < 1);

            if (communications.OutBuffer.Length >= 1)
            {
                char charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                if (charACK != 6)
                    this.StatusMessage = configSection.Name + " : -ve Response from meter.";
            }
            return true;
        }
        #endregion

        public bool ExecuteCommand(string cmdData)
        {
            try
            {
                communications.Command = cmdData;
                communications.CommandID = 3;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.OutBuffer = string.Empty;
                communications.CurrentTime = DateTime.Now;
                communications.SendCommand();
                communications.DelayExecution();
                //Thread.Sleep(200);
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout!";
                        Application.DoEvents();
                        return false;
                    }
                } while (communications.OutBuffer.Length < 1);
                return true;
            }
            catch (Exception ex)
            {
                BreakCommunication();
                return false;
            }
        }
    }
}
