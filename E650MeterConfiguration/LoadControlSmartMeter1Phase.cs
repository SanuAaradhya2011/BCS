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
    public class LoadControlSmartMeter1Phase:BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public List<byte> InputData { get; set; }
        #endregion

        #region Constructor
        public LoadControlSmartMeter1Phase(bool isLittleEndian)
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
                buffer.Add(Convert.ToByte("02"));
                buffer.Add(Convert.ToByte("06"));
                //buffer.Add(Convert.ToByte(InputData.Count));
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
            List<string> strBuffer = new List<string>();    
            try
            {

                int startDataindx = 0;
                string[] TamperpersistanceTime = new string[8];

                if (bytes[startDataindx++] == 0x02) //srtact
                {
                    int stractcount = 0;
                    int lengthodstruct = bytes[startDataindx++];//length of stract
                    while (stractcount < lengthodstruct)
                    {
                        if (bytes[startDataindx] == 18)//unsigned 2 byte
                        {
                            startDataindx++;
                            TamperpersistanceTime[0] = bytes[startDataindx++].ToString("X").PadLeft(2, '0');
                            TamperpersistanceTime[1] = bytes[startDataindx++].ToString("X").PadLeft(2, '0');
                            strBuffer.Add((Convert.ToInt32((TamperpersistanceTime[0] + TamperpersistanceTime[1]), 16) / 1M).ToString("0"));
                        }
                        else if (bytes[startDataindx] == 17)//unsigned long 1 byte
                        {
                            startDataindx++;
                            TamperpersistanceTime[0] = bytes[startDataindx++].ToString("X");
                            strBuffer.Add((Convert.ToInt32((TamperpersistanceTime[0]), 16) / 1M).ToString("0"));
                         }
                        stractcount++;
                    }

                }
                profileData.ListMeterDataPacket = GetMeterDataPackets(command, strBuffer);

            }
                      
            catch (Exception)
            {
               
            }
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
        private List<MeterDataPacket> GetMeterDataPackets(DLMSCOMMAND dlmsCommand, List<string> strBuffer)
        {

            foreach (var item in strBuffer)
            {
                DataElement dElement = GetDataElement(dlmsCommand, item, "secs");
                meterDataPacket.ListDataElementValue.Add(dElement);
            }
            meterDataPackets.Add(meterDataPacket);
            return meterDataPackets;
        }
        #endregion
    }
}
