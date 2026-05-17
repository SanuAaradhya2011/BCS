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
    public class TamperCounterBLL : IBLL
    {
        TamperCounterDAL tamperCounterDAL;
       
        public TamperCounterBLL()
        {
            tamperCounterDAL = new TamperCounterDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return tamperCounterDAL.DeleteData(meterDataId);
        }
        public IEntity InsertData(IEntity entity)
        {
            return tamperCounterDAL.InsertData(entity);
        }

        public int GetTamperCount(int meterDataId, string tamperName)
        {
            try
            {
                object obj = tamperCounterDAL.GetTamperCount(meterDataId, tamperName);
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

        public Dictionary<string,string> CreateTamperCounterDictionary()
        {
            Dictionary<string, string> tamperCounterColumns = new Dictionary<string, string>();
            tamperCounterColumns.Add("VoltageImbalanceRPhaseTamperCounter", "Voltage Imbalance R Phase Tamper");
            tamperCounterColumns.Add("VoltageImbalanceYPhaseTamperCounter", "Voltage Imbalance Y Phase Tamper");
            tamperCounterColumns.Add("VoltageImbalanceBPhaseTamperCounter", "Voltage Imbalance B Phase Tamper");
            tamperCounterColumns.Add("MissingPotentialRPhaseTamperCounter", "Missing Potential R Phase Tamper");
            tamperCounterColumns.Add("MissingPotentialYPhaseTamperCounter", "Missing Potential Y Phase Tamper");
            tamperCounterColumns.Add("MissingPotentialBPhaseTamperCounter", "Missing Potential B Phase Tamper");
            tamperCounterColumns.Add("LowUnderVoltageRPhaseTamperCounter", "Low Under Voltage R Phase Tamper");
            tamperCounterColumns.Add("LowUnderVoltageYPhaseTamperCounter", "Low/Under Voltage Y Phase Tamper");
            tamperCounterColumns.Add("LowUnderVoltageBPhaseTamperCounter", "Low/Under Voltage B Phase Tamper");
            tamperCounterColumns.Add("HighOverVoltageRPhaseTamperCounter", "High/Over Voltage R Phase Tamper");
            tamperCounterColumns.Add("HighOverVoltageYPhaseTamperCounter", "High/Over Voltage Y Phase Tamper");
            tamperCounterColumns.Add("HighOverVoltageBPhaseTamperCounter", "High/Over Voltage B Phase Tamper");
            tamperCounterColumns.Add("CTShortTamperCounter", "CT Short Tamper");
            tamperCounterColumns.Add("CTOpenRPhaseTamperCounter", "CT Open R Phase Tamper");
            tamperCounterColumns.Add("CTOpenYPhaseTamperCounter", "CT Open Y Phase Tamper");
            tamperCounterColumns.Add("CTOpenBPhaseTamperCounter", "CT Open B Phase Tamper");
            tamperCounterColumns.Add("CurrentWithoutVoltageRPhaseTamperCounter", "Current Without Voltage R Phase Tamper");
            tamperCounterColumns.Add("CurrentWithoutVoltageYPhaseTamperCounter", "Current Without Voltage Y Phase Tamper");
            tamperCounterColumns.Add("CurrentWithoutVoltageBPhaseTamperCounter", "Current Without Voltage B Phase Tamper");
            tamperCounterColumns.Add("LowPowerFactorRPhaseTamperCounter", "Low Power Factor R Phase Tamper");
            tamperCounterColumns.Add("LowPowerFactorYPhaseTamperCounter", "Low Power Factor Y Phase Tamper");
            tamperCounterColumns.Add("LowPowerFactorBPhaseTamperCounter", "Low Power Factor B Phase Tamper");
            tamperCounterColumns.Add("OnePhaseNeutralAbsentTamperCounter", "One Phase Neutral Absent Tamper");
            tamperCounterColumns.Add("CurrentPhaseReversalTamperCounter", "Current Phase Reversal Tamper");
            tamperCounterColumns.Add("VoltagePhaseReversalTamperCounter", "Voltage Phase Reversal Tamper");
            tamperCounterColumns.Add("CurrentImbalanceRPhaseTamperCounter", "Current Imbalance R Phase Tamper");
            tamperCounterColumns.Add("CurrentImbalanceYPhaseTamperCounter", "Current Imbalance Y Phase Tamper");
            tamperCounterColumns.Add("CurrentImbalanceBPhaseTamperCounter", "Current Imbalance B Phase Tamper");
            tamperCounterColumns.Add("CurrentReversalRPhaseTamperCounter", "Current Reversal R Phase Tamper");
            tamperCounterColumns.Add("CurrentReversalYPhaseTamperCounter", "Current Reversal Y Phase Tamper");
            tamperCounterColumns.Add("CurrentReversalBPhaseTamperCounter", "Current Reversal B Phase Tamper");
            tamperCounterColumns.Add("MagneticInfluenceTamperCounter", "Magnetic Influence Tamper");
            tamperCounterColumns.Add("NeutralDisturbanceTamperCounter", "Neutral Disturbance Tamper");
            tamperCounterColumns.Add("FrontCoverOpeningTamperCounter", "Front Cover Opening Tamper");
            tamperCounterColumns.Add("TerminalCoverOpeningTamperCounter", "Terminal Cover Opening Tamper");
            return tamperCounterColumns;
        }
    }
}




