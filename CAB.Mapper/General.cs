#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CAB.Entity;
using CAB.Parser.Entity;
using System.Globalization;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// mapps general data to general entity
    /// </summary>
    public class General
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
        /// Gets general data into general entity 
        /// </summary>
        /// <param name="instantData"></param>
        /// <param name="phasorData"></param>
        /// <param name="fraudEnergyData"></param>
        /// <returns></returns>
        public GeneralEntity GetData(List<ProfileData> instantData, List<ProfileData> phasorData, List<ProfileData> fraudEnergyData)
        {
            string defaultValue = "----";
            GeneralEntity generalEntity = new GeneralEntity();
            List<DataElement> phasorRecords = null;
            List<DataElement> fraudEnergyRecords = null;
            DataElement dataElement = null;
            List<DataElement> instantRecords = instantData[0].ListMeterDataPacket[0].ListDataElementValue;
            if (phasorData != null && phasorData.Count > 0 && phasorData[0].ListMeterDataPacket.Count > 0)
            {
                 phasorRecords = phasorData[0].ListMeterDataPacket[0].ListDataElementValue;
            }
            if (fraudEnergyData != null && fraudEnergyData.Count > 0 && fraudEnergyData[0].ListMeterDataPacket.Count > 0)
            {
                fraudEnergyRecords = fraudEnergyData[0].ListMeterDataPacket[0].ListDataElementValue;
            }

            string dateTime = instantData[0].ListMeterDataPacket[0].ReadingDate.ToString("dd/MM/yyyy HH:mm:ss");
            generalEntity.MeterDateTime = Common.StringToLongDateTimeFormat(dateTime);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 31);
            generalEntity.TotalActiveEnergy = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 29);
            generalEntity.ProgrammingCounter = dataElement.Value;

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 28);
            generalEntity.MDResetCounter = dataElement.Value;

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 71);
            generalEntity.RisingDemandKW = Common.FormatRisingDemand(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 72);
            DateTime elapsedTime = Convert.ToDateTime(dataElement.Value, new CultureInfo("hi-in"));
            generalEntity.ElapsedTimeKW = elapsedTime.Minute.ToString("00") + ":" + elapsedTime.Second.ToString("00") + "*MM:SS";

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 73);
            generalEntity.RisingDemandKVA = Common.FormatRisingDemand(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 80);
            TimeSpan  timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(dataElement.Value));
            generalEntity.TotalPowerOnHours = ((int)(timeSpan.TotalHours)).ToString("00") + ":" + timeSpan.Minutes.ToString("00") + "*HH:MM";

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 74);
            elapsedTime =   Convert.ToDateTime(dataElement.Value,new CultureInfo("hi-in"));
            generalEntity.ElapsedTimeKVA = elapsedTime.Minute.ToString("00") + ":" + elapsedTime.Second.ToString("00") + "*MM:SS";

        
                
         
            dataElement = Common.GetDataElementByDataDefId(instantRecords, 77);
            generalEntity.ReadoutCounter = dataElement.Value;

            generalEntity.RestorationTime = "0";
            generalEntity.OccurrenceTime = "0";
            generalEntity.LatestTamperOccurrenceID = defaultValue;
            generalEntity.LatestTamperRestorationID = defaultValue;

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 170);
            generalEntity.ErrorCode = dataElement.Value;

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 168);
            generalEntity.CumulativeMD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 169);
            generalEntity.CumulativeMD2 = Common.FormatCurrentMonthMD(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 71);
            if (dataElement.Value == "0")
            {
                generalEntity.VoltagePhaseSequence = "RYB";    
            }
            else if (dataElement.Value == "1")
            {
                generalEntity.VoltagePhaseSequence = "RBY";  
            }
            else
            {
                generalEntity.VoltagePhaseSequence = defaultValue;  
            }

            generalEntity.BateryModePowerOnHour = defaultValue;

            return generalEntity;
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
