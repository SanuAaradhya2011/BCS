
#region NameSpaces
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Parser;
using CAB.Parser.Entity;
using Hunt.EPIC.Logging;
#endregion

namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Programming support for Billing type [RisingDemand_ReverseKWH_BillingType]
    /// </summary>
    /// [BillingType_Month]
    public class BaudRate : BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BaudRate).ToString());
        #endregion

        #region Properties
        public byte InputData { get; set; }
        #endregion

        #region Constructor
        public BaudRate(bool isLittleEndian)
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
                buffer.Add(0x16);
                buffer.Add(InputData);
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
            //ExtendedBinaryReader reader = GetReader(bytes);
            //reader.ReadByte();
            //Unsigned type = new Unsigned();
            //meterDataPacket.ListDataElementValue.Add(GetDataElement(command, type.GetValue(reader, new DataElementConfiguration()), string.Empty));
            //meterDataPackets.Add(meterDataPacket);
            //profileData.ListMeterDataPacket = meterDataPackets;
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
                //string actulData;
                //if (binaryData[1] == 254) //BillingType end of month
                //{
                //    actulData = "000000";
                //}
                //else //BillingType User defined 
                //{
                //    actulData = Convert.ToInt32(binaryData[0x01]).ToString("00") + Convert.ToInt32(binaryData[0x0a]).ToString("00")
                //                    + Convert.ToInt32(binaryData[0x09]).ToString("00");
                //}
                //actulData = actulData + Convert.ToInt32(binaryData[0x07]).ToString("00");
                //meterDataPacket.ListDataElementValue.Add(GetDataElement
                //     (dlmsCommand, actulData, "secs"));
                //meterDataPackets.Add(meterDataPacket);
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

