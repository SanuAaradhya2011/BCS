#region Namespaces
using System;

using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Simulates the double long unsigned of DLMS in reversed order.
    /// </summary>
    public class LittleEndianDoubleLongUnsigned : DataType
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
        /// Converts the specified bytes to Double Long Unsigned type specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            byte[] bytes = binaryReader.ReadBytes(LengthInBytes);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0).ToString();
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


