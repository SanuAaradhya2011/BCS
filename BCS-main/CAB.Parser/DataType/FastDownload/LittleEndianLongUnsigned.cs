#region Namespaces
using System;
using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    public class LittleEndianLongUnsigned : DataType
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Constructor
        #endregion

        #region Properties
        #endregion

        #region Public Methods
        /// <summary>
        /// Converts the specified bytes to Long Unsigned type specified in protocol with keeping in mind that data is in little endian format.
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            byte[] bytes = binaryReader.ReadBytes(LengthInBytes);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0).ToString();
        }
        #endregion

        #region Protecetd Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion

    }
}
