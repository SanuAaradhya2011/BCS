#region Namespaces
using System;
using CAB.Parser;
using CAB.Parser.Entity;
using System.Globalization;
using System.Text;
#endregion
namespace CAB.Parser
{
    public class OctetString : DataType
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
        /// Converts the specified bytes to Octet string type specified in protocol
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="elementConfig"></param>
        public override string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig)
        {
            string octetStringValue = string.Empty;
            LengthInBytes = (int)binaryReader.ReadByte();
            if (LengthInBytes == 12)
            {
                octetStringValue = GetDLMSDateTime(binaryReader, LengthInBytes).ToString("dd/MM/yyyy HH:mm:ss");
            }           
            else
            {
                byte[] binaryData = binaryReader.ReadBytes(LengthInBytes);
                Array.Reverse(binaryData);
                //octetStringValue = System.Text.Encoding.ASCII.GetString(binaryData);
                octetStringValue = System.Text.ASCIIEncoding.Default.GetString(binaryData);             

            }
            return octetStringValue;
        }
       
        #endregion

        #region Protecetd Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the DLMS date time from reader in string
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns>DateTime</returns>
        private string GetDLMSDateTimeAsString(ExtendedBinaryReader binaryReader, int strLength)
        {
            int year = BitConverter.ToUInt16(binaryReader.ReadBytes(2), 0);
            int month = binaryReader.ReadByte();
            int day = binaryReader.ReadByte();
            binaryReader.ReadByte();
            int hour = binaryReader.ReadByte();
            int min = binaryReader.ReadByte();
            int sec = binaryReader.ReadByte();
            if (sec == 0xFF)
            {
                sec = 0;
            }
            for (int counter = 8; counter < strLength; counter++)
            {
                binaryReader.ReadByte();
            }
            return string.Concat(year, month.ToString("00"), day.ToString("00"), hour.ToString("00"), min.ToString("00"), sec.ToString("00"));
        }
        /// <summary>
        /// Gets the DLMS date time from reader
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns>DateTime</returns>
        private DateTime GetDLMSDateTime(ExtendedBinaryReader binaryReader, int strLength)
        {
            DateTime dateTime = DateTime.MinValue;
            int year = BitConverter.ToUInt16(binaryReader.ReadBytes(2), 0);
            int month = binaryReader.ReadByte();
            int day = binaryReader.ReadByte();
            binaryReader.ReadByte();
            int hour = binaryReader.ReadByte();
            int min = binaryReader.ReadByte();
            int sec = binaryReader.ReadByte();
            try
            {

                hour = hour == 0xFF ? 0x00 : hour;
                min = min == 0xFF ? 0x00 : min;
                sec = sec == 0xFF ? 0x00 : sec;
                for (int counter = 8; counter < strLength; counter++)
                {
                    binaryReader.ReadByte();
                }
                // For Rising Demand Elapsed Time format [RisingDemand_ElapsedTimeFormat]
                if (year == 0xFFFF && month == 0xFF && day == 0xFF)
                {
                    dateTime = new DateTime(1970, 1, 1, hour, min, sec);
                }
                else
                {
                    dateTime = new DateTime(year, month, day, hour, min, sec);
                }
            }
            catch
            {
  
            }
            return dateTime;
        }
        #endregion

    }
}


