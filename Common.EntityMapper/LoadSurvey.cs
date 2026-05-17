#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps Lod Survey  data to Load survey entity .
    /// </summary>
    public class LoadSurvey
    {
        #region Nested Types
        #endregion

        #region Constants and Variables        
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        ///  Gets Load Survey Data  to Load Survey  Entity. 
        /// </summary>
        /// <param name="loadSurveyData"></param>
        /// <returns></returns>
        public BillingGeneralNFDLMSEntity GetMappedEntity(List<ProfileData> loadSurveyData, string meterVariant)
        {
            BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
            DLMS650LoadSurveyEntity loadSurveyEntity;
            LoadSurveyParameterEntity loadSurevyColumns = new LoadSurveyParameterEntity();
            List<DLMS650LoadSurveyEntity> loadSurveyEntityList = new  List<DLMS650LoadSurveyEntity>();
            ConfigurationParser meterConfigParser = new ConfigurationParser(true);
            string lsColumnNames = "realTimeClockDateandTime,";
            string defaultValue = "----";
            DataElement dataElement = null;
            List<DataElement> loadSurevyRecords ;

            
            if (loadSurveyData != null && loadSurveyData.Count > 0 && loadSurveyData[0].ListMeterDataPacket.Count > 0)
            {
                foreach (MeterDataPacket meterDataPacket in loadSurveyData[0].ListMeterDataPacket)
                {
                    loadSurevyRecords = meterDataPacket.ListDataElementValue;

                    loadSurveyEntity = new DLMS650LoadSurveyEntity();
                    //search for date time as date time would be present if the file is read from fast download
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 30);
                    //match the element value with 0 as 0 is the default value from GetDataElementByDataDefId
                    if (!string.IsNullOrEmpty(dataElement.Value) && dataElement.Value != "0")
                    {
                        loadSurveyEntity.RealTimeClockDateandTime = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);
                    }
                    else
                    {
                        loadSurveyEntity.RealTimeClockDateandTime = CommonMapper.StringToLongDateTimeFormat(meterDataPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                    loadSurveyEntity.IsDLMS = 1;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 189);
                    loadSurveyEntity.AverageCurrent = CommonMapper.FormatData(dataElement);
                    
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 42);
                    loadSurveyEntity.RPhaseCurrent =CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 43);
                    loadSurveyEntity.YPhaseCurrent = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 44);
                    loadSurveyEntity.BPhaseCurrent = CommonMapper.FormatData(dataElement);
                    
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 188);
                    loadSurveyEntity.AverageVoltage = CommonMapper.FormatData(dataElement);
                                        
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 45);
                    loadSurveyEntity.RPhaseVoltage = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 46);
                    loadSurveyEntity.YPhaseVoltage = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 47);
                    loadSurveyEntity.BPhaseVoltage = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 48);
                    loadSurveyEntity.BlockEnergykWh = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 49);
                    loadSurveyEntity.BlockEnergykvarhlag = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 50);
                    loadSurveyEntity.BlockEnergykvarhlead = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 51);
                    loadSurveyEntity.BlockEnergykVAh = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 52);
                    loadSurveyEntity.Frequency = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 53);
                    loadSurveyEntity.TamperStatus = CommonMapper.FormatData(dataElement);

                    #region "BRPL LS Parameter"
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 54);
                    loadSurveyEntity.ActivePowerRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 55);
                    loadSurveyEntity.ActivePowerYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 56);
                    loadSurveyEntity.ActivePowerBPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 230);
                    loadSurveyEntity.ApparentPowerRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 231);
                    loadSurveyEntity.ApparentPowerYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 232);
                    loadSurveyEntity.ApparentPowerBPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 233);
                    loadSurveyEntity.ReactivePowerRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 234);
                    loadSurveyEntity.ReactivePowerYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 235);
                    loadSurveyEntity.ReactivePowerBPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2131);
                    loadSurveyEntity.PowerOffDurationLSIP = CommonMapper.FormatData(dataElement);
                    #endregion

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2130);
                    //SarkarA code change start 20180201 // Fix temperature value for fd mode
                    if (dataElement.Value.Length > 5)
                    {
                        dataElement.Value = "-25.8"; //-25.8 is default value in poweroff mode; handled in dlms650commonbll
                    }
                    //SarkarA code change end 20180201
                    loadSurveyEntity.Temperature = CommonMapper.FormatData(dataElement);


                    #region "Net Metering paramters"
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2055);
                    loadSurveyEntity.BlockEnergykWhExport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2053);
                    loadSurveyEntity.BlockEnergykWhImport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2057);
                    loadSurveyEntity.BlockEnergykvarhlagQ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2058);
                    loadSurveyEntity.BlockEnergykvarhleadQ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2056);
                    loadSurveyEntity.BlockEnergykVAhExport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2079);
                    loadSurveyEntity.BlockEnergykvarhlagQ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2080);
                    loadSurveyEntity.BlockEnergykvarhleadQ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2054);
                    loadSurveyEntity.BlockEnergykVAhImport = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2077);
                    loadSurveyEntity.BlockEnergykWhRPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2078);
                    loadSurveyEntity.BlockEnergykWhYPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2108);
                    loadSurveyEntity.BlockEnergykWhBPhase = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2075);
                    loadSurveyEntity.BlockEnergykvarhQ12 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2076);
                    loadSurveyEntity.BlockEnergykvarhQ34 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2097);
                    loadSurveyEntity.BlockEnergykvarhQ14 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2098);
                    loadSurveyEntity.BlockEnergykvarhQ23 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2099);
                    loadSurveyEntity.BlockEnergyFundamentalkWhAbsolute = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2161);//for smart meter
                    loadSurveyEntity.TemperFlag = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2162);
                    loadSurveyEntity.AVgVolt3phase = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2163);
                    loadSurveyEntity.AvgRphPF = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2164);
                    loadSurveyEntity.AvgYphPF = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2165);
                    loadSurveyEntity.AvgBphPF = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2166);
                    loadSurveyEntity.AvgTotalPF = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2167);
                    loadSurveyEntity.AvgNeuCurrent  = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2168);
                    loadSurveyEntity.THDVR = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2169);
                    loadSurveyEntity.THDVY = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2170);
                    loadSurveyEntity.THDVB = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2171);
                    loadSurveyEntity.THDIR = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2172);
                    loadSurveyEntity.THDIY = CommonMapper.FormatData(dataElement);
                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2173);
                    loadSurveyEntity.THDIB = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 174);
                    loadSurveyEntity.NeuCurrent = CommonMapper.FormatData(dataElement);


                    if (loadSurveyEntity.NeuCurrent == "0*")//add pradipta_load_neu
                    {
                        loadSurveyEntity.NeuCurrent = "0.000*A";
                    }
                    else
                    {
                        loadSurveyEntity.NeuCurrent = CommonMapper.FormatData(dataElement) + "*A";
                    }

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSurevyRecords, 2210);
                    loadSurveyEntity.AvgPhaseCurrent = CommonMapper.FormatData(dataElement);

                    #endregion

                    //only insert record with valid RTC as for fast download empty RTC are present.
                    if (!string.IsNullOrEmpty(loadSurveyEntity.RealTimeClockDateandTime.ToString()) && !(loadSurveyEntity.RealTimeClockDateandTime == 0))
                    {
                        loadSurveyEntityList.Add(loadSurveyEntity);
                    }

                }
                masterEntity.LoadSurvey = loadSurveyEntityList;


                //Get load survey columns.
                Dictionary<string, string> OBISLoadSurveyColumns  = CommonMapper.GetOBISCodeColumnNames();
                foreach (DataElement data in loadSurveyData[0].ListMeterDataPacket[0].ListDataElementValue)
                {
                    DLMSCOMMAND obisInfo = meterConfigParser.GetObisInfoFromRepository(data.DataDefinitionID);
                    foreach (KeyValuePair<string, string> pair in OBISLoadSurveyColumns)
                    {
                        if (obisInfo.OBISCODE == pair.Key)
                        {
                            if (lsColumnNames.Split(',').ToList<string>().IndexOf(pair.Value) == -1)
                            {
                                lsColumnNames += pair.Value + ",";
                                break;
                            }
                        }
                    }
                }
                //NetkWh add in case MeterVariant is 3 or 4 and not present
                if ((meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("NetkWh") == -1))
                {
                    lsColumnNames += "NetkWh,";
                }
                //NetkVAh add in case MeterVariant is 3 or 4 and not present
                if ((meterVariant == CAB.Framework.MeterVariant.THREE || meterVariant == CAB.Framework.MeterVariant.FOUR) && (lsColumnNames.Split(',').ToList<string>().IndexOf("NetkVAh") == -1))
                {
                    lsColumnNames += "NetkVAh,";
                }

                //Add Temperature in case not present for temperorary check
                ////SB Change Start 20170828
                //if (meterVariant == CAB.Framework.MeterVariant.ONE)
                //{
                //    if (lsColumnNames.Split(',').ToList<string>().IndexOf("Temperature") == -1)
                //    {
                //        lsColumnNames += "Temperature,";
                //    }
                //}
                ////SB Change End 20170828
                loadSurevyColumns.ColumnsNames = lsColumnNames.Remove(lsColumnNames.LastIndexOf(","));
                masterEntity.LSParameterColumns = loadSurevyColumns;

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
