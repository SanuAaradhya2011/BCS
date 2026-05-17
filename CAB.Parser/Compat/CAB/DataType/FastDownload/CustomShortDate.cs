#region Namespaces
using CAB.Parser.Entity;
using System;
#endregion
namespace CAB.Parser
{

    public class CustomShortDate : DataType
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
        /// Converts the specified bytes to short date without hour, minutes and second
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            return GetShortDate(binaryReader).ToString("dd/MM/yyyy HH:mm:ss");
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the short date time from reader
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns>DateTime</returns>
        private DateTime GetShortDate(ExtendedBinaryReader binaryReader)
        {
            DateTime dateTime = DateTime.MinValue;
            try
            {
                int day = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int month = Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                int year = 2000 + Convert.ToInt32(BitConverter.ToString(binaryReader.ReadBytes(1)));
                dateTime = new DateTime(year, month, day);
            }
            catch
            {

            }
            return dateTime;
        }
        #endregion

    }
}


