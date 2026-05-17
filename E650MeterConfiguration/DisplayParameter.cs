#region NameSpaces
using System;
using System.Collections.Generic;

using CAB.Parser;
using CAB.Parser.Entity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Programming support for display parameters
    /// </summary>
    public class DisplayParameter:BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public List<byte> InputData { get; set; }
        #endregion

        #region Constructor
        public DisplayParameter(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list input data
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {
                buffer.Add(Convert.ToByte("09"));
                buffer.Add(Convert.ToByte(InputData.Count));
                buffer.AddRange(InputData);

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

            try
            {
                //byte[] binaryData = reader.ReadBytes((int)reader.BaseStream.Length);
                //for (int counter = 0; counter < binaryData.Length - 1; counter++)
                //{

                //    meterDataPacket.ListDataElementValue.Add(GetDataElement
                //         (dlmsCommand, Convert.ToInt32(binaryData[counter]).ToString()
                //         , "secs"));
                //}
                //meterDataPackets.Add(meterDataPacket);
                byte[] binaryData = reader.ReadBytes((int)reader.BaseStream.Length);
                if (CAB.Framework.Utility.ConfigInfo.DisplayProgrammingVariant == CAB.Framework.DisplayProgrammingTypes.OneByte)
                {
                    for (int counter = 0; counter < binaryData.Length - 1; counter++)
                    {

                        meterDataPacket.ListDataElementValue.Add(GetDataElement
                             (dlmsCommand, Convert.ToInt32(binaryData[counter]).ToString()
                             , "secs"));
                    }
                }
                else
                {
                    Array.Reverse(binaryData);
                    int byteCount = binaryData[0];
                    int byteHi = 0, byteLo = 0, intDispParam = 0;
                    for (int counter = 1; counter <= byteCount; counter += 2)
                    {
                        byteHi = binaryData[counter];
                        byteLo = binaryData[counter + 1];
                        intDispParam = (byteHi << 8) | byteLo;
                        meterDataPacket.ListDataElementValue.Add(GetDataElement
                             (dlmsCommand, intDispParam.ToString()
                             , "secs"));
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

       

        private List<MeterDataPacket> GetMeterDataPackets(DLMSCOMMAND dlmsCommand, ExtendedBinaryReader reader, int displayProgrammingVariant)
        {
            try
            {
                byte[] binaryData = reader.ReadBytes((int)reader.BaseStream.Length);
                if (displayProgrammingVariant == (int)CAB.Framework.DisplayProgrammingTypes.OneByte)
                {
                    for (int counter = 0; counter < binaryData.Length - 1; counter++)
                    {

                        meterDataPacket.ListDataElementValue.Add(GetDataElement
                             (dlmsCommand, Convert.ToInt32(binaryData[counter]).ToString()
                             , "secs"));
                    }
                }
                else
                {
                    int  byteHi = 0, byteLo = 0, intDispParam = 0;
                    for (int counter = 0; counter < binaryData.Length - 1; counter+=2)
                    {
                        byteHi = binaryData[counter];
                        byteLo = binaryData[counter + 1];
                        intDispParam = (byteHi << 8) | byteLo;
                        meterDataPacket.ListDataElementValue.Add(GetDataElement
                             (dlmsCommand, intDispParam.ToString()
                             , "secs"));
                    }
                }
                
                meterDataPackets.Add(meterDataPacket);
            }
            catch
            {

            }
            return meterDataPackets;
        }
    }
}
