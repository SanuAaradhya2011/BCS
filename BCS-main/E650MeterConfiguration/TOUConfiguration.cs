#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;

using CAB.Parser;
using CAB.Parser.Entity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// programming support for tou configuration
    /// </summary>
    public class TOUConfiguration : BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public byte[] InputData { get; set; }
        #endregion

        #region Constructor
        public TOUConfiguration(bool isLittleEndian)
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

             buffer = InputData.ToList<byte>();
               
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
            ProfileData profileData = new ProfileData();
            profileData.ListMeterDataPacket = new List<MeterDataPacket>();
            profileData.ListMeterDataPacket.Add(new MeterDataPacket());
            profileData.ListMeterDataPacket[0].ListDataElementValue = new List<DataElement>();
            DataElement element = new DataElement();
            element.Value = allProfileData[commandIndex].Substring(16, allProfileData[commandIndex].Length - 16);
            profileData.ListMeterDataPacket[0].ListDataElementValue.Add(element);
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
            string actulData = binaryData[0].ToString();
            meterDataPacket.ListDataElementValue.Add(GetDataElement
                 (dlmsCommand, actulData, "secs"));
            meterDataPackets.Add(meterDataPacket);
            return meterDataPackets;
        }
        #endregion
    }
    
}
