using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CAB.Parser.Entity;


namespace CAB.E650MeterConfiguration
{

    public class MgntTamperIconReset3P : BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public byte InputData { get; set; }
        #endregion

        #region Constructor
        public MgntTamperIconReset3P(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from MD Reset value 
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {

                // buffer.Add(0x00);
                buffer.Add(0x09);
                buffer.Add(0x01);
                buffer.Add(0x00);

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
            commandIndex++;
            return new ProfileData();
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

            CAB.Parser.Boolean boolType = new CAB.Parser.Boolean();
            boolType.LengthInBytes = 1;
            meterDataPacket.ListDataElementValue.Add(GetDataElement
                 (dlmsCommand, boolType.GetValue(reader, new DataElementConfiguration()), "secs"));
            meterDataPackets.Add(meterDataPacket);
            return meterDataPackets;
        }
        #endregion
    }
}

