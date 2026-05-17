#region Namespaces
using System;
using System.Collections.Generic;
using CABCommunication.Common;
using CABCommunication.DataLinkLayer;
using CAB.E650MeterConfiguration;
using CAB.Framework;
using System.Threading;
using System.Windows.Forms;
using CAB.Framework.Utility;
using ManagedMath;
using DLMSLIB;
using System.Text;
using System.Linq;
using CABCommunication.PhysicalLayer;
using SerialCommunication;
using Utilities;
#endregion


namespace CABCommunication.ApplicationLayer
{
    /// <summary>
    /// This class defines operations that communication layer will 
    /// be using to send requests/commands to the connected physical channel.
    /// </summary>
    //public static class GlobalObjects
    //{
    //    public static SerialComm objSerialComm = new SerialComm();
    //    public static HDLCLIB objHDLCLIB = new HDLCLIB();
    //    public static COSEMLIB objCOSEMLIB = new COSEMLIB();
        
    //}
   public class COSEMLayer
    {
      // public IPhysicalChannel Serial { get; set; }
      Class1 ManageObj = new Class1();
       long InitializationCounter=0;
       public string DedKeystr = string.Empty;
       public byte[] DedicatedKey = new byte[16];
       // public static GlobalFunctions objGlobalObject = new GlobalFunctions();
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
        #region Nested Types
        #endregion

        #region Constants and Variables
        /// <summary>
        /// 
        /// </summary>
        private const long MaxCOSEMPakcetSize = 1056;
        /// <summary>
        /// 
        /// </summary>
        private int blockNumber = 0x00;
        /// <summary>
        /// 
        /// </summary>
        private const string DefaultPassword = "11111111";
        /// <summary>
        /// 
        /// </summary>
        private const string DefaultHLSKey = "2222222222222222";

        /// <summary>
        /// 
        /// </summary>
        private int blockIndex = 0x00; 
       
        /// <summary>
        /// 
        /// </summary>
     
        List<byte> cosemBlockBuffer ;

        /// <summary>
        /// 
        /// </summary>
        private int negotiatedPDUSize;

        /// <summary>
        /// 
        /// </summary>
        private List<byte> blockWriteBuffer;
        #endregion

        #region Properties
        /// <summary>
        /// get/set HDLC layer object
        /// </summary>
        public HDLCLayer HDLCCommunication { get; set; }
       
        /// <summary>
        ///Get , set client SAP address
        /// </summary>
        public int ClientSAP { get; set; }

        /// <summary>
        /// get , set security mechanism
        /// </summary>
        public byte SecurityMechanism { get; set; }

        /// <summary>
        /// Meter Password get/set
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Get/Set HLS key
        /// </summary>
        public string HLSKey { get; set; }

        #endregion

        #region Constructor
        public COSEMLayer(HDLCLayer hdlcCommunication)
        {
            ClientSAP = 0x20; 
            SecurityMechanism = 0x01; 
            Password = DefaultPassword;
            HLSKey = DefaultHLSKey;
            HDLCCommunication = hdlcCommunication;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdlcCommunication"></param>
        /// <param name="secMode"></param>
        /// <param name="password"></param>
        public COSEMLayer(HDLCLayer hdlcCommunication,byte secMode, String password)
        {
            SecurityMechanism = secMode;
            if (secMode == 0x01)
            {
                ClientSAP = 0x20;
                Password = password;
                HLSKey = DefaultHLSKey;
            }
            else if(secMode == 0x02)
            {
                ClientSAP = 0x30;
                Password = DefaultHLSKey;//Default Seed;
                HLSKey = password;// HLS meter key;
            }
            else if (secMode == 0x00)
            {
                ClientSAP = 0x10;
                Password = DefaultPassword;
                HLSKey = password;
            }

            HDLCCommunication = hdlcCommunication;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baud"></param>
        public void SetBaud(byte baud)
        {
            HDLCCommunication.SetBaud(baud);
        }
        /// <summary>
        /// Used to send cosem packet to HDLC layer
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="obisCode"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public Result Send(ProfileCommand profileCommand)
        {
           
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Success;
            result.RecieveDataBuffer = new List<byte>();
            cosemBlockBuffer = new List<byte>();
            HDLCCommunication.blockWithSegmentation = false;
            try
            {
                if (profileCommand.Action == ActionType.READ)
                {
                    List<byte> cosemBuffer=new List<byte>();
                   

                    if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                        cosemBuffer = GetCOSEMPacket_Cipher(profileCommand);
                     else
                     cosemBuffer = GetCOSEMPacket(profileCommand);

                    result = HDLCCommunication.Send(cosemBuffer, CommandType.I);

                    if (result != null && result.ErrorCode == CommunicationErrorType.Success)
                    {
                        //*******************AES GCM Decrypt ********************************
                        if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                        {
                            byte[] plaintextResponse = GetPlainTextFromCipheredTest(3, result.RecieveDataBuffer);
                            result.ErrorCode = CheckCOSEMResponse(plaintextResponse.ToList());
                        }
                        else
                            result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);

                        if (result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                            HDLCCommunication.lastBlockWithSegmentation = true;
                        else
                            HDLCCommunication.lastBlockWithSegmentation = false;

                        if (HDLCCommunication.blockWithSegmentation == true)
                        {
                            while (true)
                            {
                                //Segmentation Data packet parsing
                                if (HDLCCommunication.currentPktSegmentationStatus)
                                {
                                    HDLCCommunication.prevPktSegmentationStatus = HDLCCommunication.currentPktSegmentationStatus;
                                    while (true)
                                    {
                                        cosemBuffer = new List<byte>();

                                        // Added for CMRI Delay Support ///
                                        Application.DoEvents();
                                        Thread.Sleep(5);
                                        // Added for CMRI Delay Support ///
                                        result = HDLCCommunication.Send(cosemBuffer, CommandType.Nothing);
                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                        {
                                            result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);
                                            //result.ErrorCode = CheckSegmentationCOSEMResponse(result.RecieveDataBuffer);

                                            if (HDLCCommunication.currentPktSegmentationStatus == false)
                                            {
                                                result.ErrorCode = CommunicationErrorType.Success;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                }
                                if (HDLCCommunication.lastBlockWithSegmentation) break;
                                if (!HDLCCommunication.currentPktSegmentationStatus)
                                {
                                    cosemBuffer = GetBlockTransferPacket();
                                }
                                else
                                {
                                    cosemBuffer = new List<byte>();
                                }
                                Application.DoEvents();
                                Thread.Sleep(5);
                                result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);
                                    if (result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                                    {
                                        HDLCCommunication.lastBlockWithSegmentation = true;
                                        result.ErrorCode = CommunicationErrorType.Success;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            result.RecieveDataBuffer = cosemBlockBuffer;
                            result.RecieveDataLength = cosemBlockBuffer.Count;

                        }
                        else if (result.ErrorCode == CommunicationErrorType.BlockTransferNext)
                        {
                            while (true)
                            {
                                //To do:  Segmentation Parsing
                                if (!HDLCCommunication.currentPktSegmentationStatus)
                                {
                                    if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                                    
                                        cosemBuffer = GetBlockTransferPacket_Cipher();
                                        
                                    

                                    else
                                        cosemBuffer = GetBlockTransferPacket();
                                }
                                else
                                {
                                    cosemBuffer = new List<byte>();
                                }

                                // Added for CMRI Delay Support ///
                                Application.DoEvents();
                                Thread.Sleep(5);
                                // Added for CMRI Delay Support ///

                                result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                                    {
                                     byte[] plaintextResponse = GetPlainTextFromCipheredTest(3, result.RecieveDataBuffer);
                                     result.ErrorCode = CheckCOSEMResponse(plaintextResponse.ToList());
                                    }
                                    else
                                    result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);
                                    if (result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                                    {
                                        result.ErrorCode = CommunicationErrorType.Success;
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            result.RecieveDataBuffer = cosemBlockBuffer;
                            result.RecieveDataLength = cosemBlockBuffer.Count;
                        }
                        //Segmentation Data packet parsing
                        else if (HDLCCommunication.currentPktSegmentationStatus)
                        {
                            HDLCCommunication.prevPktSegmentationStatus = HDLCCommunication.currentPktSegmentationStatus;
                            while (true)
                            {
                                cosemBuffer = new List<byte>();

                                // Added for CMRI Delay Support ///
                                Application.DoEvents();
                                Thread.Sleep(5);
                                // Added for CMRI Delay Support ///
                                result = HDLCCommunication.Send(cosemBuffer, CommandType.Nothing);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);
                                    //result.ErrorCode = CheckSegmentationCOSEMResponse(result.RecieveDataBuffer);
                                    if (HDLCCommunication.currentPktSegmentationStatus == false && HDLCCommunication.blockWithSegmentation == true)
                                    {

                                    }
                                    if (HDLCCommunication.currentPktSegmentationStatus == false)
                                    {
                                        result.ErrorCode = CommunicationErrorType.Success;
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            result.RecieveDataBuffer = cosemBlockBuffer;
                            result.RecieveDataLength = cosemBlockBuffer.Count;

                        }
                        //else  if (result.ErrorCode == CommunicationErrorType.AccessDenied)
                        //{
                        //    result.RecieveDataBuffer = cosemBlockBuffer;
                        //    result.RecieveDataLength = cosemBlockBuffer.Count;
                        //}
                        else
                        {
                            result.ErrorCode = CommunicationErrorType.Success;
                            result.RecieveDataBuffer = cosemBlockBuffer;
                            result.RecieveDataLength = cosemBlockBuffer.Count;
                        }
                    }
                }
                else if (profileCommand.Action == ActionType.WRITE || profileCommand.Action == ActionType.WRITEBUFFER)
                {
                    if (profileCommand.Action == ActionType.WRITE)
                    {
                        blockWriteBuffer = CommonConfig.GetDataBuffer(profileCommand.ClassName, profileCommand.WriteDataBuffer);
                    }
                    else
                    {
                        blockWriteBuffer = (List<byte>)profileCommand.WriteDataBuffer;
                    }                    
                   if (blockWriteBuffer.Count > negotiatedPDUSize)
                    {
                        if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)                       
                            result = WriteCOSEMBlockData_Cipher(profileCommand); 
                        else
                            result = WriteCOSEMBlockData(profileCommand);
                    }
                    else
                    {
                        if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                        {
                            List<byte> cosemBuffer = GetCOSEMWritePacket_Cipher(profileCommand, false);                            
                            result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                        }
                        else
                        { 
                         List<byte> cosemBuffer = GetCOSEMWritePacket(profileCommand,false);
                         cosemBuffer.AddRange(blockWriteBuffer);
                         result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                        }
                       
                        
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {
                            //*******************AES GCM Decrypt ********************************
                            if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                            {
                                byte[] plaintextResponse = GetPlainTextFromCipheredTest(3, result.RecieveDataBuffer);
                                result.ErrorCode = CheckCOSEMWriteResponse_Cipher(plaintextResponse.ToList());
                            }
                            else
                                result.ErrorCode = CheckCOSEMWriteResponse(result.RecieveDataBuffer);
                        }
                    }
                }
                else if (profileCommand.Action == ActionType.RESET)
                {
                    //*******************AES GCM Encrypt ********************************
                    if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                    {
                        List<byte> cosemBuffer = GetCOSEMResetPacket_Ciphered(profileCommand);
                        result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                    }
                    else
                    {
                        List<byte> cosemBuffer = GetCOSEMResetPacket(profileCommand);
                        result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                    }
                       
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        //*******************AES GCM Decrypt ********************************
                        if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                        {
                            byte[] plaintextResponse = GetPlainTextFromCipheredTest(3, result.RecieveDataBuffer);
                            result.ErrorCode = CheckCOSEMResetResponse(plaintextResponse.ToList());
                        }
                        else
                        result.ErrorCode = CheckCOSEMResetResponse(result.RecieveDataBuffer);
                    }

                }
                else if (profileCommand.Action == ActionType.ACTIONREQUEST)
                {
                    //*******************AES GCM Encrypt ********************************
                    if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                    {
                        List<byte> cosemBuffer = GetCOSEMDisconnectControlPacket_Cipher(profileCommand);
                        result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                    }
                    else
                    {
                        List<byte> cosemBuffer = GetCOSEMDisconnectControlPacket(profileCommand);
                        result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        //*******************AES GCM Decrypt ********************************
                        if (ConfigSettings.GetValue("ApplicationContext") == "03" && SecurityMechanism != 0x00)
                        {
                            byte[] plaintextResponse = GetPlainTextFromCipheredTest(3, result.RecieveDataBuffer);
                            result.ErrorCode = CheckCOSEMResetResponse(plaintextResponse.ToList());
                        }
                        else
                        result.ErrorCode = CheckCOSEMResetResponse(result.RecieveDataBuffer);
                    }

                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Used to send cosem packet to HDLC layer
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="obisCode"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public Result SendWriteBlock(ProfileCommand profileCommand)
        {

            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Success;
            result.RecieveDataBuffer = new List<byte>();
            cosemBlockBuffer = new List<byte>();
            HDLCCommunication.blockWithSegmentation = false;
            try
            {
                
                if (profileCommand.Action == ActionType.WRITE || profileCommand.Action == ActionType.WRITEBUFFER)
                {
                    if (profileCommand.Action == ActionType.WRITE)
                    {
                        blockWriteBuffer = CommonConfig.GetDataBuffer(profileCommand.ClassName, profileCommand.WriteDataBuffer);
                    }
                    else
                    {
                        blockWriteBuffer = (List<byte>)profileCommand.WriteDataBuffer;
                    }

                    result = WriteCOSEMBlockData(profileCommand);
                }
                else
                {
                    result.ErrorCode = CommunicationErrorType.Nothing;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
     
        /// <summary>
        /// Used for opening Cosem session
        /// </summary>
        /// <returns></returns>
        public Result OpenSession_CipherCommands(long InitiCounter)
        {
            List<byte> cosemBuffer;
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Nothing;//Success;
            try
            {
                result = HDLCCommunication.Send(CommandType.SNRM);
                //to support segementation
                //int proposedPDUSize = 100;// receivedData[21];
                int proposedPDUSize = 0xffff; //Support Segmentation bit max length

                try
                {
                    proposedPDUSize = ((int)result.RecieveDataBuffer[(int)result.RecieveDataLength - 21]) << 8;
                    proposedPDUSize = (proposedPDUSize | (int)result.RecieveDataBuffer[(int)result.RecieveDataLength - 20]);
                }
                catch
                {
                }

                // to support FW having segmentation as fixed value

                if (proposedPDUSize == 0x83)
                    proposedPDUSize = 100;
                else if (proposedPDUSize != 126)//for CMRI
                    //Task id: 579172 Segment meter max pdu size get fixed as per f/w implementation
                    proposedPDUSize = 512; //Support Segmentation bit max length


                if (ConfigSettings.GetValue("ChannelType") == CommunicationType.GPRS.ToString() || ConfigSettings.GetValue("ChannelType") == CommunicationType.TCP.ToString())
                {
                    proposedPDUSize = 100;
                }


                //to support segementation
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    proposedPDUSize = 9999;
                    cosemBuffer = GetAARQPacket_Cipher(proposedPDUSize, InitiCounter);//Smart meter command
                    result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                       result.ErrorCode = CheckAARQResponse(result.RecieveDataBuffer);

                        
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {
                            //*************Get Plain Text from AES GCM Decryption*******
                            InitVectorDecypt = new byte[12];
                            int IIndex = 0;
                            int buffIndex = 29;
                            while (IIndex < InitVectorDecypt.Length - 4)
                            {
                                InitVectorDecypt[IIndex] = result.RecieveDataBuffer[buffIndex + IIndex];
                                IIndex++;
                            }
                            byte[] AAREplaintext = ParseAARE_generalAPDUs(0, result.RecieveDataBuffer);
                           
                            // byte[] plaintextResponse= GetPlainTextFromCipheredTest(77, result.RecieveDataBuffer);

                            if (SecurityMechanism == 0x02)
                            {
                                cosemBuffer = GetHLSChallengeCommand_Cipher(result.RecieveDataBuffer);//RLRQ
                                result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    //************* Get Plain text of RLRQ from Decryption *******
                                    byte[] RLRQResponseBuffer = GetPlainTextFromRLRQ(result.RecieveDataBuffer);
                                    result.ErrorCode = CheckCOSEMResponse(RLRQResponseBuffer.ToList());
                                    

                                }
                            }
                        }
                        //else
                        //{
                        //    cosemBuffer = GetAARQPacket(65535);
                        //    result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                        //    if (result.ErrorCode == CommunicationErrorType.Success)
                        //    {
                        //        result.ErrorCode = CheckAARQResponse(result.RecieveDataBuffer);
                        //        if (result.ErrorCode == CommunicationErrorType.Success)
                        //        {
                        //            if (SecurityMechanism == 0x02)
                        //            {
                        //                cosemBuffer = GetHLSChallengeCommand(result.RecieveDataBuffer);
                        //                result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                        //                if (result.ErrorCode == CommunicationErrorType.Success)
                        //                {
                        //                    result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }

                }
            }
            catch
            {

            }
            return result;
        }
        public Result OpenSession()
        {
            List<byte> cosemBuffer;
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Nothing;//Success;
            try
            {
                result = HDLCCommunication.Send(CommandType.SNRM);
                //to support segementation
                //int proposedPDUSize = 100;// receivedData[21];
                int proposedPDUSize = 0xffff; //Support Segmentation bit max length

                try
                {
                    proposedPDUSize = ((int)result.RecieveDataBuffer[(int)result.RecieveDataLength - 21]) << 8;
                    proposedPDUSize = (proposedPDUSize | (int)result.RecieveDataBuffer[(int)result.RecieveDataLength - 20]);
                }
                catch
                { 
                }
               
                // to support FW having segmentation as fixed value

                if (proposedPDUSize == 0x83)
                    proposedPDUSize = 100;
                else if(proposedPDUSize != 126)//for CMRI
                    //Task id: 579172 Segment meter max pdu size get fixed as per f/w implementation
                    proposedPDUSize = 512; //Support Segmentation bit max length


                if (ConfigSettings.GetValue("ChannelType") == CommunicationType.GPRS.ToString())
                {
                    proposedPDUSize = 100;
                }


                //to support segementation
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    
                cosemBuffer = GetAARQPacket(proposedPDUSize);
                    result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        result.ErrorCode = CheckAARQResponse(result.RecieveDataBuffer);
                        if (result.ErrorCode == CommunicationErrorType.Success)
                        {
                            if (SecurityMechanism == 0x02)
                            {
                                cosemBuffer = GetHLSChallengeCommand(result.RecieveDataBuffer);//RLRQ
                                result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);
                                }
                            }
                        }
                        else
                        {
                            cosemBuffer = GetAARQPacket(65535);
                            result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                result.ErrorCode = CheckAARQResponse(result.RecieveDataBuffer);
                                if (result.ErrorCode == CommunicationErrorType.Success)
                                {
                                    if (SecurityMechanism == 0x02)
                                    {
                                        cosemBuffer = GetHLSChallengeCommand(result.RecieveDataBuffer);
                                        result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                                        if (result.ErrorCode == CommunicationErrorType.Success)
                                        {
                                            result.ErrorCode = CheckCOSEMResponse(result.RecieveDataBuffer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                   
                }
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// Cosem Disconnect functionality
        /// </summary>
        /// <returns></returns>
        public Result CloseSession()
        {
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Success;
            try
            {
                result = HDLCCommunication.Send(CommandType.DISC);
            }
            catch
            {

            }
            return result;
        }       

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        /// Used to Write Cosem buffer
        /// </summary>
        /// <param name="profileCommand"></param>
        /// <returns></returns>
        private Result WriteCOSEMBlockData(ProfileCommand profileCommand)
        {
            bool blockTransfer = false;
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Success;
            result.RecieveDataBuffer = new List<byte>();
            List<byte> cosemBuffer;

            while (true)
            {
                if (!blockTransfer)
                {
                    cosemBuffer = GetCOSEMWritePacket(profileCommand, true);
                    blockNumber = 0x01;
                    blockIndex = 0x00;
                    blockTransfer = true;
                }
                else
                {
                    cosemBuffer = new List<byte>();
                    cosemBuffer.Add(0xC1);
                    cosemBuffer.Add(0x03);
                    cosemBuffer.Add(0x81);
                }

                cosemBuffer.AddRange(GetBlockBuffer());

                result = HDLCCommunication.Send(cosemBuffer, CommandType.I);
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    result.ErrorCode = CheckCOSEMWriteResponse(result.RecieveDataBuffer);
                    if (result.ErrorCode == CommunicationErrorType.InvalidGetResponseTag)
                    {
                        break;
                    }
                    else if (result.ErrorCode == CommunicationErrorType.AccessDenied)
                    {                        
                        break;
                    }
                    else if (result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                    {
                        result.ErrorCode = CommunicationErrorType.Success;
                        break;
                    }

                }

            }
            return result;

        }

        private Result WriteCOSEMBlockData_Cipher(ProfileCommand profileCommand)
        {
            bool blockTransfer = false;
            Result result = new Result();
            result.ErrorCode = CommunicationErrorType.Success;
            result.RecieveDataBuffer = new List<byte>();
            // List<byte> cosemBuffer;
             List<byte> cosemBuffer = new List<byte>();
            List<byte> PlainTextBuffer = new List<byte>();
            int dedicatedindex = 0;
            cosemBuffer.Add(0xC9);
            cosemBuffer.Add(0x00);
            dedicatedindex = HDLCCommunication.DedicatedCommand(cosemBuffer.ToArray(), dedicatedindex, "Write", cosemBuffer.Count, 0xC3);//Dedicated/Without Dedicated
            while (true)
            {
                if (!blockTransfer)
                {                   
                    if (SecurityMechanism == 0x01)
                        cosemBuffer.Add(0x20);//Security Suit
                    else
                        cosemBuffer.Add(0x30);//Security Suit
                    InitializationCounter++;
                    cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));//initcounter 4 byte
                    cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
                    cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
                    cosemBuffer.Add(Convert.ToByte(InitializationCounter & 0x00FF));
                    PlainTextBuffer.Add(0xC1);
                    if (!blockTransfer)
                    {
                        PlainTextBuffer.Add(0x02);
                    }
                    else
                    {
                        PlainTextBuffer.Add(0x01);
                    }
                    PlainTextBuffer.Add(0x81);
                    PlainTextBuffer.Add(0x00);
                    PlainTextBuffer.Add(profileCommand.ClassId);
                    string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
                    PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));   
                    PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));    
                    PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));    
                    PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));    
                    PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));   
                    PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));   
                    PlainTextBuffer.Add(profileCommand.Attribute);
                    PlainTextBuffer.Add(0x00);                  
                    blockNumber = 0x01;
                    blockIndex = 0x00;
                    blockTransfer = true;
                }
                else
                {
                    dedicatedindex = 0;
                    PlainTextBuffer = new List<byte>();
                    cosemBuffer.Add(0xC9);
                    cosemBuffer.Add(0x00);
                    dedicatedindex = HDLCCommunication.DedicatedCommand(cosemBuffer.ToArray(), dedicatedindex, "Write", cosemBuffer.Count, 0xC3);//Dedicated/Without Dedicated
                    if (SecurityMechanism == 0x01)
                        cosemBuffer.Add(0x20);//Security Suit
                    else
                        cosemBuffer.Add(0x30);//Security Suit
                    InitializationCounter++;
                    cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));//initcounter 4 byte
                    cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
                    cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
                    cosemBuffer.Add(Convert.ToByte(InitializationCounter & 0x00FF));
                    PlainTextBuffer.Add(0xC1);
                    PlainTextBuffer.Add(0x03);
                    PlainTextBuffer.Add(0x81);
                }
                PlainTextBuffer.AddRange(GetBlockBuffer());
                byte[] plaintextcommandbyte = new byte[PlainTextBuffer.Count];
                System.Buffer.BlockCopy(PlainTextBuffer.ToArray(), 0, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
                //*******************AES GCM Encrypt ********************************
                cosemBuffer.AddRange(CreateCipherCommand(plaintextcommandbyte, 0));
                cosemBuffer[1] = (byte)(cosemBuffer.Count - 2);

                result = HDLCCommunication.Send(cosemBuffer, CommandType.I);                
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    byte[] plaintextResponse = GetPlainTextFromCipheredTest(3, result.RecieveDataBuffer);                  
                    result.ErrorCode = CheckCOSEMBlockWriteResponse_Cipher(plaintextResponse.ToList());
                    cosemBuffer = new List<byte>();
                    if (result.ErrorCode == CommunicationErrorType.InvalidGetResponseTag)
                    {
                        break;
                    }
                    else if (result.ErrorCode == CommunicationErrorType.AccessDenied)
                    {
                        break;
                    }
                    else if (result.ErrorCode == CommunicationErrorType.BlockTransferLast)
                    {
                        result.ErrorCode = CommunicationErrorType.Success;
                        break;
                    }

                }

            }
            return result;
        }
            /// <summary>
            /// Used to get COSEm Buffer .
            /// </summary>
            /// <param name="blockIndex"></param>
            /// <returns></returns>
         private List<byte> GetBlockBuffer()
        {
            List<byte> cosemBuffer = new List<byte>();
            int maxBufferSize = 0x4d;
            if ((blockWriteBuffer.Count  - blockIndex) <= maxBufferSize)
            {
                maxBufferSize = blockWriteBuffer.Count - blockIndex;
                cosemBuffer.Add(0x01);
            }
            else
            {
                cosemBuffer.Add(0x00);
            }

            cosemBuffer.Add(0x00);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(Convert.ToByte(blockNumber >> 8));
            cosemBuffer.Add(Convert.ToByte(blockNumber & 0xFF));
            cosemBuffer.Add(Convert.ToByte(maxBufferSize));
            for (int i = 0; i < maxBufferSize; i++)
            {
                cosemBuffer.Add(blockWriteBuffer[blockIndex++]);
            }
            blockNumber++;
            return cosemBuffer;
        }
       
    
        /// <summary>
        /// Creates HLS Challenge
        /// </summary>
        /// <param name="recieveBuffer"></param>
        /// <returns></returns>
        private List<byte> GetHLSChallengeCommand(List<byte> recieveBuffer)
        {
            List<byte> cosemBuffer = new List<byte>();
            byte[] stocChallenge = new byte[16];
            for (int challengIndex = 0; challengIndex < 16; challengIndex++)
            {
                stocChallenge[challengIndex] = recieveBuffer[42 + challengIndex];
            }
           
            AESEncryptor aESEncryptor = new AESEncryptor();
            byte[] encryptedSTOSChallenge = aESEncryptor.fAESEncryption(HLSKey, stocChallenge);

            ////C3 01 C1 00 0F 00 00 28 00 03 FF 01 00 09 10
            cosemBuffer.Add(0xC3);
            cosemBuffer.Add(0x01);
            cosemBuffer.Add(0xC1);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(0x0F);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(0x28);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(0x03);
            cosemBuffer.Add(0xFF);
            cosemBuffer.Add(0x01);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(0x09);
            cosemBuffer.Add(0x10);
            for (int challengIndex = 0; challengIndex < 16; challengIndex++)
            {
                cosemBuffer.Add(encryptedSTOSChallenge[challengIndex]);
            }
            return cosemBuffer;
        }
       //*****************For Smart meter ******************
        private List<byte> GetHLSChallengeCommand_Cipher(List<byte> recieveBuffer)
        {
            List<byte> cosemBuffer = new List<byte>();
            byte[] stocChallenge = new byte[16];
            //for (int challengIndex = 0; challengIndex < 16; challengIndex++)
            //{
            //    stocChallenge[challengIndex] = recieveBuffer[54 + challengIndex];
            //}
            string ReceivedSeed = string.Empty;
            int icnt = 54;
            while (icnt < 54 + 16) ReceivedSeed += Convert.ToChar(recieveBuffer[icnt++]).ToString();
            AESEncryption objaes = new AESEncryption();
            string ClientCipherText = objaes.GenerateCipherText(ReceivedSeed, HLSKey);

            cosemBuffer.Add(0xCB);
            cosemBuffer.Add(0x30);
            cosemBuffer.Add(0x30);
            InitializationCounter++;
            cosemBuffer.AddRange(fInvocationCounter(InitializationCounter));
            //AESEncryptor aESEncryptor = new AESEncryptor();
            //byte[] encryptedSTOSChallenge = aESEncryptor.fAESEncryption(HLSKey, stocChallenge);
           
           //for (int challengIndex = 0; challengIndex < 16; challengIndex++)
            //{
            //    cosemBuffer.Add(encryptedSTOSChallenge[challengIndex]);
            //}
            //---------------------------------------------------------------------------------------------------
            byte[] RLRQData = new byte[16];
           int aadcount = 0;
            int countlen1 = 0;
            while (countlen1 < ClientCipherText.Length)
            {
                RLRQData[aadcount++] = Convert.ToByte(ClientCipherText.Substring(countlen1, 2), 16);
                countlen1 += 2;
            }
            string HDLCstr = "C301C1000F0000280000FF01010910";
            byte[] HDLCdata = new byte[15];
            aadcount = 0;
            countlen1 = 0;
            while (countlen1 < HDLCstr.Length)
            {
                HDLCdata[aadcount++] = Convert.ToByte(HDLCstr.Substring(countlen1, 2), 16);
                countlen1 += 2;
            }

            byte[] RLRQplainText = new byte[31];
            System.Buffer.BlockCopy(HDLCdata, 0, RLRQplainText, 0, HDLCdata.Length);
            System.Buffer.BlockCopy(RLRQData, 0, RLRQplainText, HDLCdata.Length, RLRQData.Length);

            cyphertext = new byte[31];
            AuthenticationTag = new byte[12];
            IntializationVector(clientSystemTitle, InitializationCounter);
            ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, 16, RLRQplainText, 31, ref cyphertext, ClientInitVector, 12, AAD, 17, ref AuthenticationTag, AuthTagLen, ChannelNum);
            cosemBuffer.AddRange(fAddCyphered_Tag(cyphertext));
            cosemBuffer.AddRange(fAddAuthentication_Tag(AuthenticationTag));
            return cosemBuffer;
        }

        /// <summary>
        /// Used to prepare Cosem packets 
        /// This method is called while sending Cosem packets to HDLC layer
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="obisCode"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private List<byte> GetCOSEMPacket(ProfileCommand profileCommand)
        {
            List<byte> cosemBuffer = new List<byte>();
            cosemBuffer.Add(0xC0);
            cosemBuffer.Add(0x01);
            cosemBuffer.Add(0x81);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(profileCommand.ClassId);
            string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            cosemBuffer.Add(profileCommand.Attribute);
            if (profileCommand.SelectiveAccess)
            {
                //DateTime FromTime = profileCommand.StartTime.AddDays(-profileCommand.SelectiveDays);
                cosemBuffer.AddRange(GetSelectiveAccessBuffer(profileCommand.FromTime,profileCommand.ToTime));
            }
            else
            {
                //-------To Read Case Tamper Snap using Column Wise Selected Access for 1Phase DH Tender Testing--------
                //if (profileCommand.ObisCode == "00.00.63.62.05.FF" && profileCommand.Attribute == 0x02)
                //{
                //    cosemBuffer.Add(0x01);
                //    cosemBuffer.Add(0x02);
                //    cosemBuffer.Add(0x02);
                //    cosemBuffer.Add(0x04);
                //    cosemBuffer.Add(0x06);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x01);
                //    cosemBuffer.Add(0x06);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x12);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x01);
                //    cosemBuffer.Add(0x12);
                //    cosemBuffer.Add(0x00);
                //    cosemBuffer.Add(0x07);
                //    //01 02 02 04 06 00 00 00 01 06 00 00 00 00 12 00 01 12 00 07
                //}
                //else
                //{
                //    cosemBuffer.Add(0x00);
                //}
                cosemBuffer.Add(0x00);
            }
            return cosemBuffer;
        }

       
       private List<byte> GetCOSEMPacket_Cipher(ProfileCommand profileCommand)
        {
            List<byte> cosemBuffer = new List<byte>();
            cosemBuffer.Add(0xC8);
            cosemBuffer.Add(0x00);
            if (SecurityMechanism == 0x01)
                cosemBuffer.Add(0x20);//Security Suit
            else
                cosemBuffer.Add(0x30);//Security Suit
            InitializationCounter++;
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
            cosemBuffer.Add(Convert.ToByte(InitializationCounter & 0x00FF));
            int countindex = 13;
            byte[] plaintextcommandbyte = new byte[countindex];
            plaintextcommandbyte[0]=0xC0;
            plaintextcommandbyte[1] = 0x01;
            plaintextcommandbyte[2] = 0xC1;
            plaintextcommandbyte[3] = 0x00;
            plaintextcommandbyte[4] = profileCommand.ClassId;
           string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            plaintextcommandbyte[5] = Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16);
            plaintextcommandbyte[6] = Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16);
            plaintextcommandbyte[7] = Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16);
            plaintextcommandbyte[8] = Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16);
            plaintextcommandbyte[9] = Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16);
            plaintextcommandbyte[10] = Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16);
            plaintextcommandbyte[11] = profileCommand.Attribute;
            plaintextcommandbyte[12] = 0x00;
            
            //*******************AES GCM Encrypt ********************************
           cosemBuffer.AddRange(CreateCipherCommand(plaintextcommandbyte, 0));
           cosemBuffer[1] = (byte)(cosemBuffer.Count-2);
           return cosemBuffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
       private List<byte> GetSelectiveAccessBuffer(DateTime fromDate, DateTime toDate)
       {
           List<byte> cosemBuffer = new List<byte>();
           cosemBuffer.Add(0x01);
           cosemBuffer.Add(0x01);
           cosemBuffer.Add(0x02);
           cosemBuffer.Add(0x04);
           cosemBuffer.Add(0x02);
           cosemBuffer.Add(0x04);

           cosemBuffer.Add(0x12);
           cosemBuffer.Add(0x00);
           cosemBuffer.Add(0x08);

           cosemBuffer.Add(0x09);
           cosemBuffer.Add(0x06);

           cosemBuffer.Add(0x00); //obis code
           cosemBuffer.Add(0x00);
           cosemBuffer.Add(0x01);
           cosemBuffer.Add(0x00);
           cosemBuffer.Add(0x00);
           cosemBuffer.Add(0xFF);

           cosemBuffer.Add(0x0F);
           cosemBuffer.Add(0x02);
           cosemBuffer.Add(0x12);

           cosemBuffer.Add(0x00);
           cosemBuffer.Add(0x00);

           cosemBuffer.Add(0x09);
           cosemBuffer.Add(0x0C);

           //change pks_301118 _start

           if (ConfigSettings.GetValue("chkOthersManufacture") == "1")
           {
               cosemBuffer.Add(Convert.ToByte((toDate.Year & 0xFF00) >> 8));
               cosemBuffer.Add(Convert.ToByte(toDate.Year & 0x00FF));

               cosemBuffer.Add(Convert.ToByte(fromDate.Month)); //month
               cosemBuffer.Add(Convert.ToByte(fromDate.Day));
           }
           else
           {
               cosemBuffer.Add(Convert.ToByte((fromDate.Year / 100) % 20));

               cosemBuffer.Add(Convert.ToByte(fromDate.Year % 100));

               cosemBuffer.Add(Convert.ToByte(fromDate.Month));

               cosemBuffer.Add(Convert.ToByte(fromDate.Day));
           }

           //change pks_301118 _end 




           //cosemBuffer.Add(Convert.ToByte((fromDate.Year / 100) % 20)); //year
           //cosemBuffer.Add(Convert.ToByte(0x20));
           //cosemBuffer.Add(Convert.ToByte(fromDate.Year % 100));


           cosemBuffer.Add(0xFF);

           cosemBuffer.Add(Convert.ToByte(fromDate.Hour));
           cosemBuffer.Add(Convert.ToByte(fromDate.Minute));
           cosemBuffer.Add(Convert.ToByte(fromDate.Second));

           cosemBuffer.Add(0xFF);

           cosemBuffer.Add(0x80);
           cosemBuffer.Add(0x00);

           cosemBuffer.Add(0x00);

           cosemBuffer.Add(0x09);
           cosemBuffer.Add(0x0C);

           cosemBuffer.Add(Convert.ToByte((toDate.Year & 0xFF00) >> 8));
           cosemBuffer.Add(Convert.ToByte(toDate.Year & 0x00FF));

           // cosemBuffer.Add(Convert.ToByte((toDate.Year / 100) % 20)); //year
           //cosemBuffer.Add(Convert.ToByte(0x20));
           //cosemBuffer.Add(Convert.ToByte(toDate.Year % 100));

           cosemBuffer.Add(Convert.ToByte(toDate.Month)); //month

           cosemBuffer.Add(Convert.ToByte(toDate.Day));
           cosemBuffer.Add(0xFF);

           cosemBuffer.Add(Convert.ToByte(toDate.Hour));
           cosemBuffer.Add(Convert.ToByte(toDate.Minute));
           cosemBuffer.Add(Convert.ToByte(toDate.Second));

           cosemBuffer.Add(0xFF);

           cosemBuffer.Add(0x80);
           cosemBuffer.Add(0x00);
           cosemBuffer.Add(0x00);

           cosemBuffer.Add(0x01);
           cosemBuffer.Add(0x00);

           return cosemBuffer;
       }
          /// <summary>
        /// Used to prepare Cosem packets 
        /// This method is called while sending Cosem packets to HDLC layer
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="obisCode"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private List<byte> GetCOSEMWritePacket(ProfileCommand profileCommand, bool blockTransfer)
        {
            List<byte> cosemBuffer = new List<byte>();
            cosemBuffer.Add(0xC1);
            if (blockTransfer)
            {
                cosemBuffer.Add(0x02);
            }
            else
            {
                cosemBuffer.Add(0x01);
            }
            cosemBuffer.Add(0x81);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(profileCommand.ClassId);
            string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            cosemBuffer.Add(profileCommand.Attribute);
            cosemBuffer.Add(0x00);

            return cosemBuffer;
        }

        private List<byte> GetCOSEMWritePacket_Cipher(ProfileCommand profileCommand, bool blockTransfer)
        {
            List<byte> cosemBuffer = new List<byte>();
            List<byte> PlainTextBuffer = new List<byte>();
            cosemBuffer.Add(0xC9);
            cosemBuffer.Add(0x00);
            if (SecurityMechanism == 0x01)
                cosemBuffer.Add(0x20);//Security Suit
            else
                cosemBuffer.Add(0x30);//Security Suit
            InitializationCounter++;
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));//initcounter 4 byte
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
            cosemBuffer.Add(Convert.ToByte(InitializationCounter & 0x00FF));

            PlainTextBuffer.Add(0xC1);
            if (blockTransfer)
            {
                PlainTextBuffer.Add(0x02);
            }
            else
            {
                PlainTextBuffer.Add(0x01);
            }
            PlainTextBuffer.Add(0x81);
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(profileCommand.ClassId);
            string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));
            PlainTextBuffer.Add(profileCommand.Attribute);
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.AddRange(blockWriteBuffer);

            byte[] plaintextcommandbyte = new byte[PlainTextBuffer.Count];
           
            System.Buffer.BlockCopy(PlainTextBuffer.ToArray(), 0, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
            //*******************AES GCM Encrypt ********************************
            cosemBuffer.AddRange(CreateCipherCommand(plaintextcommandbyte, 0));
            cosemBuffer[1] = (byte)(cosemBuffer.Count - 2);
            return cosemBuffer;
        }

        /// <summary>
        /// Used to prepare Cosem packets 
        /// This method is called while sending Cosem packets to HDLC layer
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="obisCode"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private List<byte> GetCOSEMResetPacket(ProfileCommand profileCommand)
        {
            List<byte> cosemBuffer = new List<byte>();
            cosemBuffer.Add(0xC3);
            cosemBuffer.Add(0x01);
            cosemBuffer.Add(0x81);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(profileCommand.ClassId);
            string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            cosemBuffer.Add(profileCommand.Attribute);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(0x09);
            cosemBuffer.Add(0x01);
            cosemBuffer.Add(0x00);
            return cosemBuffer;
        }

        private List<byte> GetCOSEMResetPacket_Ciphered(ProfileCommand profileCommand)
        {
            List<byte> cosemBuffer = new List<byte>();
            List<byte> PlainTextBuffer = new List<byte>();
            cosemBuffer.Add(0xC9);
            cosemBuffer.Add(0x00);
            if (SecurityMechanism == 0x01)
                cosemBuffer.Add(0x20);//Security Suit
            else
                cosemBuffer.Add(0x30);//Security Suit
            InitializationCounter++;
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));//initcounter 4 byte
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
            cosemBuffer.Add(Convert.ToByte(InitializationCounter & 0x00FF));

            PlainTextBuffer.Add(0xC3);
            PlainTextBuffer.Add(0x01);
            PlainTextBuffer.Add(0x81);
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(profileCommand.ClassId);
            //string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            //PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            //PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            //PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            //PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            //PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            //PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(0x0A);
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(0x01);
            PlainTextBuffer.Add(0xFF);

            PlainTextBuffer.Add(profileCommand.Attribute);
            //PlainTextBuffer.Add(0x00);
            //PlainTextBuffer.Add(0x09);
            //PlainTextBuffer.Add(0x01);
            //PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(0x01);
            PlainTextBuffer.Add(0x12);
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(0x01);


            byte[] plaintextcommandbyte = new byte[PlainTextBuffer.Count];
            System.Buffer.BlockCopy(PlainTextBuffer.ToArray(), 0, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
            //*******************AES GCM Encrypt ********************************
            cosemBuffer.AddRange(CreateCipherCommand(plaintextcommandbyte, 0));
            cosemBuffer[1] = (byte)(cosemBuffer.Count - 2);
            return cosemBuffer;
        }

        private List<byte> GetCOSEMDisconnectControlPacket(ProfileCommand profileCommand)
        {
            List<byte> cosemBuffer = new List<byte>();
           
            cosemBuffer.Add(0xC3);
            cosemBuffer.Add(0x01);
            cosemBuffer.Add(0x81);
            cosemBuffer.Add(0x00);
            cosemBuffer.Add(profileCommand.ClassId);
            string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            cosemBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            cosemBuffer.Add(profileCommand.Attribute);
            cosemBuffer.Add(0x01);
              //int valueindex=0;
              //object valueList = profileCommand.WriteDataBuffer;
              //while (valueindex < (List<byte>)valueList.)
              cosemBuffer.Add(0x0F);
              cosemBuffer.Add(0x00);
            return cosemBuffer;
        }
        
       private List<byte> GetCOSEMDisconnectControlPacket_Cipher(ProfileCommand profileCommand)
        {
            int dedicatedindex = 0;
            List<byte> cosemBuffer = new List<byte>();
            List<byte> PlainTextBuffer = new List<byte>();
            cosemBuffer.Add(0xC9);
            cosemBuffer.Add(0x00);
            dedicatedindex = HDLCCommunication.DedicatedCommand(cosemBuffer.ToArray(), dedicatedindex, "Write", cosemBuffer.Count, 0xC3);//Dedicated/Without Dedicated
            if (SecurityMechanism == 0x01)
                cosemBuffer.Add(0x20);//Security Suit
            else
                cosemBuffer.Add(0x30);//Security Suit
            InitializationCounter++;
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));//initcounter 4 byte
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
            cosemBuffer.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
            cosemBuffer.Add(Convert.ToByte(InitializationCounter & 0x00FF));
            PlainTextBuffer.Add(0xC3);
            PlainTextBuffer.Add(0x01);
            PlainTextBuffer.Add(0x81);
            PlainTextBuffer.Add(0x00);
            PlainTextBuffer.Add(profileCommand.ClassId);
            string[] ObisCodeClass = profileCommand.ObisCode.Split('.');
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[0].ToString().Trim(), 16));    //Convert.ToByte(ObisCodeClass[0]);//0x00;
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[1].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 1]);//0x00;
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[2].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 2]);//0x60;
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[3].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 3]);//0x01;
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[4].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 4]);//0x00;
            PlainTextBuffer.Add(Convert.ToByte(ObisCodeClass[5].ToString().Trim(), 16));    //Convert.ToByte(OBIS_LIST[iIndex, 5]);//0xFF;
            PlainTextBuffer.Add(profileCommand.Attribute);
            PlainTextBuffer.Add(0x01);
            PlainTextBuffer.Add(0x0F);
            PlainTextBuffer.Add(0x00);
            byte[] plaintextcommandbyte = new byte[PlainTextBuffer.Count];
            System.Buffer.BlockCopy(PlainTextBuffer.ToArray(), 0, plaintextcommandbyte, 0, plaintextcommandbyte.Length);
            //*******************AES GCM Encrypt ********************************
            cosemBuffer.AddRange(CreateCipherCommand(plaintextcommandbyte, 0));
            cosemBuffer[1] = (byte)(cosemBuffer.Count - 2);
            return cosemBuffer;           
        }
        /// <summary>
        /// Get AARQ packet 
        /// </summary>
        /// <returns></returns>
        private List<byte> GetAARQPacket(int PDUSize)
        {
            List<byte> cosemBuffer = new List<byte>();

            int SecurityLength00 =GetSecMachanismLength(29,Password.Length); //(29-Password.Length);
            int SecurityLength01 =GetSecMachanismLength(62,Password.Length); //(54-Password.Length);
            int SecurityLength02 = GetSecMachanismLength(62, Password.Length);//(62-Password.Length);
            try
            {

                cosemBuffer.Add(0x60);
                  if (SecurityMechanism == 0x00)
                      cosemBuffer.Add(Convert.ToByte(SecurityLength00));
                     // cosemBuffer.Add(0x1D);
                  else if (SecurityMechanism == 0x01)
                      cosemBuffer.Add(Convert.ToByte(SecurityLength01));
                      //cosemBuffer.Add(0x36);
                  else if (SecurityMechanism == 0x02)
                      cosemBuffer.Add(Convert.ToByte(SecurityLength02));
                  //cosemBuffer.Add(0x3E);
               
                ApplicationContextName context = new ApplicationContextName(ApplicationContext.LN);
                cosemBuffer.AddRange(context.Value);
                if (SecurityMechanism != 0x00)
                {
                    cosemBuffer.AddRange(AddSecMechanism());
                    cosemBuffer.AddRange(AddPassword());
                }
                cosemBuffer.AddRange(AddUserInf());
                cosemBuffer.AddRange(AddCnfBlock());
                // Changed to support segmentation on behalf of Gopal
               // int PDUSize = 100;// 65535;
                // Changed to support segmentation on behalf of Gopal
                cosemBuffer.Add(Convert.ToByte((PDUSize >> 8) & 0x00ff));
                cosemBuffer.Add(Convert.ToByte(PDUSize & 0x00ff));
            }
            catch 
            {
                throw;
            }
            return cosemBuffer;
        }

       private int GetSecMachanismLength (int aarqtagval, int passlength)
       {
           int AARQLength=0;
           switch (passlength)
           {
               case 16:
                   AARQLength = aarqtagval;
                   break;
               case 15:
                   AARQLength = aarqtagval - 1;
                   break;
               case 14:
                   AARQLength = aarqtagval - 2;
                   break;
               case 13:
                   AARQLength = aarqtagval - 3;
                   break;
               case 12:
                   AARQLength = aarqtagval - 4;
                   break;
               case 11:
                   AARQLength = aarqtagval - 5;
                   break;
               case 10:
                   AARQLength = aarqtagval - 6;
                   break;
               case 9:
                   AARQLength = aarqtagval - 7;
                   break;  
               case 8:
                   AARQLength = aarqtagval-8;
                   break;
               case 7:
                   AARQLength = aarqtagval-9;
                   break;
               case 6:
                   AARQLength = aarqtagval - 10;
                   break;
               case 5:
                   AARQLength = aarqtagval - 11;
                   break;
               case 4:
                   AARQLength = aarqtagval - 12;
                   break;
               case 3:
                   AARQLength = aarqtagval - 13;
                   break;
               case 2:
                   AARQLength = aarqtagval - 14;
                   break;
               case 1:
                   AARQLength = aarqtagval - 15;
                   break;              
           
           }
           return AARQLength;
       
       }

        private List<byte> GetAARQPacket_Cipher(int nPDUSize, long InitiCounter)
        {
            InitializationCounter = InitiCounter;
            List<byte> cosemBuffer = new List<byte>();
            string Securitysuit = ConfigSettings.GetValue("SecuritySuit");
           string GlobalEncyptKey = ConfigSettings.GetValue("GlobalEncryptionKey");
           string AuthenticationKey = ConfigSettings.GetValue("AuthenticationKey");
           string DedicatedKey= ConfigSettings.GetValue("DedicatedKey");
           string sYstemTitle = ConfigSettings.GetValue("ClientSystemTitle");
           string Meterpassword = ConfigSettings.GetValue("ModePassword");
           clientSystemTitle = sYstemTitle;
            try
            {
                cosemBuffer.Add(0x60);
                if (SecurityMechanism == 0x00)
                    cosemBuffer.Add(0x1D);
                else if (SecurityMechanism == 0x01)
                    cosemBuffer.Add(0x36);
                else if (SecurityMechanism == 0x02)
                    cosemBuffer.Add(0x3E);

                ApplicationContextName context = new ApplicationContextName(ApplicationContext.ARWS);
                cosemBuffer.AddRange(context.Value);
                
                cosemBuffer.AddRange(fBeforeSysTitle());
                cosemBuffer.AddRange(fSystemTitle(sYstemTitle));
                if (SecurityMechanism != 0x00)
                {
                    if (SecurityMechanism == 0x01)
                    {
                        cosemBuffer.AddRange(AddSecMechanism());
                        cosemBuffer.AddRange(GetMeterPassword(Meterpassword));
                     //  cosemBuffer.AddRange(AddPassword());
                    }
                    if (SecurityMechanism == 0x02)
                    {
                        randamPlanText = "1111111111111111";//Generate16ByteRandomHLSState();
                        cosemBuffer.AddRange(AddSecMechanism());
                        cosemBuffer.AddRange(fAddRandomKey(randamPlanText));
                    }
                }
                cosemBuffer.AddRange(SecuritySuitByte(Securitysuit,DedicatedKey));

                cosemBuffer.AddRange(fInvocationCounter(InitiCounter));
                fAddUserInf_cypher(DedicatedKey);
                AddCnfBlock();
                fAddPDUSize_Cyphered(nPDUSize);
                                           
                ////********************************* Create AES GCM Encryption ***************************************
                int countlen = 0;
                int EncCount = 0;
                int Aadcount = 0;
                AAD[0] = Convert.ToByte(Securitysuit.Substring(0, 2), 16);
                while (countlen < GlobalEncyptKey.Length)
                {
                    EncryKey[EncCount++] = Convert.ToByte(GlobalEncyptKey.Substring(countlen, 2), 16);
                    countlen += 2;
                }
                countlen = 0;
                while (countlen < AuthenticationKey.Length)
                {
                    AAD[Aadcount + 1] = Convert.ToByte(AuthenticationKey.Substring(countlen, 2), 16); ;
                    countlen += 2;
                    Aadcount++;
                }

                var user_data = UserInfo.TakeWhile((v, index) => UserInfo.Skip(index).Any(w => w != 0x00)).ToArray();
                UserInfo = user_data;//Skip null values at MR mode
                System.Buffer.BlockCopy(UserInfo, 0, plainText, 0, UserInfo.Length);//Plain Text
                System.Buffer.BlockCopy(ConfBlock, 0, plainText, UserInfo.Length, ConfBlock.Length);
                System.Buffer.BlockCopy(PDUSize, 0, plainText, UserInfo.Length + ConfBlock.Length, PDUSize.Length);

                UserInfo = new byte[22];
                var Plain_data = plainText.TakeWhile((v, index) => plainText.Skip(index).Any(w => w != 0x00)).ToArray();
                plainText = Plain_data;//Skip null values at MR mode
                IntializationVector(sYstemTitle, InitiCounter); ;//Init Vector=System title + invocation counter

                cyphertext = new byte[plainText.Length];
                AuthenticationTag = new byte[AuthTagLen];

                if (SecurityMechanism == 0x01 && DedicatedKey == "0")//MR mode Encryption Only
                {
                    AAD = new byte[17];
                    AuthenticationTag = new byte[12];
                    ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, (ushort)EncryKey.Length, plainText, (uint)plainText.Length, ref cyphertext, ClientInitVector, 12, null, 0, ref AuthenticationTag, 0, ChannelNum);
                    cosemBuffer.AddRange(fAddCyphered_Tag(cyphertext));
                    plainText = new byte[31];
                }
                else if (SecurityMechanism == 0x02 || DedicatedKey == "1")//US mode Encryption + Authentication only
                {
                    ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, (ushort)EncryKey.Length, plainText, (uint)plainText.Length, ref cyphertext, ClientInitVector, 12, AAD, 17, ref AuthenticationTag, AuthTagLen, ChannelNum);
                    cosemBuffer.AddRange(fAddCyphered_Tag(cyphertext));
                    cosemBuffer.AddRange(fAddAuthentication_Tag(AuthenticationTag));
                    plainText = new byte[31];
                }
            }
            catch
            {
                throw;
            }
            return cosemBuffer;
        }
        
        /// <summary>
        /// Used to create cobtext buffer 
        /// </summary>
        /// <param name="contextType"></param>
        /// <returns></returns>
       
        private List<byte> fInvocationCounter(long InvCount)
        {
            //00000018
            List<byte> InvoBuffer = new List<byte>();
            InvoBuffer.Add(Convert.ToByte((InvCount & 0xFF000000) >> 24));
            InvoBuffer.Add(Convert.ToByte((InvCount & 0xFF0000) >> 16));
            InvoBuffer.Add(Convert.ToByte((InvCount & 0xFF00) >> 8));
            InvoBuffer.Add(Convert.ToByte(InvCount & 0x00FF));

            return InvoBuffer;
        }
        private List<byte> fAddPDUSize_Cyphered(int pDUSize)
        {
            List<byte> PduSizeBuffer = new List<byte>();
            PduSizeBuffer.Add(Convert.ToByte((pDUSize >> 8) & 0x00ff));
            PduSizeBuffer.Add(Convert.ToByte(pDUSize & 0x00ff));
            PDUSize = PduSizeBuffer.ToArray();
            return PduSizeBuffer;
        }
       private List<byte> fAddAuthentication_Tag(byte[] AuthTag)
        {
            List<byte> AuthBuffer = new List<byte>();
            int bytecount = 0;
           // int nBufferIndex = 0;
            while (bytecount < AuthTag.Length)
            {
                //AuthBuffer[nBufferIndex++] = AuthTag[bytecount++];
                AuthBuffer.Add(AuthTag[bytecount++]);
            }

            return AuthBuffer;
        }

        private List<byte> fAddCyphered_Tag(byte[] CypherTag)
        {
           
            List<byte> CyphBuffer = new List<byte>();
           // int nBufferIndex = 0;
            int bytecount = 0;
            while (bytecount < CypherTag.Length)
            {
               // CyphBuffer[nBufferIndex++] = CypherTag[bytecount++];
                CyphBuffer.Add(CypherTag[bytecount++]);
            }
            return CyphBuffer;
        }
       private List<byte> AddContext(byte contextType)
        {
            //A109060760857405080101
            List<byte> contextBuffer = new List<byte>();
            contextBuffer.Add(0xA1);
            contextBuffer.Add(0x09);
            contextBuffer.Add(0x06);
            contextBuffer.Add(0x07);
            contextBuffer.Add(0x60);
            contextBuffer.Add(0x85);
            contextBuffer.Add(0x74);
            contextBuffer.Add(0x05);
            contextBuffer.Add(0x08);
            contextBuffer.Add(0x01);
            contextBuffer.Add(contextType);
            return contextBuffer;
        }
        /// <summary>
        /// add Security Mechanism tag and Security Mechanism to Buffer
        /// </summary>
        /// <returns></returns>
        private List<byte> AddSecMechanism()
        {
            List<byte> cosemBuffer = new List<byte>();
            //8A0207808B0760857405080201
            cosemBuffer.Add(0x8A);
            cosemBuffer.Add(0x02);
            cosemBuffer.Add(0x07);
            cosemBuffer.Add(0x80);
            cosemBuffer.Add(0x8B);
            cosemBuffer.Add(0x07);
            cosemBuffer.Add(0x60);
            cosemBuffer.Add(0x85);
            cosemBuffer.Add(0x74);
            cosemBuffer.Add(0x05);
            cosemBuffer.Add(0x08);
            cosemBuffer.Add(0x02);
            cosemBuffer.Add(SecurityMechanism);

            return cosemBuffer;
        }
        private List<byte> fBeforeSysTitle()
        {
            List<byte> cosemBuffer = new List<byte>();
            cosemBuffer.Add(0xA6);
            cosemBuffer.Add(0x0A);
            cosemBuffer.Add(0x04);
            cosemBuffer.Add(0x08);
            return cosemBuffer;
        }

        private List<byte> fSystemTitle(string SystemTitle)
        {
            List<byte> cosemBuffer = new List<byte>();
            cosemBuffer.Add(Convert.ToByte(SystemTitle[0]));
            cosemBuffer.Add(Convert.ToByte(SystemTitle[1]));
            cosemBuffer.Add(Convert.ToByte(SystemTitle[2]));
            cosemBuffer.Add(Convert.ToByte(SystemTitle[3]));
            cosemBuffer.Add(Convert.ToByte(SystemTitle[4]));
            cosemBuffer.Add(Convert.ToByte(SystemTitle[5]));
            cosemBuffer.Add(Convert.ToByte(SystemTitle[6]));
            cosemBuffer.Add(Convert.ToByte(SystemTitle[7]));
            return cosemBuffer;
        }

        private List<byte> SecuritySuitByte(string Securitysuit, string dedicatedkeyinfo)
        {
            List<byte> cosemBuffer = new List<byte>();
            cosemBuffer.Add(0xBE);
            if (dedicatedkeyinfo == "1")
                cosemBuffer.Add(0x34);
            else
                cosemBuffer.Add(0x23);
            cosemBuffer.Add(0x04);
            if (dedicatedkeyinfo == "1")
                cosemBuffer.Add(0x32);
            else
                cosemBuffer.Add(0x21);
            cosemBuffer.Add(0x21);
            if (dedicatedkeyinfo == "1")
                cosemBuffer.Add(0x30);
            else
                cosemBuffer.Add(0x1F);
            //cosemBuffer.Add(Convert.ToByte(Securitysuit));
            cosemBuffer.Add(Convert.ToByte(Securitysuit.Substring(0, 2), 16));
                return cosemBuffer;
       }

        public int SecuritySuitByte(byte[] Buffer, int nBufferIndex, int sootByte, int Dedicatekey)
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
            return nBufferIndex;
        }
       
        /// <summary>
        /// Add password tag and password to Buffer
        /// </summary>
        /// <param name="nSecMechanism"></param>
        /// <returns></returns>
        private List<byte> fAddRandomKey(string randomkey)
        {
            List<byte> RandomBuffer = new List<byte>();
            RandomBuffer.Add(0xAC);
            RandomBuffer.Add(0x12);
            RandomBuffer.Add(0x80);
            RandomBuffer.Add(0x10);
            RandomBuffer.Add(Convert.ToByte(randomkey[0]));
            RandomBuffer.Add(Convert.ToByte(randomkey[1]));
            RandomBuffer.Add(Convert.ToByte(randomkey[2]));
            RandomBuffer.Add(Convert.ToByte(randomkey[3]));
            RandomBuffer.Add(Convert.ToByte(randomkey[4]));
            RandomBuffer.Add(Convert.ToByte(randomkey[5]));
            RandomBuffer.Add(Convert.ToByte(randomkey[6]));
            RandomBuffer.Add(Convert.ToByte(randomkey[7]));
            RandomBuffer.Add(Convert.ToByte(randomkey[8]));
            RandomBuffer.Add(Convert.ToByte(randomkey[9]));
            RandomBuffer.Add(Convert.ToByte(randomkey[10]));
            RandomBuffer.Add(Convert.ToByte(randomkey[11]));
            RandomBuffer.Add(Convert.ToByte(randomkey[12]));
            RandomBuffer.Add(Convert.ToByte(randomkey[13]));
            RandomBuffer.Add(Convert.ToByte(randomkey[14]));
            RandomBuffer.Add(Convert.ToByte(randomkey[15]));
            return RandomBuffer;
        }
        private List<byte> GetMeterPassword(string Pass)
        {
            List<byte> PassBuffer = new List<byte>();
            //for (int passIndex = 0; passIndex < Pass.Length; passIndex++)
            //{
            //    PassBuffer.Add(Convert.ToByte(Pass[passIndex]));
            //}
                 
            if (SecurityMechanism == 0x01)
            {
                PassBuffer.Add(0xAC);
                PassBuffer.Add(0x0A);
                PassBuffer.Add(0x80);
                PassBuffer.Add(0x08);
                for (int passIndex = 0; passIndex < Pass.Length; passIndex++)
                {
                    PassBuffer.Add(Convert.ToByte(Pass[passIndex]));
                }
            }
            else if (SecurityMechanism == 0x02)
            {
                PassBuffer.Add(0xAC);
                PassBuffer.Add(0x12);
                PassBuffer.Add(0x80);
                PassBuffer.Add(0x10);
                for (int passIndex = 0; passIndex < Pass.Length; passIndex++)
                {
                    PassBuffer.Add(Convert.ToByte(HLSKey[passIndex]));
                }
            }

            return PassBuffer;


        }


       private List<byte> AddPassword()
        {
            List<byte> cosemBuffer = new List<byte>();
            int Passlength = Password.Length;
            if (SecurityMechanism == 0x01)
            {
                cosemBuffer.Add(0xAC);
                //cosemBuffer.Add(0x0A);
                cosemBuffer.Add(Convert.ToByte(Passlength+2));
                cosemBuffer.Add(0x80);
                cosemBuffer.Add(Convert.ToByte(Passlength));
                //cosemBuffer.Add(0x08);
               // for (int passIndex = 0; passIndex < 8; passIndex++)
                for (int passIndex = 0; passIndex < Password.Length; passIndex++)
                {
                    cosemBuffer.Add(Convert.ToByte(Password[passIndex]));
                }
            }
            else if (SecurityMechanism == 0x02)
            {
                cosemBuffer.Add(0xAC);
                cosemBuffer.Add(0x12);
                cosemBuffer.Add(0x80);
                cosemBuffer.Add(0x10);
                for (int passIndex = 0; passIndex < 16; passIndex++)
                {
                    cosemBuffer.Add(Convert.ToByte(Password[passIndex]));
                }
            }

            return cosemBuffer;
        }        

        /// <summary>
        ///  Used to Add User Info Tags and proposed DLMS Ver Number to Buffer
        /// </summary>
        /// <returns></returns>
        private List<byte> AddUserInf()
        {
            List<byte> cosemBuffer = new List<byte>();
            //BE10040E0100000006
            cosemBuffer.Add (0xBE);
            cosemBuffer.Add (0x10);
            cosemBuffer.Add (0x04);
            cosemBuffer.Add (0x0E);
            cosemBuffer.Add (0x01);
            cosemBuffer.Add (0x00);
            cosemBuffer.Add (0x00);
            cosemBuffer.Add (0x00);
            cosemBuffer.Add (0x06);      //Proposed DLMS Ver Number
            return cosemBuffer;
        }
        private List<byte> fAddUserInf_cypher(string DedicationKey)
        {
            List<byte> UserInfoBuffer = new List<byte>();
            //BE10040E0100000006
            int countlen = 0;
            int dedcounter = 0;
            int nBufferIndex=3;
            DedKeystr = "";
            UserInfoBuffer.Add(0x01);
            if (DedicationKey == "1")
            {
                DedKeystr = RandomHexString();
                UserInfoBuffer.Add(0x01);
                UserInfoBuffer.Add(0x10);
                while (countlen < DedKeystr.Length)
                {
                    UserInfoBuffer[nBufferIndex++] = Convert.ToByte(DedKeystr.Substring(countlen, 2), 16);
                    DedicatedKey[dedcounter++] = Convert.ToByte(DedKeystr.Substring(countlen, 2), 16);
                    countlen += 2;
                }
            }
            else
                UserInfoBuffer.Add(0x00);

            UserInfoBuffer.Add(0x00);
            UserInfoBuffer.Add(0x00);
            UserInfoBuffer.Add(0x06);      //Proposed DLMS Ver Number
            UserInfo = UserInfoBuffer.ToArray();
            return UserInfoBuffer;
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
        /// <summary>
        /// Used to Add Conformance Block Tags and proposed Conformance Block to Buffer
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="nBufferIndex"></param>
        /// <param name="cnfBlock"></param>
        /// <returns></returns>
        private List<byte> AddCnfBlock()
        {
            List<byte> cosemBuffer = new List<byte>();
            //5F1F040000121A
            cosemBuffer.Add (0x5F);
            cosemBuffer.Add (0x1F);
            cosemBuffer.Add (0x04);
            cosemBuffer.Add (0x00);
            //cosemBuffer.Add (0x00);
            //cosemBuffer.Add (0x18);
            //cosemBuffer.Add (0x1D);
            cosemBuffer.Add(0x1C);
            cosemBuffer.Add(0xFF);
            cosemBuffer.Add(0x3F);
            ConfBlock = cosemBuffer.ToArray();
            return cosemBuffer;
        }
           
        /// <summary>
        /// Check AARQ Response
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private CommunicationErrorType CheckAARQResponse(List <byte> buffer)
        {
            CommunicationErrorType errorCode = CommunicationErrorType.Success;
            int nCosemIndex = 0;
            if (buffer[nCosemIndex] != 0x61)
            {
                errorCode = CommunicationErrorType.InvalidAAREtag;
            }
            else
            {
                nCosemIndex = nCosemIndex + 17;
                if (buffer[nCosemIndex] != 0x00)
                {
                    errorCode = CommunicationErrorType.PasswordInavalid;
                }
                else
                {
                    if (SecurityMechanism == 0x01)
                    {
                        nCosemIndex = nCosemIndex + 22;
                    }
                    else if (SecurityMechanism == 0x02)
                    {
                        nCosemIndex = nCosemIndex + 55;
                    }

                    negotiatedPDUSize = 0;
                    negotiatedPDUSize = negotiatedPDUSize | (int)buffer[nCosemIndex] << 8;
                    nCosemIndex++;
                    negotiatedPDUSize = negotiatedPDUSize | (int)buffer[nCosemIndex];
                }
            }
            return errorCode;
        }

        public byte[] ParseAARE_generalAPDUs(int InitIndex, List<byte> buffer)
        {
            byte aareTag = buffer[InitIndex++];
            int noofReceivedBytes = buffer[InitIndex++];
            byte aareTagA1 = buffer[InitIndex++];
            if (aareTagA1 == 0xA1) //Application Context Name
            {
                byte aareTagA1Len = buffer[InitIndex++];
                InitIndex++; // No of Element
                byte lengthofElements = buffer[InitIndex++];//Length of Bytes
                 byte ApplicationContextName_Joint_iso_ccitt_Country = buffer[InitIndex++];                
                InitIndex += 2;
                byte ApplicationContextName_identified_organisation = buffer[InitIndex++];
                byte ApplicationContextName_DLMS_UA = buffer[InitIndex++];
                byte ApplicationContextName_application_context = buffer[InitIndex++];
                byte ApplicationContextName_Conext_id = buffer[InitIndex++];
            }
            byte aareTagA2 = buffer[InitIndex++];
            byte aareTagA2Len = buffer[InitIndex++];
            InitIndex += aareTagA2Len;
            byte aareTagA3 = buffer[InitIndex++];
            byte aareTagA3Len = buffer[InitIndex++];
            InitIndex += aareTagA3Len;
            byte aareTagA4 = buffer[InitIndex++];
            byte aareTagA4Len = buffer[InitIndex++];
            byte ServerSystemTytletag = buffer[InitIndex++];
            byte lengthServerSystemTytletag = buffer[InitIndex++];        
            InitIndex += lengthServerSystemTytletag;           
            byte aareTagAuthenticationRequired = buffer[InitIndex++];//acse-requirements field 0x88
            if (aareTagAuthenticationRequired == 0x88)
            {
                InitIndex++;// tagged component’s value field 0x02
                InitIndex++;//0x07
                InitIndex++;//encoding of the authentication functional unit (0) 0x80
                byte aareTagMechanism_name = buffer[InitIndex++];//Mechanism-name 0x89
                if (aareTagMechanism_name == 0x89) //Mechanism-name 0x89
                {
                    byte aareTagMechanism_len = buffer[InitIndex++];// No of Element
                    byte MechanishName_Joint_iso_ccitt_Country = buffer[InitIndex++];                   
                    InitIndex += 2;
                    byte MechanishName_identified_organisation = buffer[InitIndex++];
                    byte MechanishName_DLMS_UA = buffer[InitIndex++];
                    byte MechanishName_authentication_mechanism_name = buffer[InitIndex++];
                    byte MechanishName_mechanism_id = buffer[InitIndex++];
                }               
            }
            InitIndex++;//aare chalenge tag 0xAA
            InitIndex++;//len of aare chalenge tag
            InitIndex++;//Authentication-value tag 0x80
            byte lenofchalengeValue = buffer[InitIndex++];//Len of Encoded value field
           // GlobalObjects.objGlobalFunctions.serverchallenge = new byte[lenofchalengeValue];
           // System.Buffer.BlockCopy(buffer, InitIndex, GlobalObjects.objGlobalFunctions.serverchallenge, 0, lenofchalengeValue);
           InitIndex += lenofchalengeValue;
            InitIndex++;// encoding the tag for the user-information field 0xBE
            InitIndex++;//len of encoding the tag for the user-information field
            InitIndex++;//encoding of the choice for user-information tag 0x04
            byte lenofCipheredBytes = buffer[InitIndex++];//len of encoded data
            if (buffer[InitIndex++] == 0xDB)//Global Encryption
            {
                byte lenofSystemTytle = buffer[InitIndex++];//Len of Encoded value field
                InitIndex++;
                InitIndex += lenofSystemTytle;
            }
            lenofCipheredBytes = buffer[InitIndex++];//len of encoded data
            byte aareSecuritySuitByte = buffer[InitIndex++];//securitySuitByte 0x30
                                                            //long InvoDecrypt = DLMSPayloadParser.FormatData(buffer, InitIndex, 4, false);
            int InvoDecrypt = 0;
            InvoDecrypt = (InvoDecrypt | (int)buffer[InitIndex]) << 24;
            InvoDecrypt = (InvoDecrypt | (int)buffer[InitIndex + 1]) << 16;
            InvoDecrypt = (InvoDecrypt | (int)buffer[InitIndex + 2]) << 8;
            InvoDecrypt = (InvoDecrypt | (int)buffer[InitIndex + 3]);
            InitIndex += 4;//Invocation Counter            
            byte actuallenofCipheredBytes = (byte)(lenofCipheredBytes - 17);//1 Security Control Byte + 4 byte Invocation Counter + 12 byte Authentication Tag
            if (SecurityMechanism == 0x01) 
            {
                DecyplainText = new byte[noofReceivedBytes];
                CypherDataDecypt = new byte[noofReceivedBytes];
            }
            else if (SecurityMechanism == 0x02)
            {
                DecyplainText = new byte[actuallenofCipheredBytes];
                CypherDataDecypt = new byte[actuallenofCipheredBytes];
            }
            System.Buffer.BlockCopy(buffer.ToArray(), InitIndex, CypherDataDecypt, 0, CypherDataDecypt.Length);          
            AADDecypt = new byte[AAD.Length];
            System.Buffer.BlockCopy(AAD, 0, AADDecypt, 0, AAD.Length);          
            AuthTagDecypt = new byte[12];
            if (SecurityMechanism == 0x02)
            {
                System.Buffer.BlockCopy(buffer.ToArray(), InitIndex + CypherDataDecypt.Length, AuthTagDecypt, 0, 12);
            }          
            InitVectorDecypt[8] = Convert.ToByte((InvoDecrypt & 0xFF000000) >> 24);
            InitVectorDecypt[9] = Convert.ToByte((InvoDecrypt & 0xFF0000) >> 16);
            InitVectorDecypt[10] = Convert.ToByte((InvoDecrypt & 0xFF00) >> 8);
            InitVectorDecypt[11] = Convert.ToByte(InvoDecrypt & 0x00FF);
            if (SecurityMechanism == 0x01)
            {
                ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, (ushort)DecyplainText.Length, CypherDataDecypt, InitVectorDecypt, 12, null, 0, ref AuthTagDecypt, 0, ChannelNum);
            }
            else if (SecurityMechanism == 0x02)
            {
                uint AADLength = Convert.ToUInt16(AAD.Length + CypherDataDecypt.Length);
                ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, (ushort)DecyplainText.Length, CypherDataDecypt, InitVectorDecypt, 12, AADDecypt, (ushort)AADDecypt.Length, ref AuthTagDecypt, AuthTagLen, ChannelNum);
            }
            negotiatedPDUSize = 0;
            negotiatedPDUSize = negotiatedPDUSize | (int)DecyplainText[DecyplainText.Length - 4] << 8;
            // nCosemIndex++;
            negotiatedPDUSize = negotiatedPDUSize | (int)DecyplainText[DecyplainText.Length - 3];
            return DecyplainText;
        }

        /// <summary>
        /// Used to get Cosem Response 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private CommunicationErrorType CheckCOSEMResponse(List<byte> buffer)
        {
            CommunicationErrorType errorCode = CommunicationErrorType.Success;
            bool normalOrBlockPacket = false;
            try
            {
                int cosemIndex = 0;
                if (buffer[cosemIndex] != 0xC4 && buffer[cosemIndex] != 0xC7 && buffer[cosemIndex] != 0xbb && HDLCCommunication.prevPktSegmentationStatus == false && HDLCCommunication.currentPktSegmentationStatus == false)
                {
                    errorCode = CommunicationErrorType.InvalidGetResponseTag;
                }
                else
                {
                    if (buffer[cosemIndex] == 0xC4 || buffer[cosemIndex] == 0xC7) normalOrBlockPacket = true;
                    cosemIndex = cosemIndex + 1;
                    if (normalOrBlockPacket == false && HDLCCommunication.prevPktSegmentationStatus == true)
                    {
                        cosemBlockBuffer.AddRange(buffer);//.GetRange(0, buffer.Count); 
                    }
                    else if (buffer[cosemIndex] == 0x02)
                    {
                        cosemIndex = cosemIndex + 2;
                        if (buffer[cosemIndex] == 0x00)
                        {
                            cosemIndex = cosemIndex + 3;
                            blockNumber = 0;
                            blockNumber = blockNumber | (int)buffer[cosemIndex] << 8;
                            cosemIndex++;
                            blockNumber = blockNumber | (int)buffer[cosemIndex];

                            cosemIndex = cosemIndex + 2;
                            int nBlockByteCount = 0;
                            int lengthOfLength = buffer[cosemIndex];
                            if (HDLCCommunication.currentPktSegmentationStatus == false)
                            {
                                if (lengthOfLength > 0x80)
                                {
                                    cosemIndex = cosemIndex + 1;
                                    lengthOfLength = lengthOfLength - 0x80; /// &0x80; 
                                    for (int lengthIndex = 0; lengthIndex < lengthOfLength; lengthIndex++)
                                    {
                                        nBlockByteCount = (nBlockByteCount << 8) | buffer[cosemIndex];
                                        cosemIndex = cosemIndex + 1;
                                    }
                                }
                                else
                                {
                                    nBlockByteCount = buffer[cosemIndex];
                                    cosemIndex = cosemIndex + 1;
                                }

                                cosemBlockBuffer.AddRange(buffer.GetRange(cosemIndex, nBlockByteCount));
                                errorCode = CommunicationErrorType.BlockTransferNext;
                            }
                            else
                            {
                                int templengthOfsegmentationpkt = cosemIndex;
                                if (buffer[cosemIndex] == 0x80) { templengthOfsegmentationpkt += 1; }
                                else if (buffer[cosemIndex] == 0x81) { templengthOfsegmentationpkt += 2; }
                                else if (buffer[cosemIndex] == 0x82) { templengthOfsegmentationpkt += 3; }
                                cosemBlockBuffer.AddRange(buffer.GetRange(templengthOfsegmentationpkt, buffer.Count - templengthOfsegmentationpkt));
                                errorCode = CommunicationErrorType.Success;
                                HDLCCommunication.blockWithSegmentation = true;
                            }

                            
                        }
                        else
                        {
                            cosemIndex = cosemIndex + 6;
                            int nBlockByteCount = 0;
                            int lengthOfLength = buffer[cosemIndex];
                            if (HDLCCommunication.currentPktSegmentationStatus == false)
                            {
                                if (lengthOfLength > 0x80)
                                {
                                    cosemIndex = cosemIndex + 1;
                                    lengthOfLength = lengthOfLength - 0x80; /// &0x80; 
                                    for (int lengthIndex = 0; lengthIndex < lengthOfLength; lengthIndex++)
                                    {
                                        nBlockByteCount = (nBlockByteCount << 8) | buffer[cosemIndex];
                                        cosemIndex = cosemIndex + 1;
                                    }
                                }
                                else
                                {
                                    nBlockByteCount = buffer[cosemIndex];
                                    cosemIndex = cosemIndex + 1;
                                }
                                cosemBlockBuffer.AddRange(buffer.GetRange(cosemIndex, nBlockByteCount));
                            }
                            else
                            {
                                int templengthOfsegmentationpkt = cosemIndex;
                                if (buffer[cosemIndex] == 0x80) { templengthOfsegmentationpkt += 1; }
                                else if (buffer[cosemIndex] == 0x81) { templengthOfsegmentationpkt += 2; }
                                else if (buffer[cosemIndex] == 0x82) { templengthOfsegmentationpkt += 3; }
                                cosemBlockBuffer.AddRange(buffer.GetRange(templengthOfsegmentationpkt, buffer.Count - templengthOfsegmentationpkt));
                                errorCode = CommunicationErrorType.Success;
                                HDLCCommunication.blockWithSegmentation = true;
                            }
                            blockNumber = 0x00;
                            errorCode = CommunicationErrorType.BlockTransferLast;
                        }
                    }                   
                    else if (buffer[cosemIndex] == 0x01)
                    {
                        cosemIndex = cosemIndex + 2;
                        if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                        {

                            cosemBlockBuffer = buffer.GetRange(4, buffer.Count - 4);
                            errorCode = CommunicationErrorType.Success;
                        }
                        else
                        {
                            errorCode = CommunicationErrorType.AccessDenied;
                        }
                    }                   
                    else
                    {
                        cosemIndex = cosemIndex + 2;
                        if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                        {

                            cosemBlockBuffer = buffer.GetRange(4, buffer.Count - 4);
                            errorCode = CommunicationErrorType.Success;
                        }
                        else
                        {
                            errorCode = CommunicationErrorType.AccessDenied;
                        }
                    }
                    
                }

            }
            catch (Exception)
            {
            }
            return errorCode;
        }       

        /// <summary>
        /// Used to get Cosem Response for write command  
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private CommunicationErrorType CheckCOSEMWriteResponse(List<byte> buffer)
        {
            CommunicationErrorType errorCode = CommunicationErrorType.Success;
            int cosemIndex = 0;
            if (buffer[cosemIndex] != 0xC5)
            {
                errorCode = CommunicationErrorType.InvalidGetResponseTag;
            }
            else
            {
                cosemIndex = cosemIndex + 1;
                if (buffer[cosemIndex] == 0x02)
                {
                    cosemIndex = cosemIndex + 1;
                    if (buffer[cosemIndex + 1] != 0x00)
                        errorCode = CommunicationErrorType.AccessDenied;
                    else
                    {
                        cosemIndex = cosemIndex + 3;
                        blockNumber = 0;
                        blockNumber = blockNumber | (int)buffer[cosemIndex] << 8;
                        cosemIndex++;
                        blockNumber = blockNumber | (int)buffer[cosemIndex];
                        errorCode = CommunicationErrorType.BlockTransferNext;
                        
                    }
                }
                else if (buffer[cosemIndex] == 0x03)
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                    {
                        errorCode = CommunicationErrorType.BlockTransferLast;
                    }
                    else
                    {
                        errorCode = CommunicationErrorType.AccessDenied;
                    }
                }
                else
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                    {
                        errorCode = CommunicationErrorType.Success;
                    }
                    else
                    {
                        errorCode = CommunicationErrorType.AccessDenied;
                    }
                }
            }
            return errorCode;
        }

        private CommunicationErrorType CheckCOSEMBlockWriteResponse_Cipher(List<byte> buffer)
        {
            CommunicationErrorType errorCode = CommunicationErrorType.Success;
            int cosemIndex = 0;
            if (buffer[cosemIndex] != 0xC5)
            {
                errorCode = CommunicationErrorType.InvalidGetResponseTag;
            }
            else
            {
                cosemIndex = cosemIndex + 1;
                if (buffer[cosemIndex] == 0x02)
                {
                    cosemIndex = cosemIndex + 1;
                    if (buffer[cosemIndex + 1] != 0x00)
                        errorCode = CommunicationErrorType.AccessDenied;
                    else
                    {
                        cosemIndex = cosemIndex + 3;
                        blockNumber = 0;
                        blockNumber = blockNumber | (int)buffer[cosemIndex] << 8;
                        cosemIndex++;
                        blockNumber = blockNumber | (int)buffer[cosemIndex];
                        errorCode = CommunicationErrorType.BlockTransferNext;
                        blockNumber++;
                    }
                }
                else if (buffer[cosemIndex] == 0x03)
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                    {
                        errorCode = CommunicationErrorType.BlockTransferLast;
                    }
                    else
                    {
                        errorCode = CommunicationErrorType.AccessDenied;
                    }
                }
                else
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                    {
                        errorCode = CommunicationErrorType.Success;
                    }
                    else
                    {
                        errorCode = CommunicationErrorType.AccessDenied;
                    }
                }
            }
            return errorCode;
        }


        private CommunicationErrorType CheckCOSEMWriteResponse_Cipher(List<byte> buffer)
        {
            CommunicationErrorType errorCode = CommunicationErrorType.Success;
            int cosemIndex = 0;
            if (buffer[cosemIndex] != 0xC5)
            {
                errorCode = CommunicationErrorType.InvalidGetResponseTag;
            }
            else
            {
                cosemIndex = cosemIndex + 1;
                if (buffer[cosemIndex] == 0x02)
                {
                    cosemIndex = cosemIndex + 1;
                    if (buffer[cosemIndex + 1] != 0x00)
                        errorCode = CommunicationErrorType.AccessDenied;
                    else
                    {
                        cosemIndex = cosemIndex + 3;
                        blockNumber = 0;
                        blockNumber = blockNumber | (int)buffer[cosemIndex] << 8;
                        cosemIndex++;
                        blockNumber = blockNumber | (int)buffer[cosemIndex];
                        errorCode = CommunicationErrorType.BlockTransferNext;
                    }
                }
                else if (buffer[cosemIndex] == 0x03)
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                    {
                        errorCode = CommunicationErrorType.BlockTransferLast;
                    }
                    else
                    {
                        errorCode = CommunicationErrorType.AccessDenied;
                    }
                }
                else
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                    {
                        errorCode = CommunicationErrorType.Success;
                    }
                    else
                    {
                        errorCode = CommunicationErrorType.AccessDenied;
                    }
                }
            }
            return errorCode;
        }
        /// <summary>
        /// Used to get Cosem Response for write command  
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private CommunicationErrorType CheckCOSEMResetResponse(List<byte> buffer)
        {
            CommunicationErrorType errorCode = CommunicationErrorType.Success;
            int cosemIndex = 0;
            if (buffer[cosemIndex] != 0xC7)
            {
                errorCode = CommunicationErrorType.InvalidGetResponseTag;
            }
            else
            {
                cosemIndex = cosemIndex + 1;
                if (buffer[cosemIndex] == 0x02)
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)
                    {
                        cosemIndex = cosemIndex + 3;
                        blockNumber = 0;
                        blockNumber = blockNumber | (int)buffer[cosemIndex] << 8;
                        cosemIndex++;
                        blockNumber = blockNumber | (int)buffer[cosemIndex];

                        cosemIndex = cosemIndex + 2;
                        byte nBlockByteCount = buffer[cosemIndex];
                        cosemIndex = cosemIndex + 1;

                        cosemBlockBuffer.AddRange(buffer.GetRange(cosemIndex, nBlockByteCount));

                        errorCode = CommunicationErrorType.BlockTransferNext;
                    }
                    else
                    {
                        cosemIndex = cosemIndex + 6;
                        byte blockByteCount = buffer[cosemIndex];
                        cosemIndex = cosemIndex + 1;
                        cosemBlockBuffer.AddRange(buffer.GetRange(cosemIndex, blockByteCount));
                        blockNumber = 0x00;
                        errorCode = CommunicationErrorType.BlockTransferLast;
                    }
                }
                else
                {
                    cosemIndex = cosemIndex + 2;
                    if (buffer[cosemIndex] == 0x00)   //Get.response.Normal
                    {

                        cosemBlockBuffer = buffer.GetRange(4, buffer.Count - 4);
                        errorCode = CommunicationErrorType.Success;
                    }
                    else
                    {
                        errorCode = CommunicationErrorType.AccessDenied;
                    }
                }
            }
            return errorCode;
        }
       
        /// <summary>
        /// Creates and returns block transfer packet
        /// </summary>
        /// <returns></returns>
        private List<byte> GetBlockTransferPacket_Cipher()
        {
            List<byte> blockBufferCipher = new List<byte>();
            int countindex = 7;
            byte[] plaintextcommandbyte = new byte[countindex];

            if (DedKeystr != "")
                blockBufferCipher.Add(0xD0);
            else
                blockBufferCipher.Add(0xC8);
            blockBufferCipher.Add(0x00);
           // blockBufferCipher.Add(0x00);//security suit added "0"

            if (SecurityMechanism == 0x01)
                blockBufferCipher.Add(0x20);
            else
                blockBufferCipher.Add(0x30);//Security suit actual value
            InitializationCounter++;
            if (DedKeystr != "")
            {
               blockBufferCipher.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));
                blockBufferCipher.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
                blockBufferCipher.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
                blockBufferCipher.Add(Convert.ToByte(InitializationCounter & 0x00FF));

            }
            else
            {
                blockBufferCipher.Add(Convert.ToByte((InitializationCounter & 0xFF000000) >> 24));
                blockBufferCipher.Add(Convert.ToByte((InitializationCounter & 0xFF0000) >> 16));
                blockBufferCipher.Add(Convert.ToByte((InitializationCounter & 0xFF00) >> 8));
                blockBufferCipher.Add(Convert.ToByte(InitializationCounter & 0x00FF));
            }
            plaintextcommandbyte[0] = 0xC0;
            plaintextcommandbyte[1] = 0x02;
            plaintextcommandbyte[2] = 0x81;
            plaintextcommandbyte[3] = 0x00;
            plaintextcommandbyte[4] = 0x00;
            plaintextcommandbyte[5] = Convert.ToByte(blockNumber >> 8);
            plaintextcommandbyte[6] = Convert.ToByte(blockNumber & 0xFF);
            //blockBufferCipher.Add(0xC0);
            //blockBufferCipher.Add(0x02);      //need to change
            //blockBufferCipher.Add(0x81);
            //blockBufferCipher.Add(0x00);
            //blockBufferCipher.Add(0x00);
            //blockBufferCipher.Add(Convert.ToByte(blockNumber >> 8));
            //blockBufferCipher.Add(Convert.ToByte(blockNumber & 0xFF));

            //*******************AES GCM Encrypt ********************************
            blockBufferCipher.AddRange(CreateCipherCommand(plaintextcommandbyte, 0));
            blockBufferCipher[1] = (byte)(blockBufferCipher.Count - 2);
          
            return blockBufferCipher;
        }      
        private List<byte> GetBlockTransferPacket()
        {

            List<byte> blockBuffer = new List<byte>();
            blockBuffer.Add(0xC0);
            blockBuffer.Add(0x02);      //need to change
            blockBuffer.Add(0x81);
            blockBuffer.Add(0x00);
            blockBuffer.Add(0x00);
            blockBuffer.Add(Convert.ToByte(blockNumber >> 8));
            blockBuffer.Add(Convert.ToByte(blockNumber & 0xFF));

            return blockBuffer;
        }             
        #endregion
      
        //'******************************************************************************
        //'
        //'  NAME     : IntializationVector Client Initialization Vector by Deep
        //'
        //'  INPUT    : System Title
        //'
        //'  OUTPUT   : Invocation Counter 
        //'
        //'  PURPOSE  : Read Counter of SNRM command in pc mode
        //'
        //'*******************************************************************************

        public void IntializationVector(string systitle,long initcount)
        {
           // long clientinovationCount = GlobalObjects.objHDLCLIB.InitializationCounter;
            long clientinovationCount = initcount;
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
       
        ////'******************************************************************************
        ////'
        ////'  NAME     : fCheckHDLCResponse
        ////'
        ////'  INPUT    : none
        ////'
        ////'  OUTPUT   : true or False
        ////'
        ////'  PURPOSE  : Check Start/end tag, Check FCS , Check destination Address and Check command Byte
        ////'
        ////'*******************************************************************************
        //private bool fCheckHDLCResponse(byte[] Buffer, int nClientSAP)
        //{

        //    if (GlobalObjects.objHDLCLIB.fCheckStartEndTag(Buffer) == false)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (GlobalObjects.objHDLCLIB.fCheckFCS(Buffer) == false)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            if (GlobalObjects.objHDLCLIB.fCheckServerSAP(Buffer, nClientSAP) == false)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                if (GlobalObjects.objHDLCLIB.fCheckCommand(Buffer, GlobalObjects.objHDLCLIB.nCMDByte) == false)
        //                {
        //                    return false;
        //                }
        //                else
        //                {
        //                    return true;
        //                }
        //            }
        //        }

        //    }
        //}
        //private string GetServerSeed(int StarBytePos)
        //{
        //    try
        //    {
        //        string ReceivedSeed = string.Empty;
        //        int icnt = StarBytePos;
        //        char[] chararray = System.Text.Encoding.UTF8.GetString(GlobalObjects.objSerialComm.ReceiveBuffer).ToCharArray();
        //        while (icnt < StarBytePos + 16) ReceivedSeed += Convert.ToChar(GlobalObjects.objSerialComm.ReceiveBuffer[icnt++]).ToString();
        //        return ReceivedSeed;
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //}
        //'******************************************************************************
        //'
        //'  NAME     : Receive plaint text byte by Deep
        //'
        //'  INPUT    : Cyphered Text
        //'
        //'  OUTPUT   : Plain Text 
        //'
        //'  PURPOSE  : Receive plaint text byte from cyphered text
        //'
        //'*******************************************************************************
        public byte[] GetPlainTextFromCipheredTest(int InitIndex, List<byte> RecieveDataBuffer)
        {
            //InitVectorDecypt = new byte[12];
            //int IIndex = 0;
            //int buffIndex = 29;
            //while (IIndex < InitVectorDecypt.Length - 4)
            //{
            //    InitVectorDecypt[IIndex] = RecieveDataBuffer[buffIndex + IIndex];
            //    IIndex++;
            //}
            int noofReceivedBytes = RecieveDataBuffer[1] - (5); //5--> tag 30 0ne byte & intcounter 4 byte
           // int noofReceivedBytes = GlobalObjects.objSerialComm.ReceiveBuffer[15] - (5); //5--> tag 30 0ne byte & intcounter 4 byte
            int cipherDataStartIndex = 7;
            if (RecieveDataBuffer[1] == 0x81)
            {
                noofReceivedBytes = RecieveDataBuffer[2] - (5);
                cipherDataStartIndex++;
                InitIndex++;
            }
            else if (RecieveDataBuffer[1] == 0x82)
            {
                noofReceivedBytes = ((byte)(RecieveDataBuffer[2] & 0x1F) * 0x100 + (byte)(RecieveDataBuffer[3]));
                noofReceivedBytes = noofReceivedBytes - (5);
                cipherDataStartIndex += 2;
                InitIndex += 2;
            }

            if (SecurityMechanism == 0x01)
            {
                DecyplainText = new byte[noofReceivedBytes];
                CypherDataDecypt = new byte[noofReceivedBytes];
            }
            else if (SecurityMechanism == 0x02)
            {
                DecyplainText = new byte[noofReceivedBytes - 12];
                CypherDataDecypt = new byte[noofReceivedBytes - 12];
            }
            System.Buffer.BlockCopy(RecieveDataBuffer.ToArray(), cipherDataStartIndex, CypherDataDecypt, 0, CypherDataDecypt.Length);

            AADDecypt = new byte[AAD.Length + CypherDataDecypt.Length];
            System.Buffer.BlockCopy(AAD, 0, AADDecypt, 0, AAD.Length);
            System.Buffer.BlockCopy(CypherDataDecypt, 0, AADDecypt, AAD.Length, CypherDataDecypt.Length);

            AuthTagDecypt = new byte[12];
            if (SecurityMechanism == 0x02)
            {
                System.Buffer.BlockCopy(RecieveDataBuffer.ToArray(), cipherDataStartIndex + CypherDataDecypt.Length, AuthTagDecypt, 0, 12);
            }
            int InvoDecrypt = 0;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[InitIndex]) << 24;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[InitIndex + 1]) << 16;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[InitIndex + 2]) << 8;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[InitIndex + 3]);
            InitVectorDecypt[8] = Convert.ToByte((InvoDecrypt & 0xFF000000) >> 24);
            InitVectorDecypt[9] = Convert.ToByte((InvoDecrypt & 0xFF0000) >> 16);
            InitVectorDecypt[10] = Convert.ToByte((InvoDecrypt & 0xFF00) >> 8);
            InitVectorDecypt[11] = Convert.ToByte(InvoDecrypt & 0x00FF);
            if (SecurityMechanism == 0x01)
            {
                ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, (ushort)DecyplainText.Length, CypherDataDecypt, InitVectorDecypt, 12, null, 0, ref AuthTagDecypt, 0, ChannelNum);
            }
            else if (SecurityMechanism == 0x02)
            {
                uint AADLength = Convert.ToUInt16(AAD.Length + CypherDataDecypt.Length);
                ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, (ushort)DecyplainText.Length, CypherDataDecypt, InitVectorDecypt, 12, AADDecypt, AADLength, ref AuthTagDecypt, AuthTagLen, ChannelNum);
            }
            return DecyplainText;
        }

        public byte[] GetPlainTextFromRLRQ(List<byte> RecieveDataBuffer)
        {
            CypherDataDecypt = new byte[24];
            System.Buffer.BlockCopy(RecieveDataBuffer.ToArray(), 7, CypherDataDecypt, 0, 24);
            cyphertext = new byte[31];
            AuthTagDecypt = new byte[12];
            System.Buffer.BlockCopy(RecieveDataBuffer.ToArray(), 31, AuthTagDecypt, 0, 12);
            DecyplainText = new byte[24];
            int InvoDecrypt = 0;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[3]) << 24;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[4]) << 16;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[5]) << 8;
            InvoDecrypt = (InvoDecrypt | (int)RecieveDataBuffer[6]);
            InitVectorDecypt[8] = Convert.ToByte((InvoDecrypt & 0xFF000000) >> 24);
            InitVectorDecypt[9] = Convert.ToByte((InvoDecrypt & 0xFF0000) >> 16);
            InitVectorDecypt[10] = Convert.ToByte((InvoDecrypt & 0xFF00) >> 8);
            InitVectorDecypt[11] = Convert.ToByte(InvoDecrypt & 0x00FF);

            // InitVectorDecypt[11] = 0x01;
            ManageObj.p_securityLibDecrypt(Encryptionmethod, EncryKey, 16, ref DecyplainText, 24, CypherDataDecypt, InitVectorDecypt, 12, AADDecypt, 31, ref AuthTagDecypt, AuthTagLen, ChannelNum);
            return DecyplainText;
        }

        //********************************* Create AES GCM Encryption start***************************************
       
        private List<byte> CreateCipherCommand(byte[] cmdplaintext, int nBufferIndex)
        
        {
            List<byte> CypherBuffer = new List<byte>();
            IntializationVector(clientSystemTitle, InitializationCounter);
            cyphertext = new byte[cmdplaintext.Length];
            AuthenticationTag = new byte[AuthTagLen];
            if (SecurityMechanism == 0x01 && DedKeystr == "") //---MR Mode Encryption
            {
                ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, 16, cmdplaintext, (uint)cmdplaintext.Length, ref cyphertext, ClientInitVector, 12, null, 0, ref AuthenticationTag, 0, ChannelNum);
            }
            else if (SecurityMechanism == 0x02 || DedKeystr != "") //--US Mode Encryption with Dedicated key
            {
                if (DedKeystr != "")
                {
                    //***************Using Dedicated key in place of Encryption key*********
                    int IndexLen = 0;
                    int Enckcount = 0;
                    while (IndexLen < DedKeystr.Length)
                    {
                        // EncryKey = GlobalObjects.objCOSEMLIB.DedicatedKey;
                        EncryKey[Enckcount++] = Convert.ToByte(DedKeystr.Substring(IndexLen, 2), 16);
                        IndexLen += 2;
                    }
                }

                ManageObj.p_securityLibEncrypt(Encryptionmethod, EncryKey, 16, cmdplaintext, (uint)cmdplaintext.Length, ref cyphertext, ClientInitVector, 12, AAD, 17, ref AuthenticationTag, AuthTagLen, ChannelNum);
            }

            byte[] cypherData = cyphertext;
            byte[] AuthenData = AuthenticationTag;

            CypherBuffer.AddRange(fAddCyphered_Tag(cypherData));// Add Cypher Tag
            if (SecurityMechanism == 0x02)
                CypherBuffer.AddRange(fAddAuthentication_Tag(AuthenData));//Add Authentication Tag
            return CypherBuffer;
        }
    }
}
