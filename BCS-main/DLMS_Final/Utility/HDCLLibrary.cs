using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using DLMS_Final; 

namespace CAB.BCS.DLMS.Utility
{
    static class HDLCLibrary
    {

        private const string INVALIDSTARTORENDTAG = "Invalid Start or end Tag";
        private const string INVALIDHDLCFCS = "Invalid HDLC FCS";
        private const string INVALIDDESTINATIONADDRESS = "Invalid Destination Address";
        private const string INVALIDRESPONSETYPE = "Invalid Response Byte";
        private const string VALIDRESPONSETYPE = "Valid Response Byte";
        private const string BCS = "BCS";
        private const string MODEMTIMEOUT = "Modem Time Out.";

       static bool checkHDLCResponse;
       static byte MODEMIndex = 0;
       static byte[] MODEMCommand = new byte[20];
       static  string CommandResult = string.Empty;

       //private bool fCheckBCC(String strFileName)
       //{
       //    string[] lines = File.ReadAllLines(strFileName);
       //    StringBuilder sb = new StringBuilder();
       //    int count = lines.Length - 1; // except last line 
       //    int i;
       //    for (i = 0; i < count; i++)
       //    {
       //        sb.AppendLine(lines[i]);
       //    }
       //    File.WriteAllText("output.txt", sb.ToString());
       //    String temp = lines[i];
       //    if (temp == GetMD5ChecksumForFile("output.txt"))
       //        return true;
       //    else
       //        return false;
       //}

        /// <summary>
        /// Check Start/end tag, Check FCS , Check destination Address and Check command Byte
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="objProgressBar"></param>
        /// <param name="CoreUtility.ExpMessage"></param>
        /// <returns></returns>
        internal static bool CheckHDLCResponse(byte[] Buffer)
        {
            CoreUtility.GetIncrementedTimer();
            CoreUtility.ExpMessage = VALIDRESPONSETYPE;
            if (GlobalObjects.objHDLCLIB.fCheckStartEndTag(Buffer) == false)
            {
                GlobalObjects.objSerialComm.ClosePort();
                CoreUtility.ExpMessage = INVALIDSTARTORENDTAG;
                checkHDLCResponse = false;
            }
            else
            {
                if (GlobalObjects.objHDLCLIB.fCheckFCS(Buffer) == false)
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    CoreUtility.ExpMessage = INVALIDHDLCFCS;
                    checkHDLCResponse = false;
                }
                else
                {
                    if (GlobalObjects.objHDLCLIB.fCheckServerSAP(Buffer, SerialPortSettings.Default.ClientSAP) == false)
                    {
                        GlobalObjects.objSerialComm.ClosePort();
                        CoreUtility.ExpMessage = INVALIDDESTINATIONADDRESS;
                        checkHDLCResponse = false;
                    }
                    else
                    {
                        if (GlobalObjects.objHDLCLIB.fCheckCommand(Buffer, GlobalObjects.objHDLCLIB.nCMDByte) == false)
                        {
                            GlobalObjects.objSerialComm.ClosePort();
                            CoreUtility.ExpMessage = INVALIDRESPONSETYPE;
                            checkHDLCResponse = false;
                        }
                        else
                        {
                            checkHDLCResponse = true;
                        }
                    }
                }
            }
            return checkHDLCResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static string SendModemCommand(string command)
        {
            try
            {
               
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }

                MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');

                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    return MODEMTIMEOUT;
                }
                else
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch 
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        internal static string SendModemCommand(string command, string Number)
        {
            try
            {              
                MODEMIndex = 0;
                for (int i = 0; i < command.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToChar(command.Substring(i, 1)));
                }

                for (int i = 0; i < Number.Length; i++)
                {
                    MODEMCommand[MODEMIndex++] = Convert.ToByte(Convert.ToByte(Number.Substring(i, 1)) + 0x30);
                }

                MODEMCommand[MODEMIndex++] = Convert.ToByte('\r');

                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch 
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static string DisconnectCurrentCall()
        {
            try
            {               
                MODEMIndex = 0;
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                MODEMCommand[MODEMIndex++] = Convert.ToByte('+');
                if (GlobalObjects.objSerialComm.fSendDataToPort(MODEMCommand, MODEMIndex) == false)
                {
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
                else
                {                   
                    for (int i = 0; i < GlobalObjects.objSerialComm.bufferIndex; i++)
                    {
                        CommandResult = CommandResult + Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[i]);
                    }
                    return CommandResult;
                }
            }
            catch 
            {
                throw;
            }
        }

    }
}
