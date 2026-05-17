#region Namespaces
using System;
using System.Collections.Generic;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using CAB.Framework.Utility;
#endregion


namespace CABCommunication.DataLinkLayer
{
    /// <summary>
    /// This interface defines operations that communication layer will 
    /// be using to send requests/commands to the connected physical channel.
    /// </summary>
    public class HDLCLayer
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        private byte hdlcIndex = 0;
        private byte controlByte = 0x00;
        private byte rframecontrolByte = 0x00;
        private const long MaxHDLCPacketSize = 1056;
        private int[] ucFcs = new int[2];
        private int printFcs16 = 0xFFFF;
        private int preetyGoodPrivacyFcs16 = 0xF0B8;
        private int[] uifcstab = 
                        {   
                            0x0000, 0x1189, 0x2312,	0x329b,	0x4624,	0x57ad,	0x6536,	0x74bf,
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

        public bool currentPktSegmentationStatus = false;
        public bool prevPktSegmentationStatus = false;
        public bool blockWithSegmentation = false;
        public bool lastBlockWithSegmentation = false;
        #endregion

        ChannelInformation channelInfo = new ChannelInformation();
        
       
        #region Properties
        /// <summary>
        /// get/set Server SAP
        /// </summary>
        private int ServerSAP { get; set; }

        /// <summary>
        /// get/set Server mac address in lower
        /// </summary>
        private int ServerLowerMacAddress { get; set; }
        private bool DedicatedkeyStatus { get; set; }
        /// <summary>
        /// get/set Client SAP
        /// </summary>

        private int ClientSAP { get; set; }


        public IPhysicalChannel Serial { get; set; }

        public byte RRFrameControlByte
        {
            get;
            set;
        }

        #endregion

        #region Constructor
        public HDLCLayer()
        {
            ServerSAP = 0x01;
            //ServerLowerMacAddress = 0x100;
            ServerLowerMacAddress = Convert.ToInt16(ConfigSettings.GetValue("RS485DeviceAddress"));
            ClientSAP = 0x20;

        }
        //public HDLCLayer(Serial serial, byte clientSAP)
        //{
        //    ServerSAP = 0x01;
        //    ServerLowerMacAddress = 0x100;
        //    ClientSAP = clientSAP;
        //    Serial = serial;
        //}
        public HDLCLayer(IPhysicalChannel channel, byte clientSAP)
        {
            ServerSAP = 0x01;
            
             // ServerLowerMacAddress = 0x100;
             ServerLowerMacAddress = Convert.ToInt16(ConfigSettings.GetValue("RS485DeviceAddress"));
           ClientSAP = clientSAP;
           Serial = channel;
          // DedicatedkeyStatus = Convert.ToBoolean(Convert.ToInt32(ConfigSettings.GetValue("DedicatedKey")));            
        }
        public HDLCLayer( byte clientSAP)
        {
           ServerSAP = 0x01;
           // ServerLowerMacAddress = 0x100;
           ServerLowerMacAddress = Convert.ToInt16(ConfigSettings.GetValue("RS485DeviceAddress"));
           ClientSAP = clientSAP;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baud"></param>
        public void SetBaud(byte baud)
        {
            Serial.SetBaud(baud);
        }

        /// <summary>
        /// Sends HDLC packets to serial port, takes command atype as argument
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Result Send(CommandType commandType)
        {
            Result result = null;

            try
            {
                byte[] hdlcBuffer = GetHDLCCommandPacket(commandType);               
                result = Serial.Send(hdlcBuffer, hdlcIndex);
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    result.ErrorCode = CheckHDLCResponse(result.RecieveDataBuffer.ToArray(), commandType);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {

                        SetInitialI();

                    }
                }

            }
            catch
            {
                throw;
            }
            return result;
        }
     
        /// <summary>
        /// Sends HDLC packets to serial port, takes byte buffer and command type as argument
        /// </summary>
        /// <param name="cosemBuffer"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Result Send(List<byte> cosemBuffer,CommandType commandType)
        {
            Result result = null;
            try
            {
                byte[] hdlcBuffer;
                if (commandType == CommandType.I)
                {
                    hdlcBuffer = GetHDLCDataPacket(cosemBuffer);
                }
                else 
                {
                    hdlcBuffer = GetHDLCCommandPacket(commandType);
                }
                result = Serial.Send(hdlcBuffer, hdlcIndex);
                if (result!=null && result.ErrorCode == CommunicationErrorType.Success)
                {
                    IncRecieve();
                    //byte[] SegmentDataList = result.RecieveDataBuffer.ToArray();
                    result.ErrorCode = CheckHDLCResponse(result.RecieveDataBuffer.ToArray(), commandType);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        if (prevPktSegmentationStatus && result.RecieveDataBuffer[11] != 0xE6 && result.RecieveDataBuffer[12] != 0xE7)
                        {
                            //result.RecieveDataBuffer = result.RecieveDataBuffer.GetRange(0, Convert.ToInt32(result.RecieveDataLength) - 1);
                            result.RecieveDataBuffer = result.RecieveDataBuffer.GetRange(11, Convert.ToInt32(result.RecieveDataLength) - 14);
                        }
                        else
                        {
                            result.RecieveDataBuffer = result.RecieveDataBuffer.GetRange(14, Convert.ToInt32(result.RecieveDataLength) - 17);
                        }
                    }
                }

            }
            catch 
            {
                throw;
            }
            return result;
        }
      
        
        /// <summary>
        /// Setting SAP ID for HDLC layer.Mainly used for CMRI data Read Communication.
        /// </summary>
        /// <param name="serverSAP"></param>
        public void SetSAP(byte serverSAP)
        {
            this.ServerSAP = serverSAP;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods

        /// <summary>
        /// Check Start/end tag, Check FCS , Check destination Address and Check command Byte
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private CommunicationErrorType CheckHDLCResponse(byte[] buffer, CommandType commandType)
        {
            CommunicationErrorType errorCode = CommunicationErrorType.Success;
            if (!CheckStartEndTag(buffer))
            {
                errorCode = CommunicationErrorType.InvalidTag ;
            }
            else
            {
                if (!CheckFCS(buffer))
                {
                    errorCode = CommunicationErrorType.InvalidFCS;
                }
                else
                {
                    if (!CheckServerSAP(buffer, ClientSAP))
                    {
                        errorCode = CommunicationErrorType.InvalidServerAddress;
                    }
                    else
                    {
                        if (!CheckCommand(buffer, commandType))
                        {
                            errorCode = CommunicationErrorType.InvalidCommand;
                        }                        
                    }
                    //Check Segmentation bit
                    currentPktSegmentationStatus = IsSegmentationBitHigh(buffer);
                    
                }
            }          
            
            return errorCode;
        }

        // Check Segmentation bit
        public bool IsSegmentationBitHigh(byte[] buffer)
        {
            byte isSegmentBit = Convert.ToByte((buffer[1] & 0x8) >> 3);

            if (isSegmentBit > 0)
            {
                // mohsin
                if(RRFrameControlByte < 1)
                RRFrameControlByte = controlByte;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates HDLC data packets to send to serial port
        /// </summary>
        /// <param name="cosemBuffer"></param>
        /// <returns></returns>
        public byte[] GetHDLCDataPacket(List<byte> cosemBuffer)
        {
            byte[] HDLCCommand = new byte[MaxHDLCPacketSize];

            hdlcIndex = 0;
            try
            {
                hdlcIndex = Add7E(HDLCCommand, hdlcIndex);
                hdlcIndex = AddHDLCFrameTag(HDLCCommand, hdlcIndex);
                hdlcIndex = AddServerSAP(HDLCCommand, hdlcIndex, ServerSAP, ServerLowerMacAddress);
                hdlcIndex = AddClientSAP(HDLCCommand, hdlcIndex, ClientSAP);
                //if (controlByte != 0x10)
                //{
                //    IncSend();
                //}
                if (controlByte == 0x00) controlByte = 0x10;
                else IncSend();

                 
                hdlcIndex = AddCmdByte(HDLCCommand, hdlcIndex);
                hdlcIndex = AddBlankFCS(HDLCCommand, hdlcIndex);
                hdlcIndex = AddLLCByte(HDLCCommand, hdlcIndex);              
               
                for (int cosemIndex = 0; cosemIndex < cosemBuffer.Count; cosemIndex++)
                {
                    HDLCCommand[hdlcIndex++] = cosemBuffer[cosemIndex];
                }
                hdlcIndex = AddBlankFCS(HDLCCommand, hdlcIndex);
                FillLength(HDLCCommand, hdlcIndex);
                GenerateFCS(HDLCCommand, 1, 8);
                FillFCS(HDLCCommand, 9, 10);
                GenerateFCS(HDLCCommand, 1, hdlcIndex - 3);
                FillFCS(HDLCCommand, hdlcIndex - 2, hdlcIndex - 1);
                hdlcIndex = Add7E(HDLCCommand, hdlcIndex);
            }
            catch
            {
                throw;
            }
            return HDLCCommand;
        }
        /// <summary>
        /// Used to get HDLC command packets
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public   byte[] GetHDLCCommandPacket(CommandType commandType)
        {
            byte[] HDLCCommand = new byte[MaxHDLCPacketSize];

            hdlcIndex = 0;
            try
            {
                hdlcIndex = 0;
                hdlcIndex = Add7E(HDLCCommand, hdlcIndex);
                hdlcIndex = AddHDLCFrameTag(HDLCCommand, hdlcIndex);
                hdlcIndex = AddServerSAP(HDLCCommand, hdlcIndex, ServerSAP, ServerLowerMacAddress);
                hdlcIndex = AddClientSAP(HDLCCommand, hdlcIndex, ClientSAP);
                // Code for segmentation
                if (commandType == CommandType.Nothing && currentPktSegmentationStatus == true)
                {
                    IncSegmentationSend();
                }
                else
                    controlByte = (byte)commandType;  
             
                hdlcIndex = AddCmdByte(HDLCCommand, hdlcIndex);
                hdlcIndex = AddBlankFCS(HDLCCommand, hdlcIndex);
                FillLength(HDLCCommand, hdlcIndex);
                GenerateFCS(HDLCCommand, 1, 8);
                FillFCS(HDLCCommand, 9, 10);
                hdlcIndex = Add7E(HDLCCommand, hdlcIndex);

            }
            catch 
            {
                throw;
            }
            return HDLCCommand;
        }

        /// <summary>
        /// Calculate a new FCS given the current FCS and New data
        /// </summary>
        /// <param name="uiLocal_fcs">Initial FCS</param>
        /// <param name="Buffer">Buffer</param>
        /// <param name="endlen">Length</param>
        /// <returns>New fcs of Integer Type</returns>
        private int PreetyGoodPrivacyFcs(int localFcs, byte[] buffer, int endLenth)
        {
            int counter = 1;

            while (endLenth > 0)
            {
                localFcs = (localFcs >> 8) ^ uifcstab[(localFcs ^ buffer[counter++]) & 0xff];
                endLenth--;
            }
            return (localFcs);
        }
        /// <summary>
        /// Check FCS is correct or Not
        /// </summary>
        /// <param name="RecvBuffer"></param>
        /// <returns>True for correct FCS or False for wrong FCS</returns>
        private bool CheckFCS(byte[] recvBuffer)
        {           
            int nHDLCPktLength = ((recvBuffer[1] & 0x07) << 8) | (int)recvBuffer[2];
            //int nHDLCPktLength = ((recvBuffer[1] & 0x0F) << 8) | (int)recvBuffer[2];
            int nHCSindex = 8;                          //Depends on Address Byte Supported Need Change
            if (GenerateFCS(recvBuffer, 1, nHCSindex))    //hcs
            {
                if ((recvBuffer[nHCSindex + 1] == ucFcs[0]) && (recvBuffer[nHCSindex + 2] == ucFcs[1]))
                {
                    if (GenerateFCS(recvBuffer, 1, (nHDLCPktLength - 2)))    //FCS
                    {
                        if ((recvBuffer[nHDLCPktLength - 1] == ucFcs[0]) && (recvBuffer[nHDLCPktLength] == ucFcs[1]))
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
        /// <summary>
        /// Generate a new FCS for Buffer data
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="stlen"></param>
        /// <param name="endlen"></param>
        /// <returns>True or False and FCS in ucFcs[0] and ucFcs[1]</returns>
        private bool GenerateFCS(byte[] buffer, int stlen, int endlen)
        {
            int uitrialfcs;
            int[] ucCalcFcs = new int[2];
            int uiLocal_fcs;
            int counter = 1;

            ucFcs[0] = 0x00;
            ucFcs[1] = 0x00;
            uiLocal_fcs = printFcs16;

            uitrialfcs = PreetyGoodPrivacyFcs(printFcs16, buffer, endlen);
            uitrialfcs ^= 0xffff;


            ucCalcFcs[0] = Convert.ToByte(uitrialfcs & 0x00ff);
            ucCalcFcs[1] = Convert.ToByte((uitrialfcs >> 8) & 0x00ff);

            while (endlen > 0)
            {
                uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ buffer[counter++]) & 0xff];
                endlen--;
            }

            uiLocal_fcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ ucCalcFcs[0]) & 0xff];
            uitrialfcs = (uiLocal_fcs >> 8) ^ uifcstab[(uiLocal_fcs ^ ucCalcFcs[1]) & 0xff];


            if (uitrialfcs == preetyGoodPrivacyFcs16)
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
        /// <summary>
        /// Appends LLC bytes
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        private byte AddLLCByte(byte[] buffer, byte bufferIndex)
        {
            buffer[bufferIndex++] = 0xE6;
            buffer[bufferIndex++] = 0xE6;
            buffer[bufferIndex++] = 0x00;
            return bufferIndex;
        }

        /// <summary>
        ///  Filling FCS bytes to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="upIndex"></param>
        /// <param name="lowIndex"></param>
        private void FillFCS(byte[] buffer, int upIndex, int lowIndex)
        {
            buffer[upIndex] = Convert.ToByte(ucFcs[0]);
            buffer[lowIndex] = Convert.ToByte(ucFcs[1]);
        }

        /// <summary>
        /// Checking the Start and End tag
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private bool CheckStartEndTag(byte[] buffer)
        {
            //int nHDLCPktLength = ((buffer[1] & 0x0F) << 8) | (int)buffer[2];
            int nHDLCPktLength = ((buffer[1] & 0x07) << 8) | (int)buffer[2];

            if (buffer[0] == 0x7E && buffer[nHDLCPktLength + 1] == 0x7E)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checking the Server SAP
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nClientSAP"></param>
        /// <returns></returns>
        private bool CheckServerSAP(byte[] buffer, int clientSAP)
        {
            int tempBuffer = 0;

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(clientSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            tempBuffer = Convert.ToByte(tempBuffer & 0x00FF);
            tempBuffer = Convert.ToByte(tempBuffer | 0x01);
            if (buffer[3] == tempBuffer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checking the command 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nCommandType"></param>
        /// <returns></returns>
        private bool CheckCommand(byte[] buffer, CommandType commandType)
        {
            bool erroCode = false;
            if (commandType == CommandType.I)
            {
                if (buffer[8] == controlByte || currentPktSegmentationStatus)
                {
                    erroCode = true;
                }
                else
                {
                    erroCode = true;
                }
            }
            else if(commandType == CommandType.Nothing)
            {
                if (buffer[8] == controlByte || currentPktSegmentationStatus)
                {
                    erroCode = true;
                }
                else
                {
                    erroCode = true;
                }
            }
            else 
            {

                if (buffer[8] == (byte)CommandType.UA)
                {
                    erroCode = true;
                }
            }

            return erroCode;
        }
      

        /// <summary>
        /// Seting last status bit
        /// </summary>
        /// <param name="nStatus"></param>
        private void SetLastBit(byte status)
        {
            if (status == 0x01)
                controlByte = Convert.ToByte(controlByte | 0x01);
            else
                controlByte = Convert.ToByte(controlByte & 0xFE);
        }
        
        /// <summary>
        /// Increment recieve Sequence number
        /// </summary>
        private void IncRecieve()
        {
            rframecontrolByte = controlByte;
            int seqCounter = Convert.ToByte(controlByte & 0xE0);

            seqCounter = Convert.ToByte(seqCounter >> 5);
            seqCounter = Convert.ToByte(seqCounter + 1);
            seqCounter = seqCounter << 5;
            seqCounter = seqCounter & 0x00FF;

            controlByte = Convert.ToByte(controlByte & 0x1F);
            controlByte = Convert.ToByte(controlByte | seqCounter);
        }
        
        /// <summary>
        /// Increment Send Sequence number
        /// </summary>
        private void IncSend()
        {
            int seqCounter = 0;

            // identify is command byte is for I type or RR type
            byte isIresponse = (byte)(controlByte & 0x01);


            if (!currentPktSegmentationStatus && RRFrameControlByte != 0)
            {
                //RRFrameControlByte = 0x52;
                //controlByte = 0x5a;

                byte b1 = (byte)(RRFrameControlByte & 0x0F);
                byte b2 = (byte)(controlByte & 0xF0);

                controlByte = (byte)(b1 | b2);
                RRFrameControlByte = 0;
            }


            seqCounter = Convert.ToByte(controlByte & 0x0E);

            seqCounter = Convert.ToByte(seqCounter >> 1);
            seqCounter = Convert.ToByte(seqCounter + 1);

            seqCounter = seqCounter << 1;
            seqCounter = seqCounter & 0x00FF;

            controlByte = Convert.ToByte(controlByte & 0xF1);
            controlByte = Convert.ToByte(controlByte | seqCounter);
        }

        /// <summary>
        /// Increment Send Sequence number for Segmentation
        /// </summary>
        private void IncSegmentationSend()
        {
            int seqCounter = Convert.ToByte(rframecontrolByte & 0xE0);

            seqCounter = Convert.ToByte(seqCounter >> 5);
            seqCounter = Convert.ToByte(seqCounter + 1);

            seqCounter = seqCounter << 5;
            seqCounter = seqCounter & 0x00FF;

            controlByte = Convert.ToByte(controlByte & 0xF1);
            controlByte = Convert.ToByte(controlByte | seqCounter);
            controlByte = (byte)(controlByte | 1);
            
        }                      

        /// <summary>
        /// Set Commmand Byte as Initial I
        /// </summary>
        private void SetInitialI()
        {
            controlByte = 0x00;
            // Mohsin: change for segementation
            RRFrameControlByte = 0;
        }

      
        /// <summary>
        /// add 0x7E to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        private byte Add7E(byte[] buffer, byte bufferIndex)
        {
            buffer[bufferIndex++] = 0x7E;
            return bufferIndex;
        }

        /// <summary>
        /// add 0xA0 and space for Length to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        private byte AddHDLCFrameTag(byte[] uffer, byte bufferIndex)
        {
            uffer[bufferIndex++] = 0xA0;
            uffer[bufferIndex++] = 0x00;
            return bufferIndex;
        }

        /// <summary>
        /// Conversion of Destination Address Values and Filling into Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="nServerSAP"></param>
        /// <param name="ServerLowerMACAddress"></param>
        /// <returns></returns>
        private byte AddServerSAP(byte[] buffer, byte bufferIndex, int serverSAP, int serverLowerMACAddress)
        {
            int tempBuffer = 0;

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(serverSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            buffer[bufferIndex + 1] = Convert.ToByte(tempBuffer & 0x00FF);

            serverSAP = serverSAP << 2;
            buffer[bufferIndex] = Convert.ToByte((serverSAP >> 8) & 0x00FF);
            buffer[bufferIndex] = Convert.ToByte(buffer[bufferIndex] & 0x00FF);

            bufferIndex = Convert.ToByte(bufferIndex + 2);

            tempBuffer = 0;
            tempBuffer = Convert.ToByte(serverLowerMACAddress & 0x00FF);
            tempBuffer = tempBuffer << 1;
            tempBuffer = Convert.ToByte(tempBuffer & 0x00FF);
            buffer[bufferIndex + 1] = Convert.ToByte(tempBuffer | 0x01);

            serverLowerMACAddress = serverLowerMACAddress << 2;
            buffer[bufferIndex] = Convert.ToByte((serverLowerMACAddress >> 8) & 0x00FF);
            buffer[bufferIndex] = Convert.ToByte(buffer[bufferIndex] & 0x00FE);

            bufferIndex = Convert.ToByte(bufferIndex + 2);

            return bufferIndex;

        }

        /// <summary>
        /// Conversion of Client Address Values and Filling into Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="nClientSAP"></param>
        /// <returns></returns>
        private byte AddClientSAP(byte[] buffer, byte bufferIndex, int clientSAP)
        {
            int tempBuffer = 0;
            tempBuffer = Convert.ToByte(clientSAP & 0x00FF);
            tempBuffer = tempBuffer << 1;
            buffer[bufferIndex] = Convert.ToByte(tempBuffer & 0x00FF);
            buffer[bufferIndex] = Convert.ToByte(tempBuffer | 0x01);
            bufferIndex++;
            return bufferIndex;
        }

        /// <summary>
        /// Fill Space for FCS/HCS in Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        private byte AddBlankFCS(byte[] buffer, byte bufferIndex)
        {
            buffer[bufferIndex++] = 0x00;
            buffer[bufferIndex++] = 0x00;
            return bufferIndex;
        }

        /// <summary>
        /// Fill Command Byte in buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <returns></returns>
        private byte AddCmdByte(byte[] buffer, byte bufferIndex)
        {
            buffer[bufferIndex++] = controlByte;
            return bufferIndex;
        }
        
        /// <summary>
        /// Conversion of Client Address Values and Filling into Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        private void FillLength(byte[] buffer, byte bufferIndex)
        {
            buffer[2] = Convert.ToByte(bufferIndex - 1);      

        }

        public int DedicatedCommand(byte[] Buffer, int nBufferIndex, string InputType, int bytelength, byte DataRequestType)
        {
            if (DedicatedkeyStatus == true && InputType == "Read")
                Buffer[nBufferIndex++] = 0xD0;
            else if (DedicatedkeyStatus == false && InputType == "Read")
                Buffer[nBufferIndex++] = 0xC8;
            else if (DedicatedkeyStatus == true && InputType == "Write")
            {
                if (DataRequestType == 0xC3) Buffer[nBufferIndex++] = 0xD3; //Action Request
                else Buffer[nBufferIndex++] = 0xD1;
            }
            else if (DedicatedkeyStatus == false && InputType == "Write")
            {
                if (DataRequestType == 0xC3) Buffer[nBufferIndex++] = 0xCB; //Action Request
                else Buffer[nBufferIndex++] = 0xC9;
            }
            return nBufferIndex;
        }





        #endregion
    }
}
