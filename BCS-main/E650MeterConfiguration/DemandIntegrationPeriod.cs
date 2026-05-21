#region Namespaces
using System;
using System.Collections.Generic;
using Hunt.EPIC.Logging;
using CAB.Parser;
using CAB.Parser.Entity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
#endregion

namespace CAB.E650MeterConfiguration
{
   /// <summary>
   /// Used for programming support for DIP
   /// </summary>
    public class DemandIntegrationPeriod :BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DemandIntegrationPeriod).ToString());
        #endregion

        #region Properties
        public int InputData { get; set; }
        #endregion

        #region Constructor
        public DemandIntegrationPeriod(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from Demand Integration period  value
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
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDataBuffer()", ex);
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

            try
            {
                byte[] binaryData = reader.ReadBytes((int)reader.BaseStream.Length);

                string actulData = string.Format("{0:X2}", binaryData[0x01]) + string.Format("{0:X2}", binaryData[0x00]);
                meterDataPacket.ListDataElementValue.Add(GetDataElement
                     (dlmsCommand, actulData, "secs"));
                meterDataPackets.Add(meterDataPacket);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataPackets(DLMSCOMMAND dlmsCommand, ExtendedBinaryReader reader)", ex);
            }
            return meterDataPackets;
        }
        #endregion
    }
}
