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

namespace CAB.IECChannel.Programming
{
    public class TOUInformation : ReadBase 
    {
        public int HighLoadThreshold {get;set;}
        public int LowLoadThreshold { get; set; }
        public int TransformerRating { get; set; }
        public int DailyParamsValue { get; set; }

        public TOUInformation()
        {
            command = Command.GetInstance();
        }
        
        public string GetTOU()
        {
            char charACK;
           
            string responseTOU = string.Empty;
            List<string> touCommands = new List<string>();
            StringBuilder touBuilder = new StringBuilder();

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port.";
                    return string.Empty;
                }

                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Sign-On failed.";
                    return string.Empty;
                }
                communications.DelayExecution();
                communications.Command = command.TOUManfCommand.Replace(ReadoutConstant.BAUDRATE, "35");
                communications.OutBuffer = string.Empty;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.CommandID = 2;
                communications.CurrentTime = DateTime.Now;
                communications.SendCommand();
                communications.DelayExecution();
               
                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = "Sign-On failed.";
                        return string.Empty;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout Error.";
                        return string.Empty;
                    }
                } while (!communications.ReadFlag);

                if (communications.OutBuffer.Length >= 4)
                {
                   // communications.OutBuffer = string.Empty;
                    
                    //string rtcCommand = command.TOUPasswordCommand;
                    //rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));
                    //string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                    //rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                    //communications.Command = rtcCommand;
                    //communications.CommandID = 2;
                    //communications.ReadFlag = false;
                    //communications.IsDataReceived = false;
                    //communications.OutBuffer = string.Empty;
                    //communications.CurrentTime = DateTime.Now;
                    //communications.SendCommand();
                    //communications.DelayExecution();
                    //do
                    //{
                    //    if (communications.Timeout())
                    //    {
                    //        this.StatusMessage = "Timeout Error.";
                    //        return string.Empty;
                    //    }
                    //} while (communications.OutBuffer.Length < 1);

                    //if (communications.OutBuffer.Length >= 1)
                    //{
                    //    charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                    //    if (charACK == 6)
                    //    {
                            //this.StatusMessage = "Reading TOU data configured in the meter. Please wait...";
                            Application.DoEvents();
                            communications.OutBuffer = string.Empty;

                            //2 march 2012: Commands now read from TODConfiguration.xml instead of command.xml as now Current TOD has to be read.

                            //touCommands = GetTOUCommands(command.TOUReadCommand);
                            //foreach (string touCommand in touCommands)

                            TODConfiguration todConfiguration = null;
                            XmlSerializer serializer = new XmlSerializer(typeof(TODConfiguration));
                            TextReader textReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TODConfiguration.xml");
                            todConfiguration = (TODConfiguration)serializer.Deserialize(textReader) as TODConfiguration;
                            ReadConfigurations readConfig = new ReadConfigurations();
                            foreach (string touCommand in readConfig.GetCurrentTODCommands(todConfiguration.CurrentReadCommand))
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
                                        this.StatusMessage = "Timeout Error.";
                                        return string.Empty;
                                    }
                                } while (!communications.ReadFlag);

                                responseTOU = communications.OutBuffer;
                                responseTOU = responseTOU.Substring(responseTOU.IndexOf("("));
                                if (ReadoutCommon.CalculateBcc(responseTOU, responseTOU.Length - 2, responseTOU.Substring(responseTOU.Length - 1, 1)))
                                {
                                    touBuilder.Append(communications.OutBuffer);
                                }
                                else
                                {
                                    this.StatusMessage = "Invalid TOU Data.";
                                    return string.Empty;
                                }
                            }
                    //    }
                    //    else
                    //    {
                    //        this.StatusMessage = "Access Denied.";
                    //        return string.Empty;
                    //    }
                    //}
                    //else
                    //{
                    //    this.StatusMessage = "Access Denied.";
                    //    return string.Empty;
                    //}
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
            return touBuilder.ToString();
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

        public bool SetTOU(List<string> touCommands)
        {
            char charACK;
            IsSignOnFailure = false;
            bool flag = false;
            string data = string.Empty;

            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = "Error in opening port.";
                    return flag;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOn())
                {
                    this.StatusMessage = "Sign-On failed.";
                    return flag;
                }
                communications.DelayExecution();
                communications.Command = command.TOUManfCommand.Replace(ReadoutConstant.BAUDRATE, "35");
                communications.OutBuffer = string.Empty;
                communications.ReadFlag = false;
                communications.IsDataReceived = false;
                communications.CommandID = 2;
                communications.CurrentTime = DateTime.Now;
                communications.SendCommand();
                communications.DelayExecution();

                if (communications.ResponseSignOn != string.Empty)
                {
                    if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                    {
                        this.StatusMessage = "Sign-On failed.";
                        return flag;
                    }
                }
                int index = communications.ResponseSignOn.IndexOf("/");
                communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                communications.CurrentTime = DateTime.Now;
                do
                {
                    if (communications.Timeout())
                    {
                        this.StatusMessage = "Timeout Error.";
                        Application.DoEvents();
                        return flag;
                    }
                } while (communications.OutBuffer.Length < 5);


                communications.OutBuffer = string.Empty;

                string rtcCommand = command.TOUPasswordCommand;
                rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));
                string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                communications.Command = rtcCommand;
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
                        return flag;
                    }
                } while (communications.OutBuffer.Length < 1);

                charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                if (charACK == 6)
                {
                   // this.StatusMessage = "Programing TOU data in the meter. Please wait...";
                    Application.DoEvents();
                    communications.OutBuffer = string.Empty;
                    foreach (string touCommand in touCommands)
                    {
                        //rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(string.Concat(HighLoadThreshold.ToString(),LowLoadThreshold.ToString(),TransformerRating.ToString())));
                        //calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                        calculatedBcc = ReadoutCommon.ReturnBcc(touCommand.Substring(2));
                        //rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                        communications.Command = string.Concat(touCommand, calculatedBcc);
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
                                return flag;
                            }
                        } while (communications.OutBuffer.Length < 1);

                        charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                        if (charACK != 6)
                        {
                            this.StatusMessage = "Access Denied";
                            return flag;
                        }
                    }
                }
                else
                {
                    this.StatusMessage = "Access Denied.";
                    return flag;
                }
                ////////////////////////////////////////////
                //    communications.OutBuffer = string.Empty;
                //    rtcCommand = command.DTMLogWriteCommand;
                //    rtcCommand = rtcCommand.Replace(ReadoutConstant.DATA, ProgrammingCommon.GetASCIIValue(DailyParamsValue.ToString("X4")));
                //    calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(2, rtcCommand.Length - 5));
                //    rtcCommand = rtcCommand.Replace(ReadoutConstant.BCC, calculatedBcc);
                //    communications.Command = rtcCommand;
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
                //            this.StatusMessage = "Timeout Error.";
                //            Application.DoEvents();
                //            return flag;
                //        }
                //    } while (communications.OutBuffer.Length < 1);

                //    if (communications.OutBuffer.Length >= 1)
                //    {
                //        charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
                //        if (charACK == 6)
                //        {
                //            this.StatusMessage = "TOU set successfully.";
                //        }
                //        else if (charACK == 21)
                //        {
                //            this.StatusMessage = "Access Denied.";
                //        }
                //    }
                flag = true;
            }
            catch (Exception ex)
            {
                throw new CABException(ex);
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
            return flag;
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
    }
}




