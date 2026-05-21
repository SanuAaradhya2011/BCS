/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using System;
using System.Collections.Generic;

namespace CAB.BLL
{
    public class TamperGeneralBLL : IBLL
    {
        private Dictionary<string, string> tamperCounterColumns = new Dictionary<string, string>();
        TamperCounterGeneralDAL tamperCounterGeneralDAL;

        public TamperGeneralBLL()
        {
            tamperCounterGeneralDAL = new TamperCounterGeneralDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return tamperCounterGeneralDAL.InsertData(entity);
        }

        public IEntity InsertData(List<IEntity> entities)
        {
            return tamperCounterGeneralDAL.InsertData(entities);
        }
        public DataSet GetTamperCounter(int meterDataId,string tamperName)
        {
            DataSet ds = tamperCounterGeneralDAL.GetTamperCounter(meterDataId, tamperName);
            return CommonBLL.ConvertamperCounterToColumn(ds);
        }
        public int GetTamperCount(int meterDataId, string tamperName)
        {
            try
            {
                object obj = tamperCounterGeneralDAL.GetTamperCount(meterDataId, tamperName);
                if (obj == null)
                    return 0;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Dictionary<string, string> CreateTamperCounterDictionary()
        {
            string colAlias = "tc";
            tamperCounterColumns.Add("Voltage Imbalance  R Phase Tamper Counter", string.Concat(colAlias, ".", "VoltageImbalanceRPhaseTamperCounter"));
            tamperCounterColumns.Add("Voltage Imbalance  Y Phase Tamper Counter", string.Concat(colAlias, ".", "VoltageImbalanceYPhaseTamperCounter"));
            tamperCounterColumns.Add("Voltage Imbalance  B Phase Tamper Counter", string.Concat(colAlias, ".", "VoltageImbalanceBPhaseTamperCounter"));
            tamperCounterColumns.Add("Missing Potential R Phase Tamper Counter", string.Concat(colAlias, ".", "MissingPotentialRPhaseTamperCounter"));
            tamperCounterColumns.Add("Missing Potential Y Phase Tamper Counter", string.Concat(colAlias, ".", "MissingPotentialYPhaseTamperCounter"));
            tamperCounterColumns.Add("Missing Potential B Phase Tamper Counter", string.Concat(colAlias, ".", "MissingPotentialBPhaseTamperCounter"));
            //tamperCounterColumns.Add("Low/Under Voltage R Phase Tamper Counter", string.Concat(colAlias, ".", "LowUnderVoltageRPhaseTamperCounter"));
            //tamperCounterColumns.Add("Low/Under Voltage Y Phase Tamper Counter", string.Concat(colAlias, ".", "LowUnderVoltageYPhaseTamperCounter"));
            //tamperCounterColumns.Add("Low/Under Voltage B Phase Tamper Counter", string.Concat(colAlias, ".", "LowUnderVoltageBPhaseTamperCounter"));
            //tamperCounterColumns.Add("High/Over Voltage R Phase Tamper Counter", string.Concat(colAlias, ".", "HighOverVoltageRPhaseTamperCounter"));
            //tamperCounterColumns.Add("High/Over Voltage Y Phase Tamper Counter", string.Concat(colAlias, ".", "HighOverVoltageYPhaseTamperCounter"));
            //tamperCounterColumns.Add("High/Over Voltage B Phase Tamper Counter", string.Concat(colAlias, ".", "HighOverVoltageBPhaseTamperCounter"));
            tamperCounterColumns.Add("CT Bypass Tamper Counter", string.Concat(colAlias, ".", "CTShortTamperCounter"));
            tamperCounterColumns.Add("CT Open R Phase Tamper Counter", string.Concat(colAlias, ".", "CTOpenRPhaseTamperCounter"));
            tamperCounterColumns.Add("CT Open Y Phase Tamper Counter", string.Concat(colAlias, ".", "CTOpenYPhaseTamperCounter"));
            tamperCounterColumns.Add("CT Open B Phase Tamper Counter", string.Concat(colAlias, ".", "CTOpenBPhaseTamperCounter"));
            //tamperCounterColumns.Add("Current Without Voltage R Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentWithoutVoltageRPhaseTamperCounter"));
            //tamperCounterColumns.Add("Current Without Voltage Y Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentWithoutVoltageYPhaseTamperCounter"));
            //tamperCounterColumns.Add("Current Without Voltage B Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentWithoutVoltageBPhaseTamperCounter"));
            //tamperCounterColumns.Add("Low Power Factor R Phase Tamper Counter", string.Concat(colAlias, ".", "LowPowerFactorRPhaseTamperCounter"));
            //tamperCounterColumns.Add("Low Power Factor Y Phase Tamper Counter", string.Concat(colAlias, ".", "LowPowerFactorYPhaseTamperCounter"));
            //tamperCounterColumns.Add("Low Power Factor B Phase Tamper Counter", string.Concat(colAlias, ".", "LowPowerFactorBPhaseTamperCounter"));
            tamperCounterColumns.Add("One Phase & Neutral Absent Tamper Counter", string.Concat(colAlias, ".", "OnePhaseNeutralAbsentTamperCounter"));
            //tamperCounterColumns.Add("Current Phase Reversal Tamper Counter", string.Concat(colAlias, ".", "CurrentPhaseReversalTamperCounter"));
            tamperCounterColumns.Add("Voltage Phase Reversal Tamper Counter", string.Concat(colAlias, ".", "VoltagePhaseReversalTamperCounter"));
            tamperCounterColumns.Add("Current Imbalance R Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentImbalanceRPhaseTamperCounter"));
            tamperCounterColumns.Add("Current Imbalance Y Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentImbalanceYPhaseTamperCounter"));
            tamperCounterColumns.Add("Current Imbalance B Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentImbalanceBPhaseTamperCounter"));
            tamperCounterColumns.Add("Current Reversal R Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentReversalRPhaseTamperCounter"));
            tamperCounterColumns.Add("Current Reversal Y Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentReversalYPhaseTamperCounter"));
            tamperCounterColumns.Add("Current Reversal B Phase Tamper Counter", string.Concat(colAlias, ".", "CurrentReversalBPhaseTamperCounter"));
            tamperCounterColumns.Add("Magnetic Influence Tamper Counter", string.Concat(colAlias, ".", "MagneticInfluenceTamperCounter"));
            tamperCounterColumns.Add("Neutral Distrubance Tamper Counter", string.Concat(colAlias, ".", "NeutralDisturbanceTamperCounter"));
            tamperCounterColumns.Add("Front Cover Open Tamper Counter", string.Concat(colAlias, ".", "FrontCoverOpeningTamperCounter"));
            //tamperCounterColumns.Add("Terminal Cover Open Tamper Counter", string.Concat(colAlias, ".", "TerminalCoverOpeningTamperCounter"));
            tamperCounterColumns.Add("Billing TimeStamp", string.Concat(colAlias, ".", "BillingTimeStamp"));
            //tamperCounterColumns.Add("Billing Counter", string.Concat(colAlias, ".", "BillingCounter"));

            return tamperCounterColumns;
        }

        public bool DeleteData(long meterDataId)
        {
            return tamperCounterGeneralDAL.DeleteData(meterDataId);
        }
    }
}