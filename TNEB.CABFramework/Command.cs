using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace CAB.IECFramework
{
    public class Command
    {
        private static Command command;
        private XmlDocument doc;
        private string text = string.Empty;
        static readonly object padlock = new object();
        public static Command GetInstance()
        {
            lock (padlock)
            {
                if (command == null)
                    command = new Command();
                return command;
            }
        }
        public Command()
        {
            try
            {
                command = null;
                doc = new XmlDocument();
                string path = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\", "Commands.xml");
                doc.Load(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Command File is missing. Please contact to Vendor.");
                doc = null;
            }
        }


        #region COM Port Settings

        public int DataBit
        {
            get
            {
                text = (doc.GetElementsByTagName("COMPortSettings"))[0].ChildNodes.Item(3).InnerText;
                return Int32.Parse(text);
            }
        }

        public StopBits StopBit
        {
            get
            {
                text = (doc.GetElementsByTagName("COMPortSettings"))[0].ChildNodes.Item(4).InnerText;
                return (StopBits)Enum.Parse(typeof(StopBits), text);
            }
        }

        public Parity Parity
        {
            get
            {
                text = (doc.GetElementsByTagName("COMPortSettings"))[0].ChildNodes.Item(2).InnerText;
                return (Parity)Enum.Parse(typeof(Parity), text);
            }
        }

        #endregion

        #region General Command

        public string BreakCommand
        {
            get
            {
                //return FindUniqueCommand("BreakCommand", 0);  
                string strBreak = "";
                XmlNodeList Plist, IList;
                Plist = doc.GetElementsByTagName("BreakCommand");
                IList = Plist[0].ChildNodes;
                strBreak = Plist.Item(0).InnerText;
                return strBreak;
            }
        }

        public string SignOnCommand
        {
            get
            {
                text = FindUniqueCommand("SignOnCommand", 0);
                return string.Concat(text.Substring(0, 4), text.Substring(11, 6));
            }
        }

        public string ManufactureCommand
        {
            get
            {
                XmlNodeList Plist;
                Plist = doc.GetElementsByTagName("ManSpfCommand");
                text = Plist.Item(0).InnerText;
                return string.Concat(text.Substring(0, 4), "35", text.Substring(12, 6));
            }
        }

        public string LCommand
        {
            get
            {
                text = FindUniqueCommand("LCommand", 0);
                return string.Concat(text.Substring(0, 22), CalculateBcc(text.Substring(0, 22)), text.Substring(25, 4));
            }
        }

        public string ZCommand
        {
            get
            {
                text = FindUniqueCommand("ZCommand", 0);
                return string.Concat(text.Substring(0, 22) + CommandDate + CalculateBcc(text.Substring(0, 22) + CommandDate) + text.Substring(32, 4));
            }
        }

        public string BCommand
        {
            get
            {
                text = FindUniqueCommand("BCommand", 0);
                return string.Concat(text.Substring(0, 22), CalculateBcc(text.Substring(0, 22)), text.Substring(25, 4));

            }
        }

        public string VCommand
        {
            get
            {
                text = FindUniqueCommand("VCommand", 0);
                return string.Concat(text.Substring(0, 22), TamperEvent, CalculateBcc(text.Substring(0, 22) + TamperEvent), text.Substring((text.Length - 4), 4));
            }
        }

        public string ChangeMeterPasswordCommand
        {
            get
            {
                return FindUniqueCommand("ChangeMeterPassword", 0);
            }
        }

        public string CommandDate
        {
            get;
            set;
        }

        public string TamperEvent
        {
            get;
            set;
        }

        private string CalculateBcc(string bccText)
        {
            int bcc = 0;
            string bcc1 = string.Empty;
            byte[] comBuffer = new byte[bccText.Length / 2];
            for (int i = 0; i < bccText.Length; i += 2)
                comBuffer[i / 2] = (byte)Convert.ToByte(bccText.Substring(i, 2), 16);
            for (int j = 0; j < comBuffer.Length; j++)
                bcc = bcc ^ comBuffer[j];
            bcc1 = (Convert.ToString(bcc, 16).PadLeft(2, '0').PadRight(2, ' '));
            return bcc1;
        }

        public string ReadoutCommand
        {
            get
            {
                text = FindUniqueCommand("ReadMeterData", 0);
                return string.Concat(text.Substring(0, 4), "35", text.Substring(12, 6));
            }
        }

        #endregion

        #region Local Communication Command

        public int LocalCommunicationCommandTimeout
        {
            get
            {
                text = FindUniqueCommand("CommandTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        public int LocalCommunicationInterChatracterDelay
        {
            get
            {
                text = FindUniqueCommand("InterCharacterTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        public int LocalCommunicationInterCommandDelay
        {
            get
            {
                text = FindUniqueCommand("IntercommandDelay", 0);
                return Convert.ToInt32(text);
            }
        }

        #endregion

        #region GSM Command
        public int GSMCommandTimeout
        {
            get
            {
                text = FindUniqueCommand("GSMCommandTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        public int GSMInterCharacterTimeout
        {
            get
            {
                text = FindUniqueCommand("GSMInterCharacterTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        public int GSMATCommandTimeout
        {
            get
            {
                text = FindUniqueCommand("GSMATCommandTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        public int GSMATInterCharacterTimeout
        {
            get
            {
                text = FindUniqueCommand("GSMATInterCharacterTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        public int GSMConnectTimeout
        {
            get
            {
                text = FindUniqueCommand("GSMConnectTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        public int GSMConnectInterCharacterTimeout
        {
            get
            {
                text = FindUniqueCommand("GSMConnectInterCharacterTimeout", 0);
                return Convert.ToInt32(text);
            }

        }
        #endregion

        #region CMRI Command

        public string CMRIPortName
        {
            get
            {
                return FindUniqueCommand("CMRIPortSettings", 0);
            }
        }
        public string CMRIBaudRate
        {
            get
            {
                return FindUniqueCommand("CMRIPortSettings", 1);
            }
        }
        public string CMRIParity
        {
            get
            {
                return FindUniqueCommand("CMRIPortSettings", 2);
            }
        }
        public string CMRIDataBit
        {
            get
            {
                return FindUniqueCommand("CMRIPortSettings", 3);
            }
        }
        public string CMRIStopBit
        {
            get
            {
                return FindUniqueCommand("CMRIPortSettings", 4);
            }
        }

        public int CMRIWaitTimeout
        {
            get
            {
                text = FindUniqueCommand("CMRIWaitTimeout", 0);
                return Convert.ToInt32(text);
            }
        }

        public int CMRIPktTimeout
        {
            get
            {
                return CMRIWaitTimeout;
            }
        }

        #endregion

        # region TOU

        public string TOUManfCommand
        {
            get
            {
                return FindCommand("ReadTOU", 0);
            }
        }
        public string TOUPasswordCommand
        {
            get
            {
                string strPCommand = "";
                XmlNodeList Plist, IList, JList;
                Plist = doc.GetElementsByTagName("ReadTOU");
                IList = Plist[0].ChildNodes;
                JList = IList[1].ChildNodes;
                strPCommand = JList.Item(0).InnerText;
                return strPCommand;
                //return FindCommand("ReadTOU", 0);
            }
        }
        public string TOUReadCommand
        {
            get
            {
                return FindCommand("ReadTOU", 2);
            }
        }
        public string RTCManfCommand
        {
            get
            {
                return FindConcatinateCommand("UpdateRTC");
            }
        }
        public string RTCPasswordCommand
        {
            get
            {
                return FindCommand("UpdateRTC", 1);
            }
        }
        public string ReadRTCManfCommand
        {
            get
            {
                return FindConcatinateCommand("ReadRTC");
            }
        }
        public string ReadRTCPasswordCommand
        {
            get
            {
                return FindCommand("ReadRTC", 1);
            }
        }
        public string MDManfCommand
        {
            get
            {
                return FindConcatinateCommand("MDReset");
            }
        }
        public string MDPasswordCommand
        {
            get
            {
                return FindCommand("MDReset", 1);
            }
        }
        public string BillingDataManfCommand
        {
            get
            {
                return FindConcatinateCommand("BillingDataReset");
            }
        }
        public string BillingDataPasswordCommand
        {
            get
            {
                return FindCommand("BillingDataReset", 1);
            }
        }
        public string TamperResetManfCommand
        {
            get
            {
                return FindConcatinateCommand("TamperReset");
            }
        }
        public string TamperResetPasswordCommand
        {
            get
            {
                return FindCommand("BillingDataReset", 1);
            }
        }
        public string MgtTampericonResetManfCommand
        {
            get
            {
                return FindConcatinateCommand("MgtTamperIconReset");
            }
        }
        public string MgtTampericonResetPasswordCommand
        {
            get
            {
                return FindCommand("MgtTamperIconReset", 1);
            }
        }
        public string EnergyResetManfCommand
        {
            get
            {
                return FindConcatinateCommand("EnergyReset");
            }
        }
        public string EnergyResetPasswordCommand
        {
            get
            {
                return FindCommand("EnergyReset", 1);
            }
        }
        public string BillingEnergyResetManfCommand
        {
            get
            {
                return FindConcatinateCommand("BillingEnergyReset");
            }
        }
        public string BillingEnergyResetPasswordCommand
        {
            get
            {
                return FindCommand("BillingEnergyReset", 1);
            }
        }
        public string CTResetManfCommand
        {
            get
            {
                return FindConcatinateCommand("PrimaryCTRatio");
            }
        }
        public string CTResetPasswordCommand
        {
            get
            {
                return FindCommand("PrimaryCTRatio", 1);
            }
        }
        public string CTWriteCommand
        {
            get
            {
                return FindCommand("PrimaryCTRatio", 2);
            }
        }
        public string LPRParaManfCommand
        {
            get
            {
                return FindConcatinateCommand("LPRParameters");
            }
        }
        public string LPRParaPasswordCommand
        {
            get
            {
                return FindCommand("LPRParameters", 1);
            }
        }
        public string LPRParaWriteCommand
        {
            get
            {
                return FindCommand("LPRParameters", 2);
            }
        }
        public string DTMLogManfCommand
        {
            get
            {
                return FindConcatinateCommand("DTMDailyLog");
            }
        }
        public string DTMLogPasswordCommand
        {
            get
            {
                return FindCommand("DTMDailyLog", 1);
            }
        }
        public string DTMLogWriteCommand
        {
            get
            {
                return FindCommand("DTMDailyLog", 2);
            }
        }


        //added on 2nd march 2012 to reset the daily log data
        public string DTMLogResetCommand
        {
            get
            {
                XmlNodeList Plist, IList, JList = null;
                try
                {
                    Plist = doc.GetElementsByTagName("DTMDailyLogReset");
                    IList = Plist[0].ChildNodes;
                    JList = IList[2].ChildNodes;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
                return JList.Item(0).InnerText;
            }
        }
        //added on 2nd march 2012 to reset the daily log data

        public string HeaderInfoCommand
        {
            get
            {
                return FindCommand("MeterHeaderInfo", 1);
            }
        }

        public string TamperStatusManfCommand
        {
            get
            {
                string data = "";
                XmlNodeList Plist, IList, JList;
                Plist = doc.GetElementsByTagName("TamperStatus");
                IList = Plist[0].ChildNodes;
                JList = IList[0].ChildNodes;
                data = IList.Item(0).InnerText;
                return string.Concat(data.Substring(0, 4), "35", data.Substring(12, 6));
            }
        }
        public string TamperStatusCommand
        {
            get
            {
                return FindCommand("TamperStatus", 1);
            }
        }

        public string FraudEnergyManfCommand
        {
            get
            {
                string commandText = "";
                XmlNodeList Plist, IList, JList;
                Plist = doc.GetElementsByTagName("FraudEnergy");
                IList = Plist[0].ChildNodes;
                JList = IList[0].ChildNodes;
                commandText = IList.Item(0).InnerText;
                return string.Concat(commandText.Substring(0, 4), "35", commandText.Substring(12, 6));
            }
        }
        public string ReadFraudEnergyCommand
        {
            get
            {
                XmlNodeList Plist, IList, JList;
                Plist = doc.GetElementsByTagName("FraudEnergy");
                IList = Plist[0].ChildNodes;
                JList = IList[1].ChildNodes;
                return JList.Item(0).InnerText;
            }
        }
        public string ReverseEnergyManfCommand
        {
            get
            {
                XmlNodeList Plist, IList, JList;
                Plist = doc.GetElementsByTagName("ReverseEnergy");
                IList = Plist[0].ChildNodes;
                JList = IList[0].ChildNodes;
                string command = IList.Item(0).InnerText;
                return string.Concat(command.Substring(0, 4), "35", command.Substring(12, 6));
            }
        }
        public string ReadReverseEnergyCommand
        {
            get
            {
                return FindCommand("ReverseEnergy", 1);
            }
        }
        public string MeterProgManfCommand
        {
            get
            {
                return FindConcatinateCommand("ProgrammingTimes");
            }
        }
        public string MeterProgCommand
        {
            get
            {
                return FindCommand("ProgrammingTimes", 1);
            }
        }

        public string RTCUpdateManfCommand
        {
            get
            {
                XmlNodeList Plist, IList, JList;
                Plist = doc.GetElementsByTagName("RTCUpdateTime");
                IList = Plist[0].ChildNodes;
                JList = IList[0].ChildNodes;
                string strManCommand = IList.Item(0).InnerText;
                return string.Concat(strManCommand.Substring(0, 4), "35", strManCommand.Substring(12, 6));
            }
        }

        public string ReadRTCUpdateCommand
        {
            get
            {
                return FindCommand("RTCUpdateTime", 1);
            }
        }
        public string PhasorManfCommand
        {
            get
            {
                return FindConcatinateCommand("ReadPhasor");
            }
        }
        public string PhasorPasswordCommand
        {
            get
            {
                return FindCommand("ReadPhasor", 1);
            }
        }
        public string DTMLoadManfCommand
        {
            get
            {
                return FindConcatinateCommand("ReadDTMLoad");
            }
        }
        public string DTMLoadPasswordCommand
        {
            get
            {
                return FindCommand("ReadDTMLoad", 1);
            }
        }
        public string DTMParaManfCommand
        {
            get
            {
                return FindConcatinateCommand("ReadDTMParameter");
            }
        }
        public string DTMDailySurveyCommand_A
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("DailySurveySP", 0);
            }
        }
        public string PasswordCommandRTC_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("UpdateRTCSP", 0);
            }
        }
        public string BillingEnergyReset_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("BillingEnergyResetSP", 0);
            }
        }
        public string TamperConfig_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperConfigWriteSP", 0);
            }
        }
        public string TamperIconReset_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("MgtTamperIconResetSP", 0);
            }
        }
        public string DIPReset_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("DIPResetSP", 0);
            }
        }
        public string BillingType_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("BillingTypeResetSP", 0);
            }
        }

        // TPDDL Starlight 1P IEC Command for Billing Type//User Story 478249. Starlight Billing Type Command implemented
        public string StarLightBillingType_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("StarLightBillingTypeResetSP", 0);
            }
        }

        public string CommandUpdateRTC_SP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("UpdateRTCSP", 1);
            }
        }
        //public string TamperCommand
        //{
        //    get
        //    {
        //        return FindConcatinateCommand_A_ForSP("DailySurveySP", 0);
        //    }
        //}      
        public string ReadRTCCommandForSP
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("ReadRTCSP", 0);
            }
        }
        public string DTMDailySurveyCommand_B
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("DailySurveySP", 1);
            }
        }
        public string LoadSurveyLCommand
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("LoadSurveySP", 1);
            }
        }
        public string FTamperLCommand // Magnet Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 1);
            }
        }
        public string STamperLCommand // Earth Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 2);
            }
        }
        public string TTamperLCommand // Single wire Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 3);
            }
        }
        public string FourthTamperLCommand // Neutral Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 4);
            }
        }
        public string FifthTamperLCommand //ESD Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 5);
            }
        }
        public string SixthTamperLCommand //Reverse Tamper 
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 6);
            }
        }
        public string SeventhTamperLCommand //Low PF Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 7);
            }
        }
        public string EightTamperLCommand //Low Voltage Tamper  
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 8);
            }
        }

        public string NinthTamperLCommand //Over Load Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 9);
            }
        }

        public string TenthTamperLCommand //Cover Open Tamper
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 10);
            }
        }

        public string LoadSurveyZCommand
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("LoadSurveySP", 0);
            }
        }
        public string TamperTCommand
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("TamperSP", 0);
            }
        }    
        public string DTMParaPasswordCommand
        {
            get
            {
                return FindCommand("ReadDTMParameter", 1);
            }
        }
        public string DTMDayManfCommand
        {
            get
            {
                return FindConcatinateCommand("ReadDTMDay");
            }
        }
        public string DTMDayPasswordCommand
        {
            get
            {
                return FindCommand("ReadDTMDay", 1);
            }
        }
        public string DTMNonTNEBDayPasswordCommand
        {
            get
            {
                return FindCommand("ReadDTMDay", 2);
            }
        }
        public string DTMLoadLManfCommand
        {
            get
            {
                return FindConcatinateCommand("DTMLoadL");
            }
        }
        public string LoadLPasswordCommand
        {
            get
            {
                return FindCommand("DTMLoadL", 1);
            }
        }
        public string CMRIReadOutCommand
        {
            get
            {
                return FindCommand("rdout", 1);
            }
        }
        public string DailySurveyConfigurationCommand
        {
            get
            {
                return FindConcatinateCommand_A_ForSP("DailySurveySP", 2);                
            }
        }


        # endregion

        private string FindUniqueCommand(string CommandName, int index)
        {
            XmlNodeList Plist, IList;
            Plist = doc.GetElementsByTagName(CommandName);
            IList = Plist[0].ChildNodes;
            return IList.Item(index).InnerText;
        }
        private string FindCommand(string CommandName, int index)
        {
            XmlNodeList Plist, IList, JList;
            Plist = doc.GetElementsByTagName(CommandName);
            IList = Plist[0].ChildNodes;
            JList = IList[index].ChildNodes;
            return JList.Item(0).InnerText;
        }
        private string FindConcatinateCommand(string CommandName)
        {
            text = FindCommand(CommandName, 0);
            return string.Concat(text.Substring(0, 4), "35", text.Substring(12, 6));
        }
        private string FindConcatinateCommand_A_ForSP(string CommandName, int index)
        {
            text = FindCommand_A_ForSP(CommandName, index);
            return text;//string.Concat(text.Substring(0, 4), "35", text.Substring(12, 6));
        }
        private string FindCommand_A_ForSP(string CommandName, int index)
        {
            XmlNodeList Plist, IList;//, JList;
            Plist = doc.GetElementsByTagName(CommandName);
            IList = Plist[0].ChildNodes;
            //JList = IList[index].ChildNodes;
            return IList.Item(index).InnerText;
        }

    }
}