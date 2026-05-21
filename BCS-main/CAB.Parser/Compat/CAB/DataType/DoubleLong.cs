#region Namespaces
using CAB.Parser;
using CAB.Parser.Entity;
using System;
#endregion
namespace CAB.Parser
{
    public class DoubleLong : DataType
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
        /// Converts the specified bytes to Double Long type specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader,DataElementConfiguration elementConfig)
        {
            return BitConverter.ToInt32(binaryReader.ReadBytes(LengthInBytes), 0).ToString(); 
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


