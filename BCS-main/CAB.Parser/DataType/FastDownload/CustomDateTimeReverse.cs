#region Namespaces
using System;

using CAB.Parser.Entity;
using System.Globalization;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Class for parsing custom date time without seconds
    /// </summary>
    public class CustomDateTimeReverse : DataType
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
            return GetDLMSDateTime(binaryReader).ToString("dd/MM/yyyy HH:mm:ss");
            
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the DLMS date time from reader
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns>DateTime</returns>
        private DateTime GetDLMSDateTime(ExtendedBinaryReader binaryReader)
        {
            DateTime dateTime = DateTime.MinValue;
            try
            {
                int day = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int month = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int year = 2000 + Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int hour = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int min = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                dateTime = new DateTime(year, month, day, hour, min, 0);
            }
            catch
            {

            }
            return dateTime;
        }
        #endregion

    }
}
