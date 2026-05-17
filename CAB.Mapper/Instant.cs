#region Namespaces
using System.Collections.Generic;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// This class is responsible for Mapping parsed entity to InstantPowerEntity
    /// </summary>
    public class Instant
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
        /// Used to fill inatant power entity
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public InstantPowerEntity GetData(List<ProfileData> instantData, List<ProfileData> phasorData,List<ProfileData> fraudEnergyData)
        {            
            List<DataElement> instantRecords = null;
            List<DataElement> phasorRecords = null;
            List<DataElement> fraudEnergyRecords = null;
            InstantPowerEntity instantEntity = new InstantPowerEntity();
            DataElement dataElement = null;
            string value = string.Empty;
            string totalPF = "";
            string RPhasePF = "";
            string YPhasePF = "";
            string BPhasePF = "";
            string defaultValue = "----";
            try
            {
                instantRecords = instantData[0].ListMeterDataPacket[0].ListDataElementValue;
                if (phasorData != null && phasorData.Count > 0 && phasorData[0].ListMeterDataPacket.Count > 0 )
                {
                  phasorRecords = phasorData[0].ListMeterDataPacket[0].ListDataElementValue; 
                }
                if (fraudEnergyData != null && fraudEnergyData.Count > 0 && fraudEnergyData[0].ListMeterDataPacket.Count > 0)
                {
                    fraudEnergyRecords = fraudEnergyData[0].ListMeterDataPacket[0].ListDataElementValue;
                }

                string dateTime = instantData[0].ListMeterDataPacket[0].ReadingDate.ToString("dd/MM/yyyy HH:mm:ss");

                instantEntity.MeterDateTime = Common.StringToLongDateTimeFormat(dateTime);

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 10);
                instantEntity.CurrentRPhase = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 11);
                instantEntity.CurrentYPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(instantRecords, 12);
                instantEntity.CurrentBPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(instantRecords, 13);
                instantEntity.VoltageRPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(instantRecords, 14);
                instantEntity.VoltageYPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(instantRecords, 15);
                instantEntity.VoltageBPhase = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 21);
                totalPF = dataElement.Value.Contains("-") ? "Ex " : "Im ";    //update pf variable, will beused later             
                value = Common.FormatData(dataElement);
                instantEntity.InstantActivepower = value;

                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 159);
                instantEntity.InstantActivepowerRPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 160);
                instantEntity.InstantActivepowerYPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 161);
                instantEntity.InstantActivepowerBPhase = Common.FormatData(dataElement); 

                //Import/Export R Y B Phase
                dataElement = Common.GetDataElementByDataDefId(phasorRecords, 54);
                RPhasePF = dataElement.Value == "0" ? "Im " : "Ex ";
                dataElement = Common.GetDataElementByDataDefId(phasorRecords, 55);
                YPhasePF = dataElement.Value == "0" ? "Im " : "Ex ";
                dataElement = Common.GetDataElementByDataDefId(phasorRecords, 56);
                BPhasePF = dataElement.Value == "0" ? "Im " : "Ex ";

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 16);
                RPhasePF += dataElement.Value.Contains("-") ? "Ld " + Common.FormatData(dataElement) : "Lg " + Common.FormatData(dataElement);
                instantEntity.PowerFactorRPhase = RPhasePF;
                dataElement = Common.GetDataElementByDataDefId(instantRecords, 17);
                YPhasePF += dataElement.Value.Contains("-") ? "Ld " + Common.FormatData(dataElement) : "Lg " + Common.FormatData(dataElement);
                instantEntity.PowerFactorYPhase = YPhasePF;
                dataElement = Common.GetDataElementByDataDefId(instantRecords, 18);
                BPhasePF += dataElement.Value.Contains("-") ? "Ld " + Common.FormatData(dataElement) : "Lg " + Common.FormatData(dataElement);
                instantEntity.PowerFactorBPhase = BPhasePF;
                dataElement = Common.GetDataElementByDataDefId(instantRecords, 19);
                totalPF += dataElement.Value.Contains("-") ? "Ld " + Common.FormatData(dataElement) : "Lg " + Common.FormatData(dataElement);
                instantEntity.TotalPowerFactor = totalPF;

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 20);
                instantEntity.Frequency = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 22);
                value = Common.FormatData(dataElement);
                instantEntity.InstantApparentPower = value;

                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 165);
                instantEntity.InstantApparentpowerRPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 166);
                instantEntity.InstantApparentpowerYPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 167);
                instantEntity.InstantApparentpowerBPhase = Common.FormatData(dataElement); 

                //dataElement = Common.GetDataElementByDataDefId(instantRecords, 31);//75
                //instantEntity.TotalFundamentalActiveEnergy = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 24);
                value = Common.FormatData(dataElement);
                if (dataElement.Value.Contains("-"))
                {
                    instantEntity.InstantReactiveLeadPower = value;
                    instantEntity.InstantReactiveLagPower = "0.0000*KVAr";
                }
                else
                {
                    instantEntity.InstantReactiveLeadPower = "0.0000*KVAr";
                    instantEntity.InstantReactiveLagPower = value;
                }

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 24);
                value = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 162);
                instantEntity.InstantReactivepowerRPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 163);
                instantEntity.InstantReactivepowerYPhase = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 164);
                instantEntity.InstantReactivepowerBPhase = Common.FormatData(dataElement); 

                dataElement = Common.GetDataElementByDataDefId(instantRecords, 75);
                instantEntity.TotalFundamentalActiveEnergy = Common.FormatData(dataElement);

               

            }
            catch
            {

            }
            return instantEntity;
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
