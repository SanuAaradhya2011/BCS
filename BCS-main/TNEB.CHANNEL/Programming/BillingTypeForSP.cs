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

namespace CAB.IECChannel.Programming
{

    public class BillingTypeForSP : ReadBase
    {
        public BillingTypeForSP()
        {
            command = Command.GetInstance();
        }

        public bool UpdateBillingType(string DemandInterval)
        {

            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                Thread.Sleep(1000);
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    IsSignOnFailure = false;
                }
                communications.CurrentTime = DateTime.Now;
                if (!communications.SignOnForSPhaseIEC())
                {
                    communications.DelayExecution();
                    communications.ClosePort();
                    this.StatusMessage = "Signon failure.";
                    IsSignOnFailure = false;
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
                            return false;// "2";
                        else
                        {
                            Thread.Sleep(200);
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
                                    IsSignOnFailure = false;
                                    break; // Story - 354382 - During parameter written should break when user aborted
                                }
                                if (communications.Timeout())
                                {
                                    IsSignOnFailure = false;
                                    this.StatusMessage = "Time Out!";//MessageConstant.GetText("M000040"); 
                                    break; // Story - 354382 - During parameter written should break when timeout
                                    //return "1";
                                }
                                else
                                {
                                    if (communications.ReadFlag)
                                    {
                                        Thread.Sleep(200);
                                        passwordData = communications.OutBuffer;
                                        break;
                                    }
                                }
                            } while (true);

                            if (passwordData != string.Empty)
                            {
                                string commandPassword = command.PasswordCommandRTC_SP;
                                commandPassword = commandPassword.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));
                                commandPassword = commandPassword.Replace(ReadoutConstant.BCC, ReadoutCommon.ReturnBcc(commandPassword.Substring(2, commandPassword.Length - 5)));
                                communications.DelayExecution();
                                communications.CurrentTime = DateTime.Now;
                                communications.ReadFlag = false;
                                communications.Command = commandPassword;
                                communications.CommandID = 2;
                                communications.OutBuffer = "";
                                communications.IsDataReceived = false;
                                communications.SendCommand();

                                Thread.Sleep(200);
                                do
                                {
                                    if (communications.Timeout())
                                    {
                                        IsSignOnFailure = false;
                                        break;
                                    }
                                    if (communications.ReadFlag)
                                        break;
                                    if (IsAborted)
                                    {
                                        this.StatusMessage = "User Aborted.";
                                        Application.DoEvents();
                                        IsSignOnFailure = false;
                                        break; // Story - 354382 - During parameter written should break when user aborted
                                    }
                                } while (true);

                                if (communications.OutBuffer.Length >= 0)
                                {
                                    if (communications.OutBuffer.Contains("\x15"))
                                    {
                                        this.StatusMessage = "Access Denied.";
                                        Application.DoEvents();
                                        data = string.Empty;
                                        IsSignOnFailure = false;
                                    }
                                    if (communications.OutBuffer.Contains("\x06"))
                                    {
                                        communications.DelayExecution();
                                        communications.CurrentTime = DateTime.Now;
                                        communications.ReadFlag = false;
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
                                        communications.CommandID = 2;
                                        communications.OutBuffer = "";
                                        communications.IsDataReceived = false;
                                        communications.SendCommand();
                                        data = "";
                                        Thread.Sleep(5000);
                                        communications.CurrentTime = DateTime.Now;
                                        do
                                        {                                            
                                            //if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x03"))
                                            //{
                                            //    if (communications.OutBuffer.Contains("\x04"))
                                            //    {
                                            //    }
                                            //    else
                                            //    {
                                            //        //this.StatusMessage = "Read Completed";
                                            //        Application.DoEvents();
                                            //        data += communications.OutBuffer;
                                            //        IsSignOnFailure = true;
                                            //        break;
                                            //    }
                                            //}
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
                                            //Timeout check for break should be after check for response from Meter
                                            if (communications.Timeout())
                                            {
                                                //this.StatusMessage = MessageConstant.GetText("M000040");
                                                //Application.DoEvents();
                                                this.StatusMessage = "Timeout.";
                                                IsSignOnFailure = false;
                                                break;
                                            }
                                        } while (true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Thread.Sleep(200);
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
                Thread.Sleep(200);
            }
            return IsSignOnFailure;




            //char charACK;
            //IsSignOnFailure = false;
            //bool flag = false;
            //string data = string.Empty;
            //string rtcCommand = string.Empty;

            //try
            //{
            //    if (ConfigInfo.GetLocalMode().Equals("Optical"))
            //        communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
            //    if (!communications.OpenPort())
            //    {
            //        this.StatusMessage = "Error in opening port.";
            //        return flag;
            //    }

            //    communications.CurrentTime = DateTime.Now;
            //    if (!communications.SignOn())
            //    {
            //        this.StatusMessage = "Signon failure.";
            //        return flag;
            //    }
            //    communications.DelayExecution();
            //    communications.Command = command.MgtTampericonResetManfCommand;
            //    communications.OutBuffer = string.Empty;
            //    communications.CommandID = 2;
            //    communications.SendCommand();
            //    communications.DelayExecution();
            //    Thread.Sleep(200);
            //    if (communications.ResponseSignOn != string.Empty)
            //    {
            //        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
            //        {
            //            this.StatusMessage = "Signon failure.";
            //            return flag;
            //        }
            //    }
            //    int index = communications.ResponseSignOn.IndexOf("/");
            //    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
            //    communications.CurrentTime = DateTime.Now;

            //    communications.OutBuffer = string.Empty;
            //    rtcCommand = command.MgtTampericonResetPasswordCommand;
            //    rtcCommand = rtcCommand.Replace(ReadoutConstant.PASSWORD, ProgrammingCommon.GetASCIIValue(MeterPassword));

            //    string calculatedBcc = ReadoutCommon.ReturnBcc(rtcCommand.Substring(0, rtcCommand.Length - 7));
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
            //            this.StatusMessage = "Timeout!";
            //            Application.DoEvents();
            //            return flag;
            //        }
            //    } while (communications.OutBuffer.Length < 1);

            //    if (communications.OutBuffer.Length >= 1)
            //    {
            //        charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
            //        if (charACK == 6)
            //        {
            //            flag = true;
            //            //this.StatusMessage = "Magnetic tamper icon reset successfully.";
            //        }
            //        else
            //        {
            //            this.StatusMessage = "Access Denied.";
            //            return flag;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    new CABException(ex);
            //}
            //finally
            //{
            //    communications.Command = command.BreakCommand;
            //    communications.SendCommand();
            //    communications.DelayExecution();
            //    communications.ClosePort();
            //}
            //return flag;
        }


        #region ChangeWithCommand

        public string GetBillingType(ref string statusMsg)
        {
            IsSignOnFailure = false;
            string data = string.Empty;
            try
            {
                Thread.Sleep(1000);
                Application.DoEvents();
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate = Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    statusMsg = MessageConstant.GetText("M000038");
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
                            Thread.Sleep(200);
                            Application.DoEvents();
                            int index = communications.ResponseSignOn.IndexOf("/");
                            communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                            Thread.Sleep(200);
                            Application.DoEvents();
                            do
                            {
                                if (!IsAborted)
                                {
                                    // this.StatusMessage = MessageConstant.GetText("M000043") + communications.TotalReadBytes;
                                    Application.DoEvents();
                                }
                                else
                                {
                                    statusMsg = "User Aborted.";
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
                                        Application.DoEvents();
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
                                        statusMsg = "User Aborted.";
                                        Application.DoEvents();
                                        return string.Empty;
                                    }
                                } while (true);

                                if (communications.OutBuffer.Length >= 0)
                                {
                                    if (communications.OutBuffer.Contains("\x15"))
                                    {
                                        this.StatusMessage = "Access Denied.";
                                        Application.DoEvents();
                                        data = string.Empty;
                                    }
                                    if (communications.OutBuffer.Contains("\x06"))
                                    {
                                        communications.DelayExecution();
                                        communications.CurrentTime = DateTime.Now;
                                        communications.ReadFlag = false;
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
                                        communications.CommandID = 2;
                                        communications.OutBuffer = "";
                                        communications.IsDataReceived = false;
                                        communications.SendCommand();
                                        data = "";
                                        Thread.Sleep(500);
                                        Application.DoEvents();
                                        do
                                        {
                                            if (communications.Timeout())
                                            {
                                                this.StatusMessage = MessageConstant.GetText("M000040");
                                                Application.DoEvents();
                                                break;
                                            }
                                            if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x03"))
                                            {
                                                if (communications.OutBuffer.Contains("\x04"))
                                                {
                                                }
                                                else
                                                {
                                                    statusMsg = "Read Completed";
                                                    Application.DoEvents();
                                                    data += communications.OutBuffer;
                                                    break;
                                                }
                                            }
                                            //if (communications.OutBuffer.Length >= 0 && communications.OutBuffer.Contains("\x04"))
                                            //{
                                            //    data = data + communications.OutBuffer.Substring(0, communications.OutBuffer.Length - 1);
                                            //    communications.DelayExecution();
                                            //    communications.CurrentTime = DateTime.Now;
                                            //    communications.ReadFlag = false;
                                            //    communications.Command = "06";
                                            //    communications.CommandID = 2;
                                            //    communications.OutBuffer = "";
                                            //    communications.IsDataReceived = false;
                                            //    communications.SendCommand();
                                            //    Thread.Sleep(100);
                                            //}
                                            if (IsAborted)
                                            {
                                                statusMsg = "User Aborted.";
                                                Application.DoEvents();
                                                data = string.Empty;
                                                break;
                                            }
                                        } while (true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Thread.Sleep(200);
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
                Thread.Sleep(200);
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


    }
}
