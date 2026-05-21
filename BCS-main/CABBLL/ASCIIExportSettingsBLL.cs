using System;
using System.Data;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Collections.Generic;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using CAB.Entity;
using System.Collections;
using Hunt.EPIC.Logging;

namespace CAB.BLL
{
    public class ASCIIExportSettingsBLL : IBLL
    {
        private ASCIIExportSettingsDAL asciiExportSettingsDAL = new ASCIIExportSettingsDAL();
        private ApplicationType appType;
        DLMS650CommonBLL dlms650CommonBLL = new DLMS650CommonBLL();
        string utility = string.Empty;
        bool isPUMA = false;
        bool isMPKWCL = false;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ASCIIExportSettingsBLL).ToString());
        public ASCIIExportSettingsBLL()
        {
            if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                isPUMA = true;
            }
            if (UtilityEntity.MPKWCL == UtilityDetails.Utility)
            {
                isMPKWCL = true;
            }
            appType = ConfigInfo.GetApplicationType();
        }
        public string[] GetBillingDisplayColumnName()
        {
            string[] param = null;
            #region IEC
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[138];
                param[0] = "CT Ratio";
                param[1] = "Cumulative Active Energy";//Energy kWh
                param[2] = "Cumulative Reactive Energy (Lag)";//Energy kVArh
                param[3] = "Cumulative Reactive Energy (Lead)";//Energy kVArh
                param[4] = "Cumulative Apparent Energy";//Energy kVAh
                param[5] = "Cumulative MD1";
                param[6] = "Cumulative MD1 Time Stamp";
                param[7] = "Cumulative MD2";
                param[8] = "Cumulative MD2 Time Stamp";
                param[9] = "Cumulative MD3";
                param[10] = "Cumulative MD3 Time Stamp";
                param[11] = "Average Power Factor";
                param[12] = "Power On Hours";
                param[13] = "LoadFactor";
                param[14] = "Tariff1 Active Energy";//kWh
                param[15] = "Tariff1 Apparent Energy";//kVAh
                param[16] = "Tariff1 Reactive Energy (Lag)";//kVArh
                param[17] = "Tariff1 Reactive Energy (Lead)";//kVArh
                param[18] = "Tariff1 MD1";
                param[19] = "Tariff1 MD1 Time Stamp";
                param[20] = "Tariff1 MD2";
                param[21] = "Tariff1 MD2 Time Stamp";
                param[22] = "Tariff1 MD3";
                param[23] = "Tariff1 MD3 Time Stamp";
                param[24] = "Tariff1 Average Power Factor";
                param[25] = "Tariff2 Active Energy";//kWh
                param[26] = "Tariff2 Apparent Energy";//kVAh
                param[27] = "Tariff2 Reactive Energy (Lag)";//kVArh
                param[28] = "Tariff2 Reactive Energy (Lead)";//kVArh
                param[29] = "Tariff2 MD1";
                param[30] = "Tariff2 MD1 Time Stamp";
                param[31] = "Tariff2 MD2";
                param[32] = "Tariff2 MD2 Time Stamp";
                param[33] = "Tariff2 MD3";
                param[34] = "Tariff2 MD3 Time Stamp";
                param[35] = "Tariff2 Average Power Factor";
                param[36] = "Tariff3 Active Energy";//kWh
                param[37] = "Tariff3 Apparent Energy";//kVAh
                param[38] = "Tariff3 Reactive Energy (Lag)";//kVArh
                param[39] = "Tariff3 Reactive Energy (Lead)";//kVArh
                param[40] = "Tariff3 MD1";
                param[41] = "Tariff3 MD1 Time Stamp";
                param[42] = "Tariff3 MD2";
                param[43] = "Tariff3 MD2 Time Stamp";
                param[44] = "Tariff3 MD3";
                param[45] = "Tariff3 MD3 Time Stamp";
                param[46] = "Tariff3 Average Power Factor";
                param[47] = "Tariff4 Active Energy";//kWh
                param[48] = "Tariff4 Apparent Energy";//kVAh
                param[49] = "Tariff4 Reactive Energy (Lag)";//kVArh
                param[50] = "Tariff4 Reactive Energy (Lead)";//kVArh
                param[51] = "Tariff4 MD1";
                param[52] = "Tariff4 MD1 Time Stamp";
                param[53] = "Tariff4 MD2";
                param[54] = "Tariff4 MD2 Time Stamp";
                param[55] = "Tariff4 MD3";
                param[56] = "Tariff4 MD3 Time Stamp";
                param[57] = "Tariff4 Average Power Factor";
                param[58] = "Tariff5 Active Energy";//kWh
                param[59] = "Tariff5 Apparent Energy";//kVAh
                param[60] = "Tariff5 Reactive Energy (Lag)";//kVArh
                param[61] = "Tariff5 Reactive Energy (Lead)";//kVArh
                param[62] = "Tariff5 MD1";
                param[63] = "Tariff5 MD1 Time Stamp";
                param[64] = "Tariff5 MD2";
                param[65] = "Tariff5 MD2 Time Stamp";
                param[66] = "Tariff5 MD3";
                param[67] = "Tariff5 MD3 Time Stamp";
                param[68] = "Tariff5 Average Power Factor";
                param[69] = "Tariff6 Active Energy";//kWh
                param[70] = "Tariff6 Apparent Energy";//kVAh
                param[71] = "Tariff6 Reactive Energy (Lag)";//kVArh
                param[72] = "Tariff6 Reactive Energy (Lead)";//kVArh
                param[73] = "Tariff6 MD1";
                param[74] = "Tariff6 MD1 Time Stamp";
                param[75] = "Tariff6 MD2";
                param[76] = "Tariff6 MD2 Time Stamp";
                param[77] = "Tariff6 MD3";
                param[78] = "Tariff6 MD3 Time Stamp";
                param[79] = "Tariff6 Average Power Factor";
                param[80] = "Tariff7 Active Energy";//kWh
                param[81] = "Tariff7 Apparent Energy";//kVAh
                param[82] = "Tariff7 Reactive Energy (Lag)";//kVArh
                param[83] = "Tariff7 Reactive Energy (Lead)";//kVArh
                param[84] = "Tariff7 MD1";
                param[85] = "Tariff7 MD1 Time Stamp";
                param[86] = "Tariff7 MD2";
                param[87] = "Tariff7 MD2 Time Stamp";
                param[88] = "Tariff7 MD3";
                param[89] = "Tariff7 MD3 Time Stamp";
                param[90] = "Tariff7 Average Power Factor";
                param[91] = "Tariff8 Active Energy";//kWh
                param[92] = "Tariff8 Apparent Energy";//kVAh
                param[93] = "Tariff8 Reactive Energy (Lag)";//kVArh
                param[94] = "Tariff8 Reactive Energy (Lead)";//kVArh
                param[95] = "Tariff8 MD1";
                param[96] = "Tariff8 MD1 Time Stamp";
                param[97] = "Tariff8 MD2";
                param[98] = "Tariff8 MD2 Time Stamp";
                param[99] = "Tariff8 MD3";
                param[100] = "Tariff8 MD3 Time Stamp";
                param[101] = "Tariff8 Average Power Factor";
                param[102] = "Voltage Imbalance R Phase Tamper Counter";
                param[103] = "Voltage Imbalance Y Phase Tamper Counter";
                param[104] = "Voltage Imbalance B Phase Tamper Counter";
                param[105] = "Missing Potential R Phase Tamper Counter";
                param[106] = "Missing Potential Y Phase Tamper Counter";
                param[107] = "Missing Potential B Phase Tamper Counter";
                param[108] = "Low/Under Voltage R Phase Tamper Counter";
                param[109] = "Low/Under Voltage Y Phase Tamper Counter";
                param[110] = "Low/Under Voltage B Phase Tamper Counter";
                param[111] = "High/Over Voltage R Phase Tamper Counter";
                param[112] = "High/Over Voltage Y Phase Tamper Counter";
                param[113] = "High/Over Voltage B Phase Tamper Counter";
                param[114] = "CT Short Tamper Counter";
                param[115] = "CT Open R Phase Tamper Counter";
                param[116] = "CT Open Y Phase Tamper Counter";
                param[117] = "CT Open B Phase Tamper Counter";
                param[118] = "Current Without Voltage R Phase Tamper Counter";
                param[119] = "Current Without Voltage Y Phase Tamper Counter";
                param[120] = "Current Without Voltage B Phase Tamper Counter";
                param[121] = "LowPower Factor R Phase Tamper Counter";
                param[122] = "LowPower Factor Y Phase Tamper Counter";
                param[122] = "LowPower Factor B Phase Tamper Counter";
                param[123] = "OnePhase Neutral Absent Tamper Counter";
                param[124] = "Current Phase Reversal Tamper Counter";
                param[125] = "Voltage Phase Reversal Tamper Counter";
                param[126] = "Current Imbalance R Phase Tamper Counter";
                param[127] = "Current Imbalance YPhase Tamper Counter";
                param[128] = "Current Imbalance BPhase Tamper Counter";
                param[129] = "Current Reversal R Phase Tamper Counter";
                param[130] = "Current Reversal Y Phase Tamper Counter";
                param[131] = "Current Reversal B Phase Tamper Counter";
                param[132] = "Magnetic Influence Tamper Counter";
                param[133] = "Neutral Disturbance Tamper Counter";
                param[134] = "Front Cover Opening Tamper Counter";
                param[135] = "Terminal Cover Opening Tamper Counter";
                param[136] = "Billing Time Stamp";
                param[137] = "Billing Type";
            }
            #endregion 
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                ArrayList billingParameter = new ArrayList();
                billingParameter.Add("Billing Date");
                billingParameter.Add("System Power Factor for Billing Period");
                billingParameter.Add("Cumulative Active Energy");
                billingParameter.Add("Cumulative Active Energy Tarrif1");
                billingParameter.Add("Cumulative Active Energy Tarrif2");
                billingParameter.Add("Cumulative Active Energy Tarrif3");
                billingParameter.Add("Cumulative Active Energy Tarrif4");
                billingParameter.Add("Cumulative Active Energy Tarrif5");
                billingParameter.Add("Cumulative Active Energy Tarrif6");
                billingParameter.Add("Cumulative Active Energy Tarrif7");
                billingParameter.Add("Cumulative Active Energy Tarrif8");
                billingParameter.Add("Cumulative Reactive Energy Lag");
                billingParameter.Add("Cumulative Reactive Energy Lead");
                billingParameter.Add("Cumulative Apparent Energy");
                billingParameter.Add("Cumulative Apparent Energy Tarrif1");
                billingParameter.Add("Cumulative Apparent Energy Tarrif2");
                billingParameter.Add("Cumulative Apparent Energy Tarrif3");
                billingParameter.Add("Cumulative Apparent Energy Tarrif4");
                billingParameter.Add("Cumulative Apparent Energy Tarrif5");
                billingParameter.Add("Cumulative Apparent Energy Tarrif6");
                billingParameter.Add("Cumulative Apparent Energy Tarrif7");
                billingParameter.Add("Cumulative Apparent Energy Tarrif8");
                billingParameter.Add("MD Active ");
                billingParameter.Add("MD Active Date Time");
                billingParameter.Add("MD Active Tarrif1");
                billingParameter.Add("MD Active Tarrif1 Date Time");
                billingParameter.Add("MD Active Tarrif2");
                billingParameter.Add("MD Active Tarrif2 Date Time");
                billingParameter.Add("MD Active Tarrif3");
                billingParameter.Add("MD Active Tarrif3 Date Time");               
                billingParameter.Add("MD Active Tarrif4");
                billingParameter.Add("MD Active Tarrif4 Date Time");
                billingParameter.Add("MD Active Tarrif5");
                billingParameter.Add("MD Active Tarrif5 Date Time");
                billingParameter.Add("MD Active Tarrif6");
                billingParameter.Add("MD Active Tarrif6 Date Time");
                billingParameter.Add("MD Active Tarrif7");
                billingParameter.Add("MD Active Tarrif7 Date Time");
                billingParameter.Add("MD Active Tarrif8");
                billingParameter.Add("MD Active Tarrif8 Date Time");
                billingParameter.Add("MD Apparent");
                billingParameter.Add("MD Apparent Date Time");
                billingParameter.Add("MD Apparent Tarrif1");
                billingParameter.Add("MD Apparent Tarrif1 Date Time");
                billingParameter.Add("MD Apparent Tarrif2");
                billingParameter.Add("MD Apparent Tarrif2 Date Time");
                billingParameter.Add("MD Apparent Tarrif3");
                billingParameter.Add("MD Apparent Tarrif3 Date Time");
                billingParameter.Add("MD Apparent Tarrif4");
                billingParameter.Add("MD Apparent Tarrif4 Date Time");
                billingParameter.Add("MD Apparent Tarrif5");
                billingParameter.Add("MD Apparent Tarrif5 Date Time");
                billingParameter.Add("MD Apparent Tarrif6");
                billingParameter.Add("MD Apparent Tarrif6 Date Time");
                billingParameter.Add("MD Apparent Tarrif7");
                billingParameter.Add("MD Apparent Tarrif7 Date Time");
                billingParameter.Add("MD Apparent Tarrif8");
                billingParameter.Add("MD Apparent Tarrif8 Date Time");
                billingParameter.Add("Billing Type");
                if (isMPKWCL)
                {
                    billingParameter.Add("Cum Power Off Duration");
                    billingParameter.Add("Cum Tamper Count");
                }
                if (UtilityDetails.ShowPowerOffDurationInBilling)
                {
                    billingParameter.Add("Power Off Duration");
                }
                //if (!isMPKWCL)
                //{
                //    param = new string[59];
                //}
                //else
                //{
                //    param = new string[61];
                //}
                //param[0] = "Billing Date";
                //param[1] = "System Power Factor for Billing Period";
                //param[2] = "Cumulative Active Energy";
                //param[3] = "Cumulative Active Energy Tarrif1";
                //param[4] = "Cumulative Active Energy Tarrif2";
                //param[5] = "Cumulative Active Energy Tarrif3";
                //param[6] = "Cumulative Active Energy Tarrif4";
                //param[7] = "Cumulative Active Energy Tarrif5";
                //param[8] = "Cumulative Active Energy Tarrif6";
                //param[9] = "Cumulative Active Energy Tarrif7";
                //param[10] = "Cumulative Active Energy Tarrif8";
                //param[11] = "Cumulative Reactive Energy Lag";
                //param[12] = "Cumulative Reactive Energy Lead";
                //param[13] = "Cumulative Apparent Energy";
                //param[14] = "Cumulative Apparent Energy Tarrif1";
                //param[15] = "Cumulative Apparent Energy Tarrif2";
                //param[16] = "Cumulative Apparent Energy Tarrif3";
                //param[17] = "Cumulative Apparent Energy Tarrif4";
                //param[18] = "Cumulative Apparent Energy Tarrif5";
                //param[19] = "Cumulative Apparent Energy Tarrif6";
                //param[20] = "Cumulative Apparent Energy Tarrif7";
                //param[21] = "Cumulative Apparent Energy Tarrif8";
                //param[22] = "MD Active Energy";
                //param[23] = "MD Active Energy Date Time";
                //param[24] = "MD Active Energy Tarrif1";
                //param[25] = "MD Active Energy Tarrif1 Date Time";
                //param[26] = "MD Active Energy Tarrif2";
                //param[27] = "MD Active Energy Tarrif2 Date Time";
                //param[28] = "MD Active Energy Tarrif3";
                //param[29] = "MD Active Energy Tarrif3 Date Time";
                //param[30] = "MD Active Energy Tarrif4";
                //param[31] = "MD Active Energy Tarrif4 Date Time";
                //param[32] = "MD Active Energy Tarrif5";
                //param[33] = "MD Active Energy Tarrif5 Date Time";
                //param[34] = "MD Active Energy Tarrif6";
                //param[35] = "MD Active Energy Tarrif6 Date Time";
                //param[36] = "MD Active Energy Tarrif7";
                //param[37] = "MD Active Energy Tarrif7 Date Time";
                //param[38] = "MD Active Energy Tarrif8";
                //param[39] = "MD Active Energy Tarrif8 Date Time";
                //param[40] = "MD Active";
                //param[41] = "MD Active Date Time";//kVA
                //param[42] = "MD Active Tarrif1";//kVA
                //param[43] = "MD Active Tarrif1 Date Time";
                //param[44] = "MD Active Tarrif2";
                //param[45] = "MD Active Tarrif2 Date Time";
                //param[46] = "MD Active Tarrif3";
                //param[47] = "MD Active Tarrif3 Date Time";
                //param[48] = "MD Active Tarrif4";
                //param[49] = "MD Active Tarrif4 Date Time";
                //param[50] = "MD Active Tarrif5";
                //param[51] = "MD Active Tarrif5 Date Time";
                //param[52] = "MD Active Tarrif6";
                //param[53] = "MD Active Tarrif6 Date Time";
                //param[54] = "MD Active Tarrif7";
                //param[55] = "MD Active Tarrif7 Date Time";
                //param[56] = "MD Active Tarrif8";
                //param[57] = "MD Active Tarrif8 Date Time";
                //param[58] = "Billing Type";
                //if (isMPKWCL)
                //{
                //    param[59] = "Cum Power Off Duration";
                //    param[60] = "Cum Tamper Count";
                //}
                param = new string[billingParameter.Count];
                billingParameter.CopyTo(param);
            }

            return param;
        }
        public string[] GetBillingDBColumnName()
        {
            string[] param = null;            
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[138];
                param[0] = "A.CTRatio,";
                param[1] = "A.CumulativeEnergyKWH,";
                param[2] = "A.CumulativeEnergyKVARHLag,";
                param[3] = "A.CumulativeEnergyKVARHLead,";
                param[4] = "A.CumulativeEnergyKVAH,";
                param[5] = "A.CumulativeMD1,";
                param[6] = "A.CumulativeMD1TimeStamp,";
                param[7] = "A.CumulativeMD2,";
                param[8] = "A.CumulativeMD2TimeStamp,";
                param[9] = "A.CumulativeMD3,";
                param[10] = "A.CumulativeMD3TimeStamp,";
                param[11] = "A.AveragePowerFactor,";
                param[12] = "A.PowerOnHours,";
                param[13] = "A.LoadFactor,";
                param[14] = "B.Tariff1_kWh,";
                param[15] = "B.Tariff1_kVAh,";
                param[16] = "B.Tariff1_kVARh_lag,";
                param[17] = "B.Tariff1_kVARh_lead,";
                param[18] = "B.Tariff1_MD1,";
                param[19] = "B.Tariff1_MD1_TimeStamp,";
                param[20] = "B.Tariff1_MD2,";
                param[21] = "B.Tariff1_MD2_TimeStamp,";
                param[22] = "B.Tariff1_MD3,";
                param[23] = "B.Tariff1_MD3_TimeStamp,";
                param[24] = "B.Tariff1_Aver_PF,";
                param[25] = "B.Tariff2_kWh,";
                param[26] = "B.Tariff2_kVAh,";
                param[27] = "B.Tariff2_kVARh_lag,";
                param[28] = "B.Tariff2_kVARh_lead,";
                param[29] = "B.Tariff2_MD1,";
                param[30] = "B.Tariff2_MD1_TimeStamp,";
                param[31] = "B.Tariff2_MD2,";
                param[32] = "B.Tariff2_MD2_TimeStamp,";
                param[33] = "B.Tariff2_MD3,";
                param[34] = "B.Tariff2_MD3_TimeStamp,";
                param[35] = "B.Tariff2_Aver_PF,";
                param[36] = "B.Tariff3_kWh,";
                param[37] = "B.Tariff3_kVAh,";
                param[38] = "B.Tariff3_kVARh_lag,";
                param[39] = "B.Tariff3_kVARh_lead,";
                param[40] = "B.Tariff3_MD1,";
                param[41] = "B.Tariff3_MD1_TimeStamp,";
                param[42] = "B.Tariff3_MD2,";
                param[43] = "B.Tariff3_MD2_TimeStamp,";
                param[44] = "B.Tariff3_MD3,";
                param[45] = "B.Tariff3_MD3_TimeStamp,";
                param[46] = "B.Tariff3_Aver_PF,";
                param[47] = "B.Tariff4_kWh,";
                param[48] = "B.Tariff4_kVAh,";
                param[49] = "B.Tariff4_kVARh_lag,";
                param[50] = "B.Tariff4_kVARh_lead,";
                param[51] = "B.Tariff4_MD1,";
                param[52] = "B.Tariff4_MD1_TimeStamp,";
                param[53] = "B.Tariff4_MD2,";
                param[54] = "B.Tariff4_MD2_TimeStamp,";
                param[55] = "B.Tariff4_MD3,";
                param[56] = "B.Tariff4_MD3_TimeStamp,";
                param[57] = "B.Tariff4_Aver_PF,";
                param[58] = "B.Tariff5_kWh,";
                param[59] = "B.Tariff5_kVAh,";
                param[60] = "B.Tariff5_kVARh_lag,";
                param[61] = "B.Tariff5_kVARh_lead,";
                param[62] = "B.Tariff5_MD1,";
                param[63] = "B.Tariff5_MD1_TimeStamp,";
                param[64] = "B.Tariff5_MD2,";
                param[65] = "B.Tariff5_MD2_TimeStamp,";
                param[66] = "B.Tariff5_MD3,";
                param[67] = "B.Tariff5_MD3_TimeStamp,";
                param[68] = "B.Tariff5_Aver_PF,";
                param[69] = "B.Tariff6_kWh,";
                param[70] = "B.Tariff6_kVAh,";
                param[71] = "B.Tariff6_kVARh_lag,";
                param[72] = "B.Tariff6_kVARh_lead,";
                param[73] = "B.Tariff6_MD1,";
                param[74] = "B.Tariff6_MD1_TimeStamp,";
                param[75] = "B.Tariff6_MD2,";
                param[76] = "B.Tariff6_MD2_TimeStamp,";
                param[77] = "B.Tariff6_MD3,";
                param[78] = "B.Tariff6_MD3_TimeStamp,";
                param[79] = "B.Tariff6_Aver_PF,";
                param[80] = "B.Tariff7_kWh,";
                param[81] = "B.Tariff7_kVAh,";
                param[82] = "B.Tariff7_kVARh_lag,";
                param[83] = "B.Tariff7_kVARh_lead,";
                param[84] = "B.Tariff7_MD1,";
                param[85] = "B.Tariff7_MD1_TimeStamp,";
                param[86] = "B.Tariff7_MD2,";
                param[87] = "B.Tariff7_MD2_TimeStamp,";
                param[88] = "B.Tariff7_MD3,";
                param[89] = "B.Tariff7_MD3_TimeStamp,";
                param[90] = "B.Tariff7_Aver_PF,";
                param[91] = "B.Tariff8_kWh,";
                param[92] = "B.Tariff8_kVAh,";
                param[93] = "B.Tariff8_kVARh_lag,";
                param[94] = "B.Tariff8_kVARh_lead,";
                param[95] = "B.Tariff8_MD1,";
                param[96] = "B.Tariff8_MD1_TimeStamp,";
                param[97] = "B.Tariff8_MD2,";
                param[98] = "B.Tariff8_MD2_TimeStamp,";
                param[99] = "B.Tariff8_MD3,";
                param[100] = "B.Tariff8_MD3_TimeStamp,";
                param[101] = "B.Tariff8_Aver_PF,";
                param[102] = "C.VoltageImbalanceRPhaseTamperCounter,";
                param[103] = "C.VoltageImbalanceYPhaseTamperCounter,";
                param[104] = "C.VoltageImbalanceBPhaseTamperCounter,";
                param[105] = "C.MissingPotentialRPhaseTamperCounter,";
                param[106] = "C.MissingPotentialYPhaseTamperCounter,";
                param[107] = "C.MissingPotentialBPhaseTamperCounter,";
                param[108] = "C.LowUnderVoltageRPhaseTamperCounter,";
                param[109] = "C.LowUnderVoltageYPhaseTamperCounter,";
                param[110] = "C.LowUnderVoltageBPhaseTamperCounter,";
                param[111] = "C.HighOverVoltageRPhaseTamperCounter,";
                param[112] = "C.HighOverVoltageYPhaseTamperCounter,";
                param[113] = "C.HighOverVoltageBPhaseTamperCounter,";
                param[114] = "C.CTShortTamperCounter,";
                param[115] = "C.CTOpenRPhaseTamperCounter,";
                param[116] = "C.CTOpenYPhaseTamperCounter,";
                param[117] = "C.CTOpenBPhaseTamperCounter,";
                param[118] = "C.CurrentWithoutVoltageRPhaseTamperCounter,";
                param[119] = "C.CurrentWithoutVoltageYPhaseTamperCounter,";
                param[120] = "C.CurrentWithoutVoltageBPhaseTamperCounter,";
                param[121] = "C.LowPowerFactorRPhaseTamperCounter,";
                param[122] = "C.LowPowerFactorYPhaseTamperCounter,";
                param[122] = "C.LowPowerFactorBPhaseTamperCounter,";
                param[123] = "C.OnePhaseNeutralAbsentTamperCounter,";
                param[124] = "C.CurrentPhaseReversalTamperCounter,";
                param[125] = "C.VoltagePhaseReversalTamperCounter,";
                param[126] = "C.CurrentImbalanceRPhaseTamperCounter,";
                param[127] = "C.CurrentImbalanceYPhaseTamperCounter,";
                param[128] = "C.CurrentImbalanceBPhaseTamperCounter,";
                param[129] = "C.CurrentReversalRPhaseTamperCounter,";
                param[130] = "C.CurrentReversalYPhaseTamperCounter,";
                param[131] = "C.CurrentReversalBPhaseTamperCounter,";
                param[132] = "C.MagneticInfluenceTamperCounter,";
                param[133] = "C.NeutralDisturbanceTamperCounter,";
                param[134] = "C.FrontCoverOpeningTamperCounter,";
                param[135] = "C.TerminalCoverOpeningTamperCounter,";
                param[136] = "C.BillingTimeStamp,";
                param[137] = "B.BillingResetType,";
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                ArrayList billingParameter = new ArrayList();
                billingParameter.Add("A.BillingDate,");
                billingParameter.Add("A.SystemPowerFactorforBillingPeriod,");
                billingParameter.Add("A.CumulativeEnergykWhTZ0,");
                billingParameter.Add("A.CumulativeEnergykWhTZ1,");
                billingParameter.Add("A.CumulativeEnergykWhTZ2,");
                billingParameter.Add("A.CumulativeEnergykWhTZ3,");
                billingParameter.Add("A.CumulativeEnergykWhTZ4,");
                billingParameter.Add("A.CumulativeEnergykWhTZ5,");
                billingParameter.Add("A.CumulativeEnergykWhTZ6,");
                billingParameter.Add("A.CumulativeEnergykWhTZ7,");
                billingParameter.Add("A.CumulativeEnergykWhTZ8,");
                billingParameter.Add("A.CumulativeEnergykvarhLag,");
                billingParameter.Add("A.CumulativeEnergykvarhLead,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ0,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ1,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ2,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ3,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ4,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ5,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ6,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ7,");
                billingParameter.Add("A.CumulativeEnergykVAhTZ8,");
                billingParameter.Add("A.MDkWTZ0,");
                billingParameter.Add("A.MDkWDateTimeTZ0,");
                billingParameter.Add("A.MDkWTZ1,");
                billingParameter.Add("A.MDkWDateTimeTZ1,");
                billingParameter.Add("A.MDkWTZ2,");
                billingParameter.Add("A.MDkWDateTimeTZ2,");
                billingParameter.Add("A.MDkWTZ3,");
                billingParameter.Add("A.MDkWDateTimeTZ3,");
                billingParameter.Add("A.MDkWTZ4,");
                billingParameter.Add("A.MDkWDateTimeTZ4,");
                billingParameter.Add("A.MDkWTZ5,");
                billingParameter.Add("A.MDkWDateTimeTZ5,");
                billingParameter.Add("A.MDkWTZ6,");
                billingParameter.Add("A.MDkWDateTimeTZ6,");
                billingParameter.Add("A.MDkWTZ7,");
                billingParameter.Add("A.MDkWDateTimeTZ7,");
                billingParameter.Add("A.MDkWTZ8,");
                billingParameter.Add("A.MDkWDateTimeTZ8,");               
                billingParameter.Add("A.MDkVATZ0,");
                billingParameter.Add("A.MDkVADateTimeTZ0,");
                billingParameter.Add("A.MDkVATZ1,");
                billingParameter.Add("A.MDkVADateTimeTZ1,");
                billingParameter.Add("A.MDkVATZ2,");
                billingParameter.Add("A.MDkVADateTimeTZ2,");
                billingParameter.Add("A.MDkVATZ3,");
                billingParameter.Add("A.MDkVADateTimeTZ3,");
                billingParameter.Add("A.MDkVATZ4,");
                billingParameter.Add("A.MDkVADateTimeTZ4,");
                billingParameter.Add("A.MDkVATZ5,");
                billingParameter.Add("A.MDkVADateTimeTZ5,");
                billingParameter.Add("A.MDkVATZ6,");
                billingParameter.Add("A.MDkVADateTimeTZ6,");
                billingParameter.Add("A.MDkVATZ7,");
                billingParameter.Add("A.MDkVADateTimeTZ7,");
                billingParameter.Add("A.MDkVATZ8,");
                billingParameter.Add("A.MDkVADateTimeTZ8,");
                billingParameter.Add("A.BillingResetType,");
                //if (isMPKWCL)
                //{
                //    billingParameter.Add("A.CumPowerOffDuration,");
                    billingParameter.Add("A.CumTamperCount,");
                //}
                if (UtilityDetails.ShowPowerOffDurationInBilling)
                {
                    billingParameter.Add("A.PowerOffDuration,");
                }
                param = new string[billingParameter.Count];
                billingParameter.CopyTo(param);
                
                //if (!isMPKWCL)
                //{
                //    param = new string[59];
                //}
                //else
                //{
                //    param = new string[61];
                //}
                
                //param[0] = "A.BillingDate,";
                //param[1] = "A.SystemPowerFactorforBillingPeriod,";
                //param[2] = "A.CumulativeEnergykWhTZ0,";
                //param[3] = "A.CumulativeEnergykWhTZ1,";
                //param[4] = "A.CumulativeEnergykWhTZ2,";
                //param[5] = "A.CumulativeEnergykWhTZ3,";
                //param[6] = "A.CumulativeEnergykWhTZ4,";
                //param[7] = "A.CumulativeEnergykWhTZ5,";
                //param[8] = "A.CumulativeEnergykWhTZ6,";
                //param[9] = "A.CumulativeEnergykWhTZ7,";
                //param[10] = "A.CumulativeEnergykWhTZ8,";
                //param[11] = "A.CumulativeEnergykvarhLag,";
                //param[12] = "A.CumulativeEnergykvarhLead,";
                //param[13] = "A.CumulativeEnergykVAhTZ0,";
                //param[14] = "A.CumulativeEnergykVAhTZ1,";
                //param[15] = "A.CumulativeEnergykVAhTZ2,";
                //param[16] = "A.CumulativeEnergykVAhTZ3,";
                //param[17] = "A.CumulativeEnergykVAhTZ4,";
                //param[18] = "A.CumulativeEnergykVAhTZ5,";
                //param[19] = "A.CumulativeEnergykVAhTZ6,";
                //param[20] = "A.CumulativeEnergykVAhTZ7,";
                //param[21] = "A.CumulativeEnergykVAhTZ8,";
                //param[22] = "A.MDkWTZ0,";
                //param[23] = "A.MDkWDateTimeTZ0,";
                //param[24] = "A.MDkWTZ1,";
                //param[25] = "A.MDkWDateTimeTZ1,";
                //param[26] = "A.MDkWTZ2,";
                //param[27] = "A.MDkWDateTimeTZ2,";
                //param[28] = "A.MDkWTZ3,";
                //param[29] = "A.MDkWDateTimeTZ3,";
                //param[30] = "A.MDkWTZ4,";
                //param[31] = "A.MDkWDateTimeTZ4,";
                //param[32] = "A.MDkWTZ5,";
                //param[33] = "A.MDkWDateTimeTZ5,";
                //param[34] = "A.MDkWTZ6,";
                //param[35] = "A.MDkWDateTimeTZ6,";
                //param[36] = "A.MDkWTZ7,";
                //param[37] = "A.MDkWDateTimeTZ7,";
                //param[38] = "A.MDkWTZ8,";
                //param[39] = "A.MDkWDateTimeTZ8,";
                //param[40] = "A.MDkVATZ0,";
                //param[41] = "A.MDkVADateTimeTZ0,";
                //param[42] = "A.MDkVATZ1,";
                //param[43] = "A.MDkVADateTimeTZ1,";
                //param[44] = "A.MDkVATZ2,";
                //param[45] = "A.MDkVADateTimeTZ2,";
                //param[46] = "A.MDkVATZ3,";
                //param[47] = "A.MDkVADateTimeTZ3,";
                //param[48] = "A.MDkVATZ4,";
                //param[49] = "A.MDkVADateTimeTZ4,";
                //param[50] = "A.MDkVATZ5,";
                //param[51] = "A.MDkVADateTimeTZ5,";
                //param[52] = "A.MDkVATZ6,";
                //param[53] = "A.MDkVADateTimeTZ6,";
                //param[54] = "A.MDkVATZ7,";
                //param[55] = "A.MDkVADateTimeTZ7,";
                //param[56] = "A.MDkVATZ8,";
                //param[57] = "A.MDkVADateTimeTZ8,";
                //param[58] = "A.BillingResetType,";
                //if (isMPKWCL)
                //{
                //    param[59] = "A.CumPowerOffDuration,";
                //    param[60] = "A.CumTamperCount,";
                //}

            }
            return param;
        }
        public string[] GetGeneralDisplayColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[26];
                param[0] = "Meter Number";
                param[1] = "Meter Date Time";
                param[2] = "Error Code";
                param[3] = "Meter Constant";
                param[4] = "Firmware Version";
                param[5] = "CT Ratio";
                param[6] = "Voltage Phase Sequence";
                param[7] = "Total Active Energy";
                param[8] = "Cumulative MD1";
                param[9] = "Cumulative MD2";
                param[10] = "Cumulative MD3";
                param[11] = "Rising Demand KW";
                param[12] = "Elapsed Time KW";
                param[13] = "Rising Demand KVA";
                param[14] = "Elapsed Time KVA";
                param[15] = "Total Power On Hours";
                param[16] = "Current Month Power On Hours";
                param[17] = "MDReset Counter";
                param[18] = "Readout Counter";
                param[19] = "Programming Counter";
                param[20] = "CT Ratio Programming Counter";
                param[21] = "Latest Tamper Occurrence Id";
                param[22] = "Occurrence Time";
                param[23] = "Latest Tamper Restoration Id";
                param[24] = "Battery Mode Power On Hour";
                param[25] = "Restoration Time";
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                param =  new string[14];                
                param[0] = "Meter Serial Number";
                param[1] = "Manufacturer name";
                param[2] = "Firmware Version";
                param[3] = "Meter type (3P-3W / 3P-4W)";
                param[4] = "Internal  CT ratio";
                param[5] = "Internal PT ratio";
                param[6] = "Year Of manufacture";

                param[8] =  "Internal Firmware Version";
                param[9] =  "Voltage Rating";
                param[10] = "Basic Current Rating";
                param[11] = "Maximum Current Rating";
                param[12] = "Communication Type";
                param[13] = "Release Type";

                //if (UtilityDetails.ShowMeterModelNo)
                //{
                    param[7] = "Model - Type";
                //}
            }
            return param;
        }
        public string[] GetGeneralDBColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[26];
                param[0] = "MeterID,";
                param[1] = "MeterDateTime,";
                param[2] = "ErrorCode,";
                param[3] = "MeterConstant,";
                param[4] = "FirmwareVersion,";
                param[5] = "CTRatio,";
                param[6] = "VoltagePhaseSequence,";
                param[7] = "TotalActiveEnergy,";
                param[8] = "CumulativeMD1,";
                param[9] = "CumulativeMD2,";
                param[10] = "CumulativeMD3,";
                param[11] = "RisingDemandKW,";
                param[12] = "ElapsedTimeKW,";
                param[13] = "RisingDemandKVA,";
                param[14] = "ElapsedTimeKVA,";
                param[15] = "TotalPowerOnHours,";
                param[16] = "CurrentMonthPowerOnHours,";
                param[17] = "MDResetCounter,";
                param[18] = "ReadoutCounter,";
                param[19] = "ProgrammingCounter,";
                param[20] = "CTRatioProgrammingCounter,";
                param[21] = "LatestTamperOccurrenceID,";
                param[22] = "OccurrenceTime,";
                param[23] = "LatestTamperRestorationID,";
                param[24] = "BateryModePowerOnHour,";
                param[25] = "RestorationTime,";
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                param = new string[14];   
                param[0] = "A.meterSerialNumber,";
                param[1] = "A.manufacturername,";
                param[2] = "A.firmwareVersionformeter,";
                param[3] = "A.metertype,";
                param[4] = "A.internalCTratio,";
                param[5] = "A.internalPTratio,";
                param[6] = "A.meteryearofmanufacture,";

                param[8] = "A.InternalFirmwareVersion,";
                param[9] = "A.VoltageRating,";
                param[10]= "A.BasicCurrentRating,";
                param[11] = "A.MaximumCurrentRating,";
                param[12] = "A.CommunicationType,";
                param[13] = "A.ReleaseType,";
                
                    param[7] = "A.metermodelno,";
                //}

            }
            return param;
        }
        public string[] GetInstantDisplayColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[19];
                param[0] = "Meter Number";
                param[1] = "Meter Date Time";
                param[2] = "Voltage R Phase";
                param[3] = "Voltage Y Phase";
                param[4] = "Voltage B Phase";
                param[5] = "Current R Phase";
                param[6] = "Current Y Phase";
                param[7] = "Current B Phase";
                param[8] = "Instant Active Power";
                param[9] = "Instant Reactive Lag Power";
                param[10] = "Instant Reactive Lead Power";
                param[11] = "Instant Apparent Power";
                param[12] = "Total Power Factor";
                param[13] = "Power Factor R Phase";
                param[14] = "Power Factor Y Phase";
                param[15] = "Power Factor B Phase";
                param[16] = "Average Power Factor";
                param[17] = "Frequency";
                param[18] = "Total Fundamental Active Energy";
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                ArrayList insParameter = new ArrayList();
                insParameter.Add("Real Time Clock Date and Time");
                insParameter.Add("Current IR");
                insParameter.Add("Current IY");
                insParameter.Add("Current IB");
                insParameter.Add("Voltage VRN");
                insParameter.Add("Voltage VYN");
                insParameter.Add("Voltage VBN");
                insParameter.Add("Signed Power Factor R phase");
                insParameter.Add("Signed Power Factor Y phase");
                insParameter.Add("Signed Power Factor B phase");
                insParameter.Add("Three Phase Power Factor PF");
                insParameter.Add("Frequency");
                insParameter.Add("Apparent Power");
                // Active Power Label updated
                if (isPUMA)
                   insParameter.Add("Active Power (ABS)");
                else
                insParameter.Add("Signed Active Power (+ Forward; Reverse)");
                insParameter.Add("Signed Reactive Power (+ Lag; Lead)");
                insParameter.Add("Number of Power Failures");
                insParameter.Add("Cumulative Power Failure Duration");
                insParameter.Add("Cumulative Tamper Count");
                insParameter.Add("Cumulative Billing  Count");
                insParameter.Add("Cumulative Programming Count");
                insParameter.Add("Billing Date");
                insParameter.Add("Cumulative Active Energy");//Energy kWh
                insParameter.Add("Cumulative Reactive Energy Lag");//Energy kvarh
                insParameter.Add("Cumulative Reactive Energy Lead");
                insParameter.Add("Cumulative Apparent Energy");//Energy kVAh
                insParameter.Add("Maximum Active Demand");
                insParameter.Add("Maximum Active Demand DateTime");
                insParameter.Add("Maximum Apparent Demand");
                insParameter.Add("Maximum Apparent Demand DateTime");
                if (UtilityDetails.ShowCumulativeMDKWKVA)
                {
                    insParameter.Add("Cumulative Maximum Active Demand");
                    insParameter.Add("Cumulative Maximum Apparent Demand");
                }
                if (UtilityDetails.ShowCumulativeExportEnergyKWH)
                {
                    insParameter.Add("Cumulative Export Energy");
                }                                                            
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.KSEB.ToString())
                {
                    insParameter.Add("Reverse kWh");
                    insParameter.Add("Reverse kVAh");
                    insParameter.Add("Reverse kVArh - lag");
                    insParameter.Add("Reverse kVArh - lead");
                    insParameter.Add("Present TOD Zone");
                    insParameter.Add("Cumulative kWh with high Resolution - T1");
                    insParameter.Add("Cumulative kWh with high Resolution - T2");
                    insParameter.Add("Cumulative kWh with high Resolution - T3");
                    insParameter.Add("Cumulative kWh with high Resolution - T4");
                    insParameter.Add("Cumulative kWh with high Resolution - T5");
                    insParameter.Add("Cumulative kWh with high Resolution - T6");
                    insParameter.Add("Cumulative kWh with high Resolution - T7");
                    insParameter.Add("Cumulative kWh with high Resolution - T8");
                }
                param = new string[insParameter.Count];
                insParameter.CopyTo(param);
                

            }
            return param;
        }
        public string[] GetInstantDBColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[19];
                param[0] = "MeterID,";
                param[1] = "MeterDateTime,";
                param[2] = "VoltageRPhase,";
                param[3] = "VoltageYPhase,";
                param[4] = "VoltageBPhase,";
                param[5] = "CurrentRPhase,";
                param[6] = "CurrentYPhase,";
                param[7] = "CurrentBPhase,";
                param[8] = "InstantActivepower,";
                param[9] = "InstantReactiveLagPower,";
                param[10] = "InstantReactiveLeadPower,";
                param[11] = "InstantApparentPower,";
                param[12] = "TotalPowerFactor,";
                param[13] = "PowerFactorRPhase,";
                param[14] = "PowerFactorYPhase,";
                param[15] = "PowerFactorBPhase,";
                param[16] = "AveragePowerFactor,";
                param[17] = "Frequency,";
                param[18] = "TotalFundamentalActiveEnergy,";
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
            //    int instParamCount;
            //    if (isPUMA)
            //    { instParamCount = 31; }
            //    else
            //    { instParamCount = 29; }
            //    //VBM - add export energy in instant profile
            //    if (UtilityDetails.ShowCumulativeExportEnergyKWH)
            //    {
            //        instParamCount = instParamCount + 1;
            //    }
            //    param = new string[instParamCount];
            //    param[0] = "'Real Time Clock - Date and Time',";
            //    param[1] = "'Current - IR',";
            //    param[2] = "'Current - IY',";
            //    param[3] = "'Current - IB',";
            //    param[4] = "'Voltage – VRN',";
            //    param[5] = "'Voltage – VYN',";
            //    param[6] = "'Voltage – VBN',";
            //    param[7] = "'Signed Power Factor - R phase',";
            //    param[8] = "'Signed Power Factor - Y phase',";
            //    param[9] = "'Signed Power Factor - B phase',";
            //    param[10] = "'Three Phase Power Factor - PF',";
            //    param[11] = "'Frequency',";
            //    param[12] = "'Apparent Power - kVA',";
            //    param[13] = "'Signed Active Power - kW (+ Forward - Reverse)',";
            //    param[14] = "'Signed Reactive Power - kvar (+ Lag - Lead)',";
            //    param[15] = "'Number of Power - Failures',";
            //    param[16] = "'Cumulative Power-Failure Duration',";
            //    param[17] = "'Cumulative Tamper Count',";
            //    param[18] = "'Cumulative Billing Count',";
            //    param[19] = "'Cumulative Programming Count',";
            //    param[20] = "'Billing Date',";
            //    param[21] = "'Cumulative Energy - kWh',";
            //    param[22] = "'Cumulative Energy - kvarh - lag',";
            //    param[23] = "'Cumulative Energy - kvarh - lead',";
            //    param[24] = "'Cumulative Energy - kVAh',";
            //    param[25] = "'Maximum Demand – kW',";
            //    param[26] = "'Maximum Demand – kW DateTime',";
            //    param[27] = "'Maximum Demand – kVA',";
            //    param[28] = "'Maximum Demand – kVA DateTime',";
            //    if(isPUMA)
            //    {
            //        param[29] = "'Cumulative Maximum Demand - kW',";
            //        param[30] = "'Cumulative Maximum Demand - kVA',";
            //        if (UtilityDetails.ShowCumulativeExportEnergyKWH)
            //        {
            //            param[31] = "'Cumulative Export Energy - kWh',";
            //        }
            //    }
            //    else
            //    {
            //        if (UtilityDetails.ShowCumulativeExportEnergyKWH)
            //        {
            //            param[29] = "'Cumulative Export Energy - kWh',";
            //        }
            //    }
                ArrayList insParameter = new ArrayList();                
                insParameter.Add("'Real Time Clock - Date and Time',");
                insParameter.Add("'Current - IR',");
                insParameter.Add("'Current - IY',");
                insParameter.Add("'Current - IB',");
                insParameter.Add("'Voltage – VRN',");
                insParameter.Add("'Voltage – VYN',");
                insParameter.Add("'Voltage – VBN',");
                insParameter.Add("'Signed Power Factor - R phase',");
                insParameter.Add("'Signed Power Factor - Y phase',");
                insParameter.Add("'Signed Power Factor - B phase',");
                insParameter.Add("'Three Phase Power Factor - PF',");
                insParameter.Add("'Frequency',");
                insParameter.Add("'Apparent Power - kVA',");
                insParameter.Add("'Signed Active Power - kW (+ Forward - Reverse)',");
                insParameter.Add("'Signed Reactive Power - kvar (+ Lag - Lead)',");
                insParameter.Add("'Number of Power - Failures',");
                insParameter.Add("'Cumulative Power-Failure Duration',");
                insParameter.Add("'Cumulative Tamper Count',");
                insParameter.Add("'Cumulative Billing Count',");
                insParameter.Add("'Cumulative Programming Count',");
                insParameter.Add("'Billing Date',");
                insParameter.Add("'Cumulative Energy - kWh',");
                insParameter.Add("'Cumulative Energy - kvarh - lag',");
                insParameter.Add("'Cumulative Energy - kvarh - lead',");
                insParameter.Add("'Cumulative Energy - kVAh',");
                insParameter.Add("'Maximum Demand – kW',");
                insParameter.Add("'Maximum Demand – kW DateTime',");
                insParameter.Add("'Maximum Demand – kVA',");
                insParameter.Add("'Maximum Demand – kVA DateTime',");
                if (UtilityDetails.ShowCumulativeMDKWKVA)
                {
                    insParameter.Add("'Cumulative Maximum Demand - kW',");
                    insParameter.Add("'Cumulative Maximum Demand - kVA',");
                }
                if (UtilityDetails.ShowCumulativeExportEnergyKWH)
                {
                    insParameter.Add("'Cumulative Export Energy - kWh',");
                }               
                
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.KSEB.ToString())
                {
                    insParameter.Add("'Reverse kWh',");
                    insParameter.Add("'Reverse kVAh',");
                    insParameter.Add("'Reverse kVArh - lag',");
                    insParameter.Add("'Reverse kVArh - lead',");
                    insParameter.Add("'Present TOD Zone',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T1',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T2',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T3',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T4',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T5',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T6',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T7',");
                    insParameter.Add("'Cumulative kWh with high Resolution - T8',");
                }
                param = new string[insParameter.Count];
                insParameter.CopyTo(param);
            }
            return param;
        }

        public string[] GetTamperDBColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[66];
                param[0] = "A.VoltageImbalanceRPhaseTamperCounter,";
                param[1] = "A.VoltageImbalanceYPhaseTamperCounter,";
                param[2] = "A.VoltageImbalanceBPhaseTamperCounter,";
                param[3] = "A.MissingPotentialRPhaseTamperCounter,";
                param[4] = "A.MissingPotentialYPhaseTamperCounter,";
                param[5] = "A.MissingPotentialBPhaseTamperCounter,";
                param[6] = "A.LowUnderVoltageRPhaseTamperCounter,";
                param[7] = "A.LowUnderVoltageYPhaseTamperCounter,";
                param[8] = "A.LowUnderVoltageBPhaseTamperCounter,";
                param[9] = "A.HighOverVoltageRPhaseTamperCounter,";
                param[10] = "A.HighOverVoltageYPhaseTamperCounter,";
                param[11] = "A.HighOverVoltageBPhaseTamperCounter,";
                param[12] = "A.CTShortTamperCounter,";
                param[13] = "A.CTOpenRPhaseTamperCounter,";
                param[14] = "A.CTOpenYPhaseTamperCounter,";
                param[15] = "A.CTOpenBPhaseTamperCounter,";
                param[16] = "A.CurrentWithoutVoltageRPhaseTamperCounter,";
                param[17] = "A.CurrentWithoutVoltageYPhaseTamperCounter,";
                param[18] = "A.CurrentWithoutVoltageBPhaseTamperCounter,";
                param[19] = "A.LowPowerFactorRPhaseTamperCounter,";
                param[20] = "A.LowPowerFactorYPhaseTamperCounter,";
                param[21] = "A.LowPowerFactorBPhaseTamperCounter,";
                param[22] = "A.OnePhaseNeutralAbsentTamperCounter,";
                param[23] = "A.VoltagePhaseReversalTamperCounter,";
                param[24] = "A.CurrentImbalanceRPhaseTamperCounter,";
                param[25] = "A.CurrentImbalanceYPhaseTamperCounter,";
                param[26] = "A.CurrentImbalanceBPhaseTamperCounter,";
                param[27] = "A.CurrentReversalRPhaseTamperCounter,";
                param[28] = "A.CurrentReversalYPhaseTamperCounter,";
                param[29] = "A.CurrentReversalBPhaseTamperCounter,";
                param[30] = "A.MagneticInfluenceTamperCounter,";
                param[31] = "A.NeutralDisturbanceTamperCounter,";
                param[32] = "A.FrontCoverOpeningTamperCounter,";
                param[33] = "A.TerminalCoverOpeningTamperCounter,";
                param[34] = "A.BillingTimeStamp,";
                param[35] = "B.TotalTamperCounter,";
                param[36] = "B.PowerOnOffCounter,";
                param[37] = "B.LowLoadCounter,";
                param[38] = "B.OverLoadCounter,";
                param[39] = "C.TamperCode,";
                param[40] = "C.TamperOccurredTime,";
                param[41] = "C.TamperRestoredTime,";
                param[42] = "C.RVoltageOccurred,";
                param[43] = "C.YVoltageOccurred,";
                param[44] = "C.BVoltageOccurred,";
                param[45] = "C.RCurrentOccurred,";
                param[46] = "C.YCurrentOccurred,";
                param[47] = "C.BCurrentOccurred,";
                param[48] = "C.RPFOccurred,";
                param[49] = "C.YPFOccurred,";
                param[50] = "C.BPFOccurred,";
                param[51] = "C.TotalPFOccurred,";
                param[52] = "C.kWhOccurred,";
                param[53] = "C.kVAhOccurred,";
                param[54] = "C.RVoltageRestored,";
                param[55] = "C.YVoltageRestored,";
                param[56] = "C.BVoltageRestored,";
                param[57] = "C.RCurrentRestored,";
                param[58] = "C.YCurrentRestored,";
                param[59] = "C.BCurrentRestored,";
                param[60] = "C.RPFRestored,";
                param[61] = "C.YPFRestored,";
                param[62] = "C.BPFRestored,";
                param[63] = "C.TotalPFRestored,";
                param[64] = "C.kWhRestored,";
                param[65] = "C.kVAhRestored,";
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                //param = UtilityDetails.ShowkVAhSelectionTamperInTransaction ? new string[45] : new string[44];
                //param[0] = "1,";
                //param[1] = "2,";
                //param[2] = "3,";
                //param[3] = "4,";
                //param[4] = "5,";
                //param[5] = "6,";
                //param[6] = "7,";
                //param[7] = "8,";
                //param[8] = "9,";
                //param[9] = "10,";
                //param[10] = "11,";
                //param[11] = "12,";
                //param[12] = "51,";
                //param[13] = "52,";
                //param[14] = "53,";
                //param[15] = "54,";
                //param[16] = "55,";
                //param[17] = "56,";
                //param[18] = "57,";
                //param[19] = "58,";
                //param[20] = "59,";
                //param[21] = "60,";
                //param[22] = "61,";
                //param[23] = "62,";
                //param[24] = "63,";
                //param[25] = "64,";
                //param[26] = "65,";
                //param[27] = "66,";
                //param[28] = "67,";
                //param[29] = "68,";
                //param[30] = "101,";
                //param[31] = "102,";
                //param[32] = "151,";
                //param[33] = "152,";
                //param[34] = "153,";
                //param[35] = "154,";
                //param[36] = "155,";
                //param[37] = "201,";
                //param[38] = "202,";
                //param[39] = "203,";
                //param[40] = "204,";
                //param[41] = "205,";
                //param[42] = "206,";
                //param[43] = "251,";
                //if (UtilityDetails.ShowkVAhSelectionTamperInTransaction)
                //{
                //    param[44] = "158,";
                //}
                ArrayList insParameter = new ArrayList();
                insParameter.Add("1,");
                insParameter.Add("2,");
                insParameter.Add("3,");
                insParameter.Add("4,");
                insParameter.Add("5,");
                insParameter.Add("6,");
                insParameter.Add("7,");
                insParameter.Add("8,");
                insParameter.Add("9,");
                insParameter.Add("10,");
                insParameter.Add("11,");
                insParameter.Add("12,");                
                insParameter.Add("51,");
                insParameter.Add("52,");
                insParameter.Add("53,");
                insParameter.Add("54,");
                insParameter.Add("55,");
                insParameter.Add("56,");
                insParameter.Add("57,");
                insParameter.Add("58,");
                insParameter.Add("59,");
                insParameter.Add("60,");
                insParameter.Add("61,");
                insParameter.Add("62,");
                insParameter.Add("63,");
                insParameter.Add("64,");
                insParameter.Add("65,");
                insParameter.Add("66,");
                insParameter.Add("67,");
                insParameter.Add("68,");
                insParameter.Add("101,");
                insParameter.Add("102,");
                insParameter.Add("151,");
                insParameter.Add("152,");
                insParameter.Add("153,");
                insParameter.Add("154,");
                insParameter.Add("155,");
                insParameter.Add("201,");
                insParameter.Add("202,");
                insParameter.Add("203,");
                insParameter.Add("204,");
                insParameter.Add("205,");
                insParameter.Add("206,");
                insParameter.Add("251,");
                if (UtilityDetails.ShowkVAhSelectionTamperInTransaction)
                {
                    insParameter.Add("158,");
                }
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.KSEB.ToString())
                {
                    insParameter.Add("13,");
                    insParameter.Add("14,");
                    insParameter.Add("159,");
                    insParameter.Add("160,");
                    insParameter.Add("161,");
                    insParameter.Add("162,");
                    insParameter.Add("163,");
                }
                param = new string[insParameter.Count];
                insParameter.CopyTo(param);
            }

            return param;
        }
        public string[] GetTamperDisplayColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[66];
                param[0] =  "Voltage Imbalance R Phase Tamper Counter";
                param[1] =  "Voltage Imbalance Y Phase Tamper Counter";
                param[2] =  "Voltage Imbalance B Phase Tamper Counter";
                param[3] =  "Missing Potential R Phase Tamper Counter";
                param[4] =  "Missing Potential Y Phase Tamper Counter";
                param[5] =  "Missing Potential B Phase Tamper Counter";
                param[6] =  "Low/Under Voltage R Phase Tamper Counter";
                param[7] =  "Low/Under Voltage Y Phase Tamper Counter";
                param[8] =  "Low/Under Voltage B Phase Tamper Counter";
                param[9] =  "High/Over Voltage R Phase Tamper Counter";
                param[10] = "High/Over Voltage Y Phase Tamper Counter";
                param[11] = "High/Over Voltage B Phase Tamper Counter";
                param[12] = "CT Short Tamper Counter";
                param[13] = "CT Open R Phase Tamper Counter";
                param[14] = "CT Open Y Phase Tamper Counter";
                param[15] = "CT Open B Phase Tamper Counter";
                param[16] = "Current Without Voltage R Phase Tamper Counter";
                param[17] = "Current Without Voltage Y Phase Tamper Counter";
                param[18] = "Current Without Voltage B Phase Tamper Counter";
                param[19] = "LowPower Factor R Phase Tamper Counter";
                param[20] = "LowPower Factor Y Phase Tamper Counter";
                param[21] = "LowPower Factor B Phase Tamper Counter";
                param[22] = "OnePhase Neutral Absent Tamper Counter";
                param[23] = "Voltage Phase Reversal Tamper Counter";
                param[24] = "Current Imbalance R Phase Tamper Counter";
                param[25] = "Current Imbalance YPhase Tamper Counter";
                param[26] = "Current Imbalance BPhase Tamper Counter";
                param[27] = "Current Reversal R Phase Tamper Counter";
                param[28] = "Current Reversal Y Phase Tamper Counter";
                param[29] = "Current Reversal B Phase Tamper Counter";
                param[30] = "Magnetic Influence Tamper Counter";
                param[31] = "Neutral Disturbance Tamper Counter";
                param[32] = "Front Cover Opening Tamper Counter";
                param[33] = "Terminal Cover Opening Tamper Counter";
                param[34] = "Billing Time Stamp";
                param[35] = "Total Tamper Counter";
                param[36] = "Power On Off Counter";
                param[37] = "Low Load Counter";
                param[38] = "Over Load Counter";
                param[39] = "Tamper Code";
                param[40] = "Tamper Occurred Time";
                param[41] = "Tamper Restored Time";
                param[42] = "R Voltage Occurred";
                param[43] = "Y Voltage Occurred";
                param[44] = "B Voltage Occurred";
                param[45] = "R Current Occurred";
                param[46] = "Y Current Occurred";
                param[47] = "B Current Occurred";
                param[48] = "R PF Occurred";
                param[49] = "Y PF Occurred";
                param[50] = "B PF Occurred";
                param[51] = "Total PF Occurred";
                param[52] = "kWh Occurred";
                param[53] = "kVAh Occurred";
                param[54] = "R Voltage Restored";
                param[55] = "Y Voltage Restored";
                param[56] = "B Voltage Restored";
                param[57] = "R Current Restored";
                param[58] = "Y Current Restored";
                param[59] = "B Current Restored";
                param[60] = "R PF Restored";
                param[61] = "Y PF Restored";
                param[62] = "B PF Restored";
                param[63] = "Total PF Restored";
                param[64] = "kWh Restored";
                param[65] = "kVAh Restored";

            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                //param = UtilityDetails.ShowkVAhSelectionTamperInTransaction ? new string[45] : new string[44];
                //param[0] = "RPhase PT link Missing (Missing Potential) Occurrence";
                //param[1] = "RPhase PT link Missing (Missing Potential) Restoration";
                //param[2] = "YPhase PT link Missing (Missing Potential) Occurrence";
                //param[3] = "YPhase PT link Missing (Missing Potential) Restoration";
                //param[4] = "BPhase PT link Missing (Missing Potential) Occurrence";
                //param[5] = "BPhase PT link Missing (Missing Potential) Restoration";
                //param[6] = "Over Voltage in any Phase  Occurrence";
                //param[7] = "Over Voltage in any Phase  Restoration";
                //param[8] = "Low Voltage in any Phase  Occurrence";
                //param[9] = "Low Voltage in any Phase  Restoration";
                //param[10] = "Voltage Unbalance Occurrence";
                //param[11] = "Voltage Unbalance Restoration";
                //param[12] = "Phase R CT reverse Occurrence";
                //param[13] = "Phase R CT reverse Restoration";
                //param[14] = "Phase Y CT reverse Occurrence";
                //param[15] = "Phase Y CT reverse Restoration";
                //param[16] = "Phase B CT reverse Occurrence";
                //param[17] = "Phase B CT reverse Restoration";
                //param[18] = "Phase R CT Open Occurrence";
                //param[19] = "Phase R CT Open Restoration";
                //param[20] = "Phase Y CT Open Occurrence";
                //param[21] = "Phase Y CT Open Restoration";
                //param[22] = "Phase B CT Open Occurrence";
                //param[23] = "Phase B CT Open Restoration";
                //param[24] = "Current Unbalance Occurrence";
                //param[25] = "Current Unbalance Restoration";
                //param[26] = "CT Bypass Occurrence";
                //param[27] = "CT Bypass Restoration";
                //param[28] = "Over Current in any Phase Occurrence";
                //param[29] = "Over Current in any Phase Restoration";
                //param[30] = "Power failure (3 phase) Occurrence";
                //param[31] = "Power failure (3 phase) Restoration";
                //param[32] = "Real Time Clock Date and Time";
                //param[33] = "Demand Integration Period";
                //param[34] = "Profile Capture Period";
                //param[35] = "Single action Schedule for Billing Dates";
                //param[36] = "Activity Calendar for Time Zones";
                //param[37] = "Permanent Magnet or AC/DC Electromagnet Occurence";
                //param[38] = "Permanent Magnet or AC/DC Electromagnet Restoration";
                //param[39] = "Neutral Disturbance HF & DC Occurrence";
                //param[40] = "Neutral Disturbance HF & DC Restoration";
                //param[41] = "Very Low PF Occurrence";
                //param[42] = "Very Low PF Restoration";
                //param[43] = "Meter Cover Opening Occurrence";
                //if (UtilityDetails.ShowkVAhSelectionTamperInTransaction)
                //{
                //    param[44] = "kVAh Selection Changed";
                //}

                ArrayList insParameter = new ArrayList();
                insParameter.Add("RPhase PT link Missing (Missing Potential) Occurrence");
                insParameter.Add("RPhase PT link Missing (Missing Potential) Restoration");
                insParameter.Add("YPhase PT link Missing (Missing Potential) Occurrence");
                insParameter.Add("YPhase PT link Missing (Missing Potential) Restoration");
                insParameter.Add("BPhase PT link Missing (Missing Potential) Occurrence");
                insParameter.Add("BPhase PT link Missing (Missing Potential) Restoration");
                insParameter.Add("Over Voltage in any Phase  Occurrence");
                insParameter.Add( "Over Voltage in any Phase  Restoration");
                insParameter.Add("Low Voltage in any Phase  Occurrence");
                insParameter.Add("Low Voltage in any Phase  Restoration");
                insParameter.Add("Voltage Unbalance Occurrence");
                insParameter.Add("Voltage Unbalance Restoration");
                insParameter.Add("Phase R CT reverse Occurrence");
                insParameter.Add("Phase R CT reverse Restoration");
                insParameter.Add("Phase Y CT reverse Occurrence");
                insParameter.Add("Phase Y CT reverse Restoration");
                insParameter.Add("Phase B CT reverse Occurrence");
                insParameter.Add("Phase B CT reverse Restoration");
                insParameter.Add("Phase R CT Open Occurrence");
                insParameter.Add("Phase R CT Open Restoration");
                insParameter.Add("Phase Y CT Open Occurrence");
                insParameter.Add("Phase Y CT Open Restoration");
                insParameter.Add("Phase B CT Open Occurrence");
                insParameter.Add("Phase B CT Open Restoration");
                insParameter.Add("Current Unbalance Occurrence");
                insParameter.Add("Current Unbalance Restoration");
                insParameter.Add("CT Bypass Occurrence");
                insParameter.Add("CT Bypass Restoration");
                insParameter.Add("Over Current in any Phase Occurrence");
                insParameter.Add("Over Current in any Phase Restoration");
                insParameter.Add("Power failure (3 phase) Occurrence");
                insParameter.Add("Power failure (3 phase) Restoration");
                insParameter.Add("Real Time Clock Date and Time");
                insParameter.Add("Demand Integration Period");
                insParameter.Add("Profile Capture Period");
                insParameter.Add("Single action Schedule for Billing Dates");
                insParameter.Add("Activity Calendar for Time Zones");
                insParameter.Add("Permanent Magnet or AC/DC Electromagnet Occurence");
                insParameter.Add("Permanent Magnet or AC/DC Electromagnet Restoration");
                insParameter.Add("Neutral Disturbance HF & DC Occurrence");
                insParameter.Add("Neutral Disturbance HF & DC Restoration");
                insParameter.Add("Very Low PF Occurrence");
                insParameter.Add("Very Low PF Restoration");
                insParameter.Add("Meter Cover Opening Occurrence");
                if (UtilityDetails.ShowkVAhSelectionTamperInTransaction)
                {
                    insParameter.Add("kVAh Selection Changed");
                }
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.KSEB.ToString())
                {
                    insParameter.Add("Voltage Phase Sequence Reversal - Occurrence");
                    insParameter.Add("Voltage Phase Sequence Reversal - Restoration");
                    insParameter.Add("MD Reset");
                    insParameter.Add("Display - Push Mode Config");
                    insParameter.Add("Display - Scroll Mode Config");
                    insParameter.Add("Display - HR Mode Config");
                    insParameter.Add("Display - Scroll Time Config");                    
                   
                }
                param = new string[insParameter.Count];
                insParameter.CopyTo(param);
            }

            return param;
        }
        public string[] GetLoadSurveyDisplayColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[17];
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
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                if (isPUMA)
                {
                    param = new string[13];
                }
                else
                {
                    param = new string[11];
                }
                param[0] = "Real Time Clock Date and Time";
                param[1] = "Current IR";
                param[2] = "Current IY";
                param[3] = "Current IB";
                param[4] = "Voltage VRN";
                param[5] = "Voltage VYN";
                param[6] = "Voltage VBN";
                param[7] = "Block Active Energy";//Energy kWh
                param[8] = "Block Reactive Energy lag";
                param[9] = "Block Reactive Energy lead";
                param[10] = "Block Apparent Energy";
                if (isPUMA)
                {
                    param[11] = "Frequency";
                    param[12] = "Tamper Status";
                }
            }

            return param;
        }
        public string[] GetLoadSurveyDBColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.IEC_LTCT_650))
            {
                param = new string[17];
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
            }
            else if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                if (isPUMA)
                {
                    param = new string[13];
                }
                else
                {
                    param = new string[11];
                }
                param[0] = "A.realTimeClockDateandTime,";
                param[1] = "A.rPhaseCurrent,";
                param[2] = "A.yPhaseCurrent,";
                param[3] = "A.bPhaseCurrent,";
                param[4] = "A.rPhaseVoltage,";
                param[5] = "A.yPhaseVoltage,";
                param[6] = "A.bPhaseVoltage,";
                param[7] = "A.blockEnergykWh,";
                param[8] = "A.blockEnergykvarhlag,";
                param[9] = "A.blockEnergykvarhlead,";
                param[10] = "A.blockEnergykVAh,";
                if (isPUMA)
                {
                    param[11] = "A.frequency,";
                    param[12] = "A.tamperStatus,";
                }
            }
            return param;
        }

        //added for MVVNL
        public string[] GetMidnightEnergiesColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                param = new string[5];
                param[0] = "Date";
                param[1] = "Cumulative Active Energy";
                param[2] = "Cumulative Reactive Energy lag";
                param[3] = "Cumulative Reactive Energy lead";
                param[4] = "Cumulative Apparent Energy";
            }
            return param;
        }
        //added for MVVNL
        public string[] GetSelfDiagnosisDisplayColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                param = new string[4];
                param[0] = "FLASH";
                param[1] = "EEPROM";
                param[2] = "POWER SUPPLY";
                param[3] = "RTC";
            }
            return param;
        }
        public string[] GetSelfDiagnosisDBColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                param = new string[4];
                param[0] = "FlashStatus,";
                param[1] = "EepRamStatus,";
                param[2] = "SmpsStatus,";
                param[3] = "RtcStatus,";
                param[3] = "ErrorCodeStatus,";
            }
            return param;
        }
        public string[] GetMidnightEnergiesDBColumnName()
        {
            string[] param = null;
            if (appType.Equals(ApplicationType.DLMS_LTCT_650))
            {
                param = new string[5];
                param[0] = "realTimeClockDateandTime,";
                param[1] = "CumEnergykWh,";
                param[2] = "CumEnergykvarhlag,";
                param[3] = "CumEnergykvarhlead,";
                param[4] = "CumEnergykVAh,";
            }
            return param;
        }
        //added for MVVNL

        public string GetDBColumn(string text, SettingTypes types)
        {
            string dbText = string.Empty;
            string[] DispCol = null;
            string[] dbCol = null;
            try
            {
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
                //added for MVVNL
                else if (types.Equals(SettingTypes.MidnightEnergies))
                {
                    DispCol = GetMidnightEnergiesColumnName();
                    dbCol = GetMidnightEnergiesDBColumnName();
                }
                else if (types.Equals(SettingTypes.SelfDiagnosis))
                {
                    DispCol = GetSelfDiagnosisDisplayColumnName();
                    dbCol = GetSelfDiagnosisDBColumnName();
                }
                //added for MVVNL
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
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDBColumn(string text, SettingTypes types)", ex);
                throw ex; 
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
            DataSet data = new DataSet();
            data= asciiExportSettingsDAL.GetParameterData(qry);
            return dlms650CommonBLL.ApplyBillingEMF(data);  
        }
    }
}

