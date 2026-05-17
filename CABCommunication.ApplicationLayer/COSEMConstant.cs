#region Namespaces

#endregion


namespace CABCommunication.ApplicationLayer
{
    /// <summary>
    ///
    /// </summary>
    public class xDLMSAPDUConstant
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
       
        public const byte InitiateRequest = 1;
        public const byte ReadRequest = 5;
        public const byte WriteRequest = 6;
        public const byte InitiateResponse =7;
        public const byte ReadResponse = 12;
        public const byte WriteResponse =  13;
        public const byte ConfirmedServiceError =  14;
        public const byte UnconfirmedWriteRequest = 22;
        public const byte InformationReportRequest = 24;
        public const byte AARQ = 96;
        public const byte AARE = 97;
        public const byte RLRQ = 98;
        public const byte RLRE = 99;
        public const byte GETRequest = 192;
        public const byte SETRequest = 193;
        public const byte EVENTNOTIFICATIONRequest = 194;
        public const byte ACTIONRequest =195;
        public const byte GETResponse =196;
        public const byte SETResponse =197;
        public const byte ACTIONResponse =199;


        #endregion

        #region Properties
       

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

    /// <summary>
    /// 
    /// </summary>
    public enum ApplicationContext
    {
        /// <summary>
        /// 
        /// </summary>
        LN = 0x01,
        /// <summary>
        /// 
        /// </summary>
        SN = 0x02,
        ARWS = 0x03 //allen refrencing with ciphering
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SecurityMechanism
    {
        /// <summary>
        /// 
        /// </summary>
        NoSecurity = 0x00,
        /// <summary>
        /// 
        /// </summary>
        LowLevelSecurity = 0x01,

        /// <summary>
        /// 
        /// </summary>
        HighLevelSecurity = 0x02

    }

    
}
