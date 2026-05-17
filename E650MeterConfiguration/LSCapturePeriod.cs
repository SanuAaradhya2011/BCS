#region Namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Programming suppoert for SIP
    /// </summary>
    public  class LSCapturePeriod :BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public int InputData { get; set; }
        #endregion

        #region Constructor
        public LSCapturePeriod(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from LS Capture preiod value
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {

                buffer.Add(0x12);
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
            ExtendedBinaryReader reader = GetReader(SoapHexBinary.Parse(allProfileData[commandIndex].
                Substring(16,allProfileData[commandIndex].Length - 16)).Value); commandIndex++;
            //02
            reader.ReadByte();
            profileData.ListMeterDataPacket = GetMeterDataPackets(dlmsCommand, reader);
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
            try
            {
                LongUnsigned longUnsigned = new LongUnsigned();
                longUnsigned.LengthInBytes = 2;
                meterDataPacket.ListDataElementValue.Add(GetDataElement
                     (dlmsCommand, longUnsigned.GetValue(reader, new DataElementConfiguration()), "secs"));
                meterDataPackets.Add(meterDataPacket);
            }
            catch
            {
            }
            return meterDataPackets;
        }
        #endregion

    }
}
