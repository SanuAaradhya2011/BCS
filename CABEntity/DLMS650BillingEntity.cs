/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.Framework.Entity;
namespace CAB.Entity
{
    public class DLMS650BillingEntity : EntityBase
    {
        public long Billing_ID { get; set; }
        public long BillingDate { get; set; }
        //BhardwajG : Variable for holding next billing date
        private long nextBillingDate;
        //BhardwajG : Variable for holding mechanism
        private string mechanismForCDF;
        public string SystemPowerFactorforBillingPeriod { get; set; }
        public string CumulativeEnergykWhTZ0 { get; set; }
        public string CumulativeEnergykWhTZ1 { get; set; }
        public string CumulativeEnergykWhTZ2 { get; set; }
        public string CumulativeEnergykWhTZ3 { get; set; }
        public string CumulativeEnergykWhTZ4 { get; set; }
        public string CumulativeEnergykWhTZ5 { get; set; }
        public string CumulativeEnergykWhTZ6 { get; set; }
        public string CumulativeEnergykWhTZ7 { get; set; }
        public string CumulativeEnergykWhTZ8 { get; set; }
        public string CumulativeEnergykvarhLag { get; set; }
        public string CumulativeEnergykvarhLagTZ1 { get; set; }
        public string CumulativeEnergykvarhLagTZ2 { get; set; }
        public string CumulativeEnergykvarhLagTZ3 { get; set; }
        public string CumulativeEnergykvarhLagTZ4 { get; set; }
        public string CumulativeEnergykvarhLagTZ5 { get; set; }
        public string CumulativeEnergykvarhLagTZ6 { get; set; }
        public string CumulativeEnergykvarhLagTZ7 { get; set; }
        public string CumulativeEnergykvarhLagTZ8 { get; set; }
        public string CumulativeEnergykvarhLead { get; set; }
        public string CumulativeEnergykvarhLeadTZ1 { get; set; }
        public string CumulativeEnergykvarhLeadTZ2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ3 { get; set; }
        public string CumulativeEnergykvarhLeadTZ4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ5 { get; set; }
        public string CumulativeEnergykvarhLeadTZ6 { get; set; }
        public string CumulativeEnergykvarhLeadTZ7 { get; set; }
        public string CumulativeEnergykvarhLeadTZ8 { get; set; }
        public string CumulativeEnergykVAhTZ0 { get; set; }
        public string CumulativeEnergykVAhTZ1 { get; set; }
        public string CumulativeEnergykVAhTZ2 { get; set; }
        public string CumulativeEnergykVAhTZ3 { get; set; }
        public string CumulativeEnergykVAhTZ4 { get; set; }
        public string CumulativeEnergykVAhTZ5 { get; set; }
        public string CumulativeEnergykVAhTZ6 { get; set; }
        public string CumulativeEnergykVAhTZ7 { get; set; }
        public string CumulativeEnergykVAhTZ8 { get; set; }
        public string PowerOffDuration { get; set; }
       
        //pradipta_start_081018
        public string BillingAveragekWImportLoadFactor { get; set; }
        public string BillingAveragekWExportLoadFactor { get; set; }
        public string BillingAveragekVAImportLoadFactor { get; set; }
        public string BillingAveragekVAExportLoadFactor { get; set; }
        //pradipta_End_081018


        public string MDkWTZ0 { get; set; }
        public long MDkWDateTimeTZ0 { get; set; }
        public string MDkWTZ1 { get; set; }
        public long MDkWDateTimeTZ1 { get; set; }
        public string MDkWTZ2 { get; set; }
        public long MDkWDateTimeTZ2 { get; set; }
        public string MDkWTZ3 { get; set; }
        public long MDkWDateTimeTZ3 { get; set; }
        public string MDkWTZ4 { get; set; }
        public long MDkWDateTimeTZ4 { get; set; }
        public string MDkWTZ5 { get; set; }
        public long MDkWDateTimeTZ5 { get; set; }
        public string MDkWTZ6 { get; set; }
        public long MDkWDateTimeTZ6 { get; set; }
        public string MDkWTZ7 { get; set; }
        public long MDkWDateTimeTZ7 { get; set; }
        public string MDkWTZ8 { get; set; }
        public long MDkWDateTimeTZ8 { get; set; }
        public string MDkVATZ0 { get; set; }
        public long MDkVADateTimeTZ0 { get; set; }
        public string MDkVATZ1 { get; set; }
        public long MDkVADateTimeTZ1 { get; set; }
        public string MDkVATZ2 { get; set; }
        public long MDkVADateTimeTZ2 { get; set; }
        public string MDkVATZ3 { get; set; }
        public long MDkVADateTimeTZ3 { get; set; }
        public string MDkVATZ4 { get; set; }
        public long MDkVADateTimeTZ4 { get; set; }
        public string MDkVATZ5 { get; set; }
        public long MDkVADateTimeTZ5 { get; set; }
        public string MDkVATZ6 { get; set; }
        public long MDkVADateTimeTZ6 { get; set; }
        public string MDkVATZ7 { get; set; }
        public long MDkVADateTimeTZ7 { get; set; }
        public string MDkVATZ8 { get; set; }
        public long MDkVADateTimeTZ8 { get; set; }
        public string CumPowerOffDuration { get; set; }
        public long CumTamperCount { get; set; }
        public long DeltaTamperCount { get; set; } // Story - 345154
        public long CumPowerFailureCount { get; set; }
        public long DataIndex { get; set; }
        public long MeterData_ID { get; set; }
        public DateTime BillingDateTime { get; set; }
        public string BillingType { get; set; }
        public string BillingWisePowerOffDuration { get; set; }
        public string BillingAverageLoadFactor { get; set; }
        public string PowerOnDuration { get; set; }
        public string CumPowerOnDuration { get; set; }
        public byte PowerOnDurationDisplay { get; set; }
        public long CumBillingMDResetCount { get; set; }
        public string TODAveragePowerFactorTZ1 { get; set; }
        public string TODAveragePowerFactorTZ2 { get; set; }
        public string TODAveragePowerFactorTZ3 { get; set; }
        public string TODAveragePowerFactorTZ4 { get; set; }
        public string TODAveragePowerFactorTZ5 { get; set; }
        public string TODAveragePowerFactorTZ6 { get; set; }
        public string TODAveragePowerFactorTZ7 { get; set; }
        public string TODAveragePowerFactorTZ8 { get; set; }
        //TOD Average Export PF
        public string TODAverageExportPowerFactorTZ1 { get; set; } //story 1024441 Add TOD Export PF
        public string TODAverageExportPowerFactorTZ2 { get; set; }
        public string TODAverageExportPowerFactorTZ3 { get; set; }
        public string TODAverageExportPowerFactorTZ4 { get; set; }
        public string TODAverageExportPowerFactorTZ5 { get; set; }
        public string TODAverageExportPowerFactorTZ6 { get; set; }
        public string TODAverageExportPowerFactorTZ7 { get; set; }
        public string TODAverageExportPowerFactorTZ8 { get; set; }
        public long RPhaseMDDateTime { get; set; }
        public long YPhaseMDDateTime { get; set; }
        public long BPhaseMDDateTime { get; set; }
        public string RPhaseMDkW { get; set; }
        public string YPhaseMDkW { get; set; }
        public string BPhaseMDkW { get; set; }
        public string BillingAverageLoad { get; set; }
        public string ABCCodeBilling { get; set; }




        //Import Start
        //KWH - Import
        public string CumulativeEnergykWhTZ0Import { get; set; }
        public string CumulativeEnergykWhTZ1Import { get; set; }
        public string CumulativeEnergykWhTZ2Import { get; set; }
        public string CumulativeEnergykWhTZ3Import { get; set; }
        public string CumulativeEnergykWhTZ4Import { get; set; }
        public string CumulativeEnergykWhTZ5Import { get; set; }
        public string CumulativeEnergykWhTZ6Import { get; set; }
        public string CumulativeEnergykWhTZ7Import { get; set; }
        public string CumulativeEnergykWhTZ8Import { get; set; }
        //KVAH - Import
        public string CumulativeEnergykVAhTZ0Import { get; set; }
        public string CumulativeEnergykVAhTZ1Import { get; set; }
        public string CumulativeEnergykVAhTZ2Import { get; set; }
        public string CumulativeEnergykVAhTZ3Import { get; set; }
        public string CumulativeEnergykVAhTZ4Import { get; set; }
        public string CumulativeEnergykVAhTZ5Import { get; set; }
        public string CumulativeEnergykVAhTZ6Import { get; set; }
        public string CumulativeEnergykVAhTZ7Import { get; set; }
        public string CumulativeEnergykVAhTZ8Import { get; set; }
        //MD-KW-Import
        public string MDkWTZ0Import { get; set; }
        public string MDkWTZ1Import { get; set; }
        public string MDkWTZ2Import { get; set; }
        public string MDkWTZ3Import { get; set; }
        public string MDkWTZ4Import { get; set; }
        public string MDkWTZ5Import { get; set; }
        public string MDkWTZ6Import { get; set; }
        public string MDkWTZ7Import { get; set; }
        public string MDkWTZ8Import { get; set; }
        //MD-KVA-Import
        public string MDkVATZ0Import { get; set; }
        public string MDkVATZ1Import { get; set; }
        public string MDkVATZ2Import { get; set; }
        public string MDkVATZ3Import { get; set; }
        public string MDkVATZ4Import { get; set; }
        public string MDkVATZ5Import { get; set; }
        public string MDkVATZ6Import { get; set; }
        public string MDkVATZ7Import { get; set; }
        public string MDkVATZ8Import { get; set; }
        //MD-KW-TimeStamp-Import
        public long MDkWDateTimeTZ0Import { get; set; }
        public long MDkWDateTimeTZ1Import { get; set; }
        public long MDkWDateTimeTZ2Import { get; set; }
        public long MDkWDateTimeTZ3Import { get; set; }
        public long MDkWDateTimeTZ4Import { get; set; }
        public long MDkWDateTimeTZ5Import { get; set; }
        public long MDkWDateTimeTZ6Import { get; set; }
        public long MDkWDateTimeTZ7Import { get; set; }
        public long MDkWDateTimeTZ8Import { get; set; }
        //MD-KVA-TimeStamp-Import
        public long MDkVADateTimeTZ0Import { get; set; }
        public long MDkVADateTimeTZ1Import { get; set; }
        public long MDkVADateTimeTZ2Import { get; set; }
        public long MDkVADateTimeTZ3Import { get; set; }
        public long MDkVADateTimeTZ4Import { get; set; }
        public long MDkVADateTimeTZ5Import { get; set; }
        public long MDkVADateTimeTZ6Import { get; set; }
        public long MDkVADateTimeTZ7Import { get; set; }
        public long MDkVADateTimeTZ8Import { get; set; }
        //Import End


        #region Net param
        //KWH - Net
        public string CumulativeEnergykWhTZ0Net { get; set; }
        public string CumulativeEnergykWhTZ1Net { get; set; }
        public string CumulativeEnergykWhTZ2Net { get; set; }
        public string CumulativeEnergykWhTZ3Net { get; set; }
        public string CumulativeEnergykWhTZ4Net { get; set; }
        public string CumulativeEnergykWhTZ5Net { get; set; }
        public string CumulativeEnergykWhTZ6Net { get; set; }
        public string CumulativeEnergykWhTZ7Net { get; set; }
        public string CumulativeEnergykWhTZ8Net { get; set; }
        //KVAH - Net
        public string CumulativeEnergykVAhTZ0Net { get; set; }
        public string CumulativeEnergykVAhTZ1Net { get; set; }
        public string CumulativeEnergykVAhTZ2Net { get; set; }
        public string CumulativeEnergykVAhTZ3Net { get; set; }
        public string CumulativeEnergykVAhTZ4Net { get; set; }
        public string CumulativeEnergykVAhTZ5Net { get; set; }
        public string CumulativeEnergykVAhTZ6Net { get; set; }
        public string CumulativeEnergykVAhTZ7Net { get; set; }
        public string CumulativeEnergykVAhTZ8Net { get; set; }
        //MD-KW-Net
        public string MDkWTZ0Net { get; set; }
        public string MDkWTZ1Net { get; set; }
        public string MDkWTZ2Net { get; set; }
        public string MDkWTZ3Net { get; set; }
        public string MDkWTZ4Net { get; set; }
        public string MDkWTZ5Net { get; set; }
        public string MDkWTZ6Net { get; set; }
        public string MDkWTZ7Net { get; set; }
        public string MDkWTZ8Net { get; set; }
        //MD-KVA-Net
        public string MDkVATZ0Net { get; set; }
        public string MDkVATZ1Net { get; set; }
        public string MDkVATZ2Net { get; set; }
        public string MDkVATZ3Net { get; set; }
        public string MDkVATZ4Net { get; set; }
        public string MDkVATZ5Net { get; set; }
        public string MDkVATZ6Net { get; set; }
        public string MDkVATZ7Net { get; set; }
        public string MDkVATZ8Net { get; set; }
        //MD-KW-TimeStamp-Net
        public long MDkWDateTimeTZ0Net { get; set; }
        public long MDkWDateTimeTZ1Net { get; set; }
        public long MDkWDateTimeTZ2Net { get; set; }
        public long MDkWDateTimeTZ3Net { get; set; }
        public long MDkWDateTimeTZ4Net { get; set; }
        public long MDkWDateTimeTZ5Net { get; set; }
        public long MDkWDateTimeTZ6Net { get; set; }
        public long MDkWDateTimeTZ7Net { get; set; }
        public long MDkWDateTimeTZ8Net { get; set; }
        //MD-KVA-TimeStamp-Net
        public long MDkVADateTimeTZ0Net { get; set; }
        public long MDkVADateTimeTZ1Net { get; set; }
        public long MDkVADateTimeTZ2Net { get; set; }
        public long MDkVADateTimeTZ3Net { get; set; }
        public long MDkVADateTimeTZ4Net { get; set; }
        public long MDkVADateTimeTZ5Net { get; set; }
        public long MDkVADateTimeTZ6Net { get; set; }
        public long MDkVADateTimeTZ7Net { get; set; }
        public long MDkVADateTimeTZ8Net { get; set; }
        //Net End
        #endregion 

        //Export Start
        //KWH - Export
        public string CumulativeEnergykWhTZ0Export { get; set; }
        public string CumulativeEnergykWhTZ1Export { get; set; }
        public string CumulativeEnergykWhTZ2Export { get; set; }
        public string CumulativeEnergykWhTZ3Export { get; set; }
        public string CumulativeEnergykWhTZ4Export { get; set; }
        public string CumulativeEnergykWhTZ5Export { get; set; }
        public string CumulativeEnergykWhTZ6Export { get; set; }
        public string CumulativeEnergykWhTZ7Export { get; set; }
        public string CumulativeEnergykWhTZ8Export { get; set; }
        //KVAH - Export
        public string CumulativeEnergykVAhTZ0Export { get; set; }
        public string CumulativeEnergykVAhTZ1Export { get; set; }
        public string CumulativeEnergykVAhTZ2Export { get; set; }
        public string CumulativeEnergykVAhTZ3Export { get; set; }
        public string CumulativeEnergykVAhTZ4Export { get; set; }
        public string CumulativeEnergykVAhTZ5Export { get; set; }
        public string CumulativeEnergykVAhTZ6Export { get; set; }
        public string CumulativeEnergykVAhTZ7Export { get; set; }
        public string CumulativeEnergykVAhTZ8Export { get; set; }
        //MD-KW-Export
        public string MDkWTZ0Export { get; set; }
        public string MDkWTZ1Export { get; set; }
        public string MDkWTZ2Export { get; set; }
        public string MDkWTZ3Export { get; set; }
        public string MDkWTZ4Export { get; set; }
        public string MDkWTZ5Export { get; set; }
        public string MDkWTZ6Export { get; set; }
        public string MDkWTZ7Export { get; set; }
        public string MDkWTZ8Export { get; set; }
        //MD-KVA-Export
        public string MDkVATZ0Export { get; set; }
        public string MDkVATZ1Export { get; set; }
        public string MDkVATZ2Export { get; set; }
        public string MDkVATZ3Export { get; set; }
        public string MDkVATZ4Export { get; set; }
        public string MDkVATZ5Export { get; set; }
        public string MDkVATZ6Export { get; set; }
        public string MDkVATZ7Export { get; set; }
        public string MDkVATZ8Export { get; set; }
        //MD-KW-TimeStamp-Export
        public long MDkWDateTimeTZ0Export { get; set; }
        public long MDkWDateTimeTZ1Export { get; set; }
        public long MDkWDateTimeTZ2Export { get; set; }
        public long MDkWDateTimeTZ3Export { get; set; }
        public long MDkWDateTimeTZ4Export { get; set; }
        public long MDkWDateTimeTZ5Export { get; set; }
        public long MDkWDateTimeTZ6Export { get; set; }
        public long MDkWDateTimeTZ7Export { get; set; }
        public long MDkWDateTimeTZ8Export { get; set; }
        //MD-KVA-TimeStamp-Export
        public long MDkVADateTimeTZ0Export { get; set; }
        public long MDkVADateTimeTZ1Export { get; set; }
        public long MDkVADateTimeTZ2Export { get; set; }
        public long MDkVADateTimeTZ3Export { get; set; }
        public long MDkVADateTimeTZ4Export { get; set; }
        public long MDkVADateTimeTZ5Export { get; set; }
        public long MDkVADateTimeTZ6Export { get; set; }
        public long MDkVADateTimeTZ7Export { get; set; }
        public long MDkVADateTimeTZ8Export { get; set; }
        //Export End

        //Lag-Q1-Start
        public string CumulativeEnergykvarhLagQ1 { get; set; }
        public string CumulativeEnergykvarhLagTZ1Q1 { get; set; }
        public string CumulativeEnergykvarhLagTZ2Q1 { get; set; }
        public string CumulativeEnergykvarhLagTZ3Q1 { get; set; }
        public string CumulativeEnergykvarhLagTZ4Q1 { get; set; }
        public string CumulativeEnergykvarhLagTZ5Q1 { get; set; }
        public string CumulativeEnergykvarhLagTZ6Q1 { get; set; }
        public string CumulativeEnergykvarhLagTZ7Q1 { get; set; }
        public string CumulativeEnergykvarhLagTZ8Q1 { get; set; }
        //Lag-Q1-End

        //Lead-Q4-Start
        public string CumulativeEnergykvarhLeadQ4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ1Q4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ2Q4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ3Q4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ4Q4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ5Q4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ6Q4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ7Q4 { get; set; }
        public string CumulativeEnergykvarhLeadTZ8Q4 { get; set; }
        //Lead-Q4-End

        //Lag-Q3-Start
        public string CumulativeEnergykvarhLagQ3 { get; set; }
        public string CumulativeEnergykvarhLagTZ1Q3 { get; set; }
        public string CumulativeEnergykvarhLagTZ2Q3 { get; set; }
        public string CumulativeEnergykvarhLagTZ3Q3 { get; set; }
        public string CumulativeEnergykvarhLagTZ4Q3 { get; set; }
        public string CumulativeEnergykvarhLagTZ5Q3 { get; set; }
        public string CumulativeEnergykvarhLagTZ6Q3 { get; set; }
        public string CumulativeEnergykvarhLagTZ7Q3 { get; set; }
        public string CumulativeEnergykvarhLagTZ8Q3 { get; set; }
        //Lag-Q3-End

        //Lead-Q2-Start
        public string CumulativeEnergykvarhLeadQ2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ1Q2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ2Q2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ3Q2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ4Q2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ5Q2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ6Q2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ7Q2 { get; set; }
        public string CumulativeEnergykvarhLeadTZ8Q2 { get; set; }
        //Lead-Q2-End

        //PowerFactor
        public string SystemPowerFactorImportforBillingPeriod { get; set; }
        public string SystemPowerFactorExportforBillingPeriod { get; set; }

        //Cumulative MD kw and kva
        public string CumulativeMDkw { get; set; }
        public string CumulativeMDkva { get; set; }

        public string CumEnergykWhRPhase { get; set; }
        public string CumEnergykWhYPhase { get; set; }
        public string CumEnergykWhBPhase { get; set; }

        public string CumulativeEnergyFraudkWh { get; set; }
        public string CumulativeEnergyFraudkVAh { get; set; }

        public string MDkVArLagTZ0 { get; set; }
        public long MDkVArLagDateTimeTZ0 { get; set; }

        public string MDkVArLeadTZ0 { get; set; }
        public long MDkVArLeadDateTimeTZ0 { get; set; }

        /// <summary>
        /// Property for storing next billing date
        /// </summary>
        public long NextBillingDate
        {
            get
            {
                return nextBillingDate;
            }
            set
            {
                nextBillingDate = value;
            }
        }
        /// <summary>
        /// Property for storing mechanism
        /// </summary>
        public string MechanismForCDF
        {
            get
            {
                return mechanismForCDF;
            }
            set
            {
                mechanismForCDF = value;
            }
        }
        #region JDVVNL

        public string MinimumVoltageLSIPAcrossDayRPhase  { get; set; }
        public string MinimumVoltageLSIPAcrossDayYPhase  { get; set; }
        public string MinimumVoltageLSIPAcrossDayBPhase  { get; set; }
        #endregion

        #region SapphireS2

        public string kWhLag { get; set; }
        public string kWhLead { get; set; }
        public string kVAhLag { get; set; }
        public string kVAhLead { get; set; }
        #endregion
    }
}
