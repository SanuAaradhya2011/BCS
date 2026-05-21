#region Namespaces

#endregion

namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelInformation
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string CommunicationMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ComPort { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ModemInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
         public string TcpPort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte SecurityMechanism { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProtocolType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte NoOfRetries { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GlobalEncryptionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AuthenticationKey { get; set; }
       

        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion

    }
}
