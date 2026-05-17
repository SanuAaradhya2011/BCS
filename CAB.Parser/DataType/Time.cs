#region Namespaces
using System;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    public class Time : DataType
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
        /// Converts the specified bytes to Float 32 type specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader,DataElementConfiguration elementConfig)
        {
            binaryReader.ReadBytes(LengthInBytes);
            //Intentionaly Implemeneted 0 to support Ruby FW wrong implementation
            //return BitConverter.ToSingle(binaryReader.ReadBytes(LengthInBytes), 0).ToString();
            //Intentionaly Implemeneted 0 to support Ruby FW wrong implementation
            return "0";
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
