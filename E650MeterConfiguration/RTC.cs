#region NameSpaces
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Used for programming support for RTC .
    /// </summary>
    public class RTC:BaseConfig
    {
     
        #region Nested Types
        #endregion

        #region Constants and Variables                
        #endregion

        #region Properties
        public DateTime InputData { get; set; }
        #endregion

        #region Constructor     
        public RTC(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from input RTC value
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {
                buffer.Add(0x09);
                buffer.Add(0x0C);
                buffer.Add(Convert.ToByte((InputData.Year & 0xFF00) >> 8));
                buffer.Add(Convert.ToByte(InputData.Year & 0x00FF));
                buffer.Add(Convert.ToByte(InputData.Month));
                buffer.Add(Convert.ToByte(InputData.Day));
                //*****This condition for smart meter send dayofweek in place of FF ********
                if (Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.Smartmeter_LTCT || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.Smartmeter_WCM ||Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SM110value || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SapphireS2 || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.Sapphire_Netmeter_WCM || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.Sapphire_Netmeter_LTCT || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.BYPL_FD || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SmartM_Cipher_LTCT || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SmartM_Cipher_WCM || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SmartM_Cipher_HTCT || Convert.ToInt16(ConfigInfo.MeterModel) == NamePlateConstants.SmartM_Cipher_1PH)
                  buffer.Add(Convert.ToByte((int)InputData.DayOfWeek));
                             
                else               
                    buffer.Add(0xFF);             
                
                buffer.Add(Convert.ToByte(InputData.Hour));
                buffer.Add(Convert.ToByte(InputData.Minute));
                buffer.Add(Convert.ToByte(InputData.Second));
                buffer.Add(0xFF);
                buffer.Add(0x80);
                buffer.Add(0x00);
                ////*****This condition for Sapphire S2 & meter model 45,46,41 send FF in place of 00 (generic condition) ********
                buffer.Add(GenericRTC.RTCWRITE(Convert.ToInt16(ConfigInfo.MeterModel)).clockstatus);
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
                  .Substring(16, allProfileData[commandIndex].Length - 16)).Value,dlmsCommand);
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
        private List<MeterDataPacket> GetMeterDataPackets(DLMSCOMMAND dlmsCommand,ExtendedBinaryReader reader)
        {
            OctetString octetString = new OctetString();
            meterDataPacket.ListDataElementValue.Add(GetDataElement
                 (dlmsCommand, octetString.GetValue(reader, new DataElementConfiguration()), string.Empty));
            meterDataPackets.Add(meterDataPacket);
            return meterDataPackets;
        }
        #endregion
    }
}
