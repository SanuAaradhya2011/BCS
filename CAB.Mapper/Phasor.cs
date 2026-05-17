#region NameSpaces
using System;
using System.Collections.Generic;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Used for mapping parsed phasor data to phasor entity
    /// </summary>
    public class Phasor
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
        /// Used to fill phasor power entity
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        //public List<PhasorEntity> GetData(List<ProfileData> phasorData)
        //{
        //    List<PhasorEntity> resultData = new List<PhasorEntity>();
        //    List<DataElement> phasorRecords = null;
        //    PhasorEntity phasorEntity = new PhasorEntity();
        //    DataElement dataElement = null;
        //    string value = string.Empty;            
        //    decimal voltageValue = 0.00M;
        //    try
        //    {
        //        phasorRecords = phasorData[0].ListMeterDataPacket[0].ListDataElementValue;

        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords,9);                
        //        phasorEntity.ReadingDateTime = Common.StringToLongDateTimeFormat(dataElement.Value);

        //        //Current R Y B
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 10);
        //        phasorEntity.RPhaseCurrent = Common.FormatPhasorData(dataElement);
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 11);
        //        phasorEntity.YPhaseCurrent = Common.FormatPhasorData(dataElement);
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 12);
        //        phasorEntity.BPhaseCurrent = Common.FormatPhasorData(dataElement);

        //        //Voltage R Y B
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 13);
        //        phasorEntity.RPhaseVoltage = Common.FormatPhasorData(dataElement);
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 14);
        //        phasorEntity.YPhaseVoltage = Common.FormatPhasorData(dataElement);
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 15);
        //        phasorEntity.BPhaseVoltage = Common.FormatPhasorData(dataElement);

        //        //Power factor's 
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 16);
        //        phasorEntity.RPhasePF = Convert.ToDecimal(dataElement.Value).ToString("0.00");
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 17);
        //        phasorEntity.YPhasePF = Convert.ToDecimal(dataElement.Value).ToString("0.00");
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 18);
        //        phasorEntity.BPhasePF = Convert.ToDecimal(dataElement.Value).ToString("0.00");
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 19);
        //        phasorEntity.TotalInstantaneousPF = Convert.ToDecimal(dataElement.Value).ToString("0.00");

        //        //Frequency
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 20);
        //        phasorEntity.Frequency = Common.FormatPhasorData(dataElement); 

        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 22);
        //        phasorEntity.TotalApparentPower = Common.FormatPhasorData(dataElement); 
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 23);
        //        phasorEntity.TotalActivePower = Common.FormatPhasorData(dataElement);
        //        //Total kw direction
        //        phasorEntity.TotalkWDirection = dataElement.Value.Contains("-") ? "Export" : "Import";

        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 24);
        //        if (dataElement.Value.Contains("-"))
        //        {
        //            phasorEntity.TotalInductivePower = "0.000";
        //            phasorEntity.TotalCapacitivePower = Common.FormatPhasorData(dataElement).Substring(1);
        //            phasorEntity.Total = "Lead"; // total Laglead
        //        }
        //        else
        //        {
        //            phasorEntity.TotalCapacitivePower = "0.000";
        //            phasorEntity.TotalInductivePower = Common.FormatPhasorData(dataElement);
        //            phasorEntity.Total = "Lag";  // total Laglead
        //        }
               

        //        //Import/Export R Y B Phase
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 54);
        //        phasorEntity.RPhasekWDirection = dataElement.Value == "0" ? "Import" : "Export";
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 55);
        //        phasorEntity.YPhasekWDirection = dataElement.Value == "0" ? "Import" : "Export";
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 56);
        //        phasorEntity.BPhasekWDirection = dataElement.Value == "0" ? "Import" : "Export";
  
        //        //Lag Lead Y Y B Phase
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 57);
        //        phasorEntity.RPhaseLagLead= dataElement.Value == "0" ? "Lead" : "Lag";
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 58);
        //        phasorEntity.YPhaseLagLead = dataElement.Value == "0" ? "Lead" : "Lag";
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 59);
        //        phasorEntity.BPhaseLagLead = dataElement.Value == "0" ? "Lead" : "Lag";
               

        //        //Chaneel Missing R y  b  Phase
        //        phasorEntity.RPhaseChannel = Convert.ToDecimal(phasorEntity.RPhaseVoltage) == voltageValue ? "Absent" : "Present";
        //        phasorEntity.YPhaseChannel = Convert.ToDecimal(phasorEntity.YPhaseVoltage) == voltageValue ? "Absent" : "Present";
        //        phasorEntity.BPhaseChannel = Convert.ToDecimal(phasorEntity.BPhaseVoltage) == voltageValue ? "Absent" : "Present";


        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 60);
        //        phasorEntity.YPhaseAngleWithRPhase = Common.FormatPhasorData(dataElement);
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 61);
        //        phasorEntity.BPhaseAngleWithRPhase = Common.FormatPhasorData(dataElement);
        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 62);
        //        phasorEntity.AngleBWAnyPhasePresent = Common.FormatPhasorData(dataElement);

        //        dataElement = Common.GetDataElementByDataDefId(phasorRecords, 63);
        //        phasorEntity.PhaseSequence = dataElement.Value == "1" ? "NOT CORRECT" : "CORRECT";

        //        resultData.Add(phasorEntity);

        //    }
        //    catch
        //    {

        //    }
        //    return resultData;
        //}


        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods


        #endregion
    }
}
