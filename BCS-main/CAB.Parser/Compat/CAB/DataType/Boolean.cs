#region Namespaces
using System;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion

namespace CAB.Parser
{
    public class Boolean : DataType
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
        /// Converts the specified bytes to boolean type specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        /// <returns></returns>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
           return BitConverter.ToBoolean(binaryReader.ReadBytes(LengthInBytes), 0).ToString();
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


