#region NameSpaces
using System;
using System.Collections.Generic;
using System.Collections;
using CAB.Parser;
using CAB.Parser.Entity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Programming support for display parameters
    /// </summary>
    public class LoadControl:BaseConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        public List<byte> InputData { get; set; }
        #endregion

        #region Constructor
        public LoadControl(bool isLittleEndian)
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
        public enum LOADCONTROLBIT
        {
            // BIT 7: Over Current BIT 12: Low PF     BIT 13: OverLoad
            Over_Current = 7,
            Low_PF = 12,
            OverLoad = 13,
        }
        public override ProfileData ParseData(byte[] bytes, DLMSCOMMAND command)
        {
               
            List<string> strBuffer = new List<string>();            
            try
            {
                int startDataindx = 0;
                string[] TamperpersistanceTime = new string[8];

                if (bytes[startDataindx++] == 0x02) //struct
                {
                    int stractcount = 0;

                    int lengthodstruct = bytes[startDataindx++];//length of stract
                    while (stractcount < 8)
                    {
                        if (bytes[startDataindx] == 0x11)//unsigned 1 byte
                        {
                            startDataindx++;
                            
                           strBuffer.Add(bytes[startDataindx++].ToString());
                           
                        }
                        else if (bytes[startDataindx] == 0x04)//unsigned 2 byte
                        {
                            startDataindx++;
                            int recBytelen = bytes[startDataindx];
                            startDataindx += 1;
                            byte[] lCByteData = new byte[recBytelen / 8];
                            Array.Copy(bytes, startDataindx, lCByteData, 0, lCByteData.Length);
                            List<byte> convertedByteList = ReverseBitsofByteList(lCByteData);
                            BitArray myarra = new BitArray(convertedByteList.ToArray());
                            startDataindx += lCByteData.Length;
                            strBuffer.Add(Convert.ToString(Convert.ToInt32(myarra[(int)LOADCONTROLBIT.Over_Current])));
                            strBuffer.Add(Convert.ToString(Convert.ToInt32(myarra[(int)LOADCONTROLBIT.Low_PF])));
                            strBuffer.Add(Convert.ToString(Convert.ToInt32(myarra[(int)LOADCONTROLBIT.OverLoad])));
                            stractcount += 2;
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
        public static List<byte> ReverseBitsofByteList(byte[] recByteList)
        {
            List<byte> convertedlist = new List<byte>();
            try
            {
                foreach (byte item in recByteList)
                {
                    char[] bitarr = Convert.ToString(item, 2).PadLeft(8, '0').ToCharArray();
                    Array.Reverse(bitarr);
                    convertedlist.Add((byte)Convert.ToInt32(new string(bitarr), 2));
                }
                return convertedlist;
            }
            catch (Exception)
            {
                return null;
            }
        }

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

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion     
      
    }
}
