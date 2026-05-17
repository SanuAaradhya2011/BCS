#region Namespaces
using System;
using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Parses the ascii formatted string, length of the string will be coming from config rather than stream of bytes
    /// </summary>
    public class CustomVisibleString : DataType
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
        /// Converts the specified bytes to Visible string type specified in protocol, length is specified in corresponding config file
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            byte[] reversedArray = binaryReader.ReadBytes(elementConfig.LengthInBits/8);
            Array.Reverse(reversedArray);
            return System.Text.Encoding.ASCII.GetString(reversedArray);
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
