#region Namespaces
using System;
using System.Collections.Generic;

using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Abstract class for base config type
    /// </summary>
    public abstract class BaseConfig :BaseParser
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        protected ProfileData profileData = null;
        protected List<MeterDataPacket> meterDataPackets = null;
        protected MeterDataPacket meterDataPacket = null;
        #endregion

        #region Constructor
        public BaseConfig(bool isLittleEndian)
            : base(isLittleEndian)
        {
            profileData = new ProfileData();
            meterDataPackets = new List<MeterDataPacket>();
            meterDataPacket = new MeterDataPacket();
            meterDataPacket.ListDataElementValue = new List<DataElement>();
        }
        
        #endregion

        #region Properties
        #endregion

        #region Public Methods
        /// <summary>
        /// Abstract function that will be implemented in inheriting  class
        /// </summary>
        /// 
        /// <returns></returns>
        public abstract List<byte> GetDataBuffer();       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public abstract ProfileData ParseData(byte[] bytes, DLMSCOMMAND command);
        

        #endregion

        #region Protecetd Methods
        /// <summary>
        /// Create data element with value, unit and Data Def ID
        /// </summary>
        /// <param name="dlmsCommand"></param>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        protected DataElement GetDataElement(DLMSCOMMAND dlmsCommand,string value,string unit)
        {
            DataElement dataElement = new DataElement();
            dataElement.Value = value;
            dataElement.DataDefinitionID =  meterConfigParser.GetDataDefIdFromRepository(ConvertOBIS(dlmsCommand.OBISCODE),
                Convert.ToByte(dlmsCommand.CLASS, 16), Convert.ToByte(dlmsCommand.ATTRIBUTE, 16), 0);
            dataElement.Unit = unit;
            return dataElement;

        }       
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Converts the OBIS code into integer format
        /// </summary>
        /// <param name="obisCode"></param>
        /// <returns></returns>
        private string ConvertOBIS(string obisCode)
        {
            string convertedOBIS = string.Empty;
            string[] strArray = obisCode.Split('.');
            {
                foreach (string item in strArray)
                {
                    convertedOBIS = string.Concat(convertedOBIS,Convert.ToInt32(item,16),".");
                }
            }
            return convertedOBIS.Substring(0,convertedOBIS.Length - 1);
        }
        #endregion        
        
    }
}
