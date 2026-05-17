#region Namespaces
using System;
using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Class for parsing custom date time without seconds
    /// </summary>
    public class CustomErrorCode : DataType
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Converts the specified bytes to Octet string type specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            string octetStringValue = string.Empty;
            LengthInBytes = 4;            
            byte[] binaryData = binaryReader.ReadBytes(LengthInBytes);
            octetStringValue = octetStringValue + binaryData[0].ToString("00");
            octetStringValue = octetStringValue + binaryData[1].ToString("00");
            octetStringValue = octetStringValue + binaryData[2].ToString("00");
            octetStringValue = octetStringValue + binaryData[3].ToString("00");
            return octetStringValue;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
       
        #endregion

    }
}


