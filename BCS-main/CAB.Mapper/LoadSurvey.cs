#region Namespaces
using System.Collections.Generic;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using System;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// This class is responsible for Mapping parsed input entity to LoadSurveyEntity
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
        /// Used to prepare LoadSurveyEntity from parsed input entity
        /// </summary>
        /// <param name="headerInfoData"></param>
        /// <returns></returns>
        public List<LoadSurveyData> GetData(List<ProfileData> loadSurveyData,MDWithIPEntity demandWithIPEntity)
        {
            List<LoadSurveyData> resultEntity = new List<LoadSurveyData>();
            LoadSurveyData loadSurevyData = new LoadSurveyData();
            IECLoadSurveyEntity loadSurveyEntity = null;
            loadSurevyData.LoadSurvey = new List<IECLoadSurveyEntity>();
            
            DataElement dataElement = new DataElement();
            //string value = string.Empty;
            string defaultValue = "----";
            foreach (MeterDataPacket meterDataPacket in loadSurveyData[0].ListMeterDataPacket)
            {
                loadSurveyEntity = new IECLoadSurveyEntity();


                loadSurveyEntity.LoadSurveyDateTime = Common.StringToLongDateTimeFormat(meterDataPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                loadSurveyEntity.ReadingDateTime = loadSurveyEntity.LoadSurveyDateTime;

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 45);
                loadSurveyEntity.RPhaseVoltage = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 46);
                loadSurveyEntity.YPhaseVoltage = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 47);
                loadSurveyEntity.BPhaseVoltage = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 42);
                loadSurveyEntity.RPhaseCurrent = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 43);
                loadSurveyEntity.YPhaseCurrent = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 44);
                loadSurveyEntity.BPhaseCurrent = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 51);
                loadSurveyEntity.DemandKVA = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 48);
                loadSurveyEntity.DemandKW = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 49);
                loadSurveyEntity.DemandKVARLag = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 50);
                loadSurveyEntity.DemandKVARLead = Common.FormatLoadSurveyData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 53);
                if (Convert.ToInt32(dataElement.Value) == 0)
                {
                    loadSurveyEntity.TamperStatus = "000000";
                }
                else
                {
                    loadSurveyEntity.TamperStatus = Common.FormatLoadSurveyData(dataElement);
                }              
                if (demandWithIPEntity != null)
                {
                    loadSurveyEntity.MDIntervalPeriod = demandWithIPEntity.KWInterval;
                }
                else //default
                {
                    loadSurveyEntity.MDIntervalPeriod = 30;
                }
                loadSurveyEntity.PowerFactor = defaultValue;
                loadSurveyEntity.Parameters = ",RPhaseVoltage as 'Voltage R Phase',YPhaseVoltage as 'Voltage Y Phase',BPhaseVoltage as 'Voltage B Phase',RPhaseCurrent as 'Current R Phase',YPhaseCurrent as 'Current Y Phase',BPhaseCurrent as 'Current B Phase',DemandKVA as 'Demand kVA',DemandKW as 'Demand kW'";

                loadSurevyData.LoadSurvey.Add(loadSurveyEntity);

            }            
            resultEntity.Add(loadSurevyData);
            return resultEntity;
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
