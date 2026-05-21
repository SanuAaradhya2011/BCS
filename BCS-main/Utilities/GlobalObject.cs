using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialCommunication;
using System.Windows.Forms;
using DLMSLIB;

namespace Utilities
{
    public static class GlobalObjects
    {
        public  static SerialComm objSerialComm = new SerialComm();
        public static HDLCLIB objHDLCLIB = new HDLCLIB();
        public static COSEMLIB objCOSEMLIB = new COSEMLIB();
        public static GlobalFunctions objGlobalFunctions = new GlobalFunctions();
    }
    public class GlobalFunctions
    {
        byte clientSecuritymechanism = 0x00;
        string clientSystemTitle;
        string randamPlanText = string.Empty;
        byte[] ConfBlock = new byte[7];
        int ConfBlockCounter = 0;
        byte[] PDUSize = new byte[2];
        int PDUSizeCounter = 0;
        byte[] AAD = new byte[17];
        byte[] EncryKey = new byte[16];
        byte[] plainText = new byte[31];
        byte[] cyphertext = new byte[30];
        byte[] ClientInitVector = new byte[12];
        byte[] AuthenticationTag = new byte[12];
        uint AuthTagLen = 12;
        byte Encryptionmethod = 5;
        byte ChannelNum = 0;
        byte[] CypherDataDecypt;
        byte[] AADDecypt;
        byte[] InitVectorDecypt;
        byte[] DecyplainText;
        byte[] AuthTagDecypt;

        byte[] UserInfo = new byte[22];
        int UserInfoCounter = 0;
        string HLSpublicPwd = string.Empty;
        byte[] HDLCCommand = new byte[200];
        byte HDLCIndex = 0;
        //'******************************************************************************
        //'
        //'  NAME     : SendSNRM
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Send SNRM packet and Recieve and Check UA response
        //'
        //'*******************************************************************************
        public bool fSendSNRM(int nServerSAP, int nServerLowerMacAddress, int nClientSAP)
        {

            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex,nClientSAP);
                GlobalObjects.objHDLCLIB.fSetSNRM();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return false;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fSetUA();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP) == true)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fCheckHDLCResponse
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Check Start/end tag, Check FCS , Check destination Address and Check command Byte
        //'
        //'*******************************************************************************
        private bool fCheckHDLCResponse(byte[] Buffer,int nClientSAP)
        {
            
            if (GlobalObjects.objHDLCLIB.fCheckStartEndTag(Buffer) == false)
            {
                return false;
            }
            else
            {
                if (GlobalObjects.objHDLCLIB.fCheckFCS(Buffer) == false)
                {
                    return false;
                }
                else
                {
                    if (GlobalObjects.objHDLCLIB.fCheckServerSAP(Buffer, nClientSAP) == false)
                    {
                        return false;
                    }
                    else
                    {
                        if (GlobalObjects.objHDLCLIB.fCheckCommand(Buffer, GlobalObjects.objHDLCLIB.nCMDByte) == false)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSendAARQ
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Send AARQ packet and Recieve and Check AARE response
        //'
        //'*******************************************************************************
        public bool fSendAARQ(int nServerSAP, int nServerLowerMacAddress, int nClientSAP, byte nSecurityMechanism, string nPassword, string HLSKey)
        {
            try
            {
                //Change Needed
                byte[] cnfBlock = new byte[3];
                cnfBlock[0] = 0x00;
                // Raja byte change
                //new
                //cnfBlock[1] = 0x12;
                //cnfBlock[2] = 0x1A;
                //old
                cnfBlock[1] = 0x18;//old size uncommented (cosem connection failed) 30th march
                cnfBlock[2] = 0x1D;
                //Change Needed
                //7EA02E0002002321107ECBE6E600601DA109060760857405080101BE10040E01000000065F1F040000121AFFFFF4FF7E
                //7EA047000200234110974BE6E6006036A1090607608574050801018A0207808B0760857405080201AC0A80083132333435363738BE10040E01000000065F1F040000121AFFFFEEE07E

                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                GlobalObjects.objHDLCLIB.fSetInitialI();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                if (nSecurityMechanism == 0x01)
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x36);
                else
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x3E);
                byte nApplicationContext = 0x01;
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddContext(HDLCCommand, HDLCIndex, nApplicationContext);
                if (nSecurityMechanism == 0x01)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, nSecurityMechanism);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddPassword(HDLCCommand, HDLCIndex, nPassword);
                }
                else if (nSecurityMechanism == 0x02)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, nSecurityMechanism);
                    HDLCIndex = GlobalObjects.objCOSEMLIB.fAddRandomKey(HDLCCommand, HDLCIndex, nPassword);
                }
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddUserInf(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddCnfBlock(HDLCCommand, HDLCIndex, cnfBlock);
                //raja byte change
                //new
                //int nPDUSize = 9999;
                //old
                // Changed to support segmentation on behalf of Gopal
               // int nPDUSize = 100;// 65535;//old size uncommented (cosem connection failed) 30th march
                // Changed to support segmentation on behalf of Gopal

                int nPDUSize = 512;// As discussed with Tarun and Mohsin
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddPDUSize(HDLCCommand, HDLCIndex, nPDUSize);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return false;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP) == true)
                    {
                        if (GlobalObjects.objCOSEMLIB.fCheckAARQResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                        {
                            if (nSecurityMechanism == 0x02)
                            {
                                // GKG: 17/04/2013  US mode generic change
                                byte[] stocChallenge = new byte[16];
                                for (int challengIndex = 0; challengIndex < 16; challengIndex++)
                                {
                                    stocChallenge[challengIndex] = GlobalObjects.objSerialComm.ReceiveBuffer[56 + challengIndex];
                                }
                                AESEncryptor aESEncryptor = new AESEncryptor();
                                byte[] encryptedSTOSChallenge = aESEncryptor.fAESEncryption(HLSKey, stocChallenge);
                                // GKG: 17/04/2013  US mode generic change

                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                                //C3 01 C1 00 0F 00 00 28 00 03 FF 01 00 09 10
                                HDLCCommand[HDLCIndex++] = 0xC3;
                                HDLCCommand[HDLCIndex++] = 0x01;
                                HDLCCommand[HDLCIndex++] = 0xC1;
                                HDLCCommand[HDLCIndex++] = 0x00;
                                HDLCCommand[HDLCIndex++] = 0x0F;
                                HDLCCommand[HDLCIndex++] = 0x00;
                                HDLCCommand[HDLCIndex++] = 0x00;
                                HDLCCommand[HDLCIndex++] = 0x28;
                                HDLCCommand[HDLCIndex++] = 0x00;
                                HDLCCommand[HDLCIndex++] = 0x03;
                                HDLCCommand[HDLCIndex++] = 0xFF;
                                HDLCCommand[HDLCIndex++] = 0x01;
                                HDLCCommand[HDLCIndex++] = 0x00;
                                HDLCCommand[HDLCIndex++] = 0x09;
                                HDLCCommand[HDLCIndex++] = 0x10;

                                // GKG: 17/04/2013  US mode generic change
                                //HDLCIndex = GlobalObjects.objCOSEMLIB.fAddEncryptedKey(HDLCCommand, HDLCIndex, HLSKey);

                                for (int challengIndex = 0; challengIndex < 16; challengIndex++)
                                {
                                    HDLCCommand[HDLCIndex++] = encryptedSTOSChallenge[challengIndex];
                                }
                                // GKG: 17/04/2013  US mode generic change

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return false;
                                }
                                else
                                {
                                    ///check for response
                                    //////Application.DoEvents();
                                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP) == true)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                                return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetServerSeed(int StarBytePos)
        {
            try
            {
                string ReceivedSeed = string.Empty;
                int icnt = StarBytePos;
                char[] chararray = System.Text.Encoding.UTF8.GetString(GlobalObjects.objSerialComm.ReceiveBuffer).ToCharArray();
                while (icnt < StarBytePos + 16) ReceivedSeed += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[icnt++]).ToString();
                return ReceivedSeed;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public void IntializationVector(string systitle)
        {
            long clientinovationCount = GlobalObjects.objHDLCLIB.InitializationCounter;
            ClientInitVector[0] = Convert.ToByte(systitle[0]);
            ClientInitVector[1] = Convert.ToByte(systitle[1]);
            ClientInitVector[2] = Convert.ToByte(systitle[2]);
            ClientInitVector[3] = Convert.ToByte(systitle[3]);
            ClientInitVector[4] = Convert.ToByte(systitle[4]);
            ClientInitVector[5] = Convert.ToByte(systitle[5]);
            ClientInitVector[6] = Convert.ToByte(systitle[6]);
            ClientInitVector[7] = Convert.ToByte(systitle[7]);
            ClientInitVector[8] = Convert.ToByte((clientinovationCount & 0xFF000000) >> 24);
            ClientInitVector[9] = Convert.ToByte((clientinovationCount & 0xFF0000) >> 16);
            ClientInitVector[10] = Convert.ToByte((clientinovationCount & 0xFF00) >> 8);
            ClientInitVector[11] = Convert.ToByte(clientinovationCount & 0x00FF);

        }
        //'******************************************************************************
        //'
        //'  NAME     : fSendDISC
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Send DISC packet and Recieve and Check UA response
        //'
        //'*******************************************************************************
        public bool fSendDISC(int nServerSAP, int nServerLowerMacAddress, int nClientSAP)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, nServerSAP, nServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, nClientSAP);
                GlobalObjects.objHDLCLIB.fSetDISC();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return false;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fSetUA();//Setting Response Command type
                    if (fCheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer, nClientSAP) == true)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        

    }
   
    
}
