using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using CAB.BLL;
using DLMS_Final; 
namespace CAB.BCS.DLMS.Utility
{
    /// <summary>
    /// This class is used for connecting and disconnecting from the meter through DLMS protocal.
    /// </summary>
    class DLMSConnection : IConnection
    {
        #region Public Methods
        /// <summary>
        /// This method is used for disconnecting the DLMS meter.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                CoreUtility.ExpMessage = string.Empty;
                if (GlobalObjects.objGlobalFunctions.fSendDISC(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP) == true)
                {
                    return;
                }
                else
                {
                    CoreUtility.ExpMessage = CoreUtility.GetMessageFromResourceFile("NotableToDisconnect");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                GlobalObjects.objSerialComm.ClosePort();
            }
        }
        /// <summary>
        /// This method is used for connecting meter through the DLMS protocal.
        /// </summary>
        /// <returns>true/false a bool value</returns>
        public bool Connect()
        {
            bool dlmsConnect;
            CoreUtility.ExpMessage = string.Empty;
            GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
            GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
            GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
            SystemSettingsBLL objSystemSettingsBLL = new SystemSettingsBLL();
            if (objSystemSettingsBLL.GetSettingValue(CABConstants.USEMULTIPLEPORTS) == CABConstants.ONE)
            {
                SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue(CABConstants.CMRICOMPORT);
            }
            else
            {
                SerialPortSettings.Default.SerialPort = objSystemSettingsBLL.GetSettingValue(CABConstants.COMPORT);
            }

            try
            {
                GlobalObjects.objSerialComm.SetSerialPortSettings(SerialPortSettings.Default.SerialPort, CABConstants.BAUDRATE, CABConstants.PARITY, CABConstants.DATABITS, CABConstants.STOPBITS, SerialPortSettings.Default.CommandTimeOut, SerialPortSettings.Default.IntercharacterDelay);
                GlobalObjects.objSerialComm.OpenPort();
                if (GlobalObjects.objGlobalFunctions.fSendSNRM(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP) == true)
                {

                    if (GlobalObjects.objGlobalFunctions.fSendAARQ(SerialPortSettings.Default.ServerSAP, SerialPortSettings.Default.ServerLowerMacAddress, SerialPortSettings.Default.ClientSAP, SerialPortSettings.Default.SecurityMechanism, SerialPortSettings.Default.Password, SerialPortSettings.Default.HLSKey) == true)
                    {

                        dlmsConnect = true;
                    }
                    else
                    {
                        GlobalObjects.objSerialComm.ClosePort();
                        CoreUtility.ExpMessage = CoreUtility.GetMessageFromResourceFile("CosemConnectionFailed");
                        dlmsConnect = false;
                    }
                }
                else
                {
                    GlobalObjects.objSerialComm.ClosePort();
                    CoreUtility.ExpMessage = CoreUtility.GetMessageFromResourceFile("HDLCConnectionFailed");
                    dlmsConnect = false;
                }
            }
            catch (Exception ex)
            {
                dlmsConnect = false;
                throw ex;
            }
            finally
            {
                GlobalObjects.objCOSEMLIB.nBlockIndex = 0;
                GlobalObjects.objCOSEMLIB.nTotalPacketSize = 0;
                GlobalObjects.objCOSEMLIB.nBlockNumber = 0;
                GlobalObjects.objCOSEMLIB.nBlockTotalByteCount = 0;
            }
            return dlmsConnect;
        } 
        #endregion
    }
}
