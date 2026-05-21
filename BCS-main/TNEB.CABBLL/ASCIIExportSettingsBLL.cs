using System;
using System.Data;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Collections.Generic;
using CAB.IECFramework.Entity;
using CAB.DALC.Data;
using CAB.Entity;
using LTCTBLL;

namespace CAB.BLL
{
    public class ASCIIExportSettingsBLL : IBLL
    {
        private bool isWBExportVCL = false;
        private ASCIIExportSettingsDAL asciiExportSettingsDAL = new ASCIIExportSettingsDAL();
        private const string IMPORT = "";
        private const string EXPORT = " (E)";
        public ASCIIExportSettingsBLL()
        {
            if (UtilityDetails.UtilityName == IECUtilityEntity.WBEXPORTVCL)
            {
                isWBExportVCL = true;
            }
        }
        public string[] GetBillingDisplayColumnName()
        {
            List<string> parameters = new List<string>();
            
            parameters.Add("Billing Reset Type");
            parameters.Add("Cumulative Energy kWh" + IMPORT);
            parameters.Add("Cumulative Energy kVArh (Lag)");
            parameters.Add("Cumulative Energy kVArh (Lead)");
            parameters.Add("Cumulative Energy kVAh");
            if (isWBExportVCL)
            {
                parameters.Add("Cumulative Energy kWh" + EXPORT);
                parameters.Add("Cumulative Energy kVAh" + EXPORT);
            }
            parameters.Add("Cumulative MD1");
            parameters.Add("Cumulative MD1 Time Stamp");
            parameters.Add("Cumulative MD2");
            parameters.Add("Cumulative MD2 Time Stamp");
            parameters.Add("Average Power Factor");
            //Fix defect #158919
            if (!UtilityDetails.DisablePowerOnHourInBilling)
            {
                parameters.Add("Power On Hours");
            }
            parameters.Add("LoadFactor");
            parameters.Add("Tariff1 kWh");
            parameters.Add("Tariff1 kVAh");
            parameters.Add("Tariff1 kVArh (Lag)");
            parameters.Add("Tariff1 kVArh (Lead)");
            parameters.Add("Tariff1 MD1");
            parameters.Add("Tariff1 MD1 Time Stamp");
            parameters.Add("Tariff1 MD2");
            parameters.Add("Tariff1 MD2 Time Stamp");
            parameters.Add("Tariff1 Average Power Factor");
            parameters.Add("Tariff2 kWh");
            parameters.Add("Tariff2 kVAh");
            parameters.Add("Tariff2 kVArh (Lag)");
            parameters.Add("Tariff2 kVArh (Lead)");
            parameters.Add("Tariff2 MD1");
            parameters.Add("Tariff2 MD1 Time Stamp");
            parameters.Add("Tariff2 MD2");
            parameters.Add("Tariff2 MD2 Time Stamp");
            parameters.Add("Tariff2 Average Power Factor");
            parameters.Add("Tariff3 kWh");
            parameters.Add("Tariff3 kVAh");
            parameters.Add("Tariff3 kVArh (Lag)");
            parameters.Add("Tariff3 kVArh (Lead)");
            parameters.Add("Tariff3 MD1");
            parameters.Add("Tariff3 MD1 Time Stamp");
            parameters.Add("Tariff3 MD2");
            parameters.Add("Tariff3 MD2 Time Stamp");
            parameters.Add("Tariff3 Average Power Factor");
            parameters.Add("Tariff4 kWh");
            parameters.Add("Tariff4 kVAh");
            parameters.Add("Tariff4 kVArh (Lag)");
            parameters.Add("Tariff4 kVArh (Lead)");
            parameters.Add("Tariff4 MD1");
            parameters.Add("Tariff4 MD1 Time Stamp");
            parameters.Add("Tariff4 MD2");
            parameters.Add("Tariff4 MD2 Time Stamp");
            parameters.Add("Tariff4 Average Power Factor");
            parameters.Add("Tariff5 kWh");
            parameters.Add("Tariff5 kVAh");
            parameters.Add("Tariff5 kVArh (Lag)");
            parameters.Add("Tariff5 kVArh (Lead)");
            parameters.Add("Tariff5 MD1");
            parameters.Add("Tariff5 MD1 Time Stamp");
            parameters.Add("Tariff5 MD2");
            parameters.Add("Tariff5 MD2 Time Stamp");
            parameters.Add("Tariff5 Average Power Factor");
            parameters.Add("Tariff6 kWh");
            parameters.Add("Tariff6 kVAh");
            parameters.Add("Tariff6 kVArh (Lag)");
            parameters.Add("Tariff6 kVArh (Lead)");
            parameters.Add("Tariff6 MD1");
            parameters.Add("Tariff6 MD1 Time Stamp");
            parameters.Add("Tariff6 MD2");
            parameters.Add("Tariff6 MD2 Time Stamp");
            parameters.Add("Tariff6 Average Power Factor");
            parameters.Add("Tariff7 kWh");
            parameters.Add("Tariff7 kVAh");
            parameters.Add("Tariff7 kVArh (Lag)");
            parameters.Add("Tariff7 kVArh (Lead)");
            parameters.Add("Tariff7 MD1");
            parameters.Add("Tariff7 MD1 Time Stamp");
            parameters.Add("Tariff7 MD2");
            parameters.Add("Tariff7 MD2 Time Stamp");
            parameters.Add("Tariff7 Average Power Factor");
            parameters.Add("Tariff8 kWh");
            parameters.Add("Tariff8 kVAh");
            parameters.Add("Tariff8 kVArh (Lag)");
            parameters.Add("Tariff8 kVArh (Lead)");
            parameters.Add("Tariff8 MD1");
            parameters.Add("Tariff8 MD1 Time Stamp");
            parameters.Add("Tariff8 MD2");
            parameters.Add("Tariff8 MD2 Time Stamp");
            parameters.Add("Tariff8 Average Power Factor");
            parameters.Add("Voltage Imbalance R Phase Tamper Counter");
            parameters.Add("Voltage Imbalance Y Phase Tamper Counter");
            parameters.Add("Voltage Imbalance B Phase Tamper Counter");
            parameters.Add("Missing Potential R Phase Tamper Counter");
            parameters.Add("Missing Potential Y Phase Tamper Counter");
            parameters.Add("Missing Potential B Phase Tamper Counter");
            parameters.Add("CT Short Tamper Counter");
            parameters.Add("CT Open R Phase Tamper Counter");
            parameters.Add("CT Open Y Phase Tamper Counter");
            parameters.Add("CT Open B Phase Tamper Counter");
            parameters.Add("OnePhase Neutral Absent Tamper Counter");
            parameters.Add("Voltage Phase Reversal Tamper Counter");
            parameters.Add("Current Imbalance R Phase Tamper Counter");
            parameters.Add("Current Imbalance YPhase Tamper Counter");
            parameters.Add("Current Imbalance BPhase Tamper Counter");
            parameters.Add("Current Reversal R Phase Tamper Counter");
            parameters.Add("Current Reversal Y Phase Tamper Counter");
            parameters.Add("Current Reversal B Phase Tamper Counter");
            parameters.Add("Magnetic Influence Tamper Counter");
            parameters.Add("Neutral Disturbance Tamper Counter"); ;
            parameters.Add("Front Cover Opening Tamper Counter");
            parameters.Add("Billing Time Stamp");
            parameters.Add("Billing Counter");
            return parameters.ToArray();
        }
        public string[] GetBillingDBColumnName()
        {
            List<string> parameters = new List<string>();           
                  
            parameters.Add("A.BillingResetType,"); 
            parameters.Add("A.CumulativeEnergyKWH,");
            parameters.Add("A.CumulativeEnergyKVARHLag,"); 
            parameters.Add("A.CumulativeEnergyKVARHLead,"); 
            parameters.Add("A.CumulativeEnergyKVAH,"); 
            if (isWBExportVCL)
            {
               parameters.Add("A.CumulativeExportEnergyKWH,"); 
               parameters.Add("A.CumulativeExportEnergyKVAH,"); 
            }
            parameters.Add("A.CumulativeMD1,"); 
            parameters.Add("A.CumulativeMD1TimeStamp,"); 
            parameters.Add("A.CumulativeMD2,"); 
            parameters.Add("A.CumulativeMD2TimeStamp,"); 
            parameters.Add("A.AveragePowerFactor,"); 
            //Fix defect #158919
            if (!UtilityDetails.DisablePowerOnHourInBilling)
            {
             parameters.Add("A.PowerOnHours,"); 
            }
            parameters.Add("A.LoadFactor,"); 
            parameters.Add("B.Tariff1_kWh,"); 
            parameters.Add("B.Tariff1_kVAh,"); 
            parameters.Add("B.Tariff1_kVARh_lag,"); 
            parameters.Add("B.Tariff1_kVARh_lead,"); 
            parameters.Add("B.Tariff1_MD1,");
            parameters.Add("B.Tariff1_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff1_MD2,"); 
            parameters.Add("B.Tariff1_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff1_Aver_PF,"); 
            parameters.Add("B.Tariff2_kWh,"); 
            parameters.Add("B.Tariff2_kVAh,"); 
            parameters.Add("B.Tariff2_kVARh_lag,"); 
            parameters.Add("B.Tariff2_kVARh_lead,"); 
            parameters.Add("B.Tariff2_MD1,"); 
            parameters.Add("B.Tariff2_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff2_MD2,"); 
            parameters.Add("B.Tariff2_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff2_Aver_PF,"); 
            parameters.Add("B.Tariff3_kWh,"); 
            parameters.Add("B.Tariff3_kVAh,"); 
            parameters.Add("B.Tariff3_kVARh_lag,"); 
            parameters.Add("B.Tariff3_kVARh_lead,"); 
            parameters.Add("B.Tariff3_MD1,"); 
            parameters.Add("B.Tariff3_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff3_MD2,"); 
            parameters.Add("B.Tariff3_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff3_Aver_PF,"); 
            parameters.Add("B.Tariff4_kWh,"); 
            parameters.Add("B.Tariff4_kVAh,"); 
            parameters.Add("B.Tariff4_kVARh_lag,"); 
            parameters.Add("B.Tariff4_kVARh_lead,"); 
            parameters.Add("B.Tariff4_MD1,"); 
            parameters.Add("B.Tariff4_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff4_MD2,"); 
            parameters.Add("B.Tariff4_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff4_Aver_PF,"); 
            parameters.Add("B.Tariff5_kWh,"); 
            parameters.Add("B.Tariff5_kVAh,"); 
            parameters.Add("B.Tariff5_kVARh_lag,"); 
            parameters.Add("B.Tariff5_kVARh_lead,"); 
            parameters.Add("B.Tariff5_MD1,"); 
            parameters.Add("B.Tariff5_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff5_MD2,"); 
            parameters.Add("B.Tariff5_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff5_Aver_PF,"); 
            parameters.Add("B.Tariff6_kWh,"); 
            parameters.Add("B.Tariff6_kVAh,"); 
            parameters.Add("B.Tariff6_kVARh_lag,"); 
            parameters.Add("B.Tariff6_kVARh_lead,"); 
            parameters.Add("B.Tariff6_MD1,"); 
            parameters.Add("B.Tariff6_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff6_MD2,"); 
            parameters.Add("B.Tariff6_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff6_Aver_PF,");
            parameters.Add("B.Tariff7_kWh,");
            parameters.Add("B.Tariff7_kVAh,"); 
            parameters.Add("B.Tariff7_kVARh_lag,"); 
            parameters.Add("B.Tariff7_kVARh_lead,");
            parameters.Add("B.Tariff7_MD1,");
            parameters.Add("B.Tariff7_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff7_MD2,"); 
            parameters.Add("B.Tariff7_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff7_Aver_PF,"); 
            parameters.Add("B.Tariff8_kWh,"); 
            parameters.Add("B.Tariff8_kVAh,"); 
            parameters.Add("B.Tariff8_kVARh_lag,"); 
            parameters.Add("B.Tariff8_kVARh_lead,"); 
            parameters.Add("B.Tariff8_MD1,"); 
            parameters.Add("B.Tariff8_MD1_TimeStamp,"); 
            parameters.Add("B.Tariff8_MD2,"); 
            parameters.Add("B.Tariff8_MD2_TimeStamp,"); 
            parameters.Add("B.Tariff8_Aver_PF,"); 
            parameters.Add("C.VoltageImbalanceRPhaseTamperCounter,");
            parameters.Add("C.VoltageImbalanceYPhaseTamperCounter,"); 
            parameters.Add("C.VoltageImbalanceBPhaseTamperCounter,"); 
            parameters.Add("C.MissingPotentialRPhaseTamperCounter,"); 
            parameters.Add("C.MissingPotentialYPhaseTamperCounter,"); 
            parameters.Add("C.MissingPotentialBPhaseTamperCounter,"); 
            parameters.Add("C.CTShortTamperCounter,"); 
            parameters.Add("C.CTOpenRPhaseTamperCounter,"); 
            parameters.Add("C.CTOpenYPhaseTamperCounter,"); 
            parameters.Add("C.CTOpenBPhaseTamperCounter,"); 
            parameters.Add("C.OnePhaseNeutralAbsentTamperCounter,"); 
            parameters.Add("C.VoltagePhaseReversalTamperCounter,"); 
            parameters.Add("C.CurrentImbalanceRPhaseTamperCounter,"); 
            parameters.Add("C.CurrentImbalanceYPhaseTamperCounter,"); 
            parameters.Add("C.CurrentImbalanceBPhaseTamperCounter,"); 
            parameters.Add("C.CurrentReversalRPhaseTamperCounter,"); 
            parameters.Add("C.CurrentReversalYPhaseTamperCounter,"); 
            parameters.Add("C.CurrentReversalBPhaseTamperCounter,"); 
            parameters.Add("C.MagneticInfluenceTamperCounter,"); 
            parameters.Add("C.NeutralDisturbanceTamperCounter,"); 
            parameters.Add("C.FrontCoverOpeningTamperCounter,"); 
            parameters.Add("C.BillingTimeStamp,"); 
            parameters.Add("C.BillingCounter,");
            return parameters.ToArray();
        } 
        public string[] GetGeneralDisplayColumnName()
        {
            int counter = 0;
            int paramindex;
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
            {
                paramindex = 26;
            }
            else if (UtilityDetails.UtilityName == IECUtilityEntity.UGVCL || UtilityDetails.UtilityName == IECUtilityEntity.PVVNL || UtilityDetails.UtilityName == IECUtilityEntity.JDVVNL)
            {
                paramindex = 23;
            }
            else if (isWBExportVCL)
            {
                paramindex = 25;
            }
            else
            {
                paramindex = 23;
            }
            string[] param = new string[paramindex];
            //string[] param = new string[24];//151
            param[counter] = "Meter ID"; counter++;
            param[counter] = "Meter Date Time"; counter++;
            param[counter] = "Error Code"; counter++;
            param[counter] = "Meter Constant"; counter++;
            param[counter] = "Firmware Version"; counter++;
            param[counter] = "Voltage Phase Sequence"; counter++;
            param[counter] = "Total Active Energy" + IMPORT; counter++;       
            if (isWBExportVCL)
            {
                param[counter] = "Cumulative Energy kWh" + EXPORT; counter++;
                param[counter] = "Cumulative Energy kVAh" + EXPORT; counter++;
            }
            param[counter] = "Cumulative MD1"; counter++;
            param[counter] = "Cumulative MD2"; counter++;
            param[counter] = "Rising Demand KW"; counter++;
            param[counter] = "Elapsed Time KW"; counter++;
            param[counter] = "Rising Demand KVA"; counter++;
            param[counter] = "Elapsed Time KVA"; counter++;
            param[counter] = "Total Power On Hours"; counter++;
            param[counter] = "Current Month Power On Hours"; counter++;
            param[counter] = "MDReset Counter"; counter++;
            param[counter] = "Readout Counter"; counter++;
            param[counter] = "Programming Counter"; counter++;
            param[counter] = "Latest Tamper Occurrence Id"; counter++;
            param[counter] = "Occurrence Time"; counter++;
            param[counter] = "Latest Tamper Restoration Id"; counter++;
            param[counter] = "Battery Mode Power On Hour"; counter++;
            param[counter] = "Restoration Time"; counter++;
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
            {
                param[counter] = "Power off Days";counter++;
                param[counter] = "No Load Duration"; counter++;
                param[counter] = "No Supply Duration";
            }
            return param;
        }
        public string[] GetGeneralDBColumnName()
        {
            int counter = 0;
            int paramindex;
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
            {
                paramindex = 26;
            }
            else if (UtilityDetails.UtilityName == IECUtilityEntity.UGVCL || UtilityDetails.UtilityName == IECUtilityEntity.PVVNL || UtilityDetails.UtilityName == IECUtilityEntity.JDVVNL)
            {
                paramindex = 23;
            }
            else if (isWBExportVCL)
            {
                paramindex = 25;
            }
            else
            {
                paramindex = 23;
            }
            string[] param = new string[paramindex];//151
            param[counter] = "A.MeterID,"; counter++;
            param[counter] = "A.MeterDateTime,"; counter++;
            param[counter] = "A.ErrorCode,"; counter++;
            param[counter] = "A.MeterConstant,"; counter++;
            param[counter] = "A.FirmwareVersion,"; counter++;
            param[counter] = "A.VoltagePhaseSequence,"; counter++;
            param[counter] = "A.TotalActiveEnergy,"; counter++;
            if (isWBExportVCL)
            {
            
                    param[counter] = "A.CumulativeExportEnergyKWH,"; counter++;
                    param[counter] = "A.CumulativeExportEnergyKVAH,"; counter++;
              
            }
            param[counter] = "A.CumulativeMD1,"; counter++;
            param[counter] = "A.CumulativeMD2,"; counter++;
            param[counter] = "A.RisingDemandKW,"; counter++;
            param[counter] = "A.ElapsedTimeKW,"; counter++;
            param[counter] = "A.RisingDemandKVA,"; counter++;
            param[counter] = "A.ElapsedTimeKVA,"; counter++;
            param[counter] = "A.TotalPowerOnHours,"; counter++;
            param[counter] = "A.CurrentMonthPowerOnHours,"; counter++;
            param[counter] = "A.MDResetCounter,"; counter++;
            param[counter] = "A.ReadoutCounter,"; counter++;
            param[counter] = "A.ProgrammingCounter,"; counter++;
            param[counter] = "A.LatestTamperOccurrenceID,"; counter++;
            param[counter] = "A.OccurrenceTime,"; counter++;
            param[counter] = "A.LatestTamperRestorationID,"; counter++;
            param[counter] = "A.BateryModePowerOnHour,"; counter++;
            param[counter] = "A.RestorationTime,"; counter++;
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
            {
                param[counter] = "A.PowerOffDays,"; counter++;
                param[counter] = "H.NoLoadDuration,"; counter++;
                param[counter] = "H.NoSupplyDuration,";
            }
            return param;
        } 
        public string[] GetInstantDisplayColumnName()
        {
            int paramindex;
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB)
                paramindex = 26;
            else
                paramindex = 17;
            string[] param = new string[paramindex];
            //param[0] = "Meter ID";
            param[0] = "Meter Date Time";
            param[1] = "Voltage R Phase";
            param[2] = "Voltage Y Phase";
            param[3] = "Voltage B Phase";
            param[4] = "Current R Phase";
            param[5] = "Current Y Phase";
            param[6] = "Current B Phase";
            param[7] = "Instant Active Power";
            param[8] = "Instant Reactive Lag Power";
            param[9] = "Instant Reactive Lead Power";
            param[10] = "Instant Apparent Power";
            param[11] = "Total Power Factor";
            param[12] = "Power Factor R Phase";
            param[13] = "Power Factor Y Phase";
            param[14] = "Power Factor B Phase"; 
            param[15] = "Frequency";
            param[16] = "Total Fundamental Active Energy";
            if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB)
            {
                param[17] = "Instant Active Power R Phase";
                param[18] = "Instant Active Power Y Phase";
                param[19] = "Instant Active Power B Phase";
                param[20] = "Instant Reactive Power R Phase";
                param[21] = "Instant Reactive Power Y Phase";
                param[22] = "Instant Reactive Power B Phase";
                param[23] = "Instant Apparent Power R Phase";
                param[24] = "Instant Apparent Power Y Phase";
                param[25] = "Instant Apparent Power B Phase";
            }
            return param;
        }
        public string[] GetInstantDBColumnName()
        {
            string[] param = new string[26];
            //param[0] = "MeterID,";
            param[0] = "MeterDateTime,";
            param[1] = "VoltageRPhase,";
            param[2] = "VoltageYPhase,";
            param[3] = "VoltageBPhase,";
            param[4] = "CurrentRPhase,";
            param[5] = "CurrentYPhase,";
            param[6] = "CurrentBPhase,";
            param[7] = "InstantActivepower,";
            param[8] = "InstantReactiveLagPower,";
            param[9] = "InstantReactiveLeadPower,";
            param[10] = "InstantApparentPower,";
            param[11] = "TotalPowerFactor,";
            param[12] = "PowerFactorRPhase,";
            param[13] = "PowerFactorYPhase,";
            param[14] = "PowerFactorBPhase,"; 
            param[15] = "Frequency,";
            param[16] = "TotalFundamentalActiveEnergy,";
            param[17] = "InstantActivepowerRPhase,";
            param[18] = "InstantActivepowerYPhase,";
            param[19] = "InstantActivepowerBPhase,";
            param[20] = "InstantReactivepowerRPhase,";
            param[21] = "InstantReactivepowerYPhase,";
            param[22] = "InstantReactivepowerBPhase,";
            param[23] = "InstantApparentpowerRPhase,";
            param[24] = "InstantApparentpowerYPhase,";
            param[25] = "InstantApparentpowerBPhase,";
            return param;
        }

        public string[] GetTamperDBColumnName()
        {
            string[] param = new string[53];
            param[0] = "A.VoltageImbalanceRPhaseTamperCounter,";
            param[1] = "A.VoltageImbalanceYPhaseTamperCounter,";
            param[2] = "A.VoltageImbalanceBPhaseTamperCounter,";
            param[3] = "A.MissingPotentialRPhaseTamperCounter,";
            param[4] = "A.MissingPotentialYPhaseTamperCounter,";
            param[5] = "A.MissingPotentialBPhaseTamperCounter,";
            param[6] = "A.CTShortTamperCounter,";
            param[7] = "A.CTOpenRPhaseTamperCounter,";
            param[8] = "A.CTOpenYPhaseTamperCounter,";
            param[9] = "A.CTOpenBPhaseTamperCounter,";
            param[10] = "A.OnePhaseNeutralAbsentTamperCounter,";
            param[11] = "A.VoltagePhaseReversalTamperCounter,";
            param[12] = "A.CurrentImbalanceRPhaseTamperCounter,";
            param[13] = "A.CurrentImbalanceYPhaseTamperCounter,";
            param[14] = "A.CurrentImbalanceBPhaseTamperCounter,";
            param[15] = "A.CurrentReversalRPhaseTamperCounter,";
            param[16] = "A.CurrentReversalYPhaseTamperCounter,";
            param[17] = "A.CurrentReversalBPhaseTamperCounter,";
            param[18] = "A.MagneticInfluenceTamperCounter,";
            param[19] = "A.NeutralDisturbanceTamperCounter,";
            param[20] = "A.FrontCoverOpeningTamperCounter,";
            param[21] = "A.BillingTimeStamp,";
            param[22] = "B.TotalTamperCounter,";
            param[23] = "B.PowerOnOffCounter,";
            param[24] = "B.LowLoadCounter,";
            param[25] = "B.OverLoadCounter,";
            param[26] = "C.TamperCode,";
            param[27] = "C.TamperOccurredTime,";
            param[28] = "C.TamperRestoredTime,";
            param[29] = "C.RVoltageOccurred,";
            param[30] = "C.YVoltageOccurred,";
            param[31] = "C.BVoltageOccurred,";
            param[32] = "C.RCurrentOccurred,";
            param[33] = "C.YCurrentOccurred,";
            param[34] = "C.BCurrentOccurred,";
            param[35] = "C.RPFOccurred,";
            param[36] = "C.YPFOccurred,";
            param[37] = "C.BPFOccurred,";
            param[38] = "C.TotalPFOccurred,";
            param[39] = "C.kWhOccurred,";
            param[40] = "C.kVAhOccurred,";
            param[41] = "C.RVoltageRestored,";
            param[42] = "C.YVoltageRestored,";
            param[43] = "C.BVoltageRestored,";
            param[44] = "C.RCurrentRestored,";
            param[45] = "C.YCurrentRestored,";
            param[46] = "C.BCurrentRestored,";
            param[47] = "C.RPFRestored,";
            param[48] = "C.YPFRestored,";
            param[49] = "C.BPFRestored,";
            param[50] = "C.TotalPFRestored,";
            param[51] = "C.kWhRestored,";
            param[52] = "C.kVAhRestored,";
            return param;
        }
        public string[] GetTamperDisplayColumnName()
        {
            string[] param = new string[53];
            param[0] = "Voltage Imbalance R Phase Tamper Counter";
            param[1] = "Voltage Imbalance Y Phase Tamper Counter";
            param[2] = "Voltage Imbalance B Phase Tamper Counter";
            param[3] = "Missing Potential R Phase Tamper Counter";
            param[4] = "Missing Potential Y Phase Tamper Counter";
            param[5] = "Missing Potential B Phase Tamper Counter"; 
            param[6] = "CT Short Tamper Counter";
            param[7] = "CT Open R Phase Tamper Counter";
            param[8] = "CT Open Y Phase Tamper Counter";
            param[9] = "CT Open B Phase Tamper Counter";
            param[10] = "OnePhase Neutral Absent Tamper Counter";
            param[11] = "Voltage Phase Reversal Tamper Counter";
            param[12] = "Current Imbalance R Phase Tamper Counter";
            param[13] = "Current Imbalance YPhase Tamper Counter";
            param[14] = "Current Imbalance BPhase Tamper Counter";
            param[15] = "Current Reversal R Phase Tamper Counter";
            param[16] = "Current Reversal Y Phase Tamper Counter";
            param[17] = "Current Reversal B Phase Tamper Counter";
            param[18] = "Magnetic Influence Tamper Counter";
            param[19] = "Neutral Disturbance Tamper Counter";
            param[20] = "Front Cover Opening Tamper Counter";
           
            param[21] = "Billing Time Stamp";
            param[22] = "Total Tamper Counter";           
            param[23] = "Power On Off Counter";
            param[24] = "Low Load Counter";
            param[25] = "Over Load Counter";
            param[26] = "Tamper Code";
            param[27] = "Tamper Occurred Time";
            param[28] = "Tamper Restored Time";
            param[29] = "R Voltage Occurred";
            param[30] = "Y Voltage Occurred";
            param[31] = "B Voltage Occurred";
            param[32] = "R Current Occurred";
            param[33] = "Y Current Occurred";
            param[34] = "B Current Occurred";
            param[35] = "R PF Occurred";
            param[36] = "Y PF Occurred";
            param[37] = "B PF Occurred";
            param[38] = "Total PF Occurred";
            param[39] = "kWh Occurred";
            param[40] = "kVAh Occurred";
            param[41] = "R Voltage Restored";
            param[42] = "Y Voltage Restored";
            param[43] = "B Voltage Restored";
            param[44] = "R Current Restored";
            param[45] = "Y Current Restored";
            param[46] = "B Current Restored";
            param[47] = "R PF Restored";
            param[48] = "Y PF Restored";
            param[49] = "B PF Restored";
            param[50] = "Total PF Restored";
            param[51] = "kWh Restored";
            param[52] = "kVAh Restored";
            return param;
        }

        public string[] GetLoadSurveyDisplayColumnName()
        {
            string[] param = new string[17];
            param[0] = "Meter Reading Date time";
            param[1] = "R Phase Voltage";
            param[2] = "Y Phase Voltage";
            param[3] = "B Phase Voltage";
            param[4] = "R Phase Current";
            param[5] = "Y Phase Current";
            param[6] = "B Phase Current";
            param[7] = "Average Voltage";
            param[8] = "Average Current";
            param[9] = "Demand KVAR Lead";
            param[10] = "Demand KVA";
            param[11] = "Demand KW";
            param[12] = "Demand KVAR (Lag)";
            param[13] = "Power Factor";
            param[14] = "Tamper Status";
            param[15] = "Load Survey Date Time";
            param[16] = "MD Interval Period";
            return param;
        }
        public string[] GetLoadSurveyDBColumnName()
        {
            string[] param = new string[17];
            param[0] = "MeterReadingDatetime,";
            param[1] = "RPhaseVoltage,";
            param[2] = "YPhaseVoltage,";
            param[3] = "BPhaseVoltage,";
            param[4] = "RPhaseCurrent,";
            param[5] = "YPhaseCurrent,";
            param[6] = "BPhaseCurrent,";
            param[7] = "AvgVoltage,";
            param[8] = "AvgCurrent,";
            param[9] = "DemandKVARLead,";
            param[10] = "DemandKVA,";
            param[11] = "DemandKW,";
            param[12] = "DemandKVARLag,";
            param[13] = "PowerFactor,";
            param[14] = "TamperStatus,";
            param[15] = "LoadSurveyDateTime,";
            param[16] = "MDIntervalPeriod,";
            return param;
        }

        public string GetDBColumn(string text, SettingTypes types)
        {
            string dbText = string.Empty;
            string[] DispCol = null;
            string[] dbCol = null;
            if (types.Equals(SettingTypes.Billing))
            {
                DispCol = GetBillingDisplayColumnName();
                dbCol = GetBillingDBColumnName();
            }
            else if (types.Equals(SettingTypes.General))
            {
                DispCol = GetGeneralDisplayColumnName();
                dbCol = GetGeneralDBColumnName();
            }
            else if (types.Equals(SettingTypes.Instant))
            {
                DispCol = GetInstantDisplayColumnName();
                dbCol = GetInstantDBColumnName();
            }
            else if (types.Equals(SettingTypes.Tamper))
            {
                DispCol = GetTamperDisplayColumnName();
                dbCol = GetTamperDBColumnName();
            }
            else if (types.Equals(SettingTypes.LoadSurvey))
            {
                DispCol = GetLoadSurveyDisplayColumnName();
                dbCol = GetLoadSurveyDBColumnName();
            }
            if (DispCol != null)
            {
                for (int counter = 0; counter < DispCol.Length; counter++)
                {
                    if (text.Trim().Equals(DispCol[counter]))
                    {
                        dbText = dbCol[counter];
                        break;
                    }
                }
            }
            return dbText;
        }

        public IEntity InsertData(IEntity entity)
        {
            return asciiExportSettingsDAL.InsertData(entity);
        }
        public bool UpdateData(IEntity entity)
        {
            return asciiExportSettingsDAL.UpdateData(entity);
        }

        public bool IsValidFile(string fileName)
        {
            return asciiExportSettingsDAL.ValidateFile(fileName);
        }
        public DataSet ListDataSet()
        {
            return asciiExportSettingsDAL.ListDataSet();
        }
        public IEntity DetailData(string id)
        {
            return asciiExportSettingsDAL.GetDetailData(Convert.ToInt32(id));
        }
        public void DeleteSettings(int settingsID)
        {
            asciiExportSettingsDAL.DeleteData(settingsID);
        }

        public DataSet GetParameterData(string qry)
        {
            return asciiExportSettingsDAL.GetParameterData(qry);
        }
    }
}

