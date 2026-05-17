#region Namespaces
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using CABCommunication.ApplicationLayer;
using CABCommunication.Common;
using CABCommunication.DataLinkLayer;
using CABCommunication.PhysicalLayer;
using CAB.IECFramework.Utility;


#endregion

namespace CABCommunication.WrapperLayer
{
    /// <summary>
    /// This interface defines operations that communication layer will 
    /// be using to send requests/commands to the connected physical channel.
    /// </summary>
    public class Communication
    {       
        #region Nested Types
        #endregion

        #region Constants and Variables

        /// <summary>
        /// Stores the FD command length , the total no of bytes in FD command will be Meter ID length + FastDownloadCommandLength
        /// </summary>
        private const int FastDownloadCommandLength = 5;
        #endregion

        #region Properties
        /// <summary>
        /// Get/Set Serial communication instance
        /// </summary>
        public IPhysicalChannel PhysicalChannelDetail { get; set; }

        /// <summary>
        /// Get/Set HDLC layer instance
        /// </summary>
        private HDLCLayer HDLCLayer { get; set; }

        /// <summary>
        ///   Get/Set COSEM layer instance
        /// </summary>
        private COSEMLayer COSEMLayer { get; set; }

        private IrDALayer IrDALayer { get; set; }

        public Serial Serial { get; set; }

        #endregion

        #region Constructor


        /// <summary>
        /// 
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="securityMechanism"></param>
        /// <param name="password"></param>
        public Communication(ChannelInformation channelInfo)
        {
            PhysicalChannelDetail = Channel.CreateChannel(channelInfo);
            if (channelInfo.SecurityMechanism == 0x02)
            {
                HDLCLayer = new HDLCLayer(PhysicalChannelDetail,0x30);
            }
            else if (channelInfo.SecurityMechanism == 0x01)
            {
                HDLCLayer = new HDLCLayer(PhysicalChannelDetail, 0x20);
            }
            else if (channelInfo.SecurityMechanism == 0x00)
            {
                HDLCLayer = new HDLCLayer(PhysicalChannelDetail, 0x10);
            }
          COSEMLayer = new COSEMLayer(HDLCLayer, channelInfo.SecurityMechanism, channelInfo.Password);

          if (typeof(SerialIrDA).IsInstanceOfType(PhysicalChannelDetail))
          {
              IrDALayer = new IrDALayer((SerialIrDA)PhysicalChannelDetail);
          }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="securityMechanism"></param>
        /// <param name="password"></param>
        public Communication(Serial serial, byte securityMechanism, string password)
        {

            this.Serial = serial;
            if (securityMechanism == 0x02)
            {
                HDLCLayer = new HDLCLayer(Serial, 0x30);
            }
            else
            {
                HDLCLayer = new HDLCLayer(Serial, 0x20);
            }
            COSEMLayer = new COSEMLayer(HDLCLayer, securityMechanism, password);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens a communication session with cosem layer
        /// </summary>
        /// <returns></returns>
        //****************** For Smart Meter Ciphering ******************

        public Result OpenSessionCipher(long InitiCounter)
        {
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Nothing;
            try
            {
                if (PhysicalChannelDetail.OpenSession())
                {
                    result = COSEMLayer.OpenSession_CipherCommands(InitiCounter);//Smart meter
                    if (result.ErrorCode != CommunicationErrorType.Success)
                    {
                        COSEMLayer.CloseSession();
                        PhysicalChannelDetail.CloseSession();
                    }
                }
                else
                {
                    if (PhysicalChannelDetail.GetType() == typeof(CABCommunication.PhysicalLayer.GSM))
                    {
                        result.ErrorCode = CommunicationErrorType.RemoteModemNotConnected;
                    }
                    else
                    {
                        result.ErrorCode = CommunicationErrorType.PortInvalid;
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public Result OpenSession()
        {
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Nothing;
            try
            {
                if (PhysicalChannelDetail.OpenSession())
                {
                    result = COSEMLayer.OpenSession();
                    if (result.ErrorCode != CommunicationErrorType.Success)
                    {
                        PhysicalChannelDetail.CloseSession();
                    }
                }
                else
                {
                    if (PhysicalChannelDetail.GetType() == typeof(CABCommunication.PhysicalLayer.GSM))
                    {
                        result.ErrorCode = CommunicationErrorType.RemoteModemNotConnected;
                    }
                    else
                    {
                        result.ErrorCode = CommunicationErrorType.PortInvalid;
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public bool OpenSessionIrDA()
        {
            bool result = false; 
            try
            {
                if (PhysicalChannelDetail.OpenSession())
                {
                    result = true;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public void CloseSessionIrDA()
        {
            try
            {
                PhysicalChannelDetail.CloseSession();
            }
            catch (Exception)
            {
            }
        }

        public Result ReadIrDAByteFromMeter(byte IrDAReadCommandType, int IrDAMeterid, int IrDAhhuID, string CommandData)
        {
            return IrDALayer.ReadIrDAByteFromMeter(IrDAReadCommandType, IrDAMeterid, IrDAhhuID, CommandData);
        }

        public Result OpenSessionCMRI(byte baudindex)
        {
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Nothing;
            PhysicalChannelDetail.SetBaud(baudindex);
            try
            {
                if (PhysicalChannelDetail.OpenSession())
                {
                    result = COSEMLayer.OpenSession();
                    if (result.ErrorCode != CommunicationErrorType.Success)
                    {
                        PhysicalChannelDetail.CloseSession();
                    }
                }
                else
                {
                    if (PhysicalChannelDetail.GetType() == typeof(CABCommunication.PhysicalLayer.GSM))
                    {
                        result.ErrorCode = CommunicationErrorType.RemoteModemNotConnected;
                    }
                    else
                    {
                        result.ErrorCode = CommunicationErrorType.PortInvalid;
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }



        public Result OpenSessionforIEC()
        {
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Nothing;
            try
            {
                if (PhysicalChannelDetail.OpenSession())
                {
                    result.ErrorCode = CommunicationErrorType.Success;
                    //if (result.ErrorCode != CommunicationErrorType.Success)
                    // {
                    //     PhysicalChannelDetail.CloseSession();
                    // }
                }
                else
                {
                    result.ErrorCode = CommunicationErrorType.InvalidGetResponseTag; ;
                
                }
                
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// Opens a remote communication session with modem and cosem layer, 
        /// if Non-DLMS then doesnt opens a session with cosem layer.
        /// </summary>
        /// <returns></returns>
        public Result OpenSession(string simNumber)
        {
            Result result = new Result();
            try
            {
                IPhysicalChannel channelGPRS = (IPhysicalChannel)PhysicalChannelDetail as GPRS;
                if (channelGPRS != null)
                {
                    if (channelGPRS.OpenSession())
                        result.ErrorCode = CommunicationErrorType.ConnectedDLMS;
                }
                else
                {
                    result = Serial.OpenSession(simNumber);
                }
                if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS)
                {
                    result = COSEMLayer.OpenSession();
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        result.ErrorCode = CommunicationErrorType.ConnectedDLMS;
                    }

                }

            }
            catch (Exception ex)
            {
                result.ErrorCode = CommunicationErrorType.Nothing;
            }
            return result;
        }
        /// <summary>
        /// Checks Connection for IEC
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="initialBaud"></param>
        /// <returns></returns>
        public Result CheckSession(string comPort, string initialBaud, byte secMechanism)
        {
            Result result = new Result();
           
           
            try
            {
                string meterFwType = ConfigSettings.GetValue("MeterFirmwareType").Trim();
                string communicationType = ConfigSettings.GetValue("CommunicationType");
                string baudRate = ConfigSettings.GetValue("BaudRate");
                string initialBaudRate = ConfigSettings.GetValue("InitialBaudRate");
                ChannelDetail channelDetail = new ChannelDetail();
                Serial checkSerial = null;
                result.ErrorCode = CommunicationErrorType.Nothing;
                
                if (meterFwType == "1")
                {
                    result.ErrorCode = CommunicationErrorType.SuccessForDLMS;
                }
                else
                {
                    if (result.ErrorCode != CommunicationErrorType.SuccessForDLMS && meterFwType.Contains("1"))
                    {
                        #region CheckForDLMS

                        channelDetail.BaudRate = "9600";
                        channelDetail.InitialBaudRate =  "9600";
                        channelDetail.Parity = "None";
                        channelDetail.StopBits = "1";
                        channelDetail.DataBits = "8";
                        channelDetail.ComPort = comPort;
                        channelDetail.ResponseTimeout = 3000;
                        channelDetail.InterCharacterDelay = 2800;
                        channelDetail.NumberOfRetry = 1;
                        channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
                        checkSerial = new Serial(channelDetail);
                        if (checkSerial.OpenSession())
                        {
                          // result = GetHDLCCommandPacket(CommandType.SNRM);
                           byte[] connectData =HDLCLayer.GetHDLCCommandPacket(CommandType.DISC);//{ 0x7E, 0xA0, 0x0A, 0x00, 0x02, 0x04, 0x01, 0x21, 0x53, 0x7B, 0x73, 0x7E };
                            //if (secMechanism == 0x01)
                            //{
                            //    connectData[7] = 0x41;
                            //    connectData[9] = 0x2E;
                            //    connectData[10] = 0x16;
                            //}
                            //else if (secMechanism == 0x02)
                            //{
                            //    connectData[7] = 0x61;
                            //    connectData[9] = 0x1D;
                            //    connectData[10] = 0x35;
                            //}
                            result = checkSerial.Send(connectData, 12);
                            if (result.RecieveDataLength > 11)
                            {

                                result.ErrorCode = CommunicationErrorType.SuccessForDLMS;
                                checkSerial.CloseSession();
                            }

                            else
                            {
                                result.ErrorCode = CommunicationErrorType.ResponseTimeout;
                                checkSerial.CloseSession();
                                return result;
                            }
                        }
                        

                        else
                        {
                            result.ErrorCode = CommunicationErrorType.PortInvalid;
                            
                        }
                        #endregion
                    }
                    if (result.ErrorCode != CommunicationErrorType.SuccessForDLMS && meterFwType.Contains("3"))
                    {
                        #region SuccessForIECSP

                        channelDetail.BaudRate = "9600";
                        channelDetail.InitialBaudRate = "9600";
                        channelDetail.Parity = "None";
                        channelDetail.StopBits = "1";
                        channelDetail.DataBits = "8";
                        channelDetail.ComPort = comPort;
                        channelDetail.ResponseTimeout = 2000;
                        channelDetail.InterCharacterDelay = 1800;
                        channelDetail.NumberOfRetry = 1;
                        channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
                        checkSerial = new Serial(channelDetail);

                        if (checkSerial.OpenSession())
                        {
                            byte[] data1 = { 0x2F, 0x3F, 0x21, 0x0D, 0x0A };
                            result = checkSerial.Send(data1, 5);
                            if (result.RecieveDataLength > 5)
                            {
                                byte[] breakcmd = { 0x01, 0x42, 0x30, 0x03, 0x71 };
                                Result result1 = checkSerial.Send(breakcmd, 5);
                                checkSerial.DelayExecution(200);
                                result1 = checkSerial.Send(breakcmd, 5);
                                checkSerial.DelayExecution(200);
                                result.ErrorCode = CommunicationErrorType.SuccessForIECSP;
                                ConfigInfo.SignatureInfo = string.Empty;
                                foreach (byte item in result.RecieveDataBuffer)
	                            {
                                    ConfigInfo.SignatureInfo += (char)item;
	                            }
                               
                            }
                            ConfigSettings.ChangeNode("BaudRate", "9600");
                            checkSerial.CloseSession();
                        }

                        else
                        {
                            result.ErrorCode = CommunicationErrorType.PortInvalid;
                        }

                        #endregion
                    }
                    if (result.ErrorCode != CommunicationErrorType.SuccessForDLMS && result.ErrorCode != CommunicationErrorType.SuccessForIECSP && meterFwType.Contains("2"))
                    {
                        #region CheckForIEC

                        channelDetail.BaudRate = initialBaudRate;
                        channelDetail.InitialBaudRate = initialBaudRate;
                   
                        channelDetail.Parity = "Even";
                        channelDetail.StopBits = "1";
                        channelDetail.DataBits = "7";
                        channelDetail.ComPort = comPort;
                        channelDetail.ResponseTimeout = 2000;
                        channelDetail.InterCharacterDelay = 1800;
                        channelDetail.NumberOfRetry = 1;
                        channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
                        checkSerial = new Serial(channelDetail);

                        if (checkSerial.OpenSession())
                        {
                            byte[] data = { 0x2F, 0x3F, 0x21, 0x0D, 0x0A };
                            result = checkSerial.Send(data, 5);
                            if (result.RecieveDataLength > 5)
                            {
                                byte[] breakcmd = { 0x01, 0x42, 0x30, 0x03, 0x71 };
                                Result result1 = checkSerial.Send(breakcmd, 5);
                                checkSerial.DelayExecution(200);
                                result1 = checkSerial.Send(breakcmd, 5);
                                checkSerial.DelayExecution(200);

                                result.ErrorCode = CommunicationErrorType.SuccessForIEC;
                                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                                {
                                    ConfigSettings.ChangeNode("BaudRate", "300");
                                }
                                else
                                    ConfigSettings.ChangeNode("BaudRate", channelDetail.BaudRate);                                
                            }
                            checkSerial.CloseSession();
                        }
                        else
                        {
                            result.ErrorCode = CommunicationErrorType.PortInvalid;
                        }

                        #endregion
                    }
                   
                }
            }
            catch (Exception)
            {
            }
            return result;

            #region Commented
                        
            //ChannelDetail channelDetail = new ChannelDetail();
            //channelDetail.BaudRate = "9600";
            //channelDetail.InitialBaudRate = "9600";
            //channelDetail.Parity = "None";
            //channelDetail.StopBits = "1";
            //channelDetail.DataBits = "8";
            //channelDetail.ComPort = comPort;
            //channelDetail.ResponseTimeout = 3000;
            //channelDetail.InterCharacterDelay = 2800;
            //channelDetail.NumberOfRetry = 1;
            //channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
            //Serial checkSerial = new Serial(channelDetail);
            //if (checkSerial.OpenSession())
            //{
            //    byte[] connectData = { 0x7E, 0xA0, 0x0A, 0x00, 0x02, 0x04, 0x01, 0x21, 0x53, 0x7B, 0x73, 0x7E };
            //    if (secMechanism == 0x01)
            //    {
            //        connectData[7] = 0x41;
            //        connectData[9] = 0x2E;
            //        connectData[10] = 0x16;
            //    }
            //    else if (secMechanism == 0x02)
            //    {
            //        connectData[7] = 0x61;
            //        connectData[9] = 0x1D;
            //        connectData[10] = 0x35;
            //    }
            //    result = checkSerial.Send(connectData, 12);
            //    if (result.RecieveDataLength > 11)
            //    {

            //        result.ErrorCode = CommunicationErrorType.SuccessForDLMS;

            //    }
            //    checkSerial.CloseSession();


            //    if (result.ErrorCode != CommunicationErrorType.SuccessForDLMS)
            //    {
            //        if (ConfigInfo.GetLocalMode().Equals("Optical"))
            //        {
            //            channelDetail.BaudRate = "300";
            //            channelDetail.InitialBaudRate = "300";
            //        }
            //        else
            //        {
            //            channelDetail.BaudRate = initialBaud;
            //            channelDetail.InitialBaudRate = initialBaud;
            //        }
            //        channelDetail.Parity = "Even";
            //        channelDetail.StopBits = "1";
            //        channelDetail.DataBits = "7";
            //        channelDetail.ComPort = comPort;
            //        channelDetail.ResponseTimeout = 2000;
            //        channelDetail.InterCharacterDelay = 1800;
            //        channelDetail.NumberOfRetry = 1;
            //        channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
            //        checkSerial = new Serial(channelDetail);

            //        if (checkSerial.OpenSession())
            //        {

            //            byte[] data = { 0x2F, 0x3F, 0x21, 0x0D, 0x0A };
            //            result = checkSerial.Send(data, 5);
            //            if (result.RecieveDataLength > 5)
            //            {


            //                byte[] breakcmd = { 0x01, 0x42, 0x30, 0x03, 0x71 };
            //                Result result1 = checkSerial.Send(breakcmd, 5);
            //                checkSerial.DelayExecution(200);
            //                result1 = checkSerial.Send(breakcmd, 5);
            //                checkSerial.DelayExecution(200);

            //                result.ErrorCode = CommunicationErrorType.SuccessForIEC;
            //                if (ConfigInfo.GetLocalMode().Equals("Optical"))
            //                {
            //                    ConfigSettings.ChangeNode("BaudRate", "300");
            //                }
            //                checkSerial.CloseSession();
            //            }
            //            else
            //            {
            //                checkSerial.CloseSession();
            //                channelDetail.BaudRate = "9600";
            //                channelDetail.InitialBaudRate = "9600";
            //                channelDetail.Parity = "None";
            //                channelDetail.StopBits = "1";
            //                channelDetail.DataBits = "8";
            //                channelDetail.ComPort = comPort;
            //                channelDetail.ResponseTimeout = 2000;
            //                channelDetail.InterCharacterDelay = 1800;
            //                channelDetail.NumberOfRetry = 1;
            //                channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
            //                checkSerial = new Serial(channelDetail);

            //                if (checkSerial.OpenSession())
            //                {

            //                    byte[] data1 = { 0x2F, 0x3F, 0x21, 0x0D, 0x0A };
            //                    result = checkSerial.Send(data1, 5);
            //                    if (result.RecieveDataLength > 5)
            //                    {

            //                        byte[] breakcmd = { 0x01, 0x42, 0x30, 0x03, 0x71 };
            //                        Result result1 = checkSerial.Send(breakcmd, 5);
            //                        checkSerial.DelayExecution(200);
            //                        result1 = checkSerial.Send(breakcmd, 5);
            //                        checkSerial.DelayExecution(200);
            //                        result.ErrorCode = CommunicationErrorType.SuccessForIECSP;
            //                    }
            //                    ConfigSettings.ChangeNode("BaudRate", "9600");
            //                    checkSerial.CloseSession();
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    result.ErrorCode = CommunicationErrorType.PortInvalid;
            //}
            #endregion

        }

        //public Result CheckSession(string comPort, string initialBaud, byte secMechanism)
        //{
        //    Result result = new Result();

        //    try
        //    {
        //        ChannelDetail channelDetail = new ChannelDetail();
        //        channelDetail.BaudRate = "9600";
        //        channelDetail.InitialBaudRate = "9600";
        //        channelDetail.Parity = "None";
        //        channelDetail.StopBits = "1";
        //        channelDetail.DataBits = "8";
        //        channelDetail.ComPort = comPort;
        //        channelDetail.ResponseTimeout = 3000;
        //        channelDetail.InterCharacterDelay = 2800;
        //        channelDetail.NumberOfRetry = 1;
        //        channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
        //        Serial checkSerial = new Serial(channelDetail);
        //        if (checkSerial.OpenSession())
        //        {
        //            byte[] connectData = { 0x7E, 0xA0, 0x0A, 0x00, 0x02, 0x04, 0x01, 0x21, 0x53, 0x7B, 0x73, 0x7E };
        //            if (secMechanism == 0x01)
        //            {
        //                connectData[7] = 0x41;
        //                connectData[9] = 0x2E;
        //                connectData[10] = 0x16;
        //            }
        //            else if (secMechanism == 0x02)
        //            {
        //                connectData[7] = 0x61;
        //                connectData[9] = 0x1D;
        //                connectData[10] = 0x35;
        //            }
        //            result = checkSerial.Send(connectData, 12);
        //            if (result.RecieveDataLength > 11)
        //            {

        //                result.ErrorCode = CommunicationErrorType.SuccessForDLMS;

        //            }
        //            checkSerial.CloseSession();


        //            if (result.ErrorCode != CommunicationErrorType.SuccessForDLMS)
        //            {
        //                channelDetail.BaudRate = initialBaud;
        //                channelDetail.InitialBaudRate = initialBaud;
        //                channelDetail.Parity = "Even";
        //                channelDetail.StopBits = "1";
        //                channelDetail.DataBits = "7";
        //                channelDetail.ComPort = comPort;
        //                channelDetail.ResponseTimeout = 2000;
        //                channelDetail.InterCharacterDelay = 1800;
        //                channelDetail.NumberOfRetry = 1;
        //                channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
        //                checkSerial = new Serial(channelDetail);

        //                if (checkSerial.OpenSession())
        //                {

        //                    byte[] data = { 0x2F, 0x3F, 0x21, 0x0D, 0x0A };
        //                    result = checkSerial.Send(data, 5);
        //                    if (result.RecieveDataLength > 5)
        //                    {
        //                        result.ErrorCode = CommunicationErrorType.SuccessForIEC;
        //                    }
        //                    checkSerial.CloseSession();
        //                }
        //            }

        //        }
        //        else
        //        {
        //            result.ErrorCode = CommunicationErrorType.PortInvalid;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return result;
        //}

        /// <summary>
        /// Checks Connection for IEC
        /// </summary>
        /// <param name="comPort"></param>
        /// <param name="initialBaud"></param>
        /// <returns></returns>
        public Result CheckCMRICommunicationType(byte command, string comPort, string initialBaud)
        {
            Result result = new Result();
            try
            {
                ChannelDetail channelDetail = new ChannelDetail();
                channelDetail.BaudRate = initialBaud;
                channelDetail.InitialBaudRate = initialBaud;
                channelDetail.Parity = "None";
                channelDetail.StopBits = "1";
                channelDetail.DataBits = "8";
                channelDetail.ComPort = comPort;
                channelDetail.ResponseTimeout = 3000;
                channelDetail.InterCharacterDelay = 2800;
                channelDetail.NumberOfRetry = 1;
                channelDetail.ChannelType = CABCommunication.PhysicalLayer.ChannelType.Direct;
                Serial checkSerial = new Serial(channelDetail);
                if (checkSerial.OpenSession())
                {
                    byte[] connectData = { 0x7E, 0xA0, 0x0A, 0x00, 0x02, 0x04, 0x01, 0x21, 0x93, 0xEC, 0x02, 0x7E };
                    connectData[8] = command;
                    result = checkSerial.Send(connectData, 12);
                    if (result.RecieveDataLength > 11)
                    {
                        if (result.RecieveDataBuffer[8] == 0X00)
                        {
                            result.ErrorCode = CommunicationErrorType.SuccessForDLMS;
                        }

                    }
                    checkSerial.CloseSession();
                }
                else
                {
                    result.ErrorCode = CommunicationErrorType.PortInvalid;
                }

            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// Closes communication session with cosem layer
        /// </summary>
        /// <returns></returns>
        public Result CloseSession()
        {
            Result result = new Result();
            try
            {
                result = COSEMLayer.CloseSession();
                PhysicalChannelDetail.CloseSession();
            }
            catch (Exception)
            {
            }
            return result;
        }
        /// <summary>
        /// Closes communication session with cosem layer 
        /// </summary>
        /// <param name="simNumber"></param>
        /// <returns></returns>
        public Result CloseSession(string simNumber)
        {
            Result result = new Result();
            try
            {
                result = COSEMLayer.CloseSession();
            }
            catch (Exception)
            {
            }
            return result;
        }
        /// <summary>
        /// Closes remote communication session with cosem layer.
        /// </summary>
        /// <returns></returns>
        public Result CloseRemoteSession()
        {
            Result result = new Result();
            try
            {
                result = COSEMLayer.CloseSession();
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// Used to read Meter date time
        /// </summary>
        /// <returns></returns>
        public DateTime GetMeterDateTime()
        {
            DateTime meterTime = System.DateTime.Now;
            try
            {
                ProfileCommand profileCommand = new ProfileCommand(08, "00.00.01.00.00.FF", 02);
                Result result = Send(profileCommand);
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    DateTime dateTime = DateTime.MinValue;
                    int year = 0;
                    int dataIndex = 2;
                    year = year | (int)result.RecieveDataBuffer[dataIndex++] << 8;
                    year = year | (int)result.RecieveDataBuffer[dataIndex++];

                    int month = result.RecieveDataBuffer[dataIndex++];
                    int day = result.RecieveDataBuffer[dataIndex++];
                    dataIndex++;
                    int hour = result.RecieveDataBuffer[dataIndex++];
                    int min = result.RecieveDataBuffer[dataIndex++];
                    int sec = result.RecieveDataBuffer[dataIndex++];

                    dateTime = new DateTime(year, month, day, hour, min, sec);
                    meterTime = dateTime;
                }
            }
            catch
            {

            }
            return meterTime;
        }

        /// <summary>
        /// Used to read CMRI ID
        /// </summary>
        /// <returns></returns>
        public String GetCMRIID()
        {
            String cmriID = string.Empty;
            DateTime meterTime = System.DateTime.Now;
            try
            {
                ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.DE.FF", 02);
                Result result = Send(profileCommand);
                if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength > 2)
                {
                    for (int buffIndex = 2; buffIndex < result.RecieveDataLength; buffIndex++)
                    {
                        cmriID = cmriID + Convert.ToChar(result.RecieveDataBuffer[buffIndex]).ToString();
                    }
                }
                else
                {
                    cmriID = "unknown";
                }

            }
            catch
            {
                cmriID = "unknown";
            }
            return cmriID;
        }
        ///// <summary>
        ///// Used to read Meter date time
        ///// </summary>
        ///// <returns></returns>
        //public DateTime GetMeterDateTime(string imei)
        //{
        //    DateTime meterTime = System.DateTime.Now;
        //    try
        //    {
        //        ProfileCommand profileCommand = new ProfileCommand(08, "00.00.01.00.00.FF", 02);
        //        Result result = Send(profileCommand, imei);
        //        if (result.ErrorCode == CommunicationErrorType.Success)
        //        {
        //            DateTime dateTime = DateTime.MinValue;
        //            int year = 0;
        //            int dataIndex = 6;
        //            year = year | (int)result.RecieveDataBuffer[dataIndex++] << 8;
        //            year = year | (int)result.RecieveDataBuffer[dataIndex++];

        //            int month = result.RecieveDataBuffer[dataIndex++];
        //            int day = result.RecieveDataBuffer[dataIndex++];
        //            dataIndex++;
        //            int hour = result.RecieveDataBuffer[dataIndex++];
        //            int min = result.RecieveDataBuffer[dataIndex++];
        //            int sec = result.RecieveDataBuffer[dataIndex++];

        //            dateTime = new DateTime(year, month, day, hour, min, sec);
        //            meterTime = dateTime;
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return meterTime;
        //}




        /// <summary>
        /// Gets Signature data from meter using set of commands that are supported for meter to be read.
        /// If Signature command is not supported in meter then it constructs result having dummy signature data .
        /// </summary>
        /// <returns></returns>
        public String GetSignatureData()
        {
            string meterModel = string.Empty;
            CAB.Framework.Utility.ConfigInfo.SignatureDataLength = string.Empty;
            //int meterModelNumber = NamePlateConstants.PumaLTE650Value;
            ProfileCommand profileCommand = new ProfileCommand();
            Result result = new Result();

            if (ConfigSettings.GetValue("ApplicationContext") == "03")
            {
                profileCommand = new ProfileCommand(01, "00.00.60.80.08.FF", 02);
                 result = Send(profileCommand);
            }
            else
            {
                profileCommand = new ProfileCommand(01, "00.00.60.01.BC.FF", 02);
                 result = Send(profileCommand);
                if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength == 0)
                {
                    profileCommand = new ProfileCommand(01, "00.00.60.80.08.FF", 02);
                    result = Send(profileCommand);
                    if (result != null && result.RecieveDataLength == 0)
                    {
                        profileCommand = new ProfileCommand(01, "00.00.60.80.08.FF", 02);
                        result = Send(profileCommand);
                    }
                    //if (result.RecieveDataLength <= 0) result.ErrorCode = CommunicationErrorType.CosemConnectionFailed;
                    //return "";
                }
            }
              
            if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
            {
                // Get the signature Data Length and octet string from first two byte of the dataBuffer //User Story 478245. Voltage Rating change to 63.5 V for HK meter
                CAB.Framework.Utility.ConfigInfo.SignatureDataLength = string.Empty;
                for (int i = 0; i < 2; i++)
			    {
                    CAB.Framework.Utility.ConfigInfo.SignatureDataLength += result.RecieveDataBuffer[i].ToString("x").PadLeft(2,'0');
			    }

                for (int buffIndex = 2; buffIndex < result.RecieveDataLength; buffIndex++)
                {
                    meterModel = meterModel + Convert.ToChar(result.RecieveDataBuffer[buffIndex]).ToString();
                }
                meterModel = meterModel + "4C";
                
            }
            else
            {
                profileCommand = new ProfileCommand(01, "00.00.60.00.A6.FF", 02);
                result = COSEMLayer.Send(profileCommand);
                if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
                {

                    if (result.RecieveDataBuffer[1] == 0x01)
                    {
                        meterModel = "**1.66240010060WC";
                    }
                    else if (result.RecieveDataBuffer[1] == 0x02)
                    {
                        meterModel = "**4.64240010060LT";
                    }
                    else if (result.RecieveDataBuffer[1] == 0x03)
                    {
                        meterModel = "**0.00240010060HT";
                    }
                    else if (result.RecieveDataBuffer[1] == 0x04)
                    {
                        meterModel = "**0.00240010060LC";
                    }
                    meterModel = meterModel + "4RSL";

                }
                else
                {
                  profileCommand = new ProfileCommand(01, "01.00.00.04.03.FF", 02);
                   
                    result = Send(profileCommand);

                    if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
                    {
                        if (result.RecieveDataBuffer[1] >= 100)
                        {
                            meterModel = "**5.18240005010HM";
                        }
                        else
                        {
                            meterModel = "**2.21240010060LT";
                        }
                    }
                    else
                    {
                        meterModel = "**2.21240010060WC";
                    }
                    meterModel = meterModel + "4RSL";

                   
                }
            }
            //********** To get meter manufacturer name
           
            profileCommand = new ProfileCommand(01, "00.00.60.01.01.FF", 02);
            result = Send(profileCommand);

            if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
            {
                string Tempsignature = string.Empty;
                for (int i = 0; i < result.RecieveDataLength; i++)
                {
                    Tempsignature += result.RecieveDataBuffer[i].ToString("X").PadLeft(2, '0');
                }

                if ((!Tempsignature.Contains("4C616E646973")) && (!Tempsignature.Contains("4C475A"))) //if not landis gyr /LGZ meter
                {
                    if (ConfigSettings.GetValue("OtherManufacture") == "TRUE")
                        meterModel = "Non-Landis+GyrMeter";
                    else
                    {
                        if (MessageBox.Show("Non Landis+Gyr Make Meter Detected !" + "\n" + "Are you Sure Connected Meter is Landis+Gyr Make ?", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                            return "";
                    }
                           
                }
            }

            return meterModel;
        }

        /// <summary>
        /// Gets Meter Type data.
        /// </summary>
        /// <returns></returns>
        public int GetMeterType()
        {
            int meterType = 0;
            //int meterModelNumber = NamePlateConstants.PumaLTE650Value;
            ProfileCommand profileCommand = new ProfileCommand(01, "00.00.5E.5B.09.FF", 02);
            Result result = Send(profileCommand);
            if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
            {
                meterType = result.RecieveDataBuffer[1];
            }

            return meterType;
        }
         /// <summary>
        /// Set Baud Rate.
        /// </summary>
        /// <returns></returns>
        public bool SetBaudRate(byte baudRate)
        {
            bool success = false;
            //int meterModelNumber = NamePlateConstants.PumaLTE650Value;
            ProfileCommand profileCommand = new ProfileCommand(0x17, "00.00.16.00.00.FF", 0x02);
            profileCommand.Action = ActionType.WRITE;
            profileCommand.ClassName = "CAB.E650MeterConfiguration.BaudRate,E650MeterConfiguration";
            profileCommand.WriteDataBuffer = baudRate;
            Result result = Send(profileCommand);
            if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
            {
                COSEMLayer.SetBaud(baudRate);
                DelayExecution(1000);
            }
            return success;
        }

        /// <summary>
        /// Delays the execution 
        /// </summary>
        /// <param name="millisecondsTime"></param>
        public void DelayExecution(int millisecondsTime)
        {
            DateTime end = DateTime.UtcNow.AddMilliseconds(millisecondsTime);
            while (DateTime.UtcNow < end)
            {
            }
        }

        /// <summary>
        /// sends command to meter & fills entity.
        /// </summary>
        /// <returns></returns>
        public List<byte> GetPermissionEntity()
        {
            List<byte> permissionData = new List<byte>();
            ProfileCommand profileCommand = new ProfileCommand(15, "0.0.28.0.0.FF", 02);
            Result result = Send(profileCommand);
            if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
            {
                permissionData = result.RecieveDataBuffer;
            }
            return permissionData;
        }
        /// <summary>
        /// Sends classId , Obis code and attribute to Cosem layer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result Send(ProfileCommand profileCommand)
        {
            Result result = new Result();

            try
            {
                if (profileCommand.ClassId == 0xFF || profileCommand.ClassId == 0xFE || profileCommand.ClassId == 0xFD)
                {
                    byte[] command = GetCommand(profileCommand);
                    result = PhysicalChannelDetail.Send(command, command.Length);
                }
                else
                {
                    result = COSEMLayer.Send(profileCommand);
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Sends classId , Obis code and attribute to Cosem layer as Block request
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result SendWriteBlock(ProfileCommand profileCommand)
        {
            Result result = new Result();

            try
            {
                 result = COSEMLayer.SendWriteBlock(profileCommand);
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Setting SAP ID for HDLC layer.Mainly used for CMRI data Read Communication.
        /// </summary>
        /// <param name="serverSAP"></param>
        public void SetSAP(byte serverSAP)
        {
            HDLCLayer.SetSAP(serverSAP);
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the FD Command
        /// </summary>
        /// <param name="profileCommand"></param>
        /// <returns></returns>
        private byte[] GetCommand(ProfileCommand profileCommand)
        {
            string[] fdCommand = profileCommand.ObisCode.Split('.');
            List<byte> command = new List<byte>();
            command = profileCommand.MeterID.GetRange(0, profileCommand.MeterID.Count);
            foreach (string item in fdCommand)
            {
                if (!(item.ToUpper() == "METERID"))
                {
                    command.Add(Convert.ToByte(item.Trim(), 16));
                }
            }
            return command.ToArray();
        }

        #endregion


        public int GetDisplayProgrammingVariant()
        {
            try
            {
                ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.BC.FF", 02);
                Result result = Send(profileCommand);
                if (result != null && result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataLength != 0)
                {
                    int startDataindx = 0;
                    int startIDX = 23;
                    int EndIDX = 1;
                    string infodata = string.Empty;

                    if (result.RecieveDataBuffer[startDataindx + 1] >= 0x3F) { startIDX = 25; EndIDX = 1; } //--------For Meters like Net Metering having 64 Byte Signature Info
                    else if (result.RecieveDataBuffer[startDataindx + 1] >= 0x1E) { startIDX = 24; EndIDX = 1; } //--------For Meters like HTCT Variants with voltage 63.5V having 30 Byte Signature Info
                    if (result.RecieveDataBuffer[startDataindx] == 0x09 || result.RecieveDataBuffer[startDataindx] == 0x0A)
                    {
                        for (int buffIndex = 2; buffIndex < result.RecieveDataLength; buffIndex++)
                        {
                            infodata = infodata + Convert.ToChar(result.RecieveDataBuffer[buffIndex]).ToString();
                        }
                    }
                    else { return 0; }//---Ruby Old Meters No Meter Info
                    if (infodata.Trim().Length < startIDX + EndIDX) return 0;
                    else
                    {
                        string dispVariant = infodata.Substring(startIDX, EndIDX);
                        if (dispVariant == "2")
                        {
                            CAB.Framework.Utility.ConfigInfo.DisplayProgrammingVariant = CAB.Framework.DisplayProgrammingTypes.TwoByte;
                            return (int)CAB.Framework.DisplayProgrammingTypes.TwoByte;
                        }
                        else
                        {
                            CAB.Framework.Utility.ConfigInfo.DisplayProgrammingVariant = CAB.Framework.DisplayProgrammingTypes.OneByte;
                            return (int)CAB.Framework.DisplayProgrammingTypes.OneByte;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
