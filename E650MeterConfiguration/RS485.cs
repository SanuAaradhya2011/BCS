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
    public class RS485:BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public List<byte> InputData { get; set; }
        #endregion

        #region Constructor
        public RS485(bool isLittleEndian)
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
               
                buffer.Add(Convert.ToByte("18"));
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
            try
            {
            List<string> strBuffer = new List<string>();
            int compValue = 0;
            compValue = (compValue | (int)bytes[01]) << 8;
            compValue = (compValue | (int)bytes[02]);
            strBuffer.Add(compValue.ToString());
            profileData.ListMeterDataPacket = GetMeterDataPackets(command, strBuffer);
            }
           catch (Exception)
            {

            }
            return profileData;
        }

        private List<MeterDataPacket> GetMeterDataPackets(DLMSCOMMAND dlmsCommand, List<string> strBuffer)
        {

            foreach (var item in strBuffer)
            {
                DataElement dElement = GetDataElement(dlmsCommand, item, " ");
                meterDataPacket.ListDataElementValue.Add(dElement);               
            }
            meterDataPackets.Add(meterDataPacket);
            return meterDataPackets;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion     
      
    }
}
