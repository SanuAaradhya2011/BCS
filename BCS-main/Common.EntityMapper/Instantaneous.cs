#region NameSpaces
using System.Collections.Generic;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;
using System;

#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps instant data to instant entity .
    /// </summary>
    public class Instantaneous
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private  ConfigurationParser meterConfigParser;
        List<DataElement> instantRecords;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Instantaneous).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets instant data into instant entity 
        /// </summary>
        /// <param name="instantData"></param>
        /// <param name="phasorData"></param>
        /// <param name="fraudEnergyData"></param>
        /// <returns></returns>
        public List<DLMS650InstantaneousEntity> GetMappedEntity(List<ProfileData> instantData, DLMS650NamePlateDetailsEntity generalEntity)
        {
            List<DLMS650InstantaneousEntity> instantEntityList = new List<DLMS650InstantaneousEntity>();
            DLMS650InstantaneousEntity instantEntity, instantEntityBSEB ;//= new DLMS650InstantaneousEntity();           
            string defaultValue = "----";
            DataElement dataElement = null;
            instantRecords = new List<DataElement>();
            meterConfigParser = new ConfigurationParser(true);
            DLMSCOMMAND obisInfo;
            if (instantData != null && instantData.Count > 0 && instantData[0].ListMeterDataPacket.Count > 0)
            { 
                instantRecords = instantData[0].ListMeterDataPacket[0].ListDataElementValue;

                
                obisInfo = meterConfigParser.GetObisInfoFromRepository(9);
                instantEntity = new DLMS650InstantaneousEntity();


                //search for date time as date time would be present if the file is read from fast download
                dataElement = CommonMapper.GetDataElementByDataDefId(instantData[0].ListMeterDataPacket[0].ListDataElementValue,9);
                //match the element value with 0 as 0 is the default value from GetDataElementByDataDefId
                if (!string.IsNullOrEmpty(dataElement.Value) && dataElement.Value != "0")
                {
                    instantEntity.InstantPowerColumnValue = CommonMapper.StringToLongDateTimeFormat(dataElement.Value).ToString();
                }
                else
                {
                    instantEntity.InstantPowerColumnValue = DateUtility.DateTimeToLong(instantData[0].ListMeterDataPacket[0].ReadingDate).ToString();
                }

                instantEntity.InstantPowerObisCode = obisInfo.OBISCODE;               
                instantEntity.InstantPowerColumnName = obisInfo.CLASSNAME;
                instantEntity.InstantPowerClassID = obisInfo.CLASS;
                instantEntity.InstantPowerAttribute = obisInfo.ATTRIBUTE;
                instantEntity.InstantPowerDataIndex = 1;
                instantEntityList.Add(instantEntity);

                // change the sequenece of instant parameters based on same parameter type
                // TO Do: add 1149, 1127 data def id when value od date time come correct from meter
                int[] dataDefId = { 30, 10, 11, 12, 173, 174, 1112, 172, 13, 14, 15, 16, 17, 18, 19, 20, 192, 193, 194, 195, 21, 22, 23, 24, 2100, 2101, 2102, 2103, 2104, 2105, 2106, 2107, 25, 175, 26, 1073, 1074, 1075, 1076, 1077, 1078, 196, 31, 2051, 1079, 34, 2052, 1080, 32, 1136, 33, 1139, 1137, 1138, 1003, 242, 1115, 1116, 35, 36, 2015, 2016, 37, 38, 2033, 2034, 1148, 1149, 1126, 1127, 1046, 1047, 2186, 2187, 71, 72, 73, 74, 75, 1114, 84, 197, 2127, 2128, 183, 184, 220, 221, 222, 223, 224, 225, 302, 1195, 1196, 229, 54, 55, 56, 1109, 1110, 1111, 230, 231, 232, 233, 234, 235, 1113, 80, 28, 29, 27, 176, 177, 178, 179, 180, 181, 182, 185, 186, 202, 203, 204, 205, 206, 207, 237, 238, 208, 209, 210, 211, 212, 213, 214, 215, 228, 240, 236, 216, 217, 218, 239, 219, 226, 1117, 1118, 1198, 1119, 1197, 1120, 1121, 1122, 1123, 1128, 2099, 2100, 1184, 1185, 1186, 2111, 2112, 2113, 2114, 2115, 2116, 2126, 2132, 2133, 2134, 2135, 2136, 2137, 2138, 2139, 2141, 2142, 2143 , 2144, 2145, 2146, 2147, 2148, 2149, 2150, 2151, 2152, 2153, 2154, 2155, 2156, 2157, 2158, 2159, 1106, 2188,2191,2192,2205, 2206, 2207, 2208 }; // Story - 428915 - Instantaneous new parameter added
               
                for (int rowCount = 0; rowCount < dataDefId.Length ; rowCount++)
                {
                    //if (generalEntity != null && generalEntity.Metertype == MeterType.OnePhaseTwoWire && dataDefId[rowCount] == 26) // Story - 427028 - Hide only for single phase dlms  
                    //    continue;
                
                    instantEntity = GetInstnatEntity(dataDefId[rowCount], rowCount+2, generalEntity);
                    if (instantEntity != null)
                    {
                         instantEntityList.Add(instantEntity);
                    }
                }

                ////This is only for BSEB Tender. Will be obselete at the time of Supply      
                //if (generalEntity != null)
                //{
                //    if (generalEntity.MeterModelNo == "6" && generalEntity.ReverseKWh != null)
                //    {
                //        instantEntityBSEB = new DLMS650InstantaneousEntity();
                //        instantEntityBSEB.InstantPowerObisCode = "0.0.96.1.140.255";
                //        instantEntityBSEB.InstantPowerColumnName = "Reverse kWh";
                //        instantEntityBSEB.InstantPowerColumnValue = generalEntity.ReverseKWh;
                //        instantEntityBSEB.InstantPowerClassID = "3";
                //        instantEntityBSEB.InstantPowerAttribute = "2";
                //        instantEntityBSEB.InstantPowerDataIndex = 1;
                //        instantEntityList.Add(instantEntityBSEB);
                //    }
                //}
                //instantEntityList.Add(GetInstnatEntity(10,2));

                //instantEntityList.Add(GetInstnatEntity(11, 3));

                //instantEntityList.Add(GetInstnatEntity(12, 4 ));

                //instantEntityList.Add(GetInstnatEntity(13, 5 ));
                //instantEntityList.Add(GetInstnatEntity(14, 6 ));
                //instantEntityList.Add(GetInstnatEntity(15, 7 ));
                //instantEntityList.Add(GetInstnatEntity(16, 8 ));
                //instantEntityList.Add(GetInstnatEntity(17, 9 ));
                //instantEntityList.Add(GetInstnatEntity(18, 10));
                //instantEntityList.Add(GetInstnatEntity(19, 11));
                //instantEntityList.Add(GetInstnatEntity(20, 12));
                //instantEntityList.Add(GetInstnatEntity(21, 13));
                //instantEntityList.Add(GetInstnatEntity(22, 14));
                //instantEntityList.Add(GetInstnatEntity(24, 15));
                //instantEntityList.Add(GetInstnatEntity(25, 16));
                //instantEntityList.Add(GetInstnatEntity(26, 17));
                //instantEntityList.Add(GetInstnatEntity(27, 18));
                //instantEntityList.Add(GetInstnatEntity(28, 19));
                //instantEntityList.Add(GetInstnatEntity(29, 20));
                //instantEntityList.Add(GetInstnatEntity(30, 21));
                //instantEntityList.Add(GetInstnatEntity(31, 22));
                //instantEntityList.Add(GetInstnatEntity(32, 23));
                //instantEntityList.Add(GetInstnatEntity(33, 24));
                //instantEntityList.Add(GetInstnatEntity(34, 25));
                //instantEntityList.Add(GetInstnatEntity(35, 26));
                //instantEntityList.Add(GetInstnatEntity(36, 27));
                //instantEntityList.Add(GetInstnatEntity(37, 28));
                //instantEntityList.Add(GetInstnatEntity(38, 29));

                //#region WbTenderSpecificChanges
                //instantEntity = GetInstnatEntity(75, 30);
                //if (instantEntity != null)
                //{
                //    instantEntityList.Add(instantEntity);
                //}

                //instantEntity = GetInstnatEntity(84, 31);
                //if (instantEntity != null)
                //{
                //    instantEntityList.Add(instantEntity);
                //}
                //#endregion

                //#region Single Phase Meter Specific
                //instantEntity = GetInstnatEntity(172, 32);
                //if (instantEntity != null)
                //{
                //    instantEntityList.Add(instantEntity);
                //}
                //instantEntity = GetInstnatEntity(173, 33);
                //if (instantEntity != null)
                //{
                //    instantEntityList.Add(instantEntity);
                //}
                //instantEntity = GetInstnatEntity(174, 34);
                //if (instantEntity != null)
                //{
                //    instantEntityList.Add(instantEntity);
                //}
                //instantEntity = GetInstnatEntity(175, 35);
                //if (instantEntity != null)
                //{
                //    instantEntityList.Add(instantEntity);
                //}
               
                //#endregion


            }
            return instantEntityList;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        /// Get Instant Entity 
        /// </summary>
        /// <param name="dataDefId"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        private DLMS650InstantaneousEntity GetInstnatEntity(int dataDefId, int dataIndex, DLMS650NamePlateDetailsEntity generalEntity)
        {
            DLMS650InstantaneousEntity instantEntity = new DLMS650InstantaneousEntity();
            try
            {               
                DataElement dataElement = CommonMapper.GetDataElementByDataDefId(instantRecords, dataDefId);
                if (dataElement.DataDefinitionID == 0)
                {
                    instantEntity = null;
                }
                else
                {
                    DLMSCOMMAND obisInfo = meterConfigParser.GetObisInfoFromRepository(dataDefId);
                    instantEntity = new DLMS650InstantaneousEntity();
                    if (dataDefId == 30 || dataDefId == 36 || dataDefId == 38 || dataDefId == 72 || dataDefId == 74 || dataDefId == 1149 || dataDefId == 1127 || dataDefId == 2016 || dataDefId == 2034)
                    {
                        instantEntity.InstantPowerColumnValue = DateUtility.StringToLongDateTimeFormatDLMS(dataElement.Value);
                    }
                    else
                    {
                        instantEntity.InstantPowerColumnValue = CommonMapper.FormatData(dataElement);
                    }

                    if (dataDefId == 2133 || dataDefId == 2134 || dataDefId == 2135)//add pk_instant
                    {
                        instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue + "V";
                    }
                    if (dataDefId == 2136 || dataDefId == 2137 || dataDefId == 2138)
                    {
                        instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue + "A";
                    }
                    if (dataDefId == 2141)// for smart meter scaler not present of load limit
                    {
                        string data = string.Empty;
                        string strtemp = instantEntity.InstantPowerColumnValue;
                        long itotalmin = long.Parse(strtemp.Replace("*", ""));
                        decimal ihr = (decimal)(itotalmin / 100000M);
                        data = ihr.ToString("0.00000");//+ " kW"
                        instantEntity.InstantPowerColumnValue = data+"*" ;
                       instantEntity.InstantPowerColumnValue = instantEntity.InstantPowerColumnValue + " kW";
                    }
                    


                    instantEntity.InstantPowerObisCode = obisInfo.OBISCODE;
                    if (generalEntity != null && (generalEntity.MeterModelNo == "10" || generalEntity.MeterModelNo == NamePlateConstants.Sapphire_SM.ToString() || generalEntity.MeterModelNo == NamePlateConstants.Sapphire_sm.ToString()))
                    {
                        if (obisInfo.CLASSNAME.Contains("kW"))
                            obisInfo.CLASSNAME = obisInfo.CLASSNAME.Replace("kW", "MW");
                        else if (obisInfo.CLASSNAME.Contains("kVA"))
                            obisInfo.CLASSNAME = obisInfo.CLASSNAME.Replace("kVA", "MVA");
                        else if (obisInfo.CLASSNAME.Contains("KVA"))
                            obisInfo.CLASSNAME = obisInfo.CLASSNAME.Replace("KVA", "MVA");
                        else if (obisInfo.CLASSNAME.Contains("kvar"))
                            obisInfo.CLASSNAME = obisInfo.CLASSNAME.Replace("kvar", "Mvar");
                    }
                    instantEntity.InstantPowerColumnName = obisInfo.CLASSNAME;
                    instantEntity.InstantPowerClassID = obisInfo.CLASS;
                    instantEntity.InstantPowerAttribute = obisInfo.ATTRIBUTE;
                    instantEntity.InstantPowerDataIndex = dataIndex;
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstnatEntity(int dataDefId, int dataIndex, DLMS650NamePlateDetailsEntity generalEntity)", ex);
            }

            return instantEntity;
        }
        #endregion
    }
}
