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
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps Phasor  data to Load Phasor Entity.
    /// </summary>
    public class Phasor
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private const string Lag = "Lag";
        private const string Lead = "Lead";
        private const string Import = "Import";
        private const string Export = "Export";
        private const string Present = "Present";
        private const string Absent = "Absent";
        private const string Incorrect = "Incorrect";
        private const string Correct = "Correct";
        private const string Invalid = "Invalid";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Phasor).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets Phasor  Entity from Phasor Data.
        /// </summary>
        /// <param name="dailyProfileData"></param>
        /// <returns></returns>
        public PhasorEntity GetMappedEntity(List<ProfileData> phasorData)
        {
            PhasorEntity phasorEntity = new PhasorEntity();
            DataElement dataElement = new DataElement();
            decimal voltageValue = 0.00M;
            try
            {
                foreach (MeterDataPacket meterDataPacket in phasorData[0].ListMeterDataPacket)
                {
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 9);
                    phasorEntity.CurrentDateTime = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 10);

                    phasorEntity.RPhaseCurrent = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 11);
                    phasorEntity.YPhaseCurrent = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 12);
                    phasorEntity.BPhaseCurrent = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 13);
                    phasorEntity.RPhaseVoltage = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 14);
                    phasorEntity.YPhaseVoltage = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 15);
                    phasorEntity.BPhaseVoltage = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 16);
                    phasorEntity.RPhasePowerFactor = dataElement.Value;
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 17);
                    phasorEntity.YPhasePowerFactor = dataElement.Value;
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 18);
                    phasorEntity.BPhasePowerFactor = dataElement.Value;
                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 19);
                    phasorEntity.TotalPhasePowerFactor = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 20);
                    phasorEntity.Frequency = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 21);
                    phasorEntity.ApparentPower = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 22);
                    phasorEntity.ActivePower = dataElement.Value;

                    phasorEntity.TotalkWDirection = dataElement.Value.Contains("-") ? "Export" : "Import";

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 24);
                    phasorEntity.ReActivePower = dataElement.Value;

                    if (dataElement.Value.Contains("-"))
                    {
                        phasorEntity.TotalInductivePower = "0.000";
                        phasorEntity.TotalCapacitivePower = dataElement.Value.Substring(1);
                        phasorEntity.Total = "Lead"; // total Laglead
                    }
                    else
                    {
                        phasorEntity.TotalCapacitivePower = "0.000";
                        phasorEntity.TotalInductivePower = dataElement.Value;
                        phasorEntity.Total = "Lag";  // total Laglead
                    }

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 60);
                    if (dataElement.DataDefinitionID <= 0) dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2174);
                    // phasorEntity.AngleYR = dataElement.Value;//Commented due to KSEB scalar stack issue
                    phasorEntity.AngleYR = dataElement.Value.Length > 3 ? dataElement.Value.Substring(0, 3) : dataElement.Value;
                    

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 61);
                    if (dataElement.DataDefinitionID <= 0) dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2175);
                    //phasorEntity.AngleBR = dataElement.Value;//Commented due to KSEB scalar stack issue
                    phasorEntity.AngleBR = dataElement.Value.Length > 3 ? dataElement.Value.Substring(0, 3) : dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 62);
                    if (dataElement.DataDefinitionID <= 0) dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2176);
                    phasorEntity.AngleBetweenTwo = dataElement.Value;

                    dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 63);
                    if (dataElement.DataDefinitionID <= 0) dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2177);
                    if (dataElement.DataDefinitionID <= 0) dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2185);
                    //phasorEntity.PhaseSequence = dataElement.Value == "132" ? Incorrect : Correct;

                    if (dataElement.Value == "132" || dataElement.Value == "1")
                    {
                        //Phase sequence chnage done on behalf of Gopal
                       // phasorEntity.PhaseSequence = Incorrect;
                        phasorEntity.PhaseSequence = Invalid;
                        //Phase sequence chnage done on behalf of Gopal
                    }
                    else if (dataElement.Value == "2")
                    {
                        phasorEntity.PhaseSequence = Invalid;
                    }
                    else
                    {
                        phasorEntity.PhaseSequence = Correct;
                    }

                    //fast download Phasor Need to be corrected
                    //Export Import
                    dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 381);
                    if (dataElement.Value != null)
                    {
                        phasorEntity.RPhaseNegativePowerFlag = dataElement.Value == "0" ? "Import" : "Export";

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 382);
                        phasorEntity.YPhaseNegativePowerFlag = dataElement.Value == "0" ? "Import" : "Export";

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 383);
                        phasorEntity.BPhaseNegativePowerFlag = dataElement.Value == "0" ? "Import" : "Export";
                        //fast download Phasor Need to be corrected
                    }
                    //SarkarA code change start 20180319 // add WB parameters for active power phasewise
                    else if (CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 1109).Value != null)
                    {
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 1109);
                        phasorEntity.RPhaseNegativePowerFlag = dataElement.Value.Contains("-") ? "Export" : "Import";

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 1110);
                        phasorEntity.YPhaseNegativePowerFlag = dataElement.Value.Contains("-") ? "Export" : "Import";

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 1111);
                        phasorEntity.BPhaseNegativePowerFlag = dataElement.Value.Contains("-") ? "Export" : "Import";
                    }
                    //SarkarA code change end 20180319
                    else
                    {
                        dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 54);
                        if (dataElement.Value.Contains("-"))
                        {
                            phasorEntity.RPhaseNegativePowerFlag = "Export";
                        }
                        else
                        {
                            phasorEntity.RPhaseNegativePowerFlag = "Import";
                        }

                        dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 55);
                        if (dataElement.Value.Contains("-"))
                        {
                            phasorEntity.YPhaseNegativePowerFlag = "Export";
                        }
                        else
                        {
                            phasorEntity.YPhaseNegativePowerFlag = "Import";
                        }

                        dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 56);
                        if (dataElement.Value.Contains("-"))
                        {
                            phasorEntity.BPhaseNegativePowerFlag = "Export";
                        }
                        else
                        {
                            phasorEntity.BPhaseNegativePowerFlag = "Import";
                        }
                    }

                    phasorEntity.RPhaseCapacitiveInductiveFlag = phasorEntity.RPhasePowerFactor.Contains("-") ? Lead : Lag;
                    phasorEntity.YPhaseCapacitiveInductiveFlag = phasorEntity.YPhasePowerFactor.Contains("-") ? Lead : Lag;
                    phasorEntity.BPhaseCapacitiveInductiveFlag = phasorEntity.BPhasePowerFactor.Contains("-") ? Lead : Lag;

                    //phasorEntity.Total = "-----";
                    phasorEntity.Total = phasorEntity.TotalPhasePowerFactor.Contains("-") ? Lead : Lag;


                    //Chaneel Missing R y  b  Phase
                    phasorEntity.RPhaseChannel = Convert.ToDecimal(phasorEntity.RPhaseVoltage) == voltageValue ? "Absent" : "Present";
                    phasorEntity.YPhaseChannel = Convert.ToDecimal(phasorEntity.YPhaseVoltage) == voltageValue ? "Absent" : "Present";
                    phasorEntity.BPhaseChannel = Convert.ToDecimal(phasorEntity.BPhaseVoltage) == voltageValue ? "Absent" : "Present";

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMappedEntity(List<ProfileData> phasorData)", ex);
            }

            return phasorEntity;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// Truncates the decimals after the precision from a decimal type
        /// </summary>
        /// <param name="targetValue"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        private string TruncateToPrecision(string inputValue, int scalar)
        {
            string value = inputValue;
            try
            {
                value = (Convert.ToDecimal(inputValue) * Convert.ToDecimal(Math.Pow(10, scalar))).ToString();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "TruncateToPrecision(string inputValue, int scalar)", ex);
            }
            return value;
        }
        #endregion
    }
}
