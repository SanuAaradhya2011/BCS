using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;
using Utilities;

namespace CAB.Framework
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
                EventLogging.CallLogDetails("Command File is missing. Please contact to Vendor.");
                doc = null;
            }
        }


        #region COM Port Settings

        public int DataBit
        {
            get
            {
                //text = (doc.GetElementsByTagName("COMPortSettings"))[0].ChildNodes.Item(3).InnerText;
                //return Int32.Parse(text);
                return 7;
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

                //string strBreak = "";
                //XmlNodeList Plist, IList;
                //Plist = doc.GetElementsByTagName("BreakCommand");
                //IList = Plist[0].ChildNodes;
                //strBreak = Plist.Item(0).InnerText;
                //return strBreak;
                return "0142300371";
            }
        }

        public string SignOnCommand
        {
            get
            {
                //text = FindUniqueCommand("SignOnCommand", 0);  
                //return string.Concat(text.Substring(0, 4), text.Substring(11, 6));
                return "2F3f210D0A";
            }
        }

        public string ManufactureCommand
        {
            get
            {
                //XmlNodeList Plist;
                //Plist = doc.GetElementsByTagName("ManSpfCommand");
                //text = Plist.Item(0).InnerText;
                //return string.Concat( text.Substring(0, 4),"35", text.Substring(12, 6));
                return "063035360D0A";
            }
        }

        public string LCommand
        {
            get
            {
                //text = FindUniqueCommand("LCommand", 0);  
                //return string.Concat(text.Substring(0, 22), CalculateBcc(text.Substring(0, 22)), text.Substring(25, 4));
                return "3A30303030303030304d4c3b0D0A";
            }
        }

        public string ZCommand
        {
            get
            {
                //text = FindUniqueCommand("ZCommand", 0);  
                //return string.Concat(text.Substring(0, 22) + CommandDate + CalculateBcc(text.Substring(0, 22) + CommandDate) + text.Substring(32, 4));
                return "3A30303030303030304d5a2d0D0A";
            }
        }

        public string BCommand
        {
            get
            {
                //text = FindUniqueCommand("BCommand", 0);  
                //return string.Concat(text.Substring(0, 22), CalculateBcc(text.Substring(0, 22)), text.Substring(25, 4));
                return "3A30303030303030304d42350D0A";

            }
        }

        public string VCommand
        {
            get
            {
                //text = FindUniqueCommand("VCommand", 0);  
                //return string.Concat(text.Substring(0, 22), TamperEvent, CalculateBcc(text.Substring(0, 22) + TamperEvent), text.Substring((text.Length - 4), 4));
                return "3A30303030303030304d56210D0A";
            }
        }

        public string ChangeMeterPasswordCommand
        {
            get
            {
                //return   FindUniqueCommand("ChangeMeterPassword", 0);  
                return "015731023030373128NewPassword2903Bcc";
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
                //text = FindUniqueCommand("ReadMeterData", 0); 
                //return string.Concat(text.Substring(0, 4), "35", text.Substring(12, 6));
                return "063035300D0A";
            }
        }

        #endregion

        #region Local Communication Command

        public int LocalCommunicationCommandTimeout
        {
            get
            {
                //text = FindUniqueCommand("CommandTimeout", 0) ;
                //return Convert.ToInt32(text);
                return 2200;
            }

        }
        public int LocalCommunicationInterChatracterDelay
        {
            get
            {
                //text = FindUniqueCommand("InterCharacterTimeout", 0) ;
                //return Convert.ToInt32(text);
                return 1200;
            }

        }
        public int LocalCommunicationInterCommandDelay
        {
            get
            {
                //text = FindUniqueCommand("IntercommandDelay", 0) ;
                //return Convert.ToInt32(text);
                return 200;
            }
        }

        #endregion

        #region GSM Command
        public int GSMCommandTimeout
        {
            get
            {
                //text = FindUniqueCommand("GSMCommandTimeout",0) ;
                //return Convert.ToInt32(text);
                return 4000;
            }

        }
        public int GSMInterCharacterTimeout
        {
            get
            {
                //text = FindUniqueCommand("GSMInterCharacterTimeout", 0);
                //return Convert.ToInt32(text);
                return 6000;
            }

        }
        public int GSMATCommandTimeout
        {
            get
            {
                //text = FindUniqueCommand("GSMATCommandTimeout", 0);
                //return Convert.ToInt32(text);
                return 10000;
            }

        }
        public int GSMATInterCharacterTimeout
        {
            get
            {
                //text = FindUniqueCommand("GSMATInterCharacterTimeout", 0);
                //return Convert.ToInt32(text);
                return 5000;
            }

        }
        public int GSMConnectTimeout
        {
            get
            {
                //text = FindUniqueCommand("GSMConnectTimeout", 0) ;
                //return Convert.ToInt32(text);
                return 60000;
            }

        }
        public int GSMConnectInterCharacterTimeout
        {
            get
            {
                //text = FindUniqueCommand("GSMConnectInterCharacterTimeout", 0) ;
                //return Convert.ToInt32(text);
                return 30000;
            }

        }
        #endregion

        #region CMRI Command

        public string CMRIPortName
        {
            get
            {
                //return FindUniqueCommand("CMRIPortSettings",0);
                return "COM1";
            }
        }
        public string CMRIBaudRate
        {
            get
            {
                //return FindUniqueCommand("CMRIPortSettings",1);
                return "19200";
            }
        }
        public string CMRIParity
        {
            get
            {
                //return FindUniqueCommand("CMRIPortSettings",2);
                return "Even";
            }
        }
        public string CMRIDataBit
        {
            get
            {
                //return FindUniqueCommand("CMRIPortSettings",3) ;
                return "7";
            }
        }
        public string CMRIStopBit
        {
            get
            {
                //return FindUniqueCommand("CMRIPortSettings",4) ;
                return "1";
            }
        }

        public int CMRIWaitTimeout
        {
            get
            {
                //text = FindUniqueCommand("CMRIWaitTimeout",0);
                //return Convert.ToInt32(text);
                return 5000;
            }
        }

        public int CMRIPktTimeout
        {
            get
            {
                //return CMRIWaitTimeout;
                return 5000;
            }
        }

        #endregion

        # region TOU

        public string TOUManfCommand
        {
            get
            {
                //return FindCommand("ReadTOU", 0);
                return "0630BaudRate310D0A";
            }
        }
        public string TOUPasswordCommand
        {
            get
            {
                //string strPCommand = "";
                //XmlNodeList Plist, IList, JList;
                //Plist = doc.GetElementsByTagName("ReadTOU");
                //IList = Plist[0].ChildNodes;
                //JList = IList[1].ChildNodes;
                //strPCommand = JList.Item(0).InnerText;
                //return strPCommand;
                return "0150310228Pass4d2903Bcc";
                //return FindCommand("ReadTOU", 0);
            }
        }
        public string TOUReadCommand
        {
            get
            {
                //return FindCommand("ReadTOU", 2);
                return "\"0152310230333130283230290363\"\"0152310230333131283230290362\"\"0152310230333132283230290361\"\"0152310230333133283230290360\"\"0152310230333134283230290367\"\"0152310230333135283230290366\"\"0152310230333136283037290360\"\"0152310230333230283230290360\"\"0152310230333231283230290361\"\"0152310230333232283230290362\"\"0152310230333233283230290363\"\"0152310230333234283230290364\"\"0152310230333235283230290365\"\"0152310230333236283037290363\"\"0152310230333330283230290361\"\"0152310230333331283230290360\"\"0152310230333332283230290363\"\"0152310230333333283230290362\"\"0152310230333334283230290365\"\"0152310230333335283230290364\"\"0152310230333336283037290362\"\"0152310230333430283230290366\"\"0152310230333431283230290367\"\"0152310230333432283230290364\"\"0152310230333433283230290365\"\"0152310230333434283230290362\"\"0152310230333435283230290363\"\"0152310230333436283037290365\"\"0152310230333530283233290364\"\"0152310230333531283233290365\"\"0152310230333532283233290366\"\"0152310230333533283233290367\"\"0152310230333534283233290360\"\"0152310230333535283233290361\"\"0152310230333536283233290362\"\"0152310230333537283233290363\"\"015231023033353828323329036C\"\"015231023033353928323329036D\"\"0152310230333630283046290310\"";
            }
        }
        public string RTCManfCommand
        {
            get
            {
                //return FindConcatinateCommand("UpdateRTC");
                return "063035360D0A";
            }
        }
        public string RTCPasswordCommand
        {
            get
            {
                //return FindCommand("UpdateRTC", 1);
                return "3APass4d4330373031DataBcc0D0A";
            }
        }
        public string ReadRTCManfCommand
        {
            get
            {
                //return FindConcatinateCommand("ReadRTC");
                return "063035360D0A";
            }
        }
        public string ReadRTCPasswordCommand
        {
            get
            {
                //return FindCommand("ReadRTC", 1);
                return "3APass4d410D0A";
            }
        }
        public string MDManfCommand
        {
            get
            {
                //return FindConcatinateCommand("MDReset");
                return "063035360D0A";
            }
        }
        public string MDPasswordCommand
        {
            get
            {
                //return FindCommand("MDReset", 1);
                return "3APass4d53Bcc0D0A";
            }
        }
        public string BillingDataManfCommand
        {
            get
            {
                // return FindConcatinateCommand("BillingDataReset");
                return "063035360D0A";
            }
        }
        public string BillingDataPasswordCommand
        {
            get
            {
                //return FindCommand("BillingDataReset", 1);
                return "3APass4d54Bcc0D0A";
            }
        }
        public string TamperResetManfCommand
        {
            get
            {
                //return FindConcatinateCommand("TamperReset");
                return "063035360D0A";
            }
        }
        public string TamperResetPasswordCommand
        {
            get
            {
                //return FindCommand("BillingDataReset", 1);
                return "3APass4d54Bcc0D0A";
            }
        }
        public string MgtTampericonResetManfCommand
        {
            get
            {
                //return FindConcatinateCommand("MgtTamperIconReset");
                return "063035360D0A";
            }
        }
        public string MgtTampericonResetPasswordCommand
        {
            get
            {
                //return FindCommand("MgtTamperIconReset", 1);
                return "3APass4d49Bcc0D0A";
            }
        }
        public string EnergyResetManfCommand
        {
            get
            {
                //return FindConcatinateCommand("EnergyReset");
                return "063035360D0A";
            }
        }
        public string EnergyResetPasswordCommand
        {
            get
            {
                //return FindCommand("EnergyReset", 1);
                return "3APass4d52Bcc0D0A";
            }
        }
        public string BillingEnergyResetManfCommand
        {
            get
            {
                //return FindConcatinateCommand("BillingEnergyReset");
                return "063035360D0A";
            }
        }
        public string BillingEnergyResetPasswordCommand
        {
            get
            {
                //return FindCommand("BillingEnergyReset", 1);
                return "3APass4d63Bcc0D0A";
            }
        }
        public string CTResetManfCommand
        {
            get
            {
                //return FindConcatinateCommand("PrimaryCTRatio");
                return "063035310D0A";
            }
        }
        public string CTResetPasswordCommand
        {
            get
            {
                //return FindCommand("PrimaryCTRatio", 1);
                return "0150310228Pass4d2903Bcc";
            }
        }
        public string CTWriteCommand
        {
            get
            {
                //return FindCommand("PrimaryCTRatio", 2);
                return "015731023036343028Data45362903Bcc";
            }
        }
        public string LPRParaManfCommand
        {
            get
            {
                //return FindConcatinateCommand("LPRParameters");
                return "063035310D0A";
            }
        }
        public string LPRParaPasswordCommand
        {
            get
            {
                //return FindCommand("LPRParameters", 1);
                return "0150310228Pass4d2903Bcc";
            }
        }
        public string LPRParaWriteCommand
        {
            get
            {
                //return FindCommand("LPRParameters", 2);
                return "015731023035313428Data2903Bcc";
            }
        }
        public string DTMLogManfCommand
        {
            get
            {
                //return FindConcatinateCommand("DTMDailyLog");
                return "063035310D0A";
            }
        }
        public string DTMLogPasswordCommand
        {
            get
            {
                //return FindCommand("DTMDailyLog", 1);
                return "0150310228Pass4d2903Bcc";
            }
        }
        public string DTMLogWriteCommand
        {
            get
            {
                //return FindCommand("DTMDailyLog", 2);
                return "015731023030363828Data2903Bcc";
            }
        }

        public string TamperStatusManfCommand
        {
            get
            {
                //string data = "";
                //XmlNodeList Plist, IList, JList;
                //Plist = doc.GetElementsByTagName("TamperStatus");
                //IList = Plist[0].ChildNodes;
                //JList = IList[0].ChildNodes;
                //data = IList.Item(0).InnerText;
                //return string.Concat(data.Substring(0, 4), "35", data.Substring(12, 6));
                return "063035360D0A";
            }
        }
        public string TamperStatusCommand
        {
            get
            {
                //return FindCommand("TamperStatus", 1);
                return "3APass4d3eBcc0D0A";
            }
        }

        public string FraudEnergyManfCommand
        {
            get
            {
                //string commandText = "";
                //XmlNodeList Plist, IList, JList;
                //Plist = doc.GetElementsByTagName("FraudEnergy");
                //IList = Plist[0].ChildNodes;
                //JList = IList[0].ChildNodes;
                //commandText = IList.Item(0).InnerText;
                //return string.Concat(commandText.Substring(0, 4),"35", commandText.Substring(12, 6));
                return "063035360D0A";
            }
        }
        public string ReadFraudEnergyCommand
        {
            get
            {
                //XmlNodeList Plist, IList, JList;
                //Plist = doc.GetElementsByTagName("FraudEnergy");
                //IList = Plist[0].ChildNodes;
                //JList = IList[1].ChildNodes;
                //return JList.Item(0).InnerText;
                return "3APass4d6CBcc0D0A";
            }
        }
        public string ReverseEnergyManfCommand
        {
            get
            {
                //XmlNodeList Plist, IList, JList;
                //Plist = doc.GetElementsByTagName("ReverseEnergy");
                //IList = Plist[0].ChildNodes;
                //JList = IList[0].ChildNodes;
                //string command = IList.Item(0).InnerText; 
                //return string.Concat(command.Substring(0, 4) , "35" , command.Substring(12, 6));
                return "063035360D0A";
            }
        }
        public string ReadReverseEnergyCommand
        {
            get
            {
                //return FindCommand("ReverseEnergy", 1);
                return "3APass4d3DBcc0D0A";
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
        public string DTMLoadLManfCommand
        {
            get
            {
                return FindConcatinateCommand("DTMLoadL");
            }
        }
        public string DTMLoadLPasswordCommand
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
    }
}