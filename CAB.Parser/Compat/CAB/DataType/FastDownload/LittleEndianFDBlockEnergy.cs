#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Contains FD block energy data type. FD block energy comes in 3 bytes and in little endian format
    /// </summary>
    public class LittleEndianFDBlockEnergy : DataType
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
        /// Converts the specified bytes to liitle endian fd block energy in 3 bytes specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            byte[] bytes = binaryReader.ReadBytes(LengthInBytes);
            return UInt32.Parse(BitConverter.ToString(bytes).Replace("-", ""), System.Globalization.NumberStyles.HexNumber).ToString();
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


