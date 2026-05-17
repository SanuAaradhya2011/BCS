#region NameSpaces
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Used for programming support for PTRatio .
    /// </summary>
    public class PTRatio : BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public int InputData { get; set; }
        #endregion

        #region Constructor
        public PTRatio(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from input PTRatio value
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {
                buffer.Add(0x12);
                //buffer.Add(Convert.ToByte(InputData)); 
                buffer.Add(Convert.ToByte((InputData & 0xFF00) >> 8));
                buffer.Add(Convert.ToByte(InputData & 0x00FF));              

            }
            catch (Exception)
            {

            }
            return buffer;
        }

        /// <summary>
        /// Parses the data in allprofile data with dlms command and current pointer in allprofile array, returns the parsed data 
        /// </summary>
        /// <param name="allProfileData"></param>
        /// <param name="dlmsCommand"></param>
        /// <param name="commandIndex"></param>
        /// <returns></returns>
        public override ProfileData ParseProfile(string[] allProfileData, DLMSCOMMAND dlmsCommand, ref int commandIndex)
        {
            ProfileData profileData = ParseData(SoapHexBinary.Parse(allProfileData[commandIndex]
                  .Substring(16, allProfileData[commandIndex].Length - 16)).Value, dlmsCommand);
            commandIndex++;

            return profileData;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public override ProfileData ParseData(byte[] bytes, DLMSCOMMAND command)
        {
            try
            {
                ExtendedBinaryReader reader = GetReader(bytes);
                if (reader.ReadByte() == 0x11)
                {
                    Unsigned type = new Unsigned();
                    meterDataPacket.ListDataElementValue.Add(GetDataElement(command, type.GetValue(reader, new DataElementConfiguration()), string.Empty));
                    meterDataPackets.Add(meterDataPacket);
                }
                else
                {
                    Long type = new Long();
                    meterDataPacket.ListDataElementValue.Add(GetDataElement(command, type.GetValue(reader, new DataElementConfiguration()), string.Empty));
                    meterDataPackets.Add(meterDataPacket);
                }
                profileData.ListMeterDataPacket = meterDataPackets;
            }
            catch
            {
            }
            return profileData;
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
