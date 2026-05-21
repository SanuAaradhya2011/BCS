#region Namespaces
using System;
using System.Collections.Generic;
using GPRSCommunication;
using CABCommunication.Common; 
#endregion

namespace CABCommunication.PhysicalLayer
{
    public class GPRS : Channel,IPhysicalChannel
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private ChannelInformation channelInfo = null;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Modem IMEI number
        /// </summary>
        public string ImeiNumber { get; set; }
        #endregion

        #region Constructors     
        /// <summary>
        /// Default Constructor
        /// </summary>
        public GPRS()
        {         
        }
         /// <summary>
        /// This operation will be invoked by User to assign channel properties.
        /// </summary>
        /// <param name="channelDetail"></param>
        public GPRS(ChannelInformation channelInfo)
        {
            ImeiNumber = channelInfo.ModemInfo;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baud"></param>
        public override void SetBaud(byte baud)
        {
            
        }
        /// <summary>
        /// This operation will be invoked by the Adapter user to open session(s) .
        /// This will also open specified com/IP port.
        /// </summary>
        /// <returns></returns>
        //public bool OpenSession()
        //{

        //    bool success = false;
        //    try
        //    {
        //        if (this.IsOpen == true)
        //        {
        //            this.DiscardInBuffer();
        //            this.Close();
        //        }
        //        this.BaudRate = int.Parse(baudRate);
        //        this.DataBits = int.Parse(dataBits);
        //        this.StopBits = (StopBits)Enum.Parse(typeof(StopBits), this.stopBits);
        //        this.Parity = (Parity)Enum.Parse(typeof(Parity), this.parity);
        //        this.PortName = this.portName;
        //        this.Handshake = Handshake.None;


        //        this.RtsEnable = true;
        //        this.DtrEnable = true;

        //        this.Open();
        //        success = true;



        //    }
        //    catch (Exception ex)
        //    {
        //        success = false;
        //        logger.Log(LOGLEVELS.Error, "Error while file upload", ex);
        //    }
        //    return success;
        //}
        
        /// <summary>
        /// Sends data/command for GPRS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override Result Send(byte[] Buffer, int dataLength)
        {
            Result commResult = new Result();
            commResult.RecieveDataBuffer = new List<byte>();
            //Create GPRS Comm Manager Instance for DLMS. 
            GPRSCommManager commManager = new GPRSCommManager(MeterCommandType.DLMS);
            if (!string.IsNullOrEmpty(ImeiNumber))
            {
                //Push the data to M2M
                //byte[] response = commManager.SendDataToGPRS(ImeiNumber, Buffer);


                
                List<byte> temCommand = new List<byte>();
                int cmdIndex = 0;
                while (cmdIndex < dataLength)
                {
                    temCommand.Add(Buffer[cmdIndex++]);
                }
                byte[] response = commManager.SendDataToGPRS(ImeiNumber, temCommand.ToArray()); 









                //If response is received then communication is successful else not
                if (response != null && response.Length > 0) 
                {
                    commResult.RecieveDataBuffer.AddRange(response);
                    commResult.ErrorCode = CommunicationErrorType.Success;
                    commResult.RecieveDataLength = response.Length;
                }
                else
                {
                    commResult.ErrorCode = CommunicationErrorType.ModemORMeterNotConnected;
                   // throw new Exception("Connection with modem/meter is not available. Please try after some time.");
                }
            }
            return commResult; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool OpenSession()
        {
            ////Create GPRS Comm Manager Instance for DLMS. 
            //GPRSCommManager commManager = new GPRSCommManager(MeterCommandType.DLMS);
            //if (!string.IsNullOrEmpty(ImeiNumber))
            //{
            //    //Push the data to M2M
            //    byte[] response = commManager.SendDataToGPRS(ImeiNumber, Buffer);

            //    //If response is received then communication is successful else not
            //    if (response != null && response.Length > 0)
            //    {
            //        commResult.RecieveDataBuffer.AddRange(response);
            //        commResult.ErrorCode = CommunicationErrorType.Success;
            //        commResult.RecieveDataLength = response.Length;
            //    }
            //    else
            //    {
            //        throw new Exception("Connection with modem/meter is not available. Please try after some time.");
            //    }
            //}
            return true;  
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CloseSession()
        {
            return true;
        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

    }
}
