#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    public class VisibleString : DataType
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
        /// Converts the specified bytes to Visible string type specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            byte[] reversedArray = binaryReader.ReadBytes((int)binaryReader.ReadByte());
            Array.Reverse(reversedArray);
            return System.Text.Encoding.ASCII.GetString(reversedArray);
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
