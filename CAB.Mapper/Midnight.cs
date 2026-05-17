#region Namespaces
using System.Collections.Generic;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.Mapper
{   
    /// <summary>
    /// This class is responsible for Mapping parsed input entity to MidNight Entity
    /// </summary>
    public class Midnight
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
        /// Used to prepare Midnight Entity  from parsed input entity
        /// </summary>
        /// <param name="loadSurveyData"></param>
        /// <returns></returns>
        public List<DTMDailyProfileData> GetData(List<ProfileData> loadSurveyData)
        {
            List<DTMDailyProfileData> resultEntity = new List<DTMDailyProfileData>();
            DTMDailyProfileData midNightData = new DTMDailyProfileData();
            DTMDailyProfileEntity midNightEntity = null;
            midNightData.DTMDailyProfile = new List<DTMDailyProfileEntity>();
            
            DataElement dataElement = new DataElement();
            //string value = string.Empty;
            //string defaultValue = "----";
            foreach (MeterDataPacket meterDataPacket in loadSurveyData[0].ListMeterDataPacket)
            {
                midNightEntity = new DTMDailyProfileEntity();
                midNightEntity.ReadingDateTime = Common.StringToLongDateTimeFormat(meterDataPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                midNightEntity.DailyProfileDate = midNightEntity.ReadingDateTime;

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 31);
                midNightEntity.CumulativekWh = Common.FormatMidNightData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 34);
                midNightEntity.CumulativekVAh = Common.FormatMidNightData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 35);
                midNightEntity.DailyMD1 = Common.FormatMidNightData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 36);
                midNightEntity.MD1TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                
                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 37);
                midNightEntity.DailyMD2 = Common.FormatMidNightData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 38);
                midNightEntity.MD2TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                

                midNightEntity.AvailableDays = "30";
                midNightEntity.Parameters = "CumulativekWh as 'Cumulative kWh',CumulativekVAh as 'Cumulative kVAh',DailyMD1 as 'Daily MD1',MD1TimeStamp as 'MD1 Time Stamp',DailyMD2 as 'Daily MD2',MD2TimeStamp as 'MD2 Time Stamp'";

                midNightData.DTMDailyProfile.Add(midNightEntity);    

            }
            resultEntity.Add(midNightData);
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
