#region NameSpaces
using System;
using System.Collections.Generic;

using CAB.E650MeterConfiguration.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// programming support for 
    /// </summary>
    public class DisplayTimeoutParameter :BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public DisplayTimeout InputData { get; set; }
        #endregion

        #region Constructor
        public DisplayTimeoutParameter(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from Display timeout data . 
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {
                buffer.Add(0x02);
                buffer.Add(0x04);
                buffer.Add(0x12);
                buffer.Add(Convert.ToByte((InputData.PushTimeout & 0xFF00) >> 8));
                buffer.Add(Convert.ToByte(InputData.PushTimeout & 0x00FF));

                buffer.Add(0x12);
                buffer.Add(Convert.ToByte((InputData.ScrollTime & 0xFF00) >> 8));
                buffer.Add(Convert.ToByte(InputData.ScrollTime & 0x00FF));

                buffer.Add(0x0F);
                buffer.Add(Convert.ToByte(InputData.AutoScrollModeSelected));

                buffer.Add(0x12);
                buffer.Add(Convert.ToByte((InputData.AutoScrollTime & 0xFF00) >> 8));
                buffer.Add(Convert.ToByte(InputData.AutoScrollTime & 0x00FF));
                

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
            reader.ReadBytes(2);
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
                byte[] receivedData = (reader.ReadBytes((int)reader.BaseStream.Length));
                if (receivedData.Length > 0)
                {
                    Array.Reverse(receivedData);
                    List<int> resultData = new List<int>();
                    resultData.Add((receivedData[0x01] << 8) | (receivedData[0x02]));
                    resultData.Add((receivedData[0x04] << 8) | (receivedData[0x05]));
                    if (receivedData[0x07].ToString() != "0")
                    {
                        resultData.Add(((receivedData[0x09]) << 8) | (receivedData[0x0A]));
                    }
                    foreach (int item in resultData)
                    {
                        meterDataPacket.ListDataElementValue.Add(GetDataElement
                             (dlmsCommand, item.ToString(), "secs"));
                    }
                }
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
