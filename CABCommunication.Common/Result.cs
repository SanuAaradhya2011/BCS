#region Namespaces
using System.Collections.Generic;
#endregion

namespace CABCommunication.Common
{
    /// <summary>
    /// Represents the data structure that encapsulates the result 
    /// information for communication channel.
    /// </summary>
    public class Result
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties

        /// <summary>
        /// Specifies the result error code.
        /// </summary>
        public CommunicationErrorType  ErrorCode { get; set; }

        /// <summary>
        /// Specifies the data buffer length for the received bytes.
        /// </summary>
        public long  RecieveDataLength { get; set; }
      	
	
        /// <summary>
        /// Specifies the received data buffer.
        /// </summary>
        public List<byte>  RecieveDataBuffer { get; set; }
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
