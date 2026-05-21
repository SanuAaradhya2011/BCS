///****************************************************************************
//'*
//'*  Projet       : DLMS With LTCT
//'*
//'*  Component    : MMP
//'*
//'*  Module       : HDLC
//'*
//'*  Environment  : Visual Studio 2008 - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 1.00 | 10/08/09 | Gopal Krishna Gupta : creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;




namespace DLMSLIB
{
    public class HDLCLIB
    {
        public int SecuritysuitByte = 0;
        public long InitializationCounter = 0;
        #region Declaration
        public byte nCMDByte = 0x00;
        #endregion

        #region HDLCFCS
        public int[] ucFcs = new int[2];
        int PPPINITFCS16 = 0xFFFF;
        int PPPGOODFCS16 = 0xF0B8;
        int[] uifcstab = {  0x0000, 0x1189,0x2312,	0x329b,	0x4624,	0x57ad,	0x6536,	0x74bf,
	                        0x8c48,	0x9dc1,	0xaf5a,	0xbed3,	0xca6c,	0xdbe5,	0xe97e,	0xf8f7,
	                        0x1081,	0x0108,	0x3393,	0x221a,	0x56a5,	0x472c,	0x75b7,	0x643e,
	                        0x9cc9,	0x8d40,	0xbfdb,	0xae52,	0xdaed,	0xcb64,	0xf9ff,	0xe876,
	                        0x2102,	0x308b,	0x0210,	0x1399,	0x6726,	0x76af,	0x4434,	0x55bd,
	                        0xad4a,	0xbcc3,	0x8e58,	0x9fd1,	0xeb6e,	0xfae7,	0xc87c,	0xd9f5,
	                        0x3183,	0x200a,	0x1291,	0x0318,	0x77a7,	0x662e,	0x54b5,	0x453c,
	                        0xbdcb,	0xac42,	0x9ed9,	0x8f50,	0xfbef,	0xea66,	0xd8fd,	0xc974,
	                        0x4204,	0x538d,	0x6116,	0x709f,	0x0420,	0x15a9,	0x2732,	0x36bb,
	                        0xce4c,	0xdfc5,	0xed5e,	0xfcd7,	0x8868,	0x99e1,	0xab7a,	0xbaf3,
	                        0x5285,	0x430c,	0x7197,	0x601e,	0x14a1,	0x0528,	0x37b3,	0x263a,
	                        0xdecd,	0xcf44,	0xfddf,	0xec56,	0x98e9,	0x8960,	0xbbfb,	0xaa72,
	                        0x6306,	0x728f,	0x4014,	0x519d,	0x2522,	0x34ab,	0x0630,	0x17b9,
	                        0xef4e,	0xfec7,	0xcc5c,	0xddd5,	0xa96a,	0xb8e3,	0x8a78,	0x9bf1,
	                        0x7387,	0x620e,	0x5095,	0x411c,	0x35a3,	0x242a,	0x16b1,	0x0738,
	                        0xffcf,	0xee46,	0xdcdd,	0xcd54,	0xb9eb,	0xa862,	0x9af9,	0x8b70,
	                        0x8408,	0x9581,	0xa71a,	0xb693,	0xc22c,	0xd3a5,	0xe13e,	0xf0b7,
	                        0x0840,	0x19c9,	0x2b52,	0x3adb,	0x4e64,	0x5fed,	0x6d76,	0x7cff,
	                        0x9489,	0x8500,	0xb79b,	0xa612,	0xd2ad,	0xc324,	0xf1bf,	0xe036,
	                        0x18c1,	0x0948,	0x3bd3,	0x2a5a,	0x5ee5,	0x4f6c,	0x7df7,	0x6c7e,
	                        0xa50a,	0xb483,	0x8618,	0x9791,	0xe32e,	0xf2a7,	0xc03c,	0xd1b5,
	                        0x2942,	0x38cb,	0x0a50,	0x1bd9,	0x6f66,	0x7eef,	0x4c74,	0x5dfd,
	                        0xb58b,	0xa402,	0x9699,	0x8710,	0xf3af,	0xe226,	0xd0bd,	0xc134,
	                        0x39c3,	0x284a,	0x1ad1,	0x0b58,	0x7fe7,	0x6e6e,	0x5cf5,	0x4d7c,
	                        0xc60c,	0xd785,	0xe51e,	0xf497,	0x8028,	0x91a1,	0xa33a,	0xb2b3,
	                        0x4a44,	0x5bcd,	0x6956,	0x78df,	0x0c60,	0x1de9,	0x2f72,	0x3efb,
	                        0xd68d,	0xc704,	0xf59f,	0xe416,	0x90a9,	0x8120,	0xb3bb,	0xa232,
	                        0x5ac5,	0x4b4c,	0x79d7,	0x685e,	0x1ce1,	0x0d68,	0x3ff3,	0x2e7a,
	                        0xe70e,	0xf687,	0xc41c,	0xd595,	0xa12a,	0xb0a3,	0x8238,	0x93b1,
	                        0x6b46,	0x7acf,	0x4854,	0x59dd,	0x2d62,	0x3ceb,	0x0e70,	0x1ff9,
	                        0xf78f,	0xe606,	0xd49d,	0xc514,	0xb1ab,	0xa022,	0x92b9,	0x8330,
	                        0x7bc7,	0x6a4e,	0x58d5,	0x495c,	0x3de3,	0x2c6a,	0x1ef1,	0x0f78
                         };

        //'******************************************************************************
        //'
        //'  NAME     : fPPPfcs16
        //'
        //'  INPUT    : Initial FCS, Buffer , Length
        //'
        //'  OUTPUT   : New fcs of Integer Type
        //'
        //'  PURPOSE  : Calculate a new FCS given the current FCS and New data
        //'
        //'*******************************************************************************
        public int fPPPfcs16(int uiLocal_fcs, byte[] Buffer, int endlen)
        {
            int i = 1;

            while (endlen > 0)
            {
                uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ Buffer[i++]) & 0xff];
                endlen--;
            }
            return (uiLocal_fcs);
        }

        //'******************************************************************************
        //'
        //'  NAME     : fCheckFCS
        //'
        //'  INPUT    :  Buffer 
        //'
        //'  OUTPUT   : True for correct FCS or False for wrong FCS
        //'
        //'  PURPOSE  : Check FCS is correct or Not
        //'
        //'*******************************************************************************
        public bool fCheckFCS(byte[] RecvBuffer)
        {
            int nHDLCPktLength = RecvBuffer[2];
            int nHCSindex = 8;                          //Depends on Address Byte Supported Need Change
            if (fGenerateFCS(RecvBuffer, 1, nHCSindex))    //hcs
            {
                if ((RecvBuffer[nHCSindex + 1] == ucFcs[0]) && (RecvBuffer[nHCSindex + 2] == ucFcs[1]))
                {
                    if (fGenerateFCS(RecvBuffer, 1, (nHDLCPktLength - 2)))    //FCS
                    {
                        if ((RecvBuffer[nHDLCPktLength - 1] == ucFcs[0]) && (RecvBuffer[nHDLCPktLength] == ucFcs[1]))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        //'******************************************************************************
        //'
        //'  NAME     : fGenerateFCS
        //'
        //'  INPUT    : Buffer , Starting index and end index
        //'
        //'  OUTPUT   : True or False and FCS in ucFcs[0] and ucFcs[1]
        //'
        //'  PURPOSE  : Generate a new FCS for Buffer data
        //'
        //'*******************************************************************************
        public bool fGenerateFCS(byte[] Buffer, int stlen, int endlen)
        {
            int uitrialfcs;
            int[] ucCalcFcs = new int[2];
            int uiLocal_fcs;
            int i = 1;

            ucFcs[0] = 0x00;
            ucFcs[1] = 0x00;
            uiLocal_fcs = PPPINITFCS16;

            uitrialfcs = fPPPfcs16(PPPINITFCS16, Buffer, endlen);
            uitrialfcs ^= 0xffff;


            ucCalcFcs[0] = Convert.ToByte(uitrialfcs & 0x00ff);
            ucCalcFcs[1] = Convert.ToByte((uitrialfcs >> 8) & 0x00ff);

            while (endlen > 0)
            {
                uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ Buffer[i++]) & 0xff];
                endlen--;
            }

            uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ ucCalcFcs[0]) & 0xff];
            uitrialfcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ ucCalcFcs[1]) & 0xff];


            if (uitrialfcs == PPPGOODFCS16)
            {
                ucFcs[0] = ucCalcFcs[0];
                ucFcs[1] = ucCalcFcs[1];
                return true;
            }
            else
            {
                return false;
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fFillFCS
        //'
        //'  INPUT    : Buffer , Starting index and end index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Filling FCS bytes to Buffer
        //'
        //'*******************************************************************************
        public void fFillFCS(byte[] Buffer, int upIndex, int lowIndex)
        {
            Buffer[upIndex] = Convert.ToByte(ucFcs[0]);
            Buffer[lowIndex] = Convert.ToByte(ucFcs[1]);
        }

        #endregion

        #region HDLCChecks
        //'******************************************************************************
        //'
        //'  NAME     : fCheckStartEndTag
        //'
        //'  INPUT    : Buffer
        //'
        //'  OUTPUT   : True or False 
        //'
        //'  PURPOSE  : Checking the Start and End tag
        //'
        //'*******************************************************************************
        public bool fCheckStartEndTag(byte[] Buffer)
        {
            bool retValue = false;
            try
            {
                if (Buffer[0] == 0x7E && Buffer[Buffer[2] + 1] == 0x7E)
                {
                    retValue = true;
                }
            }
            catch
            {
            }
            
            return retValue;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fCheckStartEndTag
        //'
        //'  INPUT    : Buffer
        //'
        //'  OUTPUT   : True or False 
        //'
        //'  PURPOSE  : Checking the Start and End tag
        //'
        //'*******************************************************************************
        public bool fCheckServerSAP(byte[] Buffer, int nClientSAP)
        {
            int tempBuffer = 0;

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(nClientSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            tempBuffer = Convert.ToByte(tempBuffer & 0x00FF);
            tempBuffer = Convert.ToByte(tempBuffer | 0x01);
            if (Buffer[3] == tempBuffer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fCheckCommand
        //'
        //'  INPUT    : Buffer
        //'
        //'  OUTPUT   : True or False 
        //'
        //'  PURPOSE  : Checking the Start and End tag
        //'
        //'*******************************************************************************
        public bool fCheckCommand(byte[] Buffer, byte nCommandType)
        {
            if (Buffer[8] == nCommandType)
            {
                return true;
            }
            else
            {
                if (nCommandType == 0x73)
                {
                    if (Buffer[8] == 0x1F)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }
        #endregion

        #region HDLCSequence
        //'******************************************************************************
        //'
        //'  NAME     : fSetPFBit
        //'
        //'  INPUT    : nStatus Bit High = 1  or Low = 0
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Set poll Final bit
        //'
        //'*******************************************************************************
        public void fSetPFBit(byte nStatus)
        {
            if (nStatus == 0x01)
                nCMDByte = Convert.ToByte(nCMDByte | 0x10);
            else
                nCMDByte = Convert.ToByte(nCMDByte & 0xEF);
        }
        
        //'******************************************************************************
        //'
        //'  NAME     : fSetLastBit
        //'
        //'  INPUT    : nStatus Bit High = 1  or Low = 0
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Set last bit
        //'
        //'*******************************************************************************
        public void fSetLastBit(byte nStatus)
        {
            if (nStatus == 0x01)
                nCMDByte = Convert.ToByte(nCMDByte | 0x01);
            else
                nCMDByte = Convert.ToByte(nCMDByte & 0xFE);
        }
        //'******************************************************************************
        //'
        //'  NAME     : fIncRecieve
        //'
        //'  INPUT    : None
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Increment recieve Sequence number
        //'
        //'*******************************************************************************
        public void fIncRecieve()
        {
            int nSeqCounter = Convert.ToByte(nCMDByte & 0xE0);

            nSeqCounter = Convert.ToByte(nSeqCounter >> 5);
            nSeqCounter = Convert.ToByte(nSeqCounter + 1);
            nSeqCounter = nSeqCounter << 5;
            nSeqCounter = nSeqCounter & 0x00FF;

            nCMDByte = Convert.ToByte(nCMDByte & 0x1F);
            nCMDByte = Convert.ToByte(nCMDByte | nSeqCounter);
        }
        //'******************************************************************************
        //'
        //'  NAME     : fIncSend
        //'
        //'  INPUT    : None
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Increment Send Sequence number
        //'
        //'*******************************************************************************
        public void fIncSend()
        {
            int nSeqCounter = Convert.ToByte(nCMDByte & 0x0E);

            nSeqCounter = Convert.ToByte(nSeqCounter >> 1);
            nSeqCounter = Convert.ToByte(nSeqCounter + 1);

            nSeqCounter = nSeqCounter << 1;
            nSeqCounter = nSeqCounter & 0x00FF;

            nCMDByte = Convert.ToByte(nCMDByte & 0xF1);
            nCMDByte = Convert.ToByte(nCMDByte | nSeqCounter);
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSetSNRM
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : none
        //'
        //'  PURPOSE  : Set Commmand Byte as SNRM
        //'
        //'*******************************************************************************
        public void fSetSNRM()
        {
            nCMDByte = 0x93;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSetUI
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : none
        //'
        //'  PURPOSE  : Set Commmand Byte as UI
        //'
        //'*******************************************************************************
        public void fSetUA()
        {
            nCMDByte = 0x73;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSetDISC
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : none
        //'
        //'  PURPOSE  : Set Commmand Byte as DISC
        //'
        //'*******************************************************************************
        public void fSetDISC()
        {
            nCMDByte = 0x53;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSetUI
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : none
        //'
        //'  PURPOSE  : Set Commmand Byte as UI
        //'
        //'*******************************************************************************
        public void fSetUI()
        {
            nCMDByte = 0x13;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fSetInitialI
        //'
        //'  INPUT    : none
        //'
        //'  OUTPUT   : none
        //'
        //'  PURPOSE  : Set Commmand Byte as Initial I
        //'
        //'*******************************************************************************
        public void fSetInitialI()
        {
            nCMDByte = 0x10;
        }

        //'******************************************************************************
        //'
        //'  NAME     : IncSendRecvSeqno
        //'
        //'  INPUT    : Buffer , Starting index and end index
        //'
        //'  OUTPUT   : True or False and FCS in ucFcs[0] and ucFcs[1]
        //'
        //'  PURPOSE  : Generate a new FCS for Buffer data
        //'
        //'*******************************************************************************
        public int IncSendRecvSeqno(int seqindex)
        {
            byte[] sendseqno = { 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE };
            return sendseqno[seqindex];
        }
        #endregion

        #region FillHDLCPacket
        //'******************************************************************************
        //'
        //'  NAME     : fAdd7E
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add 0x7E to Buffer
        //'
        //'*******************************************************************************
        public byte fAdd7E(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0x7E;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddHDLCFrameTag
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add 0xA0 and space for Length to Buffer
        //'
        //'*******************************************************************************
        public byte fAddHDLCFrameTag(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xA0;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddServerSAP
        //'
        //'  INPUT    : Buffer , Buffer Index, Server SAP  and Server Lower MAC Address
        //'
        //'  OUTPUT   : Buffer Index
        //'
        //'  PURPOSE  : Conversion of Destination Address Values and Filling into Buffer
        //'
        //'*******************************************************************************
        public byte fAddServerSAP(byte[] Buffer, byte nBufferIndex, int nServerSAP, int ServerLowerMACAddress)
        {
            int tempBuffer = 0;

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(nServerSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            Buffer[nBufferIndex + 1] = Convert.ToByte(tempBuffer & 0x00FF);

            nServerSAP = nServerSAP << 2;
            Buffer[nBufferIndex] = Convert.ToByte((nServerSAP >> 8) & 0x00FF);
            Buffer[nBufferIndex] = Convert.ToByte(Buffer[nBufferIndex] & 0x00FF);

            nBufferIndex = Convert.ToByte(nBufferIndex + 2);

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(ServerLowerMACAddress & 0x00FF);
            tempBuffer = tempBuffer << 1;
            tempBuffer = Convert.ToByte(tempBuffer & 0x00FF);
            Buffer[nBufferIndex + 1] = Convert.ToByte(tempBuffer | 0x01);

            ServerLowerMACAddress = ServerLowerMACAddress << 2;
            Buffer[nBufferIndex] = Convert.ToByte((ServerLowerMACAddress >> 8) & 0x00FF);
            Buffer[nBufferIndex] = Convert.ToByte(Buffer[nBufferIndex] & 0x00FE);

            nBufferIndex = Convert.ToByte(nBufferIndex + 2);

            return nBufferIndex;

        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddClientSAP
        //'
        //'  INPUT    : Buffer,BufferIndex and Client SAP 
        //'
        //'  OUTPUT   : Buffer index
        //'
        //'  PURPOSE  : Conversion of Client Address Values and Filling into Buffer
        //'
        //'*******************************************************************************
        public byte fAddClientSAP(byte[] Buffer, byte nBufferIndex, int nClientSAP)
        {
            int tempBuffer = 0;
            tempBuffer = Convert.ToByte(nClientSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            Buffer[nBufferIndex] = Convert.ToByte(tempBuffer & 0x00FF);
            Buffer[nBufferIndex] = Convert.ToByte(tempBuffer | 0x01);
            nBufferIndex++;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddBlankFCS
        //'
        //'  INPUT    : Buffer and BufferIndex 
        //'
        //'  OUTPUT   : Buffer index
        //'
        //'  PURPOSE  : Fill Space for FCS/HCS in Buffer
        //'
        //'*******************************************************************************
        public byte fAddBlankFCS(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddCmdByte
        //'
        //'  INPUT    : Buffer and BufferIndex
        //'
        //'  OUTPUT   : Buffer index
        //'
        //'  PURPOSE  : Fill Command Byte in buffer
        //'
        //'*******************************************************************************
        public byte fAddCmdByte(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[nBufferIndex++] = nCMDByte;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : ffillLength
        //'
        //'  INPUT    : Buffer and Buffer Index 
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Conversion of Client Address Values and Filling into Buffer
        //'
        //'*******************************************************************************
        public void ffillLength(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

        }

        //'******************************************************************************
        //'
        //'  NAME     : ffillData
        //'
        //'  INPUT    : Buffer and Buffer Index 
        //'
        //'  OUTPUT   : index
        //'
        //'  PURPOSE  : Filling 0x09 0x01 0x00 into Buffer
        //'
        //'*******************************************************************************
        public byte ffillData(byte[] Buffer, byte nBufferIndex)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }


        //'******************************************************************************
        //'
        //'  NAME     : ffillMeterID
        //'
        //'  INPUT    : Buffer, Buffer Index and Meter ID
        //'
        //'  OUTPUT   : index
        //'
        //'  PURPOSE  : Filling 0x09 0x01 0x00 into Buffer
        //'
        //'*******************************************************************************
        public byte ffillMeterID(byte[] Buffer, byte nBufferIndex, string meterID)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            foreach (char ch in meterID)
            {
                Buffer[nBufferIndex++] = Convert.ToByte(ch);
            }
            return nBufferIndex;
        }

        public byte ffillResolution(byte[] Buffer, byte nBufferIndex, byte energyResolution, byte MDResolution, byte highResolution)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = energyResolution;
            Buffer[nBufferIndex++] = MDResolution;
            Buffer[nBufferIndex++] = highResolution;

            return nBufferIndex;
        }

        public byte ffillLSCaptureObject(byte[] Buffer, byte nBufferIndex, byte LSFirstByte, byte LSSecondByte)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = LSSecondByte;
            Buffer[nBufferIndex++] = LSFirstByte;

            return nBufferIndex;
        }

        public byte ffillLCDBacklight(byte[] Buffer, byte nBufferIndex, byte firstByte, byte secondByte)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x11;
            Buffer[nBufferIndex++] = firstByte;
            Buffer[nBufferIndex++] = 0x11;
            Buffer[nBufferIndex++] = secondByte;

            return nBufferIndex;
        }

        public byte ffillResetLockoutDays(byte[] Buffer, byte nBufferIndex, byte resetLockoutDays)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = 0x11;
            Buffer[nBufferIndex++] = resetLockoutDays;

            return nBufferIndex;
        }


        public byte ffillDisplayParameters(byte[] Buffer, byte nBufferIndex, List<byte> displayParams)
        {
            Buffer[nBufferIndex++] = Convert.ToByte(displayParams.Count);


            foreach (byte b in displayParams)
            {
                Buffer[nBufferIndex++] = b;
            }
            return nBufferIndex;
        }

        public byte ffillNumberOfEvents(byte[] Buffer, byte nBufferIndex, int Compartment1, int Compartment2, int Compartment3, int Compartment4, int Compartment5, int Compartment6)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = Convert.ToByte((Compartment1 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment1 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment2 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment2 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment3 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment3 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment4 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment4 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment5 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment5 & 0x00FF);

            Buffer[nBufferIndex++] = Convert.ToByte((Compartment6 & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(Compartment6 & 0x00FF);

            return nBufferIndex;
        }

        public byte ffillCTRatio(byte[] Buffer, byte nBufferIndex, byte[] CTRatio)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = CTRatio[1];
            Buffer[nBufferIndex++] = CTRatio[0];
            return nBufferIndex;
        }

        public byte ffillPTRatio(byte[] Buffer, byte nBufferIndex, byte PTRatio)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
     
            Buffer[nBufferIndex++] = PTRatio;
            return nBufferIndex;
        }
        /*GKG 02/12/2013 PT RATIO CHANGES*/
        public byte ffillPTRatio(byte[] Buffer, byte nBufferIndex, int PTRatio)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = Convert.ToByte((PTRatio & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(PTRatio & 0x00FF);
            return nBufferIndex;
        }
        /*GKG 02/12/2013 PT RATIO CHANGES*/

        public byte ffillRTC(byte[] Buffer, byte nBufferIndex)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            Buffer[nBufferIndex++] = Convert.ToByte((System.DateTime.Now.Year & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(System.DateTime.Now.Year & 0x00FF);

            //Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = Convert.ToByte(System.DateTime.Now.Year % 100);
            Buffer[nBufferIndex++] = Convert.ToByte(System.DateTime.Now.Month);
            Buffer[nBufferIndex++] = Convert.ToByte(System.DateTime.Now.Day);
            Buffer[nBufferIndex++] = 0xFF;//Convert.ToByte(System.DateTime.Now.DayOfWeek);//chnaged to resolve bug 72362 9th april 2012
            Buffer[nBufferIndex++] = Convert.ToByte(System.DateTime.Now.Hour);
            Buffer[nBufferIndex++] = Convert.ToByte(System.DateTime.Now.Minute);
            Buffer[nBufferIndex++] = Convert.ToByte(System.DateTime.Now.Second);

            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte ffillPassword(byte[] Buffer, byte nBufferIndex, string meterPassword)
        {
            foreach (char ch in meterPassword)
            {
                Buffer[nBufferIndex++] = Convert.ToByte(ch);
            }
            return nBufferIndex;
        }



        public byte ffillBaudRate(byte[] Buffer, byte nBufferIndex, int baudRate)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte


            if (baudRate == 9600)
            {
                Buffer[nBufferIndex++] = 0x05;
            }
            return nBufferIndex;
        }


        public byte ffillIntegrationPeriod(byte[] Buffer, byte nBufferIndex, int integrationPeriod)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte
            Buffer[nBufferIndex++] = Convert.ToByte((integrationPeriod & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(integrationPeriod & 0x00FF);

            return nBufferIndex;
        }

        public byte ffillLSCapturePeriod(byte[] Buffer, byte nBufferIndex, int capturePeriod)
        {
            Buffer[nBufferIndex++] = Convert.ToByte((capturePeriod & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(capturePeriod & 0x00FF);
            return nBufferIndex;
        }


        public byte ffillDisplayParamsTimeouts(byte[] Buffer, byte nBufferIndex, int scrollTime, int pushTimeout, int autoScrollTime, int autoScrollModeSelected)
        {
            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = Convert.ToByte((pushTimeout & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(pushTimeout & 0x00FF);

            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = Convert.ToByte((scrollTime & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(scrollTime & 0x00FF);

            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = Convert.ToByte(autoScrollModeSelected);

            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = Convert.ToByte((autoScrollTime & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(autoScrollTime & 0x00FF);
            return nBufferIndex;
        }

        public byte ffillInterFrameTimeout(byte[] Buffer, byte nBufferIndex, int interFrameTimeout)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            Buffer[nBufferIndex++] = Convert.ToByte((interFrameTimeout & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(interFrameTimeout & 0x00FF);

            return nBufferIndex;
        }


        public byte ffillInterActivityTimeout(byte[] Buffer, byte nBufferIndex, int InactivityTimeout)
        {
            //Buffer[2] = Convert.ToByte(nBufferIndex - 1);      //Filling length Byte

            Buffer[nBufferIndex++] = Convert.ToByte((InactivityTimeout & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(InactivityTimeout & 0x00FF);

            return nBufferIndex;
        }
        #endregion

    } 
    
    public class COSEMLIB
    {
        public string DedKeystr = string.Empty;
        public byte[] DedicatedKey = new byte[16]; 
        public DataSet _obisQueryDSet;
        public byte[] Readout_CMD = new byte[80];  //statically fixed query length 
        bool flgBlockTransfer = false;
        // added for All data Readout by gopal
        public int nBlockTotalByteCount = 0x00;
        // added for All data Readout by gopal
        public int nBlockNumber = 0x00;
        public int nBlockIndex = 0x00;
        public int nTotalPacketSize = 0x00;
        public byte[] BlockBuffer = new byte[25000000];
        #region FillCOSEMPacket
        //'******************************************************************************
        //'
        //'  NAME     : fAddLLCByte
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add LLC Byte to Buffer
        //'
        //'*******************************************************************************
        public byte fAddLLCByte(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xE6;
            Buffer[nBufferIndex++] = 0xE6;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddAARQTAG
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add AARQ Tag and  Length to Buffer
        //'
        //'*******************************************************************************
        public byte fAddAARQTAG(byte[] Buffer, byte nBufferIndex,byte nLength)
        {
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = nLength;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddContext
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add Context tag and Context name to Buffer
        //'
        //'*******************************************************************************
        public byte fAddContext(byte[] Buffer, byte nBufferIndex, byte nContextType)
        {
            //A109060760857405080101
            Buffer[nBufferIndex++] = 0xA1;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0x74;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = nContextType;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddSecMechanism
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add Security Mechanism tag and Security Mechanism to Buffer
        //'
        //'*******************************************************************************
        public byte fAddSecMechanism(byte[] Buffer, byte nBufferIndex, byte nSecMechanism)
        {
            //8A0207808B0760857405080201
            Buffer[nBufferIndex++] = 0x8A;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x8B;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0x74;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = nSecMechanism;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddPassword
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add password tag and password to Buffer
        //'
        //'*******************************************************************************
        public byte fAddPassword(byte[] Buffer, byte nBufferIndex, string password)
        {
            //AC0A80083132333435363738
            Buffer[nBufferIndex++] = 0xAC;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = Convert.ToByte(password[0]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[1]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[2]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[3]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[4]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[5]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[6]);
            Buffer[nBufferIndex++] = Convert.ToByte(password[7]);

            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddPassword
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add password tag and password to Buffer
        //'
        //'*******************************************************************************
        public byte fAddRandomKey(byte[] Buffer, byte nBufferIndex, string password)
        {
            //AC0A80083132333435363738
            // Added to try catch.
            try
            {
                Buffer[nBufferIndex++] = 0xAC;
                Buffer[nBufferIndex++] = 0x12;
                Buffer[nBufferIndex++] = 0x80;
                Buffer[nBufferIndex++] = 0x10;
                Buffer[nBufferIndex++] = Convert.ToByte(password[0]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[1]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[2]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[3]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[4]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[5]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[6]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[7]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[8]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[9]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[10]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[11]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[12]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[13]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[14]);
                Buffer[nBufferIndex++] = Convert.ToByte(password[15]);
            }
            catch (Exception ex)
            {
                return nBufferIndex;
                
            }

            return nBufferIndex;
        }
          //'******************************************************************************
        //'
        //'  NAME     : fAddPassword
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add password tag and password to Buffer
        //'
        //'*******************************************************************************
        public byte fAddEncryptedKey(byte[] Buffer, byte nBufferIndex, string HLSKey)
        {
            //AC0A80083132333435363738
            //Buffer[nBufferIndex++] = 0xAC;
            //Buffer[nBufferIndex++] = 0x12;
            //Buffer[nBufferIndex++] = 0x80;
            //Buffer[nBufferIndex++] = 0x10;

            for (int i = 0; i < HLSKey.Length; i = i + 2)
            {
                Buffer[nBufferIndex++] = Convert.ToByte(HLSKey.Substring(i, 2), 16);
            }

            return nBufferIndex;
        }
        

        //'******************************************************************************
        //'
        //'  NAME     : fAddUserInf
        //'
        //'  INPUT    : Buffer and Buffer Index
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add User Info Tags and proposed DLMS Ver Number to Buffer
        //'
        //'*******************************************************************************
        public byte fAddUserInf(byte[] Buffer, byte nBufferIndex)
        {
            //BE10040E0100000006
            Buffer[nBufferIndex++] = 0xBE;
            Buffer[nBufferIndex++] = 0x10;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x0E;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;      
            Buffer[nBufferIndex++] = 0x06;      //Proposed DLMS Ver Number
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddCnfBlock
        //'
        //'  INPUT    : Buffer and Buffer Index and Conformance Block
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add Conformance Block Tags and proposed Conformance Block to Buffer
        //'
        //'*******************************************************************************
        public byte fAddCnfBlock(byte[] Buffer, byte nBufferIndex, byte[] cnfBlock)
        {
            //5F1F040000121A
            Buffer[nBufferIndex++] = 0x5F;
            Buffer[nBufferIndex++] = 0x1F;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = cnfBlock[0];
            Buffer[nBufferIndex++] = cnfBlock[1];
            Buffer[nBufferIndex++] = cnfBlock[2];
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fAddPDUSize
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add PDU Size to Buffer
        //'
        //'*******************************************************************************
        public byte fAddPDUSize(byte[] Buffer, byte nBufferIndex, int PDUSize)
        {
            Buffer[nBufferIndex++] = Convert.ToByte((PDUSize >> 8) & 0x00ff);
            Buffer[nBufferIndex++] = Convert.ToByte(PDUSize & 0x00ff);
            return nBufferIndex;
        }
        #endregion
        public DataSet ObisQueryDSet
        {
            get
            {
                return _obisQueryDSet;
            }
            set
            {
                _obisQueryDSet = value;
            }

        }
        public byte GetQueryReadAnamoly(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 91 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x9D;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        

        public byte GetQueryReadPCBAStatus(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 91 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x94;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        public byte GetQuery(byte[] Buffer,byte nBufferIndex, int iIndex)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            string ClassID = ObisQueryDSet.Tables["CLASS"].Rows[iIndex][0].ToString().Trim();
            Buffer[nBufferIndex++] = Convert.ToByte(ClassID);
            string ObisCode = ObisQueryDSet.Tables["OBISCODE"].Rows[iIndex][0].ToString().Trim();
            string[] ObisCodeClass = ObisCode.Split('.');
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16);    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16);    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            string AttributeID = ObisQueryDSet.Tables["ATTRIBUTE"].Rows[iIndex][0].ToString().Trim();
            Buffer[nBufferIndex++] = Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;


        }
        public byte GetQueryOBISList(byte[] Buffer, byte nBufferIndex,int Mode)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; // 0000280000FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            if ( Mode == 0x10 )
                Buffer[nBufferIndex++] = 0x01;
            else if (Mode == 0x20)
                Buffer[nBufferIndex++] = 0x02;
            else if (Mode == 0x30)
                Buffer[nBufferIndex++] = 0x03;
            else
                Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x02;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;


        }
        /// <summary>
        /// This method is uesd for get command for scalar of meter accuarcy check parameters
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte GetQueryAccuracyCheckScalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;//GET.Request.Normal
            Buffer[nBufferIndex++] = 0x01;//GET.Request.Normal
            Buffer[nBufferIndex++] = 0x81;//InokeID & Priority
            Buffer[nBufferIndex++] = 0x00;//Class ID
            Buffer[nBufferIndex++] = 0x07;//Class ID
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x60;//OBIS Code
            Buffer[nBufferIndex++] = 0x01;//OBIS Code
            Buffer[nBufferIndex++] = 0x9C;//OBIS Code
            Buffer[nBufferIndex++] = 0xFF;//OBIS Code
            Buffer[nBufferIndex++] = atb;//AttributeID
            Buffer[nBufferIndex++] = 0x00;//Access Selector
            return nBufferIndex;
        }
        /// <summary>
        /// This method is uesd for get command for data of meter accuarcy check parameters
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte GetQueryAccuracyCheckProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x9B;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        public byte GetQueryInstantProfile(byte[] Buffer, byte nBufferIndex,byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        /// <summary>
        /// query for phasor readout in normal mode 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte GetQueryPhasorProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xBD;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        
        //added PUMA
        public byte GetCumulativeKW(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;//GET.Request. Normal
            Buffer[nBufferIndex++] = 0x01;//GET.Request. Normal
            Buffer[nBufferIndex++] = 0x81;//InokeID & Priority
            Buffer[nBufferIndex++] = 0x00;//Class ID
            Buffer[nBufferIndex++] = 0x04;//Class ID
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x60;//OBIS Code
            Buffer[nBufferIndex++] = 0x01;//OBIS Code
            Buffer[nBufferIndex++] = 0x95;//OBIS Code
            Buffer[nBufferIndex++] = 0xFF;//OBIS Code
            Buffer[nBufferIndex++] = atb;//AttributeID
            Buffer[nBufferIndex++] = 0x00;//Access Selector
            return nBufferIndex;
        }

        //added PUMA
        public byte GetCumulativeKVA(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;//GET.Request. Normal
            Buffer[nBufferIndex++] = 0x01;//GET.Request. Normal
            Buffer[nBufferIndex++] = 0x81;//InokeID & Priority
            Buffer[nBufferIndex++] = 0x00;//Class ID
            Buffer[nBufferIndex++] = 0x04;//Class ID
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x60;//OBIS Code
            Buffer[nBufferIndex++] = 0x01;//OBIS Code
            Buffer[nBufferIndex++] = 0x96;//OBIS Code
            Buffer[nBufferIndex++] = 0xFF;//OBIS Code
            Buffer[nBufferIndex++] = atb;//AttributeID
            Buffer[nBufferIndex++] = 0x00;//Access Selector
            return nBufferIndex;
        }

        public byte GetQuerySAPList(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x11;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x41;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x02;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        //added PUMA
        public byte GetQueryCumulativeScalarProfileKW(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;//GET.Request.Normal
            Buffer[nBufferIndex++] = 0x01;//GET.Request.Normal
            Buffer[nBufferIndex++] = 0x81;//InokeID & Priority
            Buffer[nBufferIndex++] = 0x00;//Class ID
            Buffer[nBufferIndex++] = 0x04;//Class ID
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x60;//OBIS Code
            Buffer[nBufferIndex++] = 0x01;//OBIS Code
            Buffer[nBufferIndex++] = 0x95;//OBIS Code
            Buffer[nBufferIndex++] = 0xFF;//OBIS Code
            Buffer[nBufferIndex++] = atb;//AttributeID
            Buffer[nBufferIndex++] = 0x00;//Access Selector
            return nBufferIndex;
        }

        //added PUMA
        public byte GetQueryCumulativeScalarProfileKVA(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;//GET.Request.Normal
            Buffer[nBufferIndex++] = 0x01;//GET.Request.Normal
            Buffer[nBufferIndex++] = 0x81;//InokeID & Priority
            Buffer[nBufferIndex++] = 0x00;//Class ID
            Buffer[nBufferIndex++] = 0x04;//Class ID
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x00;//OBIS Code
            Buffer[nBufferIndex++] = 0x60;//OBIS Code
            Buffer[nBufferIndex++] = 0x01;//OBIS Code
            Buffer[nBufferIndex++] = 0x96;//OBIS Code
            Buffer[nBufferIndex++] = 0xFF;//OBIS Code
            Buffer[nBufferIndex++] = atb;//AttributeID
            Buffer[nBufferIndex++] = 0x00;//Access Selector
            return nBufferIndex;
        }

        public byte GetQueryInstantScalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 5E 5B 03 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        public byte GetQueryPhasorScalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//classId
            Buffer[nBufferIndex++] = 0x00; 
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xBE;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryResetEnergy(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        public byte GetQueryResetSoftwareMD(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte fGetQueryKey(byte[] Buffer, byte nBufferIndex)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0x02;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x10;
            return nBufferIndex;
        }


        public byte GetQueryResetLoadSurvey(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryResetMD(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryResetTamper(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }


        public byte GetQueryResetBillingData(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }


        public byte GetQueryResetMagneticTamper(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryResetOthers(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; // 00 01 0A 08 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryBillingScalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }


        public byte GetQueryLoadSurveyScalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        public byte GetQueryMidNightSacalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        //added for MVVNL

        public byte fGetQueryMidnightDataProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x9A;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        // For PUMA
        public byte GetQueryMidnightDataScalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; 
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x9B;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        // added for MVVNL

        public byte GetQueryTamperScalarProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x5E;
            Buffer[nBufferIndex++] = 0x5B;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryWriteMeterID(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x08;
 
            return nBufferIndex;
        }
        public byte GetQueryWritePCBAID(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 8B 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8B;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0D;

            return nBufferIndex;
        }

        public byte GetQueryReadMeterID(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        public byte GetQueryReadPCBAID(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8B;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryReadInterFrameTimeout(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryReadInterActivityTimeout(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryWriteResolution(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 84 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x84;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        public byte GetQueryWriteKVAhSelection(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 8F FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8F;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x01;
            return nBufferIndex;
        }


        public byte GetQueryWriteLSCaptureObject(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 85 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }


        public byte GetQueryWriteLCDBacklight(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	8A	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8A;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }


        public byte GetQueryWriteResetLockoutDays(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	89	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x89;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        public byte GetQueryWriteTOU(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x14; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	0D	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0D;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        public byte GetQueryWriteTOUBlock(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x14; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	0D	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0D;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        public byte GetQueryReadTOU(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x14; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	0D	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0D;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
        /// <summary>
        /// VBM - Read phasor command.
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte GetQueryReadPhasor(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 9E FF 
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x9E;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }
       
        public byte GetQueryWriteTamperThreshold(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x02;          //block transfer
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	89	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryReadTamperThreshold(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;          //block transfer
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01; //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	60	01	89	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryWritePushDisplayParameter(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 86 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x86;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }

        public byte GetQueryWriteScrollDisplayParameter(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 87 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x87;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }

        public byte GetQueryWriteHighResolutionDisplayParameter(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 88 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x88;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x03;

            return nBufferIndex;
        }

        public byte GetQueryWriteDisplayParameterTimeouts(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 80 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x04;
            //Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }

        public byte GetQueryWriteEvents(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 82 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x82;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            return nBufferIndex;
        }

        public byte GetQueryReadResolution(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 84 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x84;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        public byte GetQueryReadKVAhSelection(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 8f FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8F;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryWriteRTC(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 01 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            return nBufferIndex;
        }

        public byte GetQueryWritePassword(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; ////00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00; 
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryReadMeterPassword(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x28;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryReadRTC(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 01 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        public byte GetQueryReadMeterModel(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 01 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xA6;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        /// <summary>
        /// Create command to read Signature for UP Contractors 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte GetQueryReadSignature(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 01 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xBC;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        public byte GetQueryWriteBaudRate(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 255
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x16;

            return nBufferIndex;
        }

        public byte GetQueryReadBaudRate(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 255
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x16;

            return nBufferIndex;
        }

        public byte GetQueryWriteIntegrationPeriod(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 08 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }

        public byte GetQueryWriteLSCapturePeriod(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //0x01,0x00,0x00,0x08,0x04,0xFF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }
        public byte GetQueryWriteManuYear(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; 
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            return nBufferIndex;
        }


        public byte GetQueryReadIntegrationPeriod(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 08 00 255
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;

            return nBufferIndex;
        }

        public byte GetQueryWriteInterFrameTimeout(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 FF
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }


        public byte GetQueryWriteInactivityTimeout(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x17;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 16 00 00 255
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x16;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryWriteCTRatio(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 04 02 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            //02	00	11
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            //Buffer[nBufferIndex++] = 0x7B;
            //Buffer[nBufferIndex++] = 0xAC;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        public byte GetQueryWritePTRatio(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 04 02 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0xFF;
            //02	00	11
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x11;

            //Buffer[nBufferIndex++] = 0x7B;
            //Buffer[nBufferIndex++] = 0xAC;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /*GKG 02/12/2013 PT RATIO CHANGES*/
        public byte GetQueryWritePTRatio1(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 04 02 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0xFF;
            //02	00	11
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x12;

            //Buffer[nBufferIndex++] = 0x7B;
            //Buffer[nBufferIndex++] = 0xAC;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        /*GKG 02/12/2013 PT RATIO CHANGES*/

        public byte GetQueryReadCTRatio(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 04 02 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
               //Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        public byte GetQueryReadPTRatio(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //01 00 00 04 02 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x03;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryReadLSCapturePeriod(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01; //0x01,0x00,0x00,0x08,0x04,0xFF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        public byte GetQueryReadManuYear(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //0x01,0x00,0x00,0x08,0x04,0xFF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x11;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }

        public byte GetQueryWriteBillingDatetime(byte[] Buffer, byte nBufferIndex, byte atb, byte paramDate, byte paramHour, byte paramMinute)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x16;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 0F 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x02;
           
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = paramHour;
            Buffer[nBufferIndex++] = paramMinute;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = paramDate;
            Buffer[nBufferIndex++] = 0xFF;

            return nBufferIndex;
        }

        public byte GetQueryReadBillingDatetime(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x16;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 01 0F 00 00 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0x02;
            //Buffer[nBufferIndex++] = 0x02;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x05;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = paramDate;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x04;
            //Buffer[nBufferIndex++] = paramHour;
            //Buffer[nBufferIndex++] = paramMinute;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte GetQueryReadLSCaptureObject(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 85 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            
            return nBufferIndex;
        }

        public byte GetQueryReadTamperEvents(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 82 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x82;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        public byte GetQueryReadPushDisplayParameter(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 86 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x86;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }


        public byte GetQueryReadPushDisplayParameterTimeouts(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; ///00 00 60 01 80 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        public byte GetQueryReadLCDBacklight(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 8A FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8A;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }
        public byte GetQueryMRUSWritePassword(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; ////00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8E;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x10;

            return nBufferIndex;
        }

        public byte GetQueryMRUSReadMeterPassword(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00	00	28	00	00	FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x8E;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x09;
            //Buffer[nBufferIndex++] = 0x08;

            return nBufferIndex;
        }
        public byte GetQueryReadLockOutDays(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 89 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x89;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        public byte GetQueryReadScrollDisplayParameter(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 87 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x87;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        public byte GetQueryReadHighResolutionDisplayParameter(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 60 01 88 FF
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x88;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;

            return nBufferIndex;
        }

        /// <summary>
        /// Retuns command for MD Reset Command
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public byte GetQueryMDReset(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC3;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        //'******************************************************************************
        //'
        //'  NAME     : fCheckAARQResponse
        //'
        //'  INPUT    : Buffer 
        //'
        //'  OUTPUT   : true or False
        //'
        //'  PURPOSE  : Check AARQ Response
        //'
        //'*******************************************************************************
        public bool fCheckAARQResponse(byte[] Buffer)
        {
            int nCosemIndex = 0x0E;
            if (Buffer[nCosemIndex] != 0x61)   //AARQ.response
            {
                return false;//Fail
            }
            else
            {
                nCosemIndex = nCosemIndex + 17;
                if (Buffer[nCosemIndex] == 0x00)   //Association response
                {
                    return true;    /// Success
                }
                else
                {
                    return false; ///fail
                }
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : SetBlockTransferPacket
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add PDU Size to Buffer
        //'
        //'*******************************************************************************
        public byte fSetBlockTransferPacket(byte[] Buffer, byte nBufferIndex, byte[] nDataArray, bool flgTransfer)
        {
            int nMaxBufferSize=0x4d;
            if (flgTransfer == false)
            {
                for (int i = 0; i < nTotalPacketSize; i++)
                {
                    BlockBuffer[i] = nDataArray[i];
                }
                nBlockNumber = 0x01;
                nBlockIndex = 0x00;
                flgBlockTransfer = true;
            }
            if ((nTotalPacketSize - nBlockIndex) <= nMaxBufferSize)
            {
                nMaxBufferSize = nTotalPacketSize - nBlockIndex;
                Buffer[nBufferIndex++] = 0x01;
            }
            else
                Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber & 0xFF);
            Buffer[nBufferIndex++] = Convert.ToByte (nMaxBufferSize);
            for (int i = 0; i < nMaxBufferSize; i++)
            {
                Buffer[nBufferIndex++] = BlockBuffer[nBlockIndex++];
            }

            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fCheckCOSEMResponse
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add PDU Size to Buffer
        //'
        //'*******************************************************************************
        public byte fCheckCOSEMResponse(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC4 )   //Get.response
            {
                return 0x03;//Tag Mismatch
            }
            else
            { 
                nCosemIndex = nCosemIndex + 1 ;
                if (Buffer[nCosemIndex] == 0x02 )   //Get.response.Next
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] == 0x00)   //Get.response.Next
                    {
                        nCosemIndex = nCosemIndex + 3;
                        flgBlockTransfer = true;

                        nBlockNumber = 0;
                        nBlockNumber = nBlockNumber | (int)Buffer[nCosemIndex] << 8;
                        nCosemIndex++;
                        nBlockNumber = nBlockNumber | (int)Buffer[nCosemIndex];

                        nCosemIndex = nCosemIndex + 2;
                        byte nBlockByteCount=Buffer[nCosemIndex];
                        nCosemIndex = nCosemIndex + 1;  //Skipping the block Count Byte
                        for (byte i = 0; i < nBlockByteCount ; i++)
                        {
                            BlockBuffer[nBlockIndex++] = Buffer[nCosemIndex++];
                        }
                        return 0x02;
                    }
                    else 
                    {
                        nCosemIndex = nCosemIndex + 6;
                        byte nBlockByteCount = Buffer[nCosemIndex];
                        nCosemIndex = nCosemIndex + 1;  //Skipping the block Count Byte
                        for (byte i = 0; i < nBlockByteCount; i++)
                        {
                            BlockBuffer[nBlockIndex++] = Buffer[nCosemIndex++];
                        }
                        flgBlockTransfer = false;
                        nBlockTotalByteCount = nBlockIndex;
                        nBlockNumber = 0x00;
                        nBlockIndex = 0x00;
                        return 0x01; ///Block Trasfer Completed
                    }
                }
                else 
                {
                     nCosemIndex = nCosemIndex + 2;
                     if (Buffer[nCosemIndex] == 0x00)   //Get.response.Normal
                     {
                         //code written to handle tamper compartment readout when data comes in normal transfer 
                         int nBlockByteCount = Buffer[nCosemIndex + 2];
                         if (nBlockByteCount == 0x00)
                         {
                             flgBlockTransfer = false;
                             return 0x07; ///Get.Request.normal
                         }
                         else
                         {
                             nBlockByteCount = Buffer[nCosemIndex - 15];
                             nBlockByteCount = nBlockByteCount - 20;
                             nCosemIndex = nCosemIndex + 1;
                             for (byte i = 0; i <= nBlockByteCount; i++)  //included = in the condition for tamper compartment readout 
                             {
                                 BlockBuffer[nBlockIndex++] = Buffer[nCosemIndex++];
                             }
                             flgBlockTransfer = false;
                             nBlockTotalByteCount = nBlockIndex;
                             nBlockNumber = 0x00;
                             nBlockIndex = 0x00;
                             return 0x01; ///Get.Request.normal
                         }
                        
                     }
                     else
                     {
                         return 0x05; ///Get.Request.normal
                     }
                }
            }
        }

        //'******************************************************************************
        //'
        //'  NAME     : fCheckCOSEMResponseForC1
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add PDU Size to Buffer
        //'
        //'*******************************************************************************
        public byte fCheckCOSEMResponseForSet(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC5)   //Get.response
            {
                return 0x03;//Fail
            }
            else
            {
                nCosemIndex = nCosemIndex + 1;
                if (Buffer[nCosemIndex] == 0x02)   //Get.response.Next
                {
                     nCosemIndex = nCosemIndex + 2;
                     if (Buffer[nCosemIndex] == 0x01)
                     {
                         return 0x02;/// Access Denied
                     }
                     // Solved bug 90130.
                     else if (Buffer[nCosemIndex] == 0xFA)
                     {
                         return 0x02; // Access denied in case of PUMA when rtc is written in case of battery mode
                     }
                     else
                     {
                         if ((nTotalPacketSize > nBlockIndex))
                         {
                             nBlockNumber++;
                             return 0x04;    /// Access Denied
                         }
                         else
                             return 0x01; ///Get.Request.normal
                     }
                }
                else
                {
                    nCosemIndex = nCosemIndex + 2;
                    if (Buffer[nCosemIndex] == 0x00)   //Get.response.Next
                    {
                        return 0x01;    
                        /// 
                    }
                    else if (Buffer[nCosemIndex] == 0x01)
                    {
                        return 0x02;/// Access Denied
                    }
                    // Solved bug 90130.
                    else if (Buffer[nCosemIndex] == 0xFA)
                    {
                        return 0x02; // Access denied in case of PUMA when rtc is written in case of battery mode
                    }

                    else
                    {
                        return 0x03; ///Get.Request.normal
                    }
                }
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fCheckCOSEMResponseForGet
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add PDU Size to Buffer
        //'
        //'*******************************************************************************
        public byte fCheckCOSEMResponseForGet(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC4)   //Get.response
            {
                return 0x00;//Fail
            }
            else
            {
                nCosemIndex = nCosemIndex + 3;
                if (Buffer[nCosemIndex] == 0x01)   //Failure
                {
                    nCosemIndex = nCosemIndex + 1;
                    if (Buffer[nCosemIndex] == 0x03 )
                        return 0x03;    // Access Denied
                    else if (Buffer[nCosemIndex] == 0x0E )
                        return 0x0E;    //Data block unavailable
                    else if (Buffer[nCosemIndex] ==0xD)
                        return 0x04;    // Access Denied
                    else
                        return 0x02;
                }
                else
                {
                    return 0x01; ///Get.Request.normal (Success)
                }
            }
        }
        //'******************************************************************************
        //'
        //'  NAME     : fCheckCOSEMResponseForReset
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : add PDU Size to Buffer
        //'
        //'*******************************************************************************
        public byte fCheckCOSEMResponseForReset(byte[] Buffer)
        {
            int nCosemIndex = 14;
            if (Buffer[nCosemIndex] != 0xC7)   //Get.response
            {
                if (Buffer[nCosemIndex] == 0x0E)   //Get.response
                {
                    return 0x02;//Fail
                }
                else
                    return 0x03;//Fail
            }

            else
            {
                nCosemIndex = nCosemIndex + 3;
                if (Buffer[nCosemIndex] == 0x00)
                {
                    return 0x01;
                }
                else if (Buffer[nCosemIndex] == 0xFA)   //Get.response.Next
                {
                    return 0x02;    /// Access Denied
                }
                else
                {
                    return 0x03; ///Get.Request.normal
                }
            }
      
        }
        //'******************************************************************************
        //'
        //'  NAME     : fGetBlockTransferPacket
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Create Block Tranfsfer packet
        //'
        //'*******************************************************************************
        public byte fGetBlockTransferPacket(byte[] Buffer, byte nBufferIndex)
        {
            //7EA01402232154 7E15 E6E600 C002C10000000151BE7E

            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x02;      //need to change
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;

            //Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber);

            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(nBlockNumber & 0xFF);
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fGetQueryBillingProfile
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Create Block Tranfsfer packet
        //'
        //'*******************************************************************************
        public byte fGetQueryBillingProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x62;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
       
        public byte fGetQueryLoadSurveyProfile(byte[] Buffer, byte nBufferIndex, byte atb)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x63;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;


            //Buffer[nBufferIndex++] = 0xC0;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0xC1;
            //Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x01;  //Convert.ToByte(ClassID); 
            //Buffer[nBufferIndex++] = 0x00;  ///00 00 60 01 91 FF 
            //Buffer[nBufferIndex++] = 0x00;
            //Buffer[nBufferIndex++] = 0x60;
            //Buffer[nBufferIndex++] = 0x01;
            //Buffer[nBufferIndex++] = 0x94;
            //Buffer[nBufferIndex++] = 0xFF;
            //Buffer[nBufferIndex++] = 2;// Convert.ToByte(AttributeID); 
            //return nBufferIndex;
                    }



        public byte fGetQueryTamperProfile(byte[] Buffer, byte nBufferIndex, byte atb, byte compartment)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x81;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x07;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = 0x00; //00 00 63 62 compartment(00 to 05) ff
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x63;
            Buffer[nBufferIndex++] = 0x62;
            Buffer[nBufferIndex++] = compartment;
            Buffer[nBufferIndex++] = 0xFF;
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }
        //'******************************************************************************
        //'
        //'  NAME     : fGetSelectiveAccessByEntry
        //'
        //'  INPUT    : Buffer and Buffer Index and PDU Size
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : Create Block Tranfsfer packet
        //'
        //'*******************************************************************************
        public byte fGetSelectiveAccessByEntry(byte[] Buffer, byte nBufferIndex,byte fromEntry,byte toEntry)
        {
            //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
            nBufferIndex--;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x02;      //need to change
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(fromEntry);
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = Convert.ToByte(toEntry);
            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        public byte fGetSelectiveAccessByEntry(byte[] Buffer, byte nBufferIndex, DateTime fromDate, DateTime toDate)
        {
            //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
            nBufferIndex--;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x02;      
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x02;      
            Buffer[nBufferIndex++] = 0x04;

            Buffer[nBufferIndex++] = 0x12;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x08;

            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x06;
            
            Buffer[nBufferIndex++] = 0x00; //obis code
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x0F;
            Buffer[nBufferIndex++] = 0x02;
            Buffer[nBufferIndex++] = 0x12;

            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            Buffer[nBufferIndex++] = Convert.ToByte((fromDate.Year / 100) % 20); //year

            Buffer[nBufferIndex++] = Convert.ToByte( fromDate.Year % 100);

            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Month); //month

            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Day);
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Hour);
            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Minute);
            Buffer[nBufferIndex++] = Convert.ToByte(fromDate.Second); 

            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x00;
            
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x0C;

            Buffer[nBufferIndex++] = Convert.ToByte((toDate.Year / 100) % 20); //year
            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Year % 100);

            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Month); //month

            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Day);
            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Hour);
            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Minute);
            Buffer[nBufferIndex++] = Convert.ToByte(toDate.Second); 

            Buffer[nBufferIndex++] = 0xFF;

            Buffer[nBufferIndex++] = 0x80;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0x00;
           
            return nBufferIndex;
        }
        public byte fAddContext_Cyphered(byte[] Buffer, int nBufferIndex, byte nContextType)
        {
            //A109060760857405080103
            Buffer[nBufferIndex++] = 0xA1;
            Buffer[nBufferIndex++] = 0x09;
            Buffer[nBufferIndex++] = 0x06;
            Buffer[nBufferIndex++] = 0x07;
            Buffer[nBufferIndex++] = 0x60;
            Buffer[nBufferIndex++] = 0x85;
            Buffer[nBufferIndex++] = 0x74;
            Buffer[nBufferIndex++] = 0x05;
            Buffer[nBufferIndex++] = 0x08;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = nContextType;
            return Convert.ToByte(nBufferIndex);
        }

        public byte fBeforeSysTitle(byte[] Buffer, int nBufferIndex, string SystemTitle)
        {
            Buffer[nBufferIndex++] = 0xA6;
            Buffer[nBufferIndex++] = 0x0A;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x08;

            return Convert.ToByte(nBufferIndex);
        }
        public byte fSystemTitle(byte[] Buffer, int nBufferIndex, string SystemTitle)
        {

            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[0]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[1]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[2]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[3]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[4]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[5]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[6]);
            Buffer[nBufferIndex++] = Convert.ToByte(SystemTitle[7]);
            return Convert.ToByte(nBufferIndex);
        }
        public byte SecuritySuitByte(byte[] Buffer, int nBufferIndex, int sootByte, int Dedicatekey)
        {
            //BE230421211F
            Buffer[nBufferIndex++] = 0xBE;
            if (Dedicatekey == 1)
                Buffer[nBufferIndex++] = 0x34;
            else
                Buffer[nBufferIndex++] = 0x23;

            Buffer[nBufferIndex++] = 0x04;
            if (Dedicatekey == 1)
                Buffer[nBufferIndex++] = 0x32;
            else
                Buffer[nBufferIndex++] = 0x21;

            Buffer[nBufferIndex++] = 0x21;
            if (Dedicatekey == 1)
                Buffer[nBufferIndex++] = 0x30;
            else
                Buffer[nBufferIndex++] = 0x1F;
            Buffer[nBufferIndex++] = Convert.ToByte(Convert.ToString(sootByte));
            return Convert.ToByte(nBufferIndex);
        }
        public byte fInvocationCounter(byte[] Buffer, int nBufferIndex, long InvCount)
        {
            //00000018
            //int clientinovationCount = GlobalObjects.objHDLCLIB.InitializationCounter;
            Buffer[nBufferIndex++] = Convert.ToByte((InvCount & 0xFF000000) >> 24);
            Buffer[nBufferIndex++] = Convert.ToByte((InvCount & 0xFF0000) >> 16);
            Buffer[nBufferIndex++] = Convert.ToByte((InvCount & 0xFF00) >> 8);
            Buffer[nBufferIndex++] = Convert.ToByte(InvCount & 0x00FF);
            return Convert.ToByte(nBufferIndex);
        }
        public string RandomHexString()
        {
            Random rdm = new Random();
            string RandomhexVal = string.Empty;
            int num;

            for (int i = 0; i < 4; i++)
            {
                num = rdm.Next(0, int.MaxValue);
                RandomhexVal += num.ToString("X8");
            }

            return RandomhexVal;
        }
        public byte fAddUserInf_cypher(byte[] Buffer, int nBufferIndex, int DedicationKey)
        {
            //BE10040E0100000006
            int countlen = 0;
            int dedcounter = 0;
            DedKeystr = "";
            Buffer[nBufferIndex++] = 0x01;
            if (DedicationKey == 1)
            {
                // DedKeystr = "3C0B155618776321585A62117D735D41";
                DedKeystr = RandomHexString();
                Buffer[nBufferIndex++] = 0x01;
                Buffer[nBufferIndex++] = 0x10;
                while (countlen < DedKeystr.Length)
                {
                    Buffer[nBufferIndex++] = Convert.ToByte(DedKeystr.Substring(countlen, 2), 16);
                    DedicatedKey[dedcounter++] = Convert.ToByte(DedKeystr.Substring(countlen, 2), 16);
                    countlen += 2;
                }
            }
            else
                Buffer[nBufferIndex++] = 0x00;

            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = 0x06;      //Proposed DLMS Ver Number
            return Convert.ToByte(nBufferIndex);
        }
        public byte fAddCnfBlock_Cyphered(byte[] Buffer, int nBufferIndex, byte[] cnfBlock)
        {
            //5F1F040000121A
            Buffer[nBufferIndex++] = 0x5F;
            Buffer[nBufferIndex++] = 0x1F;
            Buffer[nBufferIndex++] = 0x04;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = cnfBlock[0];
            Buffer[nBufferIndex++] = cnfBlock[1];
            Buffer[nBufferIndex++] = cnfBlock[2];
            return Convert.ToByte(nBufferIndex);
        }
        public byte fAddPDUSize_Cyphered(byte[] Buffer, int nBufferIndex, int PDUSize)
        {
            Buffer[nBufferIndex++] = Convert.ToByte((PDUSize >> 8) & 0x00ff);
            Buffer[nBufferIndex++] = Convert.ToByte(PDUSize & 0x00ff);
            return Convert.ToByte(nBufferIndex);
        }
        public byte fAddCyphered_Tag(byte[] Buffer, int nBufferIndex, byte[] CypherTag)
        {
            int bytecount = 0;
            while (bytecount < CypherTag.Length)
            {
                Buffer[nBufferIndex++] = CypherTag[bytecount++];
            }

            return Convert.ToByte(nBufferIndex);
        }

        public byte fAddAuthentication_Tag(byte[] Buffer, int nBufferIndex, byte[] AuthTag)
        {
            int bytecount = 0;
            while (bytecount < AuthTag.Length)
            {
                Buffer[nBufferIndex++] = AuthTag[bytecount++];
            }

            return Convert.ToByte(nBufferIndex);
        }
        public byte GetQueryReadByClassOBIS(byte[] Buffer, byte nBufferIndex, byte atb, byte[] obisCode, byte classCode)
        {
            Buffer[nBufferIndex++] = 0xC0;
            Buffer[nBufferIndex++] = 0x01;
            Buffer[nBufferIndex++] = 0xC1;//0xC1;
            Buffer[nBufferIndex++] = 0x00;
            Buffer[nBufferIndex++] = classCode;//Convert.ToByte(ClassID);
            Buffer[nBufferIndex++] = obisCode[0]; //00 00 60 01 83 FF
            Buffer[nBufferIndex++] = obisCode[1];
            Buffer[nBufferIndex++] = obisCode[2];
            Buffer[nBufferIndex++] = obisCode[3];
            Buffer[nBufferIndex++] = obisCode[4];
            Buffer[nBufferIndex++] = obisCode[5];
            Buffer[nBufferIndex++] = atb;// Convert.ToByte(AttributeID);
            Buffer[nBufferIndex++] = 0x00;
            return nBufferIndex;
        }

        
    }
}
