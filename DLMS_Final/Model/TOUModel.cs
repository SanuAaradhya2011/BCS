using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using CAB.BLL;
using CAB.BCS.DLMS.Utility;
using DLMS_Final;

namespace CAB.BCS.DLMS.Model
{
    class TOUModel
    {
        byte HDLCIndex = 0;
        byte[] HDLCCommand = new byte[200];
       
        #region TOU Functions
        /// <summary>
        /// This method is used for writing the TOU profils into meter.
        /// </summary>
        /// <param name="nDataArray"></param>
        /// <param name="nLength"></param>
        /// <param name="atb"></param>
        /// <returns></returns>
        public int WriteTOU(byte[] nDataArray, int nLength, byte atb)
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryWriteTOU(HDLCCommand, HDLCIndex, atb);

                for (int i = 0; i < nLength; i++)
                {
                    HDLCCommand[HDLCIndex++] = nDataArray[i];
                }

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
                else
                {
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForSet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        else if (ret == 0x02)
                        {
                            return (int)CoreUtility.DLMSResultType.AccessDenied;
                        }
                        else
                        {
                            return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
            }
            catch
            {
                return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
            }
        }
        public int WritePTRatio()
        {
            HDLCIndex = 0;
          
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
            GlobalObjects.objHDLCLIB.fIncSend();
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

            HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadPTRatio(HDLCCommand, HDLCIndex, 2);
            

            HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
            {
                return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
            }
            else
            {
                GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                {
                    int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                   
                    if (ret == 0x01)
                    {



                        return (int)CoreUtility.DLMSResultType.Success;


                    }
                    /* GKG no need to use 0x02 but not changing as it is old code */
                    //else if (ret == 0x02 )
                    else if (ret == 0x02 || ret == 0x03)
                    {
                        return (int)CoreUtility.DLMSResultType.AccessDenied;
                    }

                    else
                    {
                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                    }
                }
                else
                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;

            }
        }

        public int ReadMeterModelNumber()
        {
            try
            {
                HDLCIndex = 0;
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, SerialPortSettings.Default.ClientSAP);
                GlobalObjects.objHDLCLIB.fIncSend();
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objCOSEMLIB.GetQueryReadMeterModel(HDLCCommand, HDLCIndex, 2);

                //HDLCIndex = GlobalObjects.objHDLCLIB.ffillMeterID(HDLCCommand, HDLCIndex);

                HDLCIndex = GlobalObjects.objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                GlobalObjects.objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                GlobalObjects.objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = GlobalObjects.objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

                if (GlobalObjects.objSerialComm.fSendDataToPort(HDLCCommand, HDLCIndex) == false)
                {
                    return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
                else
                {
                    //////Application.DoEvents();
                    GlobalObjects.objHDLCLIB.fIncRecieve();//Setting Response Command type

                    if (HDLCLibrary.CheckHDLCResponse(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        int ret = GlobalObjects.objCOSEMLIB.fCheckCOSEMResponseForGet(GlobalObjects.objSerialComm.ReceiveBuffer);
                        if (ret == 0x01)
                        {
                            return (int)CoreUtility.DLMSResultType.Success;
                        }
                        else if (ret == 0x0E) //Data block unavailable
                        {
                            return (int)CoreUtility.DLMSResultType.DataUnavailable;
                        }
                        else if (ret == 0x03) //Access denied
                        {
                            return (int)CoreUtility.DLMSResultType.AccessDenied;
                        }
                        else
                        {
                            return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                        }
                    }
                    else
                        return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
                }
            }
            catch (Exception ex)
            {
                return (int)CoreUtility.DLMSResultType.CosemConnectionFailed;
            }
        }
       
        #endregion    

    }
}
