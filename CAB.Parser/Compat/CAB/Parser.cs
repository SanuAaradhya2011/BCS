#region Namespaces
using System;
using System.IO;

using CAB.Serialization;
using CAB.Parser.Entity;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Base class for parsing the data
    /// </summary>
    public class BaseParser
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        protected bool IsLittleEndian;
        protected Serializer serializer = null;
        protected StructureInfoManager infoManager = null;
        protected DataTypeFactory dataTypeFactory = null;
        private static object syncRoot = new object();
        protected ConfigurationParser meterConfigParser = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public BaseParser(bool isLittleEndian)
        {
            serializer = new Serializer();
            IsLittleEndian = isLittleEndian;
            infoManager = new StructureInfoManager(serializer);
            dataTypeFactory = new DataTypeFactory();
            meterConfigParser = new ConfigurationParser(IsLittleEndian);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Parses the data in allprofile data, fills the profiledata with parsed data and returns the current pointer in allprofiledata array
        /// </summary>
        /// <param name="allProfileData"></param>
        /// <param name="profileData"></param>
        /// <returns></returns>
        public virtual ProfileData ParseProfile(string[] allProfileData, DLMSCOMMAND dlmsCommand, ref int commandIndex)
        {
            ProfileData pf = null;
            try
            {
                Type type = Type.GetType(dlmsCommand.CLASSNAME);
                BaseParser baseParser = (BaseParser)Activator.CreateInstance(type, new object[] { IsLittleEndian });
                pf = baseParser.ParseProfile(allProfileData, dlmsCommand, ref commandIndex);
            }
            catch (Exception ex)
            {
                
                
            }

            return pf;
        }



        #endregion

        #region Protected Methods
        /// <summary>
        /// Gets the binary reader of raw data 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ExtendedBinaryReader GetReader(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            ExtendedBinaryReader binaryReader = new ExtendedBinaryReader(memoryStream, IsLittleEndian);
            binaryReader.BaseStream.Position = 0;
            return binaryReader;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="scale"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        protected DataElement GetUnitConvertedWithPrecision(DataElement element, int scale, int precision)
        {
            string unit = element.Unit;
            string value = string.Empty;
            string additionalUnit = "k";
            if (unit.ToUpper() == "W" || unit.ToUpper() == "VA" || unit.ToUpper() == "VAR" || unit.ToUpper() == "WH" || unit.ToUpper() == "VAH" || unit.ToUpper() == "VARH")
            {
                if (unit.ToUpper() == "VAR")
                {
                    unit = "VAr";
                }
                else if (unit.ToUpper() == "W")
                {
                    unit = unit.ToUpper();
                }
                else if (unit.ToUpper() == "VA")
                {
                    unit = unit.ToUpper();
                }


                if (LNG.Framework.Utility.MeterDataTypes.HTCT_MEGA == LNG.Framework.Utility.CommonMethods.MeterDataType || scale > 3)
                {
                    additionalUnit = "M";
                    scale = scale - 6;
                }
                else
                {
                    scale = scale - 3;
                }
                               

                element.Value = TruncateToPrecision(element.Value, scale, (uint)scale);


                element.Unit = additionalUnit + unit;
            }
            else
            {
                element.Value = TruncateToPrecision(element.Value, scale, (uint)scale);
            }

            return element;
        }
        /// Truncates the decimals after the precision from a decimal type
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public string TruncateToPrecision(string parsedValue, int scalar, uint precision)
        {
            decimal targetValue = Convert.ToDecimal(parsedValue) * Convert.ToDecimal(Math.Pow(10, scalar));
            string value = targetValue.ToString();
            try
            {
                if (scalar < 0 && precision < 20)
                {
                    decimal step = (decimal)Math.Pow(10, precision);
                    targetValue = (Int64)Math.Truncate(step * targetValue) / step;
                    value = string.Format("{0:F" + precision.ToString() + "}", targetValue);
                }

            }
            catch (Exception ex)
            {
            }
            return value;
        }
        #endregion

        public string TruncateToPrecisionCF(string parsedValue, int scalar, uint precision)
        {           
                    
            
            if (scalar > 127)
            {
                scalar = 255 - scalar + 1;
                scalar = -scalar;
            }     
            decimal targetValue = Convert.ToDecimal(parsedValue) * Convert.ToDecimal(Math.Pow(10, scalar));
            string value = targetValue.ToString();
            try
            {
                if (scalar < 0 && precision < 20)
                {
                    decimal step = (decimal)Math.Pow(10, precision);
                    targetValue = (Int64)Math.Truncate(step * targetValue) / step;
                    value = string.Format("{0:F" + precision.ToString() + "}", targetValue);
                }

            }
            catch (Exception ex)
            {
            }
            return value;
        }

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion
    }
}


