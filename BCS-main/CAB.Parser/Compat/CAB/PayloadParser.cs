#region Namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Parses DLMS payload
    /// </summary>
    public class PayloadParser : BaseParser
    {
        #region Nested Types
        #endregion

        #region Constants and Variables       
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public PayloadParser(bool isLittleEndian) 
            : base(isLittleEndian)
        {
            
                   
        }
        #endregion

        #region Public Methods   
       

        /// <summary>
        /// Parse the Hexadecimal string containing  meter data as per the data configuration supplied in the DataPacketConfiguration parameter
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="dataPacket"></param>
        /// <returns></returns>
        public List<MeterDataPacket> ParseProfile(CAB.Parser.Entity.Profile profile, DataPacketConfiguration dataPacket)
        {
            return Parse(SoapHexBinary.Parse(profile.DataBuffer).Value, dataPacket);

        }
        /// <summary>
        /// Parses the data in allprofile data, fills the profiledata with parsed data and returns the current pointer in allprofiledata array
        /// </summary>
        /// <param name="allProfileData"></param>
        /// <param name="profileData"></param>
        /// <returns></returns>
        public override ProfileData ParseProfile(string[] allProfileData, DLMSCOMMAND dlmsCommand, ref int index)
        {
            CAB.Parser.Entity.Profile profile = new CAB.Parser.Entity.Profile();
            ProfileData profileData = new ProfileData();
            DataPacketConfiguration dataPacketConfig = new DataPacketConfiguration();
            string obisInfo = allProfileData[index].Substring(0, 16);
            BufferType bufferType;
            if (obisInfo.ToUpper().Contains("03000060018CFF02"))
            {
                bufferType = BufferType.DLMSNonProfileData;
            }
            else
            {
                bufferType = (BufferType)Convert.ToInt32(obisInfo.Substring(0, 2), 16);

            }
            if (bufferType == BufferType.DLMSData)
            {
                if (obisInfo.ToUpper().Contains("0700005E5B0AFF03"))
                {
                    profile = GetDummyNamePlateProfileData();
                    profile.DataCaptureObjects = allProfileData[index].Substring(16, allProfileData[index].Length - 16); index++;
                    profile.DataBuffer = allProfileData[index].Substring(16, allProfileData[index].Length - 16); index++;
                    dataPacketConfig.DataElements = meterConfigParser.GetConfiguration(profile);
                    dataPacketConfig.IsNumberOfRecordsIncluded = true;
                    dataPacketConfig.IsDateTimeSensitive = false;
                }
                else
                {
                    profile.DataCaptureObjects = allProfileData[index].Substring(16, allProfileData[index].Length - 16); index++;
                    profile.DataBuffer = allProfileData[index].Substring(16, allProfileData[index].Length - 16); index++;
                    profile.DataScalarObjects = allProfileData[index].Substring(16, allProfileData[index].Length - 16); index++;
                    profile.DataScalarBuffer = allProfileData[index].Substring(16, allProfileData[index].Length - 16); index++;
                    //Get Configuration                                    
                    dataPacketConfig.DataElements = meterConfigParser.GetConfiguration(profile);
                    dataPacketConfig.IsNumberOfRecordsIncluded = true;
                    if (obisInfo.Contains("07000060019BFF"))
                    {
                        dataPacketConfig.IsDateTimeSensitive = false;
                    }
                    else
                    {
                        if (obisInfo.Contains("070000636281FF03"))
                            dataPacketConfig.IsDateTimeSensitive = false;
                        else
                        dataPacketConfig.IsDateTimeSensitive = true;
                    }

                }
            }
            else if (bufferType == BufferType.DLMSNonProfileData)
            {
                dataPacketConfig = new DataPacketConfiguration();

                profile = GetDummyProfileData(Convert.ToInt32(dlmsCommand.CLASS).ToString("00"), dlmsCommand.OBISCODE.Replace(".", ""), Convert.ToInt32(dlmsCommand.ATTRIBUTE).ToString("00"));
                profile.DataBuffer = "01010201" + allProfileData[index].Substring(16, allProfileData[index].Length - 16); index++;
                //Get Configuration                     
                dataPacketConfig.DataElements = meterConfigParser.GetConfiguration(profile);
                dataPacketConfig.IsNumberOfRecordsIncluded = true;
                dataPacketConfig.IsDateTimeSensitive = false;
            }
            profileData.ListMeterDataPacket = ParseProfile(profile, dataPacketConfig);
            return profileData;

        }
        #endregion

        #region Protected Methods
        #endregion

        #region EventHandlers
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the value according to DLMS data type. Used for Array and structure type only.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private UInt64 GetLength(ExtendedBinaryReader reader)
        {
            reader.ReadByte();//For array or structure
            string length = String.Empty;
            int signedByte = reader.ReadByte();
            if (signedByte == 0x81) signedByte = reader.ReadByte();
            else if (signedByte == 0x82)
            {
                byte[] ParalenByte = reader.ReadBytes(2);
                signedByte = 0;
                signedByte = (signedByte | (int)ParalenByte[1]) << 8;
                signedByte = (signedByte | (int)ParalenByte[0]);
            }
            if (signedByte < 0)
            {
                length = GetArrayLength(reader, (sbyte)signedByte);
            }
            else
            {
                length = Convert.ToUInt64(signedByte).ToString();
            }
            return Convert.ToUInt64(length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetASCIIData(ExtendedBinaryReader reader, int length)
        {
            string convertedString = string.Empty;
            for (int index = 0; index < length ; index++)
            {
                convertedString = convertedString + Convert.ToChar(reader.ReadByte());
            }
            return convertedString;

        }

        /// <summary>
        /// Gets the array length according to DLMS standard
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="signedByte"></param>
        /// <param name="isLittleEndian"></param>
        /// <returns></returns>
        private string GetArrayLength(ExtendedBinaryReader reader, sbyte signedByte)
        {
            string value = string.Empty;
            //add 128 to the signed byte value to find out number of bytes which contains length
            int signed = Convert.ToInt32(signedByte) + 128;
            byte[] bytes = reader.ReadBytes(signed);
            Array.Reverse(bytes);
            value = BitConverter.ToString(bytes, 0).Replace("-", "");
            value = Int64.Parse(value, System.Globalization.NumberStyles.HexNumber).ToString();
            return value;
        }
        /// <summary>
        /// Gets the list of meter data elements according to DLMS standard from extended binary reader 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<MeterDataPacket> GetMeterDataPackets(DataPacketConfiguration dataPacketConfig, ExtendedBinaryReader reader)
        {
            UInt64 counter = 0;
            UInt64 arrayLength = 0;
            UInt64 structureLength = 0;
            string data = string.Empty;
            MeterDataPacket dataRow = null;
            bool isDateTimeSensitive = false;
            List<MeterDataPacket> meterDataPackets = new List<MeterDataPacket>();
            List<DataElementConfiguration> elementConfigs = dataPacketConfig.DataElements;        
            try
            {
                // Length of Array
                if (dataPacketConfig.IsNumberOfRecordsIncluded)
                {
                   arrayLength = GetLength(reader);
                }
                else
                {
                    arrayLength = (UInt64)((reader.BaseStream.Length) / (dataPacketConfig.PacketLength));
                }
                while (counter < arrayLength)
                {
                    dataRow = new MeterDataPacket();
                    //structure Length
                    structureLength = GetLength(reader);
                    dataRow.ListDataElementValue = new List<DataElement>();
                    //if (reader.PeekChar() == 9)
                    //{
                        //--If Data Type is Oct String for dateTime Type
                        if (dataPacketConfig.IsDateTimeSensitive && reader.PeekChar() == 0x9)
                        {
                            reader.ReadByte(); // DLMS Data Type octet string
                            dataRow.ReadingDate = GetDLMSDateTime(reader, reader.ReadByte());
                            isDateTimeSensitive = true;
                        }
                   // }
                    byte elementCount = 0;
                    foreach (DataElementConfiguration elementConfig in elementConfigs)
                    {
                        if (elementCount >= structureLength)
                        {
                            break;
                        }
                        //skip the first record if config is date time sensitive
                        if (isDateTimeSensitive)
                        {
                            isDateTimeSensitive = false;
                        }
                        else
                        {
                           dataRow.ListDataElementValue.Add(GetDataElement(elementConfig, reader));
                        }

                        if (dataRow.ReadingDate == new DateTime() && dataRow.ListDataElementValue[elementCount].DataDefinitionID == 9)
                        {
                            string strdate = dataRow.ListDataElementValue[elementCount].Value;

                            dataRow.ReadingDate = new DateTime(int.Parse(strdate.Substring(6, 4)), int.Parse(strdate.Substring(3, 2)), int.Parse(strdate.Substring(0, 2)), int.Parse(strdate.Substring(11, 2)), int.Parse(strdate.Substring(14, 2)), int.Parse(strdate.Substring(17, 2)));
                        }
                        elementCount++;
                    }
                    meterDataPackets.Add(dataRow);
                    counter++;
                }
            }
            catch(Exception ex)
            {
            }

            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return meterDataPackets;
        }
        
        /// <summary>
        /// Gets the DLMS date time from reader in string
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns>DateTime</returns>
        private string GetDLMSDateTimeAsString(ExtendedBinaryReader binaryReader, int strLength)
        {
            int year = BitConverter.ToUInt16(binaryReader.ReadBytes(2), 0);
            int month = binaryReader.ReadByte();
            int day = binaryReader.ReadByte();
            binaryReader.ReadByte();
            int hour = binaryReader.ReadByte();
            int min = binaryReader.ReadByte();
            int sec = binaryReader.ReadByte();
            if (sec == 0xFF)
            {
                sec = 0;
            }
            for (int counter = 8; counter < strLength; counter++)
            {
                binaryReader.ReadByte();
            }
            return string.Concat(year, month.ToString("00"), day.ToString("00"), hour.ToString("00"), min.ToString("00"), sec.ToString("00"));
        }        
        /// <summary>
        /// Fills the element with element configuration and data contained in reader
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elementConfig"></param>
        /// <param name="binaryReader"></param>
        private DataElement GetDataElement(DataElementConfiguration elementConfig, ExtendedBinaryReader reader)
        {
            DataElement element = new DataElement();
            try
            {
                int info = Convert.ToInt32(reader.ReadByte());
                DataType dataType = dataTypeFactory.GetDataType(infoManager.GetUnitInfo(info));
                element.Value = dataType.GetValue(reader, elementConfig);
                element.Unit = elementConfig.Unit;
                element.DataDefinitionID = elementConfig.DataDefID;
                /*Commented by VBM as in case of 3 decimal place resolution for demand scale is coming as 0 */
               // if (elementConfig.Scalar != 0 && !string.IsNullOrEmpty(elementConfig.Unit)) //scalar can be zero for RTC or counters
                if (elementConfig.Scalar == 0 && string.IsNullOrEmpty(elementConfig.Unit)) //scalar can be zero for RTC or counters
                {
                }
                else
                {
                    DataElement newElement = GetUnitConvertedWithPrecision(element, elementConfig.Scalar, elementConfig.Precision);
                    element.Value = newElement.Value;
                    element.Unit = newElement.Unit;
                }
                /*Commented by VBM as in case of 3 decimal place resolution for demand scale is coming as 0 */
            }
            catch 
            { 
            
            }
            return element;
        }
        

        

       
        /// <summary>
        /// Convert hexadecimal string to integer
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private int ConvertHexToDecimal(string val)
        {
            return int.Parse(val, System.Globalization.NumberStyles.HexNumber);
        }
        /// <summary>
        /// Gets the DLMS date time from reader
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns>DateTime</returns>
        private DateTime GetDLMSDateTime(ExtendedBinaryReader binaryReader, int strLength)
        {
            try
            {

            
            int year = BitConverter.ToUInt16(binaryReader.ReadBytes(2), 0);
            int month = binaryReader.ReadByte();
            int day = binaryReader.ReadByte();
            binaryReader.ReadByte();
            int hour = binaryReader.ReadByte();
            int min = binaryReader.ReadByte();
            int sec = binaryReader.ReadByte();
            hour = hour == 0xFF ? 0x00 : hour;
            min = min == 0xFF ? 0x00 : min;
            sec = sec == 0xFF ? 0x00 : sec;
            for (int counter = 8; counter < strLength; counter++)
            {
                binaryReader.ReadByte();
            }
            return new DateTime(year, month, day, hour, min, sec);
            }
            catch (Exception)
            {

                return new DateTime(2001,1,1,12,0,0);
            }
        }
        /// <summary>
        /// Get the obis code
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        private string GetOBISCode(ExtendedBinaryReader binaryReader, string delimiter)
        {
            string obisCode = string.Empty;
            for (int counter = 0; counter < 6; counter++)
            {
                if (counter == 5)
                {
                    obisCode = string.Concat(obisCode, binaryReader.ReadByte().ToString());
                }
                else
                {
                    obisCode = string.Concat(obisCode, binaryReader.ReadByte().ToString(), delimiter);
                }
                
            }
        
            return obisCode;
        }
        /// <summary>
        /// Parses the raw bytes as per the data configuration supplied in the DataPacketConfiguration parameter  
        /// </summary>
        /// <param name="data"> Byte array coming from the NPL layer without AMR and Generic Header</param>
        /// <param name="dataPacket">Data packet containing sorted set of data element information</param>
        /// 
        /// <returns>Parsed objet to metrology parser</returns>
        private List<MeterDataPacket> Parse(byte[] data, DataPacketConfiguration dataPacketConfig)
        {
            List<MeterDataPacket> meterDataPackets = GetMeterDataPackets(dataPacketConfig, GetReader(data));
            return meterDataPackets;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obisInfo"></param>
        /// <returns></returns>
        private CAB.Parser.Entity.Profile GetDummyProfileData(string classID, string obis, string attribute)
        {
            CAB.Parser.Entity.Profile profile = new CAB.Parser.Entity.Profile();
            profile.DataCaptureObjects = "010102041200" + classID + "0906" + obis + "0F" + attribute + "120000";
            profile.DataScalarObjects = "010102041200" + classID + "0906" + obis + "0F" + attribute + "120000";
            profile.DataScalarBuffer = "0101020102020F0016FF";
            return profile;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private CAB.Parser.Entity.Profile GetDummyNamePlateProfileData()
        {
            CAB.Parser.Entity.Profile profile = new CAB.Parser.Entity.Profile();
           // profile.DataCaptureObjects = "0109020412000109060000600100FF0F02120000020412000109060000600101FF0F02120000020412000109060100000200FF0F021200000204120001090600005E5B09FF0F021200000204120001090600005E5B0BFF0F021200000204120001090600005E5B0CFF0F02120000020412000109060100000402FF0F02120000020412000109060100000403FF0F02120000020412000109060000600104FF0F02120000";
            profile.DataScalarObjects = "0109020412000109060000600100FF0F02120000020412000109060000600101FF0F02120000020412000109060100000200FF0F021200000204120001090600005E5B09FF0F021200000204120001090600005E5B0BFF0F021200000204120001090600005E5B0CFF0F02120000020412000109060100000402FF0F02120000020412000109060100000403FF0F02120000020412000109060000600104FF0F02120000";
            profile.DataScalarBuffer = "0101020102020F0016FF";
            return profile;
        }

        
       
        #endregion

    }
}


