using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Parser.Entity;

namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Fills Reset lockout data. 
    /// </summary>
    public class ResetLockOutDays :BaseConfig
    {
         #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public byte InputData { get; set; }
        #endregion

        #region Constructor
        public ResetLockOutDays(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from input property
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {
                
                int writeData = InputData * 4 * 24;
                buffer.Add(0x12);
                buffer.Add(Convert.ToByte((writeData & 0xFF00) >> 8));
                buffer.Add(Convert.ToByte(writeData & 0x00FF));
                            
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
            ExtendedBinaryReader reader = GetReader(bytes);
            reader.ReadByte();
            profileData.ListMeterDataPacket = GetMeterDataPackets(command, reader);
            return profileData;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets a list of meter data packets
        /// </summary>
        /// <param name="dlmsCommand"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<MeterDataPacket> GetMeterDataPackets(DLMSCOMMAND dlmsCommand, ExtendedBinaryReader reader)
        {

            byte[] binaryData = reader.ReadBytes((int)reader.BaseStream.Length);
            Array.Reverse(binaryData);
            string actulData = string.Format("{0:X2}", binaryData[0x00]) + string.Format("{0:X2}", binaryData[0x01]);
            meterDataPacket.ListDataElementValue.Add(GetDataElement
                 (dlmsCommand, actulData, "secs"));
            meterDataPackets.Add(meterDataPacket);
            return meterDataPackets;
        }
        #endregion
    }
}
