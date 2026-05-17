#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps Midnight  data to Load MidNight Entity.
    /// </summary>
    public class DailyLoadProfile
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DailyLoadProfile).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets Midnight  Entity from midnight data.
        /// </summary>
        /// <param name="dailyProfileData"></param>
        /// <returns></returns>
        public BillingGeneralNFDLMSEntity GetMappedEntity(List<ProfileData> dailyProfileData, string meterVariant)
        {
            List<DLMS650MidnightDataEntity> resultEntity = new List<DLMS650MidnightDataEntity>();
            BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
            DLMS650MidnightDataEntity midNightEntity = null;
            DTMDailyProfileParameterEntity MidnightColumns = new DTMDailyProfileParameterEntity();
            ConfigurationParser meterConfigParser = new ConfigurationParser(true);
            string midnightColumnNames = "realTimeClockDateandTime,";
            string defaultValue = "----";
            DataElement dataElement = new DataElement();
            bool isMidnightColumnsFetched = false;
            //string value = string.Empty;
            //string defaultValue = "----";
            try
            {
                foreach (MeterDataPacket meterDataPacket in dailyProfileData[0].ListMeterDataPacket)
                {
                    midNightEntity = new DLMS650MidnightDataEntity();
                    //search for date time as date time would be present if the file is read from fast download
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 30);
                    //match the element value with 0 as 0 is the default value from GetDataElementByDataDefId
                    if (!string.IsNullOrEmpty(dataElement.Value) && dataElement.Value != "0")
                    {
                        midNightEntity.RealTimeClockDateandTime = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);
                    }
                    else
                    {
                        midNightEntity.RealTimeClockDateandTime = CommonMapper.StringToLongDateTimeFormat(meterDataPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                    }

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 31);
                    midNightEntity.CumEnergykWh = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 32);
                    midNightEntity.CumEnergykvarhlag = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 33);
                    midNightEntity.CumEnergykvarhlead = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 34);
                    midNightEntity.CumEnergykVAh = CommonMapper.FormatData(dataElement);

                    //Maximum Demand Data
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 35);
                    midNightEntity.MDKW = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 36);
                    midNightEntity.MDKWDateTime = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 37);
                    midNightEntity.MDKVA = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 38);
                    midNightEntity.MDKVADateTime = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);

                    //added three parameterd for apspdcl for midnight energy
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 386);// OBIS Code changed for APSPDCL : Daily Survey Requirement
                    midNightEntity.PowerOnDuration = CommonMapper.FormatData(dataElement);

                    //added three parameterd for JVVNL for midnight energy
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 191);// OBIS Code changed for JVVNL : Daily
                    midNightEntity.PowerOnDurationGeneric = CommonMapper.FormatData(dataElement);

                    // Added for Cumulative power on duration in DP
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 196);// OBIS Code changed for JVVNL : Daily Survey Requirement
                    
                    midNightEntity.PowerOnDurationGeneric1P = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 83);
                    midNightEntity.PowerFailureDuration = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 385);
                    midNightEntity.PowerOnDurationThreePhases = CommonMapper.FormatData(dataElement);

                    #region "Net metering Parameters
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1079);
                    midNightEntity.CumEnergykWhExport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1138);
                    midNightEntity.CumEnergykvarhlagQ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1137);
                    midNightEntity.CumEnergykvarhleadQ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1080);
                    midNightEntity.CumEnergykVAhExport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2051);
                    midNightEntity.CumEnergykWhImport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1136);
                    midNightEntity.CumEnergykvarhlagQ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1139);
                    midNightEntity.CumEnergykvarhleadQ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2052);
                    midNightEntity.CumEnergykVAhImport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1184);
                    midNightEntity.CumEnergykWhRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1185);
                    midNightEntity.CumEnergykWhYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 1186);
                    midNightEntity.CumEnergykWhBPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2104);
                    midNightEntity.CumEnergykvarhQ12 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2105);
                    midNightEntity.CumEnergykvarhQ34 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2106);
                    midNightEntity.CumEnergykvarhQ14 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2107);
                    midNightEntity.CumEnergykvarhQ23 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 75);
                    midNightEntity.FundamentalAbsolutekWH = CommonMapper.FormatData(dataElement);

                    #endregion
                    #region "JDVVNL
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2117);
                    midNightEntity.MinVoltageLSIPAcrossDayRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2118);
                    midNightEntity.MinVoltageLSIPAcrossDayYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2119);
                    midNightEntity.MinVoltageLSIPAcrossDayBPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2120);
                    midNightEntity.HighestCurrentLSIPAcrossDayRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2121);
                    midNightEntity.HighestCurrentLSIPAcrossDayYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2122);
                    midNightEntity.HighestCurrentLSIPAcrossDayBPhase = CommonMapper.FormatData(dataElement);

                    #endregion

                    //only insert record with valid RTC as for fast download empty RTC are present.
                    if (!string.IsNullOrEmpty(midNightEntity.RealTimeClockDateandTime.ToString()) && !(midNightEntity.RealTimeClockDateandTime == 0))
                    {
                        resultEntity.Add(midNightEntity);
                    }
                    #region FetchMidnightParameterNames
                    //Get midnight columns.
                    if (!isMidnightColumnsFetched)
                    {
                        Dictionary<string, string> OBISMidnightColumns = CommonMapper.GetOBISCodeColumnNamesMidnight();
                        foreach (DataElement data in meterDataPacket.ListDataElementValue)
                        {
                            DLMSCOMMAND obisInfo = meterConfigParser.GetObisInfoFromRepository(data.DataDefinitionID);
                            foreach (KeyValuePair<string, string> pair in OBISMidnightColumns)
                            {
                                if (obisInfo.OBISCODE == pair.Key)
                                {
                                    // THIS IS TO ADD 2 COLUMNS OF POWER ON Duration for APSPDCL
                                    if (pair.Value.Contains("PowerOnDuration"))
                                    {
                                        midnightColumnNames += pair.Value + ",";
                                        isMidnightColumnsFetched = true;
                                        break;
                                    }
                                    else if (midnightColumnNames.Split(',').ToList<string>().IndexOf(pair.Value) == -1)
                                    {
                                        midnightColumnNames += pair.Value + ",";
                                        isMidnightColumnsFetched = true;
                                        break;
                                    }
                                }
                            }
                        }

                        //NetkWh add in case MeterVariant is 3 or 4 and not present
                        if ((meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR) && (midnightColumnNames.Split(',').ToList<string>().IndexOf("netkWh") == -1))
                        {
                            midnightColumnNames += "netkWh,";
                        }
                        //NetkVAh add in case MeterVariant is 3 or 4 and not present
                        if ((meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR) && (midnightColumnNames.Split(',').ToList<string>().IndexOf("netkVAh") == -1))
                        {
                            midnightColumnNames += "netkVAh,";
                        }



                        MidnightColumns.ColumnsNames = midnightColumnNames.Remove(midnightColumnNames.LastIndexOf(","));
                    }
                    #endregion
                }

                masterEntity.MidnightData = resultEntity;
                masterEntity.MidnightParameterColumns = MidnightColumns;

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMappedEntity(List<ProfileData> dailyProfileData, string meterVariant)", ex);
            }
            return masterEntity;
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
