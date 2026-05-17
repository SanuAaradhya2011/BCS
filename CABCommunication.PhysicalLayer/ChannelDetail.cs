#region Namespaces
using CABCommunication.Common;
#endregion



namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// Represents the data structure that encapsulates the 
    /// properties of communication channel.
    /// </summary>
    public class ChannelDetail
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties

        /// <summary>
        /// Specified COM Port number.
        /// </summary>
        public string ComPort { get; set; }

        /// <summary>
        /// Baud  rate for the opening sequence.
        /// </summary>
        public string  InitialBaudRate { get; set; }

        /// <summary>
        /// Baud  rate for the Communication sequence
        /// </summary>
        public string  BaudRate { get; set; }

        /// <summary>
        /// Used for error detection purposes. It gives the data either an odd or even parity, 
        /// which is used to validate the integrity of the data.
        /// </summary>
        public string  Parity { get; set; }    
            
        /// <summary>
        /// Used to send at the end of every character allow the receiving signal hardware to detect 
        /// the end of a character and to resynchronize with the character stream. 
        /// </summary>
        public string  StopBits { get; set; }  
           
        /// <summary>
        /// Used to specify the number of data bits in each character.
        /// </summary>
        public string  DataBits { get; set; }  
        
        /// <summary>
        /// Defines the time, expressed in milliseconds over which, 
        /// when any frame is received, client will process a disconnection.
        /// </summary>
        public long  ResponseTimeout { get; set; }  

        /// <summary>
        /// Defines the time, expressed in milliseconds, over which, when any 
        /// character is received, the device will treat the already received data as a complete frame.
        /// </summary>
        public long  InterCharacterDelay { get; set; }

        /// <summary>
        /// If no response frame is received, the client will repeat the 
        /// transmission of the command frame NumberOfRetries times.
        /// </summary>
        public byte  NumberOfRetry { get; set; }

        /// <summary>
        /// Specifies the Channel type.
        /// </summary>
        public ChannelType ChannelType { get; set; }

        /// <summary>
        /// Specifies the communication type.
        /// </summary>
        public CommunicationTypeInfo CommunicationType { get; set; }
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
