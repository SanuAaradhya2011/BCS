#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

using CAB.Parser.Entity;
using CAB.Serialization;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Parses the configuration data in the meter such as scalar capture objects, scalar buffer and data capture objects
    /// </summary>
    public class ConfigurationParser
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static Repository repository = null;
        private static object syncRoot = new object();
        private StructureUnitManager structureUnitInfo = null;
        private Serializer serializer = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public ConfigurationParser(bool isLittleEndian)
        {
            serializer = new Serializer();
            lock (syncRoot)
            {
                if (repository == null)
                {
                    repository = (Repository)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "DataDefinitionRepository.xml"), typeof(Repository));
                }
            }            
            structureUnitInfo = new StructureUnitManager(serializer);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public List<DataElementConfiguration> GetConfiguration(Profile profile)
        {
            List<DataElementConfiguration> elementConfig = new List<DataElementConfiguration>();
            // Getting Capture Object with Data Id
            List<DataElementConfiguration> listCaptureobjects = GetCaptureObjectList(GetReader(profile.DataCaptureObjects), false);
            // Getting Scaler Capture Object with Data Id
            List<DataElementConfiguration> listScalarObjects = GetCaptureObjectList(GetReader(profile.DataScalarObjects), true);
            // Updating Scaler Capture Object list with Scale and Unit
            listScalarObjects = UpdateScaleCaptureObject(GetReader(profile.DataScalarBuffer), listScalarObjects);
            // Updating Capture Object list with Scale and Unit
            elementConfig = FillScalarandUnitInConfigList(listCaptureobjects, listScalarObjects);

            return elementConfig;
        }
        /// <summary>
        /// Gets the configuration for data parsing
        /// </summary>
        /// <param name="profileID"></param>
        /// <returns></returns>
        public DataPacketConfiguration GetConfiguration(int profileID,int meterModel)
        {
            DataPacketConfiguration dataPacketConfiguration = null; 
            List<DataPacketConfiguration> dataPacketConfigurations = GetConfigurationFromFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory,
                             ((ProfileId)Convert.ToInt32(profileID)).ToString() + "DataElements.xml"));

            dataPacketConfiguration = dataPacketConfigurations.Find(delegate(DataPacketConfiguration dataPacketConfig)
            {
                return dataPacketConfig.MeterModel == meterModel.ToString();

            });

             return dataPacketConfiguration;
        }

        public DataPacketConfiguration GetConfiguration(int profileID, int meterModel, string XMLName)
        {
            DataPacketConfiguration dataPacketConfiguration = null;
            List<DataPacketConfiguration> dataPacketConfigurations = GetConfigurationFromFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory,
                             ((ProfileId)Convert.ToInt32(profileID)).ToString() + XMLName));

            dataPacketConfiguration = dataPacketConfigurations.Find(delegate(DataPacketConfiguration dataPacketConfig)
            {
                return dataPacketConfig.MeterModel == meterModel.ToString();

            });

            return dataPacketConfiguration;
        }
       




        /// <summary>
        /// Gets the configuration infromation stored in file
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        private List<DataPacketConfiguration> GetConfigurationFromFile(string xmlFilePath)
        {
            DataPacket dataPacket = (DataPacket)serializer.DeserializeToObject(xmlFilePath, typeof(DataPacket));
            return dataPacket.DataPacketConfiguration;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the binary reader of raw data 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ExtendedBinaryReader GetReader(string data)
        {
            MemoryStream memoryStream = new MemoryStream(SoapHexBinary.Parse(data).Value );
            ExtendedBinaryReader binaryReader = new ExtendedBinaryReader(memoryStream, true);
            binaryReader.BaseStream.Position = 0;
            return binaryReader;
        }

        /// <summary>
        /// Gets the list of meter data elements according to DLMS standard from extended binary reader 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="matchAttribute"></param>
        /// <returns></returns>
        private List<DataElementConfiguration> GetCaptureObjectList(ExtendedBinaryReader reader, bool isScaler)
        {
            UInt64 arrayCounter = 0;
            UInt64 arrayLength = 0;
            UInt64 structureLength = 0;
            UInt64 structureCounter = 0;
            string data = string.Empty;
            List<DataElementConfiguration> listConfiguration = new List<DataElementConfiguration>();
            string obisCode = string.Empty;
            byte classID = 0;
            byte attribute = 0;
            byte dataIndex = 0;
            try
            {
                // Length of Array                
                arrayLength = GetLength(reader);
                while (arrayCounter < arrayLength)
                {
                    structureCounter = 0;
                    structureLength = GetLength(reader);
                    while (structureCounter < structureLength)
                    {
                        switch (structureCounter)
                        {
                            case 0:
                                reader.ReadByte();
                                classID = Convert.ToByte(BitConverter.ToUInt16(reader.ReadBytes(2), 0));
                                break;
                            case 1:
                                reader.ReadBytes(2);
                                obisCode = GetOBISCode(reader, ".");
                                break;
                            case 2:
                                reader.ReadByte();
                                attribute = reader.ReadByte();
                                if (isScaler)
                                { 
                                    attribute = 2;
                                }
                                break;
                            case 3:
                                reader.ReadByte();
                                dataIndex = Convert.ToByte(BitConverter.ToUInt16(reader.ReadBytes(2), 0));
                                break;
                        }
                        structureCounter++;
                    }

                    DataElementConfiguration dataElementConfiguration = new DataElementConfiguration();
                    dataElementConfiguration.DataDefID = GetDataDefIdFromRepository(obisCode, classID, attribute, dataIndex); ;
                    listConfiguration.Add(dataElementConfiguration);

                    arrayCounter++;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return listConfiguration;
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
                signedByte = (signedByte | (int)ParalenByte[0]) << 8;
                signedByte = (signedByte | (int)ParalenByte[1]);
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
        /// Gets the list of meter data elements according to DLMS standard for binary reader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="scalarCaptureObjectList"></param>
        /// <returns></returns>
        private List<DataElementConfiguration> UpdateScaleCaptureObject(ExtendedBinaryReader reader, List<DataElementConfiguration> scalarCaptureObjectList)
        {
            int arrayCounter = 0;
            int arrayLength = 0;
            int structureLength = 0;
            int structureCounter = 0;
            string data = string.Empty;
            try
            {
                // Length of Array

                arrayLength = (int)GetLength(reader);
                //For multiple entry meters, structure behaves as array
                if (arrayLength == 1)
                {
                    arrayLength = (int)GetLength(reader);
                }
                while (arrayCounter < arrayLength)
                {
                    structureCounter = 0;
                    structureLength = (int)GetLength(reader);
                    while (structureCounter < structureLength)
                    {
                        switch (structureCounter)
                        {
                            case 0:
                                reader.ReadByte();
                                scalarCaptureObjectList[arrayCounter].Scalar = (sbyte)reader.ReadByte();
                                break;
                            case 1:
                                reader.ReadByte();
                                scalarCaptureObjectList[arrayCounter].Unit = structureUnitInfo.GetUnit(reader.ReadByte());
                                break;
                        }
                        structureCounter++;
                    }
                    arrayCounter++;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return scalarCaptureObjectList;
        }
     
        /// <summary>
        /// Get repository parameter
        /// </summary>
        /// <param name="obisCode"></param>
        /// <param name="classID"></param>
        /// <param name="attribute"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public int GetDataDefIdFromRepository(string obisCode, byte classID, byte attribute, byte dataIndex)
        {
            int dataDefID = -1;
            foreach (RepositoryParameter parameter in repository.Parameter)
            {
                if ((parameter.OBISCode == obisCode) && (parameter.ClassID == classID) 
                    && (parameter.Attribute == attribute) && (parameter.DataIndex == dataIndex))
                {
                    dataDefID = parameter.DataDefID;
                    break;
                }
            }
            return dataDefID;
        }

        /// <summary>
        /// Used to get obis info from data definition repository using dataDefId
        /// </summary>
        /// <param name="dataDefId"></param>
        /// <returns></returns>
        public DLMSCOMMAND GetObisInfoFromRepository(int dataDefId)
        {
            DLMSCOMMAND dlmsCommand = new DLMSCOMMAND();
            foreach (RepositoryParameter parameter in repository.Parameter)
            {
                if (parameter.DataDefID == dataDefId )
                {
                    dlmsCommand.ATTRIBUTE = parameter.Attribute.ToString();
                    dlmsCommand.OBISCODE = parameter.OBISCode;
                    dlmsCommand.CLASS = parameter.ClassID.ToString();
                    dlmsCommand.CLASSNAME = parameter.ParameterName;
                    break;
                }
            }
            return dlmsCommand;
        }


        /// <summary>
        /// Fills Scalar , precision and Unit
        /// </summary>
        /// <param name="objectList"></param>
        /// <param name="scalarandUnitList"></param>
        /// <returns></returns>
        private List<DataElementConfiguration> FillScalarandUnitInConfigList(List<DataElementConfiguration> objectList, List<DataElementConfiguration> scalarandUnitList)
        {
            foreach (DataElementConfiguration elementConfig in objectList)
            {
                foreach (DataElementConfiguration scalarElementConfig in scalarandUnitList)
                {
                    if (elementConfig.DataDefID == scalarElementConfig.DataDefID)
                    {
                        elementConfig.Scalar = scalarElementConfig.Scalar;
                        elementConfig.Precision = Math.Abs(scalarElementConfig.Scalar);
                        elementConfig.Unit = scalarElementConfig.Unit;
                    }
                  
                }
            }
            return objectList;
        }
        #endregion

    }
}
