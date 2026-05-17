using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.Framework.Utility
{
   public static class GlobalConstants
   {

       #region "Cumulative Energy Constants"
       public const string conCumulativeEnergyColWH = "{0}Wh (1.0.1.8.0.255;3;2)";
       public const string conCumulativeEnergyVAH = "{0}VAh (1.0.9.8.0.255;3;2)";
       public const string conCumulativeEnergyVARHLAG = "{0}VArh - Lag (1.0.5.8.0.255;3;2)";//varh - Lag
       public const string conCumulativeEnergyVARHLEAD = "{0}VArh - Lead (1.0.8.8.0.255;3;2)";//varh 
       public const string conCumulativeEnergyBILLINGTYPE = "Billing Type (0.0.96.1.165.255;1;2)";
       public const string conCumulativeEnergyBILLINGTYPE_WB = "Billing Type (0.0.96.64.0.255;1;2)"; //Story no: 490966 WB tender specific check implemented for Billing Reset Type OBIS code change
       public const string conCumulativeEnergyBILLINGRESETYPE_SM = "Billing Type (1.0.96.50.2.255;1;2)";//For smart meter falcon2
       public const string conCumulativeEnergyColWHExport = "{0}Wh Export (1.0.2.8.0.255;3;2)";
       public const string conCumulativeEnergyColVAHExport = "{0}VAh Export (1.0.10.8.0.255;3;2)";
       public const string conCumulativeEnergyColWHImport = "{0}Wh Forward (1.0.143.128.128.255;3;2)";
       public const string conCumulativeEnergyColVAHImport = "{0}VAh Forward (1.0.144.128.128.255;3;2)";

       public const string conCumulativeEnergyColLagQ1 = "{0}VArh - Lag Q1 (1.0.145.128.128.255;3;2)";
       public const string conCumulativeEnergyColVAHLeadQ4 = "{0}VArh - Lead Q4 (1.0.146.128.128.255;3;2)";//varh - Lead
       public const string conCumulativeEnergyColWHLagQ3 = "{0}VArh - Lag Q3 (1.0.7.8.0.255;3;2)";//varh 
       public const string conCumulativeEnergyColVAHLeadQ2 = "{0}VArh - Lead Q2 (1.0.6.8.0.255;3;2)";//varh 
       public const string conCumulativeEnergyColkWhRPhase = "{0}Wh R Phase (1.0.21.8.0.255;3;2)";
       public const string conCumulativeEnergyColkWhYPhase = "{0}Wh Y Phase (1.0.41.8.0.255;3;2)";
       public const string conCumulativeEnergyColkWhBPhase = "{0}Wh B Phase (1.0.61.8.0.255;3;2)";

       public const string conCumulativeEnergyColWHNet = "Net - {0}Wh";
       public const string conCumulativeEnergyColVAHNet = "Net - {0}VAh";

       public const string consCumulativeEnergyFraudkWh = "Fraud - {0}Wh (0.0.96.1.218.255;3;2)";
       public const string consCumulativeEnergyFraudkVAh = "Fraud - {0}VAh (0.0.96.2.189.255;3;2)";

        public const string consKwhLag = "{0}Wh - Lag (1.0.1.8.128.255;3;2)";
        public const string consKwhLead = "{0}Wh - Lead (1.0.1.8.129.255;3;2)";
        public const string consKVAhLag = "{0}VAh - Lag (1.0.9.8.128.255;3;2)";
        public const string consKVAhLead = "{0}VAh - Lead (1.0.9.8.129.255;3;2)";
        #region JDVVNL

        public const string minimumVoltageLSIPAcrossDayRPhase = "Min Voltage R Phase (1.0.32.3.128.255;3;2)";
       public const string minimumVoltageLSIPAcrossDayYPhase = "Min Voltage Y Phase (1.0.52.3.128.255;3;2)";
       public const string minimumVoltageLSIPAcrossDayBPhase = "Min Voltage B Phase (1.0.72.3.128.255;3;2)";     
    
       #endregion

       #endregion 

       #region "Billing Energy- Consumption Tab Constants"
       public const string consConsumptionHistory = "History";
       public const string consConsumptionEnergyWH = "{0}Wh";
       public const string consConsumptionEnergyVAH = "{0}VAh";
       public const string consConsumptionEnergyVARHLAG = "{0}VArh - Lag";//"{0}varh - Lag";
       public const string consConsumptionEnergyVARHLEAD = "{0}VArh - Lead";     
       public const string consConsumptionEnergyFraudWH = "Fraud {0}Wh";
       public const string consConsumptionEnergyFraudVAH = "Fraud {0}VAh";
      #endregion


       #region "Maximum Demand Constants"
       public const string conMaximumDemandHistory = "History";
       public const string conMaximumDemandBillingDateTime = "Billing Date Time (0.0.0.1.2.255;3;2)";
       public const string conMaximumDemandKW = "MD {0}W(1.0.1.6.0.255;4;2)";
       public const string conMaximumDemandKWTIMESTAMP = "MD {0}W Time Stamp (1.0.1.6.0.255;4;5)";
       public const string conMaximumDemandKVA = "MD {0}VA (1.0.9.6.0.255;4;2)";
       public const string conMaximumDemandKVATIMESTAMP = "MD {0}VA Time Stamp (1.0.9.6.0.255;4;5)";
       public const string conMaximumDemandRPhaseKW = "MD {0}W R Phase(1.0.21.6.0.255;4;2)";
       public const string conMaximumDemandRPhaseTIMESTAMP = "MD {0}W R Phase Time Stamp (1.0.21.6.0.255;4;5)";
       public const string conMaximumDemandYPhaseKW = "MD {0}W Y Phase(1.0.41.6.0.255;4;2)";
       public const string conMaximumDemandYPhaseTIMESTAMP = "MD {0}W Y Phase Time Stamp (1.0.41.6.0.255;4;5)";
       public const string conMaximumDemandBPhaseKW = "MD {0}W B Phase(1.0.61.6.0.255;4;2)";
       public const string conMaximumDemandBPhaseTIMESTAMP = "MD {0}W B Phase Time Stamp (1.0.61.6.0.255;4;5)";
       public const string conMaximumDemandKWExport = "MD {0}W Export(1.0.2.6.0.255;4;2)";
       public const string conMaximumDemandKWTIMESTAMPExport = "MD {0}W Export Time Stamp (1.0.2.6.0.255;4;5)";
       public const string conMaximumDemandKVAExport = "MD {0}VA Export(1.0.10.6.0.255;4;2)";
       public const string conMaximumDemandKVATIMESTAMPExport = "MD {0}VA Export Time Stamp (1.0.10.6.0.255;4;5)";
       public const string conMaximumDemandKWImport = "MD {0}W Forward(1.0.151.128.128.255;4;2)";
       public const string conMaximumDemandKWTIMESTAMPImport = "MD {0}W Forward Time Stamp (1.0.151.128.128.255;4;5)";
       public const string conMaximumDemandKVAImport = "MD {0}VA Forward(1.0.152.128.128.255;4;2)";
       public const string conMaximumDemandKVATIMESTAMPImport = "MD {0}VA Forward Time Stamp (1.0.152.128.128.255;4;5)";

        // User Story - 1000867
        public const string conMaximumDemandKVARLag = "MD {0}VAr Lag(1.0.5.6.0.255;4;2)";
        public const string conMaximumDemandKVARLagTIMESTAMP = "MD {0}VAr Lag Time Stamp (1.0.5.6.0.255;4;5)";
        public const string conMaximumDemandKVARLead = "MD {0}VAr Lead (1.0.8.6.0.255;4;2)";
        public const string conMaximumDemandKVARLeadTIMESTAMP = "MD {0}VAr Lead Time Stamp (1.0.8.6.0.255;4;5)";


        public const string conMDKW = "Maximum Demand - {0}W";
       public const string conMDKWDateTime= "Maximum Demand - {0}W DateTime";
       public const string conMDKVA    =   "Maximum Demand - {0}VA";
       public const string conMDKVADateTime = "Maximum Demand - {0}VA DateTime";
       public const string conCumulativeMDKW ="Cumulative Maximum Demand – {0}W";
       public const string conCumulativeMDKVA = "Cumulative Maximum Demand – {0}VA";
       public const string conCumulativeEnergyKWH = "Cumulative Energy - {0}Wh";
       public const string conCumulativeEnergyKVARHLag = "Cumulative Energy - {0}VArh - lag";//{0}varh - lag"
       public const string conCumulativeEnergyKVARHLead = "Cumulative Energy - {0}VArh - lead";//{0}varh - lag"
       public const string conCumulativeEnergyKVAH = "Cumulative Energy - {0}VAh";
       public const string conCumulativeExportEnergyKWH = "Cumulative Export Energy - {0}Wh"; // VBM - Add cumulative export energy column 

        // User Story - 1000867
        public const string conMDkVArLag = "Maximum Demand - {0}VAr Lag";
        public const string conMDkVArLagDateTime = "Maximum Demand - {0}VAr Lag  DateTime";
        public const string conMDkVArLead = "Maximum Demand - {0}VAr Lead";
        public const string conMDkVArLeadDateTime = "Maximum Demand - {0}VAr Lead DateTime";


        //VBM - Added  for KSEB       
        public const string conReverseKWh = "Reverse kWh";
       public const string conReverseKVAH = "Reverse kVAh";
       public const string conReversKVArhLag = "Reverse VArh - lag";//kVArh
       public const string conReversKVArhLead = "Reverse VArh - lead";//kVArh
       public const string conPresentTimeZone = "Present TOD Zone";
       public const string conCumulativeKkWhWithHighResolutionT1 = "Cumulative kWh with high Resolution - T1";
       public const string conCumulativeKkWhWithHighResolutionT2 = "Cumulative kWh with high Resolution - T2";
       public const string conCumulativeKkWhWithHighResolutionT3 = "Cumulative kWh with high Resolution - T3";
       public const string conCumulativeKkWhWithHighResolutionT4 = "Cumulative kWh with high Resolution - T4";     
       public const string conCumulativeKkWhWithHighResolutionT5 = "Cumulative kWh with high Resolution - T5";
       public const string conCumulativeKkWhWithHighResolutionT6 = "Cumulative kWh with high Resolution - T6";
       public const string conCumulativeKkWhWithHighResolutionT7 = "Cumulative kWh with high Resolution - T7";
       public const string conCumulativeKkWhWithHighResolutionT8 = "Cumulative kWh with high Resolution - T8";

      #endregion

       #region "Power Factor Constants"
       public const string conPowerFactorHistory = "History";
       public const string conPowerFactor = "Power Factor (1.0.13.0.0.255;3;2)";
       public const string conPowerFactorImport = "Power Factor Forward (1.0.140.128.128.255;3;2)";
       public const string conPowerFactorExport = "Power Factor Export (1.0.84.0.0.255;3;2)";
       public const string conApparentPowerKVA = "Apparent Power - {0}VA";
       public const string conSingnedActivePowerKW = "Signed Active Power - {0}W (+ Forward - Reverse)";
       public const string conSignedReactivePowerKVAR =  "Signed Reactive Power - {0}var (+ Lag - Lead)";
       public const string TODAveragePowerFactor = "TOD Average PF(1.0.13.0.T.255)";
       public const string TODAverageExportPowerFactor = "TOD Average Export PF(1.0.84.0.T.255)";//story 1024441 Add TOD Export PF
        //************ Smart meter Avg PF ***************
        public const string TODAveragePowerFactor_smart = "TOD Average PF (1.0.0.2.1.T;3;2)";
       #endregion

       #region "TOD - Energy Constants"
       public const string conTODEnergyKWH = "{0}Wh (1.0.1.8.T.255;3;2)";
       public const string conTODEnergyKVAH = "{0}VAh (1.0.9.8.T.255;3;2)";

       public const string conTODEnergyKWHImport = "{0}Wh Forward (1.0.143.128.T.255;3;2)";
       public const string conTODEnergyKVAHImport = "{0}VAh Forward (1.0.144.128.T.255;3;2)";
       public const string conTODEnergyKWHExport = "{0}Wh Export (1.0.2.8.T.255;3;2)";
       public const string conTODEnergyKVAHExport = "{0}VAh Export (1.0.10.8.T.255;3;2)";
       public const string conTODEnergyKWHNet = "Net - {0}Wh";
       public const string conTODEnergyKVAHNet = "Net - {0}VAh";
       public const string conTODEnergyKVARHLAG = "{0}VArh - Lag (1.0.5.8.T.255;3;2)";//varh
       public const string conTODEnergyKVARHLEAD = "{0}VArh - Lead (1.0.8.8.T.255;3;2)";//varh

      // public const string conTODEnergyKVARHLAGQ1 = "{0}varh - Lag (Q1) (1.0.145.128.T.255;3;2)";
       public const string conTODEnergyKVARHLAGQ1 = "{0}VArh - Lag (Q1) (1.0.5.5.T.255;3;2)";//varh - Lag (Q1) commet pradipta
      // public const string conTODEnergyKVARHLEADQ4 = "{0}varh - Lead (Q4) (1.0.146.128.T.255;3;2)";
       public const string conTODEnergyKVARHLEADQ4 = "{0}VArh - Lead (Q4) (1.0.8.8.T.255;3;2)";//varh commet pradipta

       public const string conTODEnergyKVARHLAGQ3 = "{0}VArh - Lag (Q3) (1.0.7.8.T.255;3;2)";//varh
       public const string conTODEnergyKVARHLEADQ2 = "{0}VArh - Lead (Q2) (1.0.6.8.T.255;3;2)";//varh

       //************this code is for Smart meter Avg PF ***************
       public const string conTODEnergyKVARHLAG_smart = "{0}VArh - Lag (1.0.0.2.1.T;3;2)";//varh
       public const string conTODEnergyKVARHLEAD_smart = "{0}VArh - Lead (1.0.0.2.1.T;3;2)";//varh


       public const string conTODConsumptionKWH = "{0}Wh";
       public const string conTODConsumptionKVAH = "{0}VAh";
       public const string conTODConsumptionKVARHLAG = "{0}VArh - Lag (1.0.5.8.T.255;3;2)";//varh - Lag
       public const string conTODConsumptionKVARHLEAD = "{0}VArh - Lead (1.0.8.8.T.255;3;2)";//varh
       //************this code is for Smart meter Avg PF ***************
       public const string conTODConsumptionKVARHLAG_smart = "{0}VArh - Lag (1.0.0.2.1.T;3;2)";//varh
       public const string conTODConsumptionKVARHLEAD_smart = "{0}VArh - Lead (1.0.0.2.1.T;3;2)";//varh

       #endregion

       #region "TOD- MD Data"
       
       public const string conTODMD_KW = "MD {0}W(1.0.1.6.T.255;4;2)";
       public const string conTODMD_KWTimeStamp = "MD {0}W Time Stamp (1.0.1.6.T.255;4;5)";
       public const string conTODMD_KVA = "MD {0}VA (1.0.9.6.T.255;4;2)";
       public const string conTODMD_KVATimeStamp = "MD {0}VA Time Stamp (1.0.9.6.T.255;4;5)";


       public const string conTODMD_KW_Export = "MD {0}W Export(1.0.2.6.0.255;4;2)";
       public const string conTODMD_KWTimeStamp_Export = "MD {0}W Export Time Stamp (1.0.2.6.0.255;4;5)";
       public const string conTODMD_KVA_Export = "MD {0}VA Export(1.0.10.6.0.255;4;2)";
       public const string conTODMD_KVATimeStamp_Export = "MD {0}VA Export Time Stamp (1.0.10.6.0.255;4;5)";

       public const string conTODMD_KW_Import = "MD {0}W Forward(1.0.151.128.128.255;4;2)";
       public const string conTODMD_KWTimeStamp_Import = "MD {0}W Forward Time Stamp (1.0.151.128.128.255;4;5)";
       public const string conTODMD_KVA_Import = "MD {0}VA Forward(1.0.152.128.128.255;4;2)";
       public const string conTODMD_KVATimeStamp_Import = "MD {0}VA Forward Time Stamp (1.0.152.128.128.255;4;5)";
       #endregion

       #region "LS - Constants"
       public const string conLSBlockDemandKW = "Block Demand - {0}W (1.0.1.29.0.255;3;2)";
       public const string conLSBlockDemandKVARLag = "Block Demand - {0}VAr - lag (1.0.5.29.0.255;3;2)";//var ";
       public const string conLSBlockDemandKVARLead = "Block Demand - {0}VAr - lead (1.0.8.29.0.255;3;2)";//var;
       public const string conLSBlockDemandKVA = "Block Demand - {0}VA (1.0.9.29.0.255;3;2)";
       public const string conLSBlockDemandEnergyKWH = "Block Energy - {0}Wh (1.0.1.29.0.255;3;2)";
       public const string conLSEnergyKVARHLag = "Block Energy - {0}VArh - lag (1.0.5.29.0.255;3;2)";//varh;
       public const string conLSEnergyKVARHLead = "Block Energy - {0}VArh - lead (1.0.8.29.0.255;3;2)";//varh ";
       public const string conLSEnergyKVAH = "Block Energy - {0}VAh (1.0.9.29.0.255;3;2)";

       public const string conLSBlockDemandKWExport = "Block Demand - {0}W Export (1.0.2.29.0.255;3;2)";
       public const string conLSBlockDemandKVARLagQ3 = "Block Demand - {0}VAr - lag Q3 (1.0.7.29.0.255;3;2)";//var";
       public const string conLSBlockDemandKVARLeadQ2 = "Block Demand - {0}VAr - lead Q2 (1.0.6.29.0.255;3;2)";//var ";
       public const string conLSBlockDemandKVAExport = "Block Demand - {0}VA Export (1.0.10.29.0.255;3;2)";
       public const string conLSEnergyKWHExport = "Block Energy - {0}Wh Export (1.0.2.29.0.255;3;2)";
       public const string conLSEnergyKVARHLagQ3 = "Block Energy - {0}VArh - lag Q3 (1.0.7.29.0.255;3;2)";//varh ";
       public const string conLSEnergyKVARHLeadQ2 = "Block Energy - {0}VArh - lead Q2 (1.0.6.29.0.255;3;2)";//varh";
       public const string conLSEnergyKVAHExport = "Block Energy - {0}VAh Export (1.0.10.29.0.255;3;2)";

       public const string conLSBlockDemandKWImport = "Block Demand - {0}W Forward (1.0.147.128.128.255;3;2)";
       public const string conLSBlockDemandKVARLagQ1 = "Block Demand - {0}VAr - lag Q1 (1.0.149.128.128.255;3;2)";//var ;
       public const string conLSBlockDemandKVARLeadQ4 = "Block Demand - {0}VAr - lead Q4 (1.0.150.128.128.255;3;2)";//var ";
       public const string conLSBlockDemandKVAImport = "Block Demand - {0}VA Forward (1.0.148.128.128.255;3;2)";
       public const string conLSEnergyKWHImport = "Block Energy - {0}Wh Forward (1.0.147.128.128.255;3;2)";
       public const string conLSEnergyKVARHLagQ1 = "Block Energy - {0}VArh - lag Q1 (1.0.149.128.128.255;3;2)";//varh ;
       public const string conLSEnergyKVARHLeadQ4 = "Block Energy - {0}VArh - lead Q4 (1.0.150.128.128.255;3;2)";//varh ;
       public const string conLSEnergyKVAHImport = "Block Energy - {0}VAh Forward (1.0.148.128.128.255;3;2)";

       public const string conLSBlockDemandKWRPhase = "Block Demand - {0}Wh R Phase (1.0.21.29.0.255;3;2)";
       public const string conLSBlockDemandKWYPhase = "Block Demand - {0}Wh Y Phase (1.0.41.29.0.255;3;2)";
       public const string conLSBlockDemandKWBPhase = "Block Demand - {0}Wh B Phase (1.0.61.29.0.255;3;2)";
       public const string conLSEnergyKWHRPhase = "Block Energy - {0}Wh R Phase (1.0.21.29.0.255;3;2)";
       public const string conLSEnergyKWHYPhase = "Block Energy - {0}Wh Y Phase (1.0.41.29.0.255;3;2)";
       public const string conLSEnergyKWHBPhase = "Block Energy - {0}Wh B Phase (1.0.61.29.0.255;3;2)";

       public const string conLSBlockDemandkvarhQ12 = "Block Demand - {0}VAr Q12 (1.0.9.29.0.255;3;2)";//var Q12
       //public const string conLSBlockDemandkvarhQ34 = "Block Energy - {0}varh Q34 (1.0.1.29.0.255;3;2)";

       public const string conLSBlockDemandkvarhQ34 = "Block Demand - {0}VAr Q34 (1.0.1.29.0.255;3;2)";//var Q12


       public const string conLSBlockDemandkvarhQ14 = "Block Energy - {0}VArh Q14 (1.0.155.128.128.255;3;2)";//varh Q14
       public const string conLSBlockDemandkvarhQ23 = "Block Energy - {0}VArh Q23 (1.0.156.128.128.255;3;2)";//varh Q23
       public const string conLSEnergykvarhQ12 = "Block Energy - {0}VArh Q12 (1.0.1.29.0.255;3;2)";//varh Q12
       public const string conLSEnergykvarhQ34 = "Block Energy - {0}VArh Q34 (1.0.5.29.0.255;3;2)";//varh Q34
       public const string conLSEnergykvarhQ14 = "Block Energy - {0}VArh Q14 (1.0.155.128.128.255;3;2)";//varh Q14
       public const string conLSEnergykvarhQ23 = "Block Energy - {0}VArh Q23 (1.0.156.128.128.255;3;2)";//varh Q23

       public const string conLSBlockDemandFundamentalkWhAbsolute = "Block Demand Fundamental - {0}Wh (1.0.128.29.1.255;3;2)";
       public const string conLSEnergyFundamentalkWhAbsolute = "Block Energy Fundamental - {0}Wh (1.0.128.29.1.255;3;2)";

       public const string conLSDemandNetkWh = "Net - {0}W";
       public const string conLSDemandNetkVAh = "Net - {0}VA";

       public const string conLSEnergyNetkWh = "Net - {0}Wh";
       public const string conLSEnergyNetkVAh = "Net - {0}VAh";

       public const string conLSActivePowerRPhaseKW = "Active Power R Phase  - {0}W (1.1.24.7.0.255;3;2)";
       public const string conLSActivePowerYPhaseKW = "Active Power Y Phase  - {0}W (1.1.56.7.0.255;3;2)";
       public const string conLSActivePowerBPhaseKW = "Active Power B Phase  - {0}W (1.1.76.7.0.255;3;2)";
       public const string conLSApparentPowerRPhaseKVA = "Apparent Power R Phase - {0}VA (1.0.29.7.0.255;3;2)";
       public const string conLSApparentPowerYPhaseKVA = "Apparent Power Y Phase - {0}VA (1.0.49.7.0.255;3;2)";
       public const string conLSApparentPowerBPhaseKVA = "Apparent Power B Phase - {0}VA (1.0.69.7.0.255;3;2)";
       public const string conLSReactivePowerRPhaseKVAr = "Reactive Power R Phase - {0}VAr (1.0.23.7.0.255;3;2)";
       public const string conLSReactivePowerYPhaseKVAr = "Reactive Power Y Phase - {0}VAr (1.0.43.7.0.255;3;2)";
       public const string conLSReactivePowerBPhaseKVAr = "Reactive Power B Phase - {0}VAr (1.0.63.7.0.255;3;2)";
       public const string conLSpowerOffDurationLSIP = "Total Power Off duration during LSIP - Min (0.0.94.91.128.255;3;2)";
       //Task no: 591495- Temperature added in load survey for 1P DLMS Firmware X39.51L (DHBVN QD-755)*/
      // public const string conLSTemperature = "Temperature °C (0.0.94.91.128.255;3;2)";//add pradipta_fastdownload
       public const string conLSTamperflag = "Tamper Flag (0.0.96.10.128.255;1;2)";//smart meter
       public const string conLSAvgvolt3ph = "AvgvoltageofThreePH (1.0.12.27.128.255;3;2)";
       public const string conLSAvgRphPF = "AvgRphasePF (1.0.33.29.0.255;3;2)";
       public const string conLSAvgYphPF = "AvgYphasePF (1.0.53.29.0.255;3;2)";
       public const string conLSAvgBphPF = "AvgBphasePF (1.0.73.29.0.255;3;2)";
       public const string conLSAvgTotalPF = "AvgTotalPF (1.0.13.29.0.255;3;2)";

       public const string conLSneucurrent = "AvgNeutralCurrent (1.0.91.29.0.255;3;2)";
       public const string conLSTHDVR = "THDVR (1.0.32.128.124.255;3;2)";
       public const string conLSTHDVY = "THDVY (1.0.52.128.124.255;3;2)";
       public const string conLSTHDVB = "THDVB (1.0.72.128.124.255;3;2)";
       public const string conLSTHDIR = "THDIR (1.0.31.128.124.255;3;2)";
       public const string conLSTHDIY = "THDIY (1.0.51.128.124.255;3;2)";
       public const string conLSTHDIB = "THDIB (1.0.71.128.124.255;3;2)";


       public const string conLSTemperature = "Temperature °C (0.0.96.9.129.255;3;2)";//add pradipta_fastdownload
       #endregion
   }
}
