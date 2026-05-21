#region Namespaces
using System;

using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// This type is for custom date time with seconds
    /// </summary>
    public class CustomDateTimeSec : DataType
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
                int sec = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int min = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int hour = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int day = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int month = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int year = 2000 + Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                dateTime = new DateTime(year, month, day, hour, min, sec);
            }
            catch
            { 
              
            }
            return dateTime;
        }
        #endregion

    }
}

