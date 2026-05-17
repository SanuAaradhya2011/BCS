#region NameSpaces 
using System.Collections.Generic;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using System;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Used for mapping parsed Fraud energy data to FraudEnergyEntity entity
    /// </summary>
    public class FraudEnergy
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
        /// Used to fill farud energy entity
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        //public List<FraudEnergyEntity> GetData(List<ProfileData> fraudEnergyData)
        //{
        //    List<FraudEnergyEntity> resultData = new List<FraudEnergyEntity>();
        //    List<DataElement> fraudEnergyRecords = null;
        //    FraudEnergyEntity fraudEnergyEntity = new FraudEnergyEntity();
        //    DataElement dataElement = null;
        //    string value = string.Empty;
        //    //string defaultValue = "----";
        //   // decimal voltageValue = 0.00M;
        //    try
        //    {
        //        fraudEnergyRecords = fraudEnergyData[0].ListMeterDataPacket[0].ListDataElementValue;

        //        //dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 9);
        //        //fraudEnergyEntity.ReadingDateTime = Common.StringToLongDateTimeFormat(dataElement.Value);

        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 66);
        //        fraudEnergyEntity.MagneticInfluenceKWh = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 65);
        //        fraudEnergyEntity.MagneticInflueneceKVAh = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 67);
        //        fraudEnergyEntity.MagneticInflueneceKVARhLag = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 68);
        //        fraudEnergyEntity.MagneticInflueneceKVARhLead = dataElement.Value;
        //        //Reverse energy
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 69);
        //        fraudEnergyEntity.ReverseEnergyKVAh = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 70);
        //        fraudEnergyEntity.ReverseEnergyKWh = dataElement.Value;

        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 78);
        //        fraudEnergyEntity.ReverseEnergyKVARhLag = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 79);
        //        fraudEnergyEntity.ReverseEnergyKVARhLead = dataElement.Value;

        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 84);
        //        fraudEnergyEntity.THDVoltageRPhase = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 85);
        //        fraudEnergyEntity.THDVoltageYPhase = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 86);
        //        fraudEnergyEntity.THDVoltageBPhase = dataElement.Value;

        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 87);
        //        fraudEnergyEntity.THDCurrentRPhase = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 88);
        //        fraudEnergyEntity.THDCurrentYPhase = dataElement.Value;
        //        dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 89);
        //        fraudEnergyEntity.THDCurrentBPhase = dataElement.Value;
                
        //        resultData.Add(fraudEnergyEntity);

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
