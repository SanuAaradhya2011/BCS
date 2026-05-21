#region NameSpaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Hunt.EPIC.Logging;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.E650MeterConfiguration
{
    public enum PulseEnergyValues
    {
        [Description("Active Energy")]
        Active =0,
        [Description("Apparent Energy")]
        Apparent =1,
        [Description("Reactive Energy")]
        Reactive =2
    }
    /// <summary>
    /// Used for programming support for Meter Pulse Energy Type  .
    /// </summary>
    public class PulseEnergy : BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CTRatio).ToString());
        #endregion

        #region Properties
        public int InputData { get; set; }
        #endregion

        #region Constructor
        public PulseEnergy(bool isLittleEndian)
            : base(isLittleEndian)
        { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get byte list from Meter Pulse Energy Type  value
        /// </summary>
        /// <returns></returns>
        public override List<byte> GetDataBuffer()
        {
            List<byte> buffer = new List<byte>();
            try
            {
                buffer.Add(0x11);
                buffer.Add(Convert.ToByte(InputData));

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
            try
            {
                ExtendedBinaryReader reader = GetReader(bytes);
                if (reader.ReadByte() == 0x11)
                {
                    Unsigned type = new Unsigned();
                    meterDataPacket.ListDataElementValue.Add(GetDataElement(command, type.GetValue(reader, new DataElementConfiguration()), string.Empty));
                    meterDataPackets.Add(meterDataPacket);
                }
                profileData.ListMeterDataPacket = meterDataPackets;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ParseData(byte[] bytes, DLMSCOMMAND command)", ex);
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
