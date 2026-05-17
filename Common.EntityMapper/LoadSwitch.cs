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
    public class LoadSwitch
    {
        

        #region Public Methods
        /// <summary>
        ///  Gets Load Survey Data  to Load Survey  Entity. 
        /// </summary>
        /// <param name="loadSurveyData"></param>
        /// <returns></returns>
        public BillingGeneralNFDLMSEntity GetMappedEntity(List<ProfileData> loadSwitchData, string meterVariant)
        {
            BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
            LoadSwitchEntity  loadSwitchEntity;
            LoadSwitchParameterEntity loadSwitchColumns = new LoadSwitchParameterEntity();
            List<LoadSwitchEntity> loadSwitchEntityList = new List<LoadSwitchEntity>();
            ConfigurationParser meterConfigParser = new ConfigurationParser(true);
           // string loadswitchColumnNames = "RTC,";
            string loadswitchColumnNames = "Controleventconnectdisconnect,";
            //string defaultValue = "----";
            DataElement dataElement = null;
            List<DataElement> loadSwitchRecords ;


            if (loadSwitchData != null && loadSwitchData.Count > 0 && loadSwitchData[0].ListMeterDataPacket.Count > 0)
            {
                foreach (MeterDataPacket meterDataPacket in loadSwitchData[0].ListMeterDataPacket)
                {
                    loadSwitchRecords = meterDataPacket.ListDataElementValue;

                    loadSwitchEntity = new LoadSwitchEntity();

                   

                    //search for date time as date time would be present if the file is read from fast download
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 9);
                    //match the element value with 0 as 0 is the default value from GetDataElementByDataDefId
                    if (!string.IsNullOrEmpty(dataElement.Value) && dataElement.Value != "0")
                    {
                        loadSwitchEntity.RealTimeClock = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);
                    }
                    else
                    {
                        loadSwitchEntity.RealTimeClock = CommonMapper.StringToLongDateTimeFormat(meterDataPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                    }
                   // loadSwitchEntity.IsDLMS = 1;
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 76);
                    loadSwitchEntity.ControlEventConnectDisconnect = CommonMapper.FormatData(dataElement);
                   
                    //dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 9);
                    //loadSwitchEntity.RealTimeClock = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 2180);
                    loadSwitchEntity.ReasonSwitchOperation = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 31);
                    loadSwitchEntity.CumulativeEnergykwh = CommonMapper.FormatData(dataElement);
                    
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 304);
                    loadSwitchEntity.CumulativeEnergykwhTZ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 305);
                    loadSwitchEntity.CumulativeEnergykwhTZ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 306);
                    loadSwitchEntity.CumulativeEnergykwhTZ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 307);
                    loadSwitchEntity.CumulativeEnergykwhTZ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 308);
                    loadSwitchEntity.CumulativeEnergykwhTZ5 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 309);
                    loadSwitchEntity.CumulativeEnergykwhTZ6 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 310);
                    loadSwitchEntity.CumulativeEnergykwhTZ7 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 311);
                    loadSwitchEntity.CumulativeEnergykwhTZ8 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 34);
                    loadSwitchEntity.CumulativeEnergykvah = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 315);
                    loadSwitchEntity.CumulativeEnergykvahTZ1 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 316);
                    loadSwitchEntity.CumulativeEnergykvahTZ2 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 317);
                    loadSwitchEntity.CumulativeEnergykvahTZ3 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 318);
                    loadSwitchEntity.CumulativeEnergykvahTZ4 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 319);
                    loadSwitchEntity.CumulativeEnergykvahTZ5 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 320);
                    loadSwitchEntity.CumulativeEnergykvahTZ6 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 321);
                    loadSwitchEntity.CumulativeEnergykvahTZ7 = CommonMapper.FormatData(dataElement);

                    dataElement = CommonMapper.GetDataElementByDataDefId(loadSwitchRecords, 322);
                    loadSwitchEntity.CumulativeEnergykvahTZ8 = CommonMapper.FormatData(dataElement);
                    //only insert record with valid RTC as for fast download empty RTC are present.
                    if (!string.IsNullOrEmpty(loadSwitchEntity.RealTimeClock.ToString()) && !(loadSwitchEntity.RealTimeClock  == 0))
                    {
                        loadSwitchEntityList.Add(loadSwitchEntity);
                    }

                }
                masterEntity.LoadSwitch = loadSwitchEntityList;


                //Get load survey columns.
                Dictionary<string, string> OBISLoadSwitchColumns = CommonMapper.GetLoadswitchOBISCodeColumnNames();
                foreach (DataElement data in loadSwitchData[0].ListMeterDataPacket[0].ListDataElementValue)
                {
                    DLMSCOMMAND obisInfo = meterConfigParser.GetObisInfoFromRepository(data.DataDefinitionID);
                    foreach (KeyValuePair<string, string> pair in OBISLoadSwitchColumns)
                    {
                        if (obisInfo.OBISCODE == pair.Key)
                        {
                            if (loadswitchColumnNames.Split(',').ToList<string>().IndexOf(pair.Value) == -1)
                            {
                                loadswitchColumnNames += pair.Value + ",";
                                break;
                            }
                        }
                    }
                }


                loadSwitchColumns.ColumnsNames = loadswitchColumnNames.Remove(loadswitchColumnNames.LastIndexOf(","));
                masterEntity.LoadSwitchParameterColumns = loadSwitchColumns;

            }
            return masterEntity;
        }

        #endregion

       
    }
}
