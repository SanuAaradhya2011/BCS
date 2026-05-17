using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using CAB.BLL;
using CAB.BCS.DLMS.Utility;
using DLMS_Final;
using System.Data;
using System.Xml; 
namespace CAB.BCS.DLMS.Model
{
    class CMRIModel
    {
        byte HDLCIndex = 0;
        byte[] HDLCCommand = new byte[200];

        public bool ReadSAPlist()
        {
            try
            {
               // CoreUtility.GetIncrementedTimer();                 
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQuerySAPList(HDLCCommand, HDLCIndex);
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
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return true;
                        }
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                //CoreUtility.GetIncrementedTimer();                 
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return false;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte ReadInastantaneous(byte attribute)
        {
            try
            {
                HDLCIndex = 0;
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;

                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Start and End tag for Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Start and End for HDLC
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Server Address
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Client Address
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryInstantProfile(HDLCCommand, HDLCIndex, attribute);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {                   
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                CoreUtility.GetIncrementedTimer();
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public byte ReadScalarProfile(byte attribute, byte nProfileindex)
        {
            try
            {
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (nProfileindex == 0)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryInstantScalarProfile(HDLCCommand, HDLCIndex, attribute);
                }
                else if (nProfileindex == 1)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryBillingScalarProfile(HDLCCommand, HDLCIndex, attribute);
                }
                else if (nProfileindex == 2)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryLoadSurveyScalarProfile(HDLCCommand, HDLCIndex, attribute);
                }
                else if (nProfileindex == 3)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryTamperScalarProfile(HDLCCommand, HDLCIndex, attribute);
                }
                else if (nProfileindex == 4)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryCumulativeScalarProfileKW(HDLCCommand, HDLCIndex, attribute);
                }
                else if (nProfileindex == 5)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryCumulativeScalarProfileKVA(HDLCCommand, HDLCIndex, attribute);
                }
                //added for MVVNL
                else if (nProfileindex == 6)
                {
                    HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryMidnightDataScalarProfile(HDLCCommand, HDLCIndex, attribute);
                }
                //added for MVVNL
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {                    
                    GlobalObjects.objHDLCLIB.fIncRecieve();
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                               CoreUtility.GetIncrementedTimer();                                
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
        public byte ReadCumulativeKVA(byte attribute)
        {
            try
            {
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                GlobalObjects.objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetCumulativeKVA(HDLCCommand, HDLCIndex, attribute);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {                        
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);

                        if (ret == 0x01)//success
                            return 0x01;
                        else
                        {
                            return 0x07;
                        }
                    }
                    else
                        return 0x07;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }
        public byte ReadCumulativeKW(byte attribute)
        {
            try
            {
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Opening Flag of Frame
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);//Frame Type & Length

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);//Destination Adr Upper
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);//Destination Address Lower

                GlobalObjects.objHDLCLIB.fIncSend();//Setting Request Command type

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);//Header Check Sequence
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);//LLC Bytes

                //GET.Request. Normal + InokeID & Priority + Class ID + OBIS Code + Attribute ID + Access Selector
                HDLCIndex = GlobalObjects.objCOSEMLIB.GetCumulativeKW(HDLCCommand, HDLCIndex, attribute);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);

                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);//Frame Check Sequence
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);//Frame Check Sequence

                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);//Closing Flag of Frame

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {                      
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)//success
                            return 0x01;
                        else
                        {
                            return 0x07;
                        }
                    }
                    else
                        return 0x07;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public byte ReadBillingProfile(byte attribute, bool isReadlast, bool isReadBetween, string dateTo, string dateFrom, string LastFromDate)
        {
            try
            {
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryBillingProfile(HDLCCommand, HDLCIndex, attribute);

                //added by gopal for Selective Access By Entry
                if (attribute == 0x02)
                {
                    if (isReadlast)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(LastFromDate));

                    }
                    else if (isReadBetween)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(dateFrom), Convert.ToByte(dateTo));
                    }

                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                CoreUtility.GetIncrementedTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        public byte ReadLoadSurveyProfile(byte attribute, bool isReadBetween, DateTime toDate, DateTime fromDate)
        {
            try
            {
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryLoadSurveyProfile(HDLCCommand, HDLCIndex, attribute);

                //added by dhirendra for Selective Access By Range
                if (attribute == 0x02)
                {
                    if (isReadBetween)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, fromDate, toDate);
                    }
                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                CoreUtility.GetIncrementedTimer();
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        public byte ReadTamperProfile(byte attribute, byte tamperCompartment, bool isReadLast, bool isReadBetween, string dateTo, string dateFrom, string LastFromDate)
        {
            try
            {
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryTamperProfile(HDLCCommand, HDLCIndex, attribute, tamperCompartment);

                //added by gopal for Selective Access By Entry
                if (attribute == 0x02)
                {
                    if (isReadLast)
                    {
                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, 1, Convert.ToByte(LastFromDate));

                    }
                    else if (isReadBetween)
                    {

                        HDLCIndex = GlobalObjects.objCOSEMLIB.fGetSelectiveAccessByEntry(HDLCCommand, HDLCIndex, Convert.ToByte(dateFrom), Convert.ToByte(dateTo));
                    }
                }
                //added by gopal for Selective Access
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                                CoreUtility.GetIncrementedTimer();
                                //7EA01402232154 7E15 E6E600 C002C10000000151BE7E
                                //Send Block tarsfer Command


                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                                //7EA014022321766E17E6E600C002C100000002CA8C7E
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        public int GetMeterID(int iIndex)
        {
            bool isCurrentCommandOfPTRatio = false;
            DataSet OBISLIST = null;
            XmlDataDocument xmlDatadoc = null;
            xmlDatadoc = new XmlDataDocument(); 
            try
            {
                               
                string path = AppDomain.CurrentDomain.BaseDirectory + "Name Plate Details.xml";
                xmlDatadoc.DataSet.ReadXml(path);
                OBISLIST = new DataSet("OBis List Dataset");                
                OBISLIST = xmlDatadoc.DataSet;
                GlobalObjects.objCOSEMLIB.ObisQueryDSet = OBISLIST;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQuery(HDLCCommand, HDLCIndex, iIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                CoreUtility.GetIncrementedTimer();  
                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {                   
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        // This is a workaround as LTCT,HTCT meters will not support PT Ratio.
                        // if access denied and current command is for PT ratio return success.
                        if (ret == 0x03 && isCurrentCommandOfPTRatio)
                        {
                            return 0x01;
                        }
                        if (ret == 0x01)
                        {
                            return 0x01; //Success
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            //MessageBox.Show("Data unavailable", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return 0x0E;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            //MessageBox.Show("Access denied", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return 0x03;
                        }
                        else
                        {
                            return 0x00; //Fail
                        }
                    }
                    else
                    {
                        return 0x00;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte ReadMidnightProfile(byte attribute)
        {
            try
            {
                SerialPortSettings.Default.ServerSAP = 0x01;
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetQueryMidnightDataProfile(HDLCCommand, HDLCIndex, attribute);
              
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return 0x00;
                }
                else
                {

                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type
                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                            return 0x01;
                        else if (ret == 0x02)
                        {
                            while (true)
                            {
                              CoreUtility.GetIncrementedTimer();                               
                                HDLCIndex = 0;
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                                GlobalObjects.objHDLCLIB.fIncSend();
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);

                                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                                GlobalObjects.objHDLCLIB.fIncRecieve();                               
                                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                                {
                                    return 0x00;
                                }
                                else
                                {
                                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                                    {
                                        ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponse(GlobalObjects.objSerialComm.ReceiveBuffer);
                                        if (ret == 0x01)
                                            break;
                                        else if (ret == 0x02)
                                            continue;
                                    }
                                    else
                                    {
                                        return 0x00;
                                    }
                                }
                            }

                            return 0x01;
                        }
                        else if (ret == 0x05)
                        {
                            return 0x05;
                        }
                        else if (ret == 0x07)
                        {
                            return 0x07;
                        }
                        else
                        {
                            return 0x00;
                        }
                    }
                    else
                        return 0x00;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
