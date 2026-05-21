
/* 
* |----------------------------------------------------------------------------------------------------------------|
* |											All rights reserved to Cabcon Technologies 	 								|
* | 																												|
* |											Author : Piyush Singh. 	 								|
* | 																												|
* |----------------------------------------------------------------------------------------------------------------| 
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class DLMS650BillingDAL : DALBase
    {

        public string meter_cat;
        public string Billing_ID = "Billing_ID";
        public string BillingDate = "BillingDate";
        public string BillingResetType = "BillingResetType";
        public string ABCCodeBilling = "ABCCode";
        public const string Cumulativemdkw = "CumulativeMDkW";
        public const string Cumulativemdkva = "CumulativeMDkva";


        public string SystemPowerFactorforBillingPeriod = "SystemPowerFactorforBillingPeriod";
        public string CumulativeEnergykWhTZ0 = "CumulativeEnergykWhTZ0";
        public string CumulativeEnergykWhTZ1 = "CumulativeEnergykWhTZ1";
        public string CumulativeEnergykWhTZ2 = "CumulativeEnergykWhTZ2";
        public string CumulativeEnergykWhTZ3 = "CumulativeEnergykWhTZ3";
        public string CumulativeEnergykWhTZ4 = "CumulativeEnergykWhTZ4";
        public string CumulativeEnergykWhTZ5 = "CumulativeEnergykWhTZ5";
        public string CumulativeEnergykWhTZ6 = "CumulativeEnergykWhTZ6";
        public string CumulativeEnergykWhTZ7 = "CumulativeEnergykWhTZ7";
        public string CumulativeEnergykWhTZ8 = "CumulativeEnergykWhTZ8";
        //Net Start
        //KWH - Net
        public string CumulativeEnergykWhTZ0Net = "CumulativeEnergykWhTZ0Net";
        public string CumulativeEnergykWhTZ1Net = "CumulativeEnergykWhTZ1Net";
        public string CumulativeEnergykWhTZ2Net = "CumulativeEnergykWhTZ2Net";
        public string CumulativeEnergykWhTZ3Net = "CumulativeEnergykWhTZ3Net";
        public string CumulativeEnergykWhTZ4Net = "CumulativeEnergykWhTZ4Net";
        public string CumulativeEnergykWhTZ5Net = "CumulativeEnergykWhTZ5Net";
        public string CumulativeEnergykWhTZ6Net = "CumulativeEnergykWhTZ6Net";
        public string CumulativeEnergykWhTZ7Net = "CumulativeEnergykWhTZ7Net";
        public string CumulativeEnergykWhTZ8Net = "CumulativeEnergykWhTZ8Net";
        //KVAH - Net
        public string CumulativeEnergykVAhTZ0Net = "CumulativeEnergykVAhTZ0Net";
        public string CumulativeEnergykVAhTZ1Net = "CumulativeEnergykVAhTZ1Net";
        public string CumulativeEnergykVAhTZ2Net = "CumulativeEnergykVAhTZ2Net";
        public string CumulativeEnergykVAhTZ3Net = "CumulativeEnergykVAhTZ3Net";
        public string CumulativeEnergykVAhTZ4Net = "CumulativeEnergykVAhTZ4Net";
        public string CumulativeEnergykVAhTZ5Net = "CumulativeEnergykVAhTZ5Net";
        public string CumulativeEnergykVAhTZ6Net = "CumulativeEnergykVAhTZ6Net";
        public string CumulativeEnergykVAhTZ7Net = "CumulativeEnergykVAhTZ7Net";
        public string CumulativeEnergykVAhTZ8Net = "CumulativeEnergykVAhTZ8Net";
        //MD-KW-Net
        public string MDkWTZ0Net = "MDkWTZ0Net";
        public string MDkWTZ1Net = "MDkWTZ1Net";
        public string MDkWTZ2Net = "MDkWTZ2Net";
        public string MDkWTZ3Net = "MDkWTZ3Net";
        public string MDkWTZ4Net = "MDkWTZ4Net";
        public string MDkWTZ5Net = "MDkWTZ5Net";
        public string MDkWTZ6Net = "MDkWTZ6Net";
        public string MDkWTZ7Net = "MDkWTZ7Net";
        public string MDkWTZ8Net = "MDkWTZ8Net";
        //MD-KVA-Net
        public string MDkVATZ0Net = "MDkVATZ0Net";
        public string MDkVATZ1Net = "MDkVATZ1Net";
        public string MDkVATZ2Net = "MDkVATZ2Net";
        public string MDkVATZ3Net = "MDkVATZ3Net";
        public string MDkVATZ4Net = "MDkVATZ4Net";
        public string MDkVATZ5Net = "MDkVATZ5Net";
        public string MDkVATZ6Net = "MDkVATZ6Net";
        public string MDkVATZ7Net = "MDkVATZ7Net";
        public string MDkVATZ8Net = "MDkVATZ8Net";
        //MD-KW-TimeStamp-Net
        public string MDkWDateTimeTZ0Net = "MDkWDateTimeTZ0Net";
        public string MDkWDateTimeTZ1Net = "MDkWDateTimeTZ1Net";
        public string MDkWDateTimeTZ2Net = "MDkWDateTimeTZ2Net";
        public string MDkWDateTimeTZ3Net = "MDkWDateTimeTZ3Net";
        public string MDkWDateTimeTZ4Net = "MDkWDateTimeTZ4Net";
        public string MDkWDateTimeTZ5Net = "MDkWDateTimeTZ5Net";
        public string MDkWDateTimeTZ6Net = "MDkWDateTimeTZ6Net";
        public string MDkWDateTimeTZ7Net = "MDkWDateTimeTZ7Net";
        public string MDkWDateTimeTZ8Net = "MDkWDateTimeTZ8Net";
        //MD-KVA-TimeStamp-Net
        public string MDkVADateTimeTZ0Net = "MDkVADateTimeTZ0Net";
        public string MDkVADateTimeTZ1Net = "MDkVADateTimeTZ1Net";
        public string MDkVADateTimeTZ2Net = "MDkVADateTimeTZ2Net";
        public string MDkVADateTimeTZ3Net = "MDkVADateTimeTZ3Net";
        public string MDkVADateTimeTZ4Net = "MDkVADateTimeTZ4Net";
        public string MDkVADateTimeTZ5Net = "MDkVADateTimeTZ5Net";
        public string MDkVADateTimeTZ6Net = "MDkVADateTimeTZ6Net";
        public string MDkVADateTimeTZ7Net = "MDkVADateTimeTZ7Net";
        public string MDkVADateTimeTZ8Net = "MDkVADateTimeTZ8Net";
        //Net End
        //Import Start
        //KWH - Import
        public string CumulativeEnergykWhTZ0Import = "CumulativeEnergykWhTZ0Import";
        public string CumulativeEnergykWhTZ1Import = "CumulativeEnergykWhTZ1Import";
        public string CumulativeEnergykWhTZ2Import = "CumulativeEnergykWhTZ2Import";
        public string CumulativeEnergykWhTZ3Import = "CumulativeEnergykWhTZ3Import";
        public string CumulativeEnergykWhTZ4Import = "CumulativeEnergykWhTZ4Import";
        public string CumulativeEnergykWhTZ5Import = "CumulativeEnergykWhTZ5Import";
        public string CumulativeEnergykWhTZ6Import = "CumulativeEnergykWhTZ6Import";
        public string CumulativeEnergykWhTZ7Import = "CumulativeEnergykWhTZ7Import";
        public string CumulativeEnergykWhTZ8Import = "CumulativeEnergykWhTZ8Import";
        //KVAH - Import
        public string CumulativeEnergykVAhTZ0Import = "CumulativeEnergykVAhTZ0Import";
        public string CumulativeEnergykVAhTZ1Import = "CumulativeEnergykVAhTZ1Import";
        public string CumulativeEnergykVAhTZ2Import = "CumulativeEnergykVAhTZ2Import";
        public string CumulativeEnergykVAhTZ3Import = "CumulativeEnergykVAhTZ3Import";
        public string CumulativeEnergykVAhTZ4Import = "CumulativeEnergykVAhTZ4Import";
        public string CumulativeEnergykVAhTZ5Import = "CumulativeEnergykVAhTZ5Import";
        public string CumulativeEnergykVAhTZ6Import = "CumulativeEnergykVAhTZ6Import";
        public string CumulativeEnergykVAhTZ7Import = "CumulativeEnergykVAhTZ7Import";
        public string CumulativeEnergykVAhTZ8Import = "CumulativeEnergykVAhTZ8Import";
        //MD-KW-Import
        public string MDkWTZ0Import = "MDkWTZ0Import";
        public string MDkWTZ1Import = "MDkWTZ1Import";
        public string MDkWTZ2Import = "MDkWTZ2Import";
        public string MDkWTZ3Import = "MDkWTZ3Import";
        public string MDkWTZ4Import = "MDkWTZ4Import";
        public string MDkWTZ5Import = "MDkWTZ5Import";
        public string MDkWTZ6Import = "MDkWTZ6Import";
        public string MDkWTZ7Import = "MDkWTZ7Import";
        public string MDkWTZ8Import = "MDkWTZ8Import";
        //MD-KVA-Import
        public string MDkVATZ0Import = "MDkVATZ0Import";
        public string MDkVATZ1Import = "MDkVATZ1Import";
        public string MDkVATZ2Import = "MDkVATZ2Import";
        public string MDkVATZ3Import = "MDkVATZ3Import";
        public string MDkVATZ4Import = "MDkVATZ4Import";
        public string MDkVATZ5Import = "MDkVATZ5Import";
        public string MDkVATZ6Import = "MDkVATZ6Import";
        public string MDkVATZ7Import = "MDkVATZ7Import";
        public string MDkVATZ8Import = "MDkVATZ8Import";
        //MD-KW-TimeStamp-Import
        public string MDkWDateTimeTZ0Import = "MDkWDateTimeTZ0Import";
        public string MDkWDateTimeTZ1Import = "MDkWDateTimeTZ1Import";
        public string MDkWDateTimeTZ2Import = "MDkWDateTimeTZ2Import";
        public string MDkWDateTimeTZ3Import = "MDkWDateTimeTZ3Import";
        public string MDkWDateTimeTZ4Import = "MDkWDateTimeTZ4Import";
        public string MDkWDateTimeTZ5Import = "MDkWDateTimeTZ5Import";
        public string MDkWDateTimeTZ6Import = "MDkWDateTimeTZ6Import";
        public string MDkWDateTimeTZ7Import = "MDkWDateTimeTZ7Import";
        public string MDkWDateTimeTZ8Import = "MDkWDateTimeTZ8Import";
        //MD-KVA-TimeStamp-Import
        public string MDkVADateTimeTZ0Import = "MDkVADateTimeTZ0Import";
        public string MDkVADateTimeTZ1Import = "MDkVADateTimeTZ1Import";
        public string MDkVADateTimeTZ2Import = "MDkVADateTimeTZ2Import";
        public string MDkVADateTimeTZ3Import = "MDkVADateTimeTZ3Import";
        public string MDkVADateTimeTZ4Import = "MDkVADateTimeTZ4Import";
        public string MDkVADateTimeTZ5Import = "MDkVADateTimeTZ5Import";
        public string MDkVADateTimeTZ6Import = "MDkVADateTimeTZ6Import";
        public string MDkVADateTimeTZ7Import = "MDkVADateTimeTZ7Import";
        public string MDkVADateTimeTZ8Import = "MDkVADateTimeTZ8Import";
        //Import End
        //Export  Start
        //KWH - Export
        public string CumulativeEnergykWhTZ0Export = "CumulativeEnergykWhTZ0Export";
        public string CumulativeEnergykWhTZ1Export = "CumulativeEnergykWhTZ1Export";
        public string CumulativeEnergykWhTZ2Export = "CumulativeEnergykWhTZ2Export";
        public string CumulativeEnergykWhTZ3Export = "CumulativeEnergykWhTZ3Export";
        public string CumulativeEnergykWhTZ4Export = "CumulativeEnergykWhTZ4Export";
        public string CumulativeEnergykWhTZ5Export = "CumulativeEnergykWhTZ5Export";
        public string CumulativeEnergykWhTZ6Export = "CumulativeEnergykWhTZ6Export";
        public string CumulativeEnergykWhTZ7Export = "CumulativeEnergykWhTZ7Export";
        public string CumulativeEnergykWhTZ8Export = "CumulativeEnergykWhTZ8Export";
        //KVAH - Export
        public string CumulativeEnergykVAhTZ0Export = "CumulativeEnergykVAhTZ0Export";
        public string CumulativeEnergykVAhTZ1Export = "CumulativeEnergykVAhTZ1Export";
        public string CumulativeEnergykVAhTZ2Export = "CumulativeEnergykVAhTZ2Export";
        public string CumulativeEnergykVAhTZ3Export = "CumulativeEnergykVAhTZ3Export";
        public string CumulativeEnergykVAhTZ4Export = "CumulativeEnergykVAhTZ4Export";
        public string CumulativeEnergykVAhTZ5Export = "CumulativeEnergykVAhTZ5Export";
        public string CumulativeEnergykVAhTZ6Export = "CumulativeEnergykVAhTZ6Export";
        public string CumulativeEnergykVAhTZ7Export = "CumulativeEnergykVAhTZ7Export";
        public string CumulativeEnergykVAhTZ8Export = "CumulativeEnergykVAhTZ8Export";
        //MD-KW-Export
        public string MDkWTZ0Export = "MDkWTZ0Export";
        public string MDkWTZ1Export = "MDkWTZ1Export";
        public string MDkWTZ2Export = "MDkWTZ2Export";
        public string MDkWTZ3Export = "MDkWTZ3Export";
        public string MDkWTZ4Export = "MDkWTZ4Export";
        public string MDkWTZ5Export = "MDkWTZ5Export";
        public string MDkWTZ6Export = "MDkWTZ6Export";
        public string MDkWTZ7Export = "MDkWTZ7Export";
        public string MDkWTZ8Export = "MDkWTZ8Export";
        //MD-KVA-Export
        public string MDkVATZ0Export = "MDkVATZ0Export";
        public string MDkVATZ1Export = "MDkVATZ1Export";
        public string MDkVATZ2Export = "MDkVATZ2Export";
        public string MDkVATZ3Export = "MDkVATZ3Export";
        public string MDkVATZ4Export = "MDkVATZ4Export";
        public string MDkVATZ5Export = "MDkVATZ5Export";
        public string MDkVATZ6Export = "MDkVATZ6Export";
        public string MDkVATZ7Export = "MDkVATZ7Export";
        public string MDkVATZ8Export = "MDkVATZ8Export";
        //MD-KW-TimeStamp-Export
        public string MDkWDateTimeTZ0Export = "MDkWDateTimeTZ0Export";
        public string MDkWDateTimeTZ1Export = "MDkWDateTimeTZ1Export";
        public string MDkWDateTimeTZ2Export = "MDkWDateTimeTZ2Export";
        public string MDkWDateTimeTZ3Export = "MDkWDateTimeTZ3Export";
        public string MDkWDateTimeTZ4Export = "MDkWDateTimeTZ4Export";
        public string MDkWDateTimeTZ5Export = "MDkWDateTimeTZ5Export";
        public string MDkWDateTimeTZ6Export = "MDkWDateTimeTZ6Export";
        public string MDkWDateTimeTZ7Export = "MDkWDateTimeTZ7Export";
        public string MDkWDateTimeTZ8Export = "MDkWDateTimeTZ8Export";
        //MD-KVA-TimeStamp-Export
        public string MDkVADateTimeTZ0Export = "MDkVADateTimeTZ0Export";
        public string MDkVADateTimeTZ1Export = "MDkVADateTimeTZ1Export";
        public string MDkVADateTimeTZ2Export = "MDkVADateTimeTZ2Export";
        public string MDkVADateTimeTZ3Export = "MDkVADateTimeTZ3Export";
        public string MDkVADateTimeTZ4Export = "MDkVADateTimeTZ4Export";
        public string MDkVADateTimeTZ5Export = "MDkVADateTimeTZ5Export";
        public string MDkVADateTimeTZ6Export = "MDkVADateTimeTZ6Export";
        public string MDkVADateTimeTZ7Export = "MDkVADateTimeTZ7Export";
        public string MDkVADateTimeTZ8Export = "MDkVADateTimeTZ8Export";
        //Cumulative-Energy-Lag-Q1
        public string CumulativeEnergykvarhLagQ1 = "CumulativeEnergykvarhLagQ1";
        public string CumulativeEnergykvarhLagTZ1Q1 = "CumulativeEnergykvarhLagTZ1Q1";
        public string CumulativeEnergykvarhLagTZ2Q1 = "CumulativeEnergykvarhLagTZ2Q1";
        public string CumulativeEnergykvarhLagTZ3Q1 = "CumulativeEnergykvarhLagTZ3Q1";
        public string CumulativeEnergykvarhLagTZ4Q1 = "CumulativeEnergykvarhLagTZ4Q1";
        public string CumulativeEnergykvarhLagTZ5Q1 = "CumulativeEnergykvarhLagTZ5Q1";
        public string CumulativeEnergykvarhLagTZ6Q1 = "CumulativeEnergykvarhLagTZ6Q1";
        public string CumulativeEnergykvarhLagTZ7Q1 = "CumulativeEnergykvarhLagTZ7Q1";
        public string CumulativeEnergykvarhLagTZ8Q1 = "CumulativeEnergykvarhLagTZ8Q1";
        //Cumulative-Energy-Lead-Q4
        public string CumulativeEnergykvarhLeadQ4 = "CumulativeEnergykvarhLeadQ4";
        public string CumulativeEnergykvarhLeadTZ1Q4 = "CumulativeEnergykvarhLeadTZ1Q4";
        public string CumulativeEnergykvarhLeadTZ2Q4 = "CumulativeEnergykvarhLeadTZ2Q4";
        public string CumulativeEnergykvarhLeadTZ3Q4 = "CumulativeEnergykvarhLeadTZ3Q4";
        public string CumulativeEnergykvarhLeadTZ4Q4 = "CumulativeEnergykvarhLeadTZ4Q4";
        public string CumulativeEnergykvarhLeadTZ5Q4 = "CumulativeEnergykvarhLeadTZ5Q4";
        public string CumulativeEnergykvarhLeadTZ6Q4 = "CumulativeEnergykvarhLeadTZ6Q4";
        public string CumulativeEnergykvarhLeadTZ7Q4 = "CumulativeEnergykvarhLeadTZ7Q4";
        public string CumulativeEnergykvarhLeadTZ8Q4 = "CumulativeEnergykvarhLeadTZ8Q4";
        //Cumulative-Energy-Lag-Q3
        public string CumulativeEnergykvarhLagQ3 = "CumulativeEnergykvarhLagQ3";
        public string CumulativeEnergykvarhLagTZ1Q3 = "CumulativeEnergykvarhLagTZ1Q3";
        public string CumulativeEnergykvarhLagTZ2Q3 = "CumulativeEnergykvarhLagTZ2Q3";
        public string CumulativeEnergykvarhLagTZ3Q3 = "CumulativeEnergykvarhLagTZ3Q3";
        public string CumulativeEnergykvarhLagTZ4Q3 = "CumulativeEnergykvarhLagTZ4Q3";
        public string CumulativeEnergykvarhLagTZ5Q3 = "CumulativeEnergykvarhLagTZ5Q3";
        public string CumulativeEnergykvarhLagTZ6Q3 = "CumulativeEnergykvarhLagTZ6Q3";
        public string CumulativeEnergykvarhLagTZ7Q3 = "CumulativeEnergykvarhLagTZ7Q3";
        public string CumulativeEnergykvarhLagTZ8Q3 = "CumulativeEnergykvarhLagTZ8Q3";
        //Cumulative-Energy-Lead-Q2
        public string CumulativeEnergykvarhLeadQ2 = "CumulativeEnergykvarhLeadQ2";
        public string CumulativeEnergykvarhLeadTZ1Q2 = "CumulativeEnergykvarhLeadTZ1Q2";
        public string CumulativeEnergykvarhLeadTZ2Q2 = "CumulativeEnergykvarhLeadTZ2Q2";
        public string CumulativeEnergykvarhLeadTZ3Q2 = "CumulativeEnergykvarhLeadTZ3Q2";
        public string CumulativeEnergykvarhLeadTZ4Q2 = "CumulativeEnergykvarhLeadTZ4Q2";
        public string CumulativeEnergykvarhLeadTZ5Q2 = "CumulativeEnergykvarhLeadTZ5Q2";
        public string CumulativeEnergykvarhLeadTZ6Q2 = "CumulativeEnergykvarhLeadTZ6Q2";
        public string CumulativeEnergykvarhLeadTZ7Q2 = "CumulativeEnergykvarhLeadTZ7Q2";
        public string CumulativeEnergykvarhLeadTZ8Q2 = "CumulativeEnergykvarhLeadTZ8Q2";

        public string SystemPowerFactorImportforBillingPeriod = "SystemPowerFactorImportforBillingPeriod";
        public string SystemPowerFactorExportforBillingPeriod = "SystemPowerFactorExportforBillingPeriod";

        public string CumEnergykWhRPhase = "CumEnergykWhRPhase";
        public string CumEnergykWhYPhase = "CumEnergykWhYPhase";
        public string CumEnergykWhBPhase = "CumEnergykWhBPhase";

        //Export End
        public string CumulativeEnergykvarhLag = "CumulativeEnergykvarhLag";
        public string CumulativeEnergykvarhLagTZ1 = "CumulativeEnergykvarhLagTZ1";
        public string CumulativeEnergykvarhLagTZ2 = "CumulativeEnergykvarhLagTZ2";
        public string CumulativeEnergykvarhLagTZ3 = "CumulativeEnergykvarhLagTZ3";
        public string CumulativeEnergykvarhLagTZ4 = "CumulativeEnergykvarhLagTZ4";
        public string CumulativeEnergykvarhLagTZ5 = "CumulativeEnergykvarhLagTZ5";
        public string CumulativeEnergykvarhLagTZ6 = "CumulativeEnergykvarhLagTZ6";
        public string CumulativeEnergykvarhLagTZ7 = "CumulativeEnergykvarhLagTZ7";
        public string CumulativeEnergykvarhLagTZ8 = "CumulativeEnergykvarhLagTZ8";
        public string CumulativeEnergykvarhLead = "CumulativeEnergykvarhLead";
        public string CumulativeEnergykvarhLeadTZ1 = "CumulativeEnergykvarhLeadTZ1";
        public string CumulativeEnergykvarhLeadTZ2 = "CumulativeEnergykvarhLeadTZ2";
        public string CumulativeEnergykvarhLeadTZ3 = "CumulativeEnergykvarhLeadTZ3";
        public string CumulativeEnergykvarhLeadTZ4 = "CumulativeEnergykvarhLeadTZ4";
        public string CumulativeEnergykvarhLeadTZ5 = "CumulativeEnergykvarhLeadTZ5";
        public string CumulativeEnergykvarhLeadTZ6 = "CumulativeEnergykvarhLeadTZ6";
        public string CumulativeEnergykvarhLeadTZ7 = "CumulativeEnergykvarhLeadTZ7";
        public string CumulativeEnergykvarhLeadTZ8 = "CumulativeEnergykvarhLeadTZ8";
        public string CumulativeEnergykVAhTZ0 = "CumulativeEnergykVAhTZ0";
        public string CumulativeEnergykVAhTZ1 = "CumulativeEnergykVAhTZ1";
        public string CumulativeEnergykVAhTZ2 = "CumulativeEnergykVAhTZ2";
        public string CumulativeEnergykVAhTZ3 = "CumulativeEnergykVAhTZ3";
        public string CumulativeEnergykVAhTZ4 = "CumulativeEnergykVAhTZ4";
        public string CumulativeEnergykVAhTZ5 = "CumulativeEnergykVAhTZ5";
        public string CumulativeEnergykVAhTZ6 = "CumulativeEnergykVAhTZ6";
        public string CumulativeEnergykVAhTZ7 = "CumulativeEnergykVAhTZ7";
        public string CumulativeEnergykVAhTZ8 = "CumulativeEnergykVAhTZ8";
        public string MDkWTZ0 = "MDkWTZ0";
        public string MDkWDateTimeTZ0 = "MDkWDateTimeTZ0";
        public string MDkWTZ1 = "MDkWTZ1";
        public string MDkWDateTimeTZ1 = "MDkWDateTimeTZ1";
        public string MDkWTZ2 = "MDkWTZ2";
        public string MDkWDateTimeTZ2 = "MDkWDateTimeTZ2";
        public string MDkWTZ3 = "MDkWTZ3";
        public string MDkWDateTimeTZ3 = "MDkWDateTimeTZ3";
        public string MDkWTZ4 = "MDkWTZ4";
        public string MDkWDateTimeTZ4 = "MDkWDateTimeTZ4";
        public string MDkWTZ5 = "MDkWTZ5";
        public string MDkWDateTimeTZ5 = "MDkWDateTimeTZ5";
        public string MDkWTZ6 = "MDkWTZ6";
        public string MDkWDateTimeTZ6 = "MDkWDateTimeTZ6";
        public string MDkWTZ7 = "MDkWTZ7";
        public string MDkWDateTimeTZ7 = "MDkWDateTimeTZ7";
        public string MDkWTZ8 = "MDkWTZ8";
        public string MDkWDateTimeTZ8 = "MDkWDateTimeTZ8";
        public string MDkVATZ0 = "MDkVATZ0";
        public string MDkVADateTimeTZ0 = "MDkVADateTimeTZ0";
        public string MDkVATZ1 = "MDkVATZ1";
        public string MDkVADateTimeTZ1 = "MDkVADateTimeTZ1";
        public string MDkVATZ2 = "MDkVATZ2";
        public string MDkVADateTimeTZ2 = "MDkVADateTimeTZ2";
        public string MDkVATZ3 = "MDkVATZ3";
        public string MDkVADateTimeTZ3 = "MDkVADateTimeTZ3";
        public string MDkVATZ4 = "MDkVATZ4";
        public string MDkVADateTimeTZ4 = "MDkVADateTimeTZ4";
        public string MDkVATZ5 = "MDkVATZ5";
        public string MDkVADateTimeTZ5 = "MDkVADateTimeTZ5";
        public string MDkVATZ6 = "MDkVATZ6";
        public string MDkVADateTimeTZ6 = "MDkVADateTimeTZ6";
        public string MDkVATZ7 = "MDkVATZ7";
        public string MDkVADateTimeTZ7 = "MDkVADateTimeTZ7";
        public string MDkVATZ8 = "MDkVATZ8";
        public string MDkVADateTimeTZ8 = "MDkVADateTimeTZ8";
        public string RPhaseMDDateTime = "RPhaseMDDateTime";
        public string YPhaseMDDateTime = "YPhaseMDDateTime";
        public string BPhaseMDDateTime = "BPhaseMDDateTime";
        public string RPhaseMDkW = "RPhaseMDkW";
        public string YPhaseMDkW = "YPhaseMDkW";
        public string BPhaseMDkW = "BPhaseMDkW";
        public string DataIndex = "DataIndex";
        public string MeterData_ID = "MeterData_ID";
        // Added for MPKWCL 
        public const string CUMPOWEROFFDURATION = "CumPowerOffDuration";
        public const string BILLINGWISEPOWEROFFDURATION = "BillingWisePowerOffDuration";
        public const string BillingAverageLoadFactor = "BillingAvgLoadFactor";
        //pradipta_start_081018

        public const string BillingAvgkWImportLoadFactor = "BillingAveragekWImportLoadFactor";
        public const string BillingAvgkWExportLoadFactor = "BillingAveragekWExportLoadFactor";
        public const string BillingAvgkVAImportLoadFactor = "BillingAveragekVAImportLoadFactor";
        public const string BillingAvgkVAExportLoadFactor = "BillingAveragekVAExportLoadFactor";
        //pradipta_End_081018

        public const string BillingAverageLoad = "BillingAverageLoad";
        public const string PowerOnDuration = "PowerOnDuration";
        public const string CumPowerOnDuration = "CumPowerOnDuration";
        public const string PowerOnDurationDisplay = "PowerOnDurationDisplay";
        public const string CUMTAMPERCOUNT = "CumTamperCount";
        public const string CUMPOWERFAILURECOUNT = "CumPowerFailureCount";
        public const string CUMBILLINGMDRESETCOUNT = "CumBillingMDResetCount";
        public const string DELTATAMPERCOUNT = "DeltaTamperCount"; // Story - 345154

        public string TODAveragePowerFactorTZ1 = "TODAveragePowerFactorTZ1";
        public string TODAveragePowerFactorTZ2 = "TODAveragePowerFactorTZ2";
        public string TODAveragePowerFactorTZ3 = "TODAveragePowerFactorTZ3";
        public string TODAveragePowerFactorTZ4 = "TODAveragePowerFactorTZ4";
        public string TODAveragePowerFactorTZ5 = "TODAveragePowerFactorTZ5";
        public string TODAveragePowerFactorTZ6 = "TODAveragePowerFactorTZ6";
        public string TODAveragePowerFactorTZ7 = "TODAveragePowerFactorTZ7";
        public string TODAveragePowerFactorTZ8 = "TODAveragePowerFactorTZ8";


        public string TODAverageExportPowerFactorTZ1 = "TODAverageExportPowerFactorTZ1"; //story 1024441 Add TOD Export PF
        public string TODAverageExportPowerFactorTZ2 = "TODAverageExportPowerFactorTZ2";
        public string TODAverageExportPowerFactorTZ3 = "TODAverageExportPowerFactorTZ3";
        public string TODAverageExportPowerFactorTZ4 = "TODAverageExportPowerFactorTZ4";
        public string TODAverageExportPowerFactorTZ5 = "TODAverageExportPowerFactorTZ5";
        public string TODAverageExportPowerFactorTZ6 = "TODAverageExportPowerFactorTZ6";
        public string TODAverageExportPowerFactorTZ7 = "TODAverageExportPowerFactorTZ7";
        public string TODAverageExportPowerFactorTZ8 = "TODAverageExportPowerFactorTZ8";


        public string CumulativeEnergyFraudkWh = "CumulativeEnergyFraudkWh";
        public string CumulativeEnergyFraudkVAh = "CumulativeEnergyFraudkVAh";

        // User Story - 1000867
        public string MDkVArLagTZ0 = "MDkVArLagTZ0";
        public string MDkVArLagDateTimeTZ0 = "MDkVArLagDateTimeTZ0";
        public string MDkVArLeadTZ0 = "MDkVArLeadTZ0";
        public string MDkVArLeadDateTimeTZ0 = "MDkVArLeadDateTimeTZ0";

        public string kWhLag = "kWhLag";
        public string kWhLead = "kWhLead";
        public string kVAhLag = "kVAhLag";
        public string kVAhLead = "kVAhLead";


        public string MeterID = "MeterID";
        public string RelatedTo = "RelatedTo";
        public string FileName = "FileName";
        public string Group_ID = "Group_ID";
        private UtilityEntity utilityName = UtilityEntity.DEFAULT;

        #region JDVVNL
        public string MinimumVoltageLSIPAcrossDayRPhase = "MinimumVoltageLSIPAcrossDayRPhase";
        public string MinimumVoltageLSIPAcrossDayYPhase = "MinimumVoltageLSIPAcrossDayYPhase";
        public string MinimumVoltageLSIPAcrossDayBPhase = "MinimumVoltageLSIPAcrossDayBPhase";

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DLMS650BillingDAL).ToString());
        #endregion


        // bool isMPKWCL = false;

        /// <summary>
        /// Retrun true if property is MPKWL
        /// </summary>
        private bool isMPKWCL
        {
            get { return (utilityName == UtilityEntity.MPKWCL); }
        }

        /// <summary>
        /// Returns true if property is PGVCL
        /// </summary>
        private bool isPGVCL
        {
            get { return (utilityName == UtilityEntity.PGVCL); }
        }

        public DLMS650BillingDAL()
            : base("meterdata_billing", "Billing_ID")
        {
        }
        //public DLMS650BillingDAL(bool IsMPKWCL)
        //    : base("meterdata_billing", "Billing_ID")
        //{
        //    isMPKWCL = IsMPKWCL;
        //}
        public DLMS650BillingDAL(UtilityEntity utility)
            : base("meterdata_billing", "Billing_ID")
        {
            utilityName = utility;
        }
        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            DLMS650BillingEntity dLMS650BillingEntity = new DLMS650BillingEntity();
            if (NotNullAndNotDBNull(row, Billing_ID)) dLMS650BillingEntity.Billing_ID = Convert.ToInt64(row[Billing_ID]);
            if (NotNullAndNotDBNull(row, BillingDate)) dLMS650BillingEntity.BillingDate = Convert.ToInt64(row[BillingDate]);
            if (NotNullAndNotDBNull(row, SystemPowerFactorforBillingPeriod)) dLMS650BillingEntity.SystemPowerFactorforBillingPeriod = Convert.ToString(row[SystemPowerFactorforBillingPeriod]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ0)) dLMS650BillingEntity.CumulativeEnergykWhTZ0 = Convert.ToString(row[CumulativeEnergykWhTZ0]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ1)) dLMS650BillingEntity.CumulativeEnergykWhTZ1 = Convert.ToString(row[CumulativeEnergykWhTZ1]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ2)) dLMS650BillingEntity.CumulativeEnergykWhTZ2 = Convert.ToString(row[CumulativeEnergykWhTZ2]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ3)) dLMS650BillingEntity.CumulativeEnergykWhTZ3 = Convert.ToString(row[CumulativeEnergykWhTZ3]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ4)) dLMS650BillingEntity.CumulativeEnergykWhTZ4 = Convert.ToString(row[CumulativeEnergykWhTZ4]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ5)) dLMS650BillingEntity.CumulativeEnergykWhTZ5 = Convert.ToString(row[CumulativeEnergykWhTZ5]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ6)) dLMS650BillingEntity.CumulativeEnergykWhTZ6 = Convert.ToString(row[CumulativeEnergykWhTZ6]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ7)) dLMS650BillingEntity.CumulativeEnergykWhTZ7 = Convert.ToString(row[CumulativeEnergykWhTZ7]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykWhTZ8)) dLMS650BillingEntity.CumulativeEnergykWhTZ8 = Convert.ToString(row[CumulativeEnergykWhTZ8]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykvarhLag)) dLMS650BillingEntity.CumulativeEnergykvarhLag = Convert.ToString(row[CumulativeEnergykvarhLag]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykvarhLead)) dLMS650BillingEntity.CumulativeEnergykvarhLead = Convert.ToString(row[CumulativeEnergykvarhLead]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ0)) dLMS650BillingEntity.CumulativeEnergykVAhTZ0 = Convert.ToString(row[CumulativeEnergykVAhTZ0]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ1)) dLMS650BillingEntity.CumulativeEnergykVAhTZ1 = Convert.ToString(row[CumulativeEnergykVAhTZ1]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ2)) dLMS650BillingEntity.CumulativeEnergykVAhTZ2 = Convert.ToString(row[CumulativeEnergykVAhTZ2]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ3)) dLMS650BillingEntity.CumulativeEnergykVAhTZ3 = Convert.ToString(row[CumulativeEnergykVAhTZ3]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ4)) dLMS650BillingEntity.CumulativeEnergykVAhTZ4 = Convert.ToString(row[CumulativeEnergykVAhTZ4]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ5)) dLMS650BillingEntity.CumulativeEnergykVAhTZ5 = Convert.ToString(row[CumulativeEnergykVAhTZ5]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ6)) dLMS650BillingEntity.CumulativeEnergykVAhTZ6 = Convert.ToString(row[CumulativeEnergykVAhTZ6]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ7)) dLMS650BillingEntity.CumulativeEnergykVAhTZ7 = Convert.ToString(row[CumulativeEnergykVAhTZ7]);
            if (NotNullAndNotDBNull(row, CumulativeEnergykVAhTZ8)) dLMS650BillingEntity.CumulativeEnergykVAhTZ8 = Convert.ToString(row[CumulativeEnergykVAhTZ8]);
            if (NotNullAndNotDBNull(row, MDkWTZ0)) dLMS650BillingEntity.MDkWTZ0 = Convert.ToString(row[MDkWTZ0]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ0)) dLMS650BillingEntity.MDkWDateTimeTZ0 = Convert.ToInt64(row[MDkWDateTimeTZ0]);
            if (NotNullAndNotDBNull(row, MDkWTZ1)) dLMS650BillingEntity.MDkWTZ1 = Convert.ToString(row[MDkWTZ1]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ1)) dLMS650BillingEntity.MDkWDateTimeTZ1 = Convert.ToInt64(row[MDkWDateTimeTZ1]);
            if (NotNullAndNotDBNull(row, MDkWTZ2)) dLMS650BillingEntity.MDkWTZ2 = Convert.ToString(row[MDkWTZ2]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ2)) dLMS650BillingEntity.MDkWDateTimeTZ2 = Convert.ToInt64(row[MDkWDateTimeTZ2]);
            if (NotNullAndNotDBNull(row, MDkWTZ3)) dLMS650BillingEntity.MDkWTZ3 = Convert.ToString(row[MDkWTZ3]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ3)) dLMS650BillingEntity.MDkWDateTimeTZ3 = Convert.ToInt64(row[MDkWDateTimeTZ3]);
            if (NotNullAndNotDBNull(row, MDkWTZ4)) dLMS650BillingEntity.MDkWTZ4 = Convert.ToString(row[MDkWTZ4]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ4)) dLMS650BillingEntity.MDkWDateTimeTZ4 = Convert.ToInt64(row[MDkWDateTimeTZ4]);
            if (NotNullAndNotDBNull(row, MDkWTZ5)) dLMS650BillingEntity.MDkWTZ5 = Convert.ToString(row[MDkWTZ5]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ5)) dLMS650BillingEntity.MDkWDateTimeTZ5 = Convert.ToInt64(row[MDkWDateTimeTZ5]);
            if (NotNullAndNotDBNull(row, MDkWTZ6)) dLMS650BillingEntity.MDkWTZ6 = Convert.ToString(row[MDkWTZ6]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ6)) dLMS650BillingEntity.MDkWDateTimeTZ6 = Convert.ToInt64(row[MDkWDateTimeTZ6]);
            if (NotNullAndNotDBNull(row, MDkWTZ7)) dLMS650BillingEntity.MDkWTZ7 = Convert.ToString(row[MDkWTZ7]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ7)) dLMS650BillingEntity.MDkWDateTimeTZ7 = Convert.ToInt64(row[MDkWDateTimeTZ7]);
            if (NotNullAndNotDBNull(row, MDkWTZ8)) dLMS650BillingEntity.MDkWTZ8 = Convert.ToString(row[MDkWTZ8]);
            if (NotNullAndNotDBNull(row, MDkWDateTimeTZ8)) dLMS650BillingEntity.MDkWDateTimeTZ8 = Convert.ToInt64(row[MDkWDateTimeTZ8]);
            if (NotNullAndNotDBNull(row, MDkVATZ0)) dLMS650BillingEntity.MDkVATZ0 = Convert.ToString(row[MDkVATZ0]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ0)) dLMS650BillingEntity.MDkVADateTimeTZ0 = Convert.ToInt64(row[MDkVADateTimeTZ0]);
            if (NotNullAndNotDBNull(row, MDkVATZ1)) dLMS650BillingEntity.MDkVATZ0 = Convert.ToString(row[MDkVATZ0]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ1)) dLMS650BillingEntity.MDkVADateTimeTZ1 = Convert.ToInt64(row[MDkVADateTimeTZ1]);
            if (NotNullAndNotDBNull(row, MDkVATZ2)) dLMS650BillingEntity.MDkVATZ2 = Convert.ToString(row[MDkVATZ2]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ2)) dLMS650BillingEntity.MDkVADateTimeTZ2 = Convert.ToInt64(row[MDkVADateTimeTZ2]);
            if (NotNullAndNotDBNull(row, MDkVATZ3)) dLMS650BillingEntity.MDkVATZ3 = Convert.ToString(row[MDkVATZ3]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ3)) dLMS650BillingEntity.MDkVADateTimeTZ3 = Convert.ToInt64(row[MDkVADateTimeTZ3]);
            if (NotNullAndNotDBNull(row, MDkVATZ4)) dLMS650BillingEntity.MDkVATZ4 = Convert.ToString(row[MDkVATZ4]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ4)) dLMS650BillingEntity.MDkVADateTimeTZ4 = Convert.ToInt64(row[MDkVADateTimeTZ4]);
            if (NotNullAndNotDBNull(row, MDkVATZ5)) dLMS650BillingEntity.MDkVATZ5 = Convert.ToString(row[MDkVATZ5]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ5)) dLMS650BillingEntity.MDkVADateTimeTZ5 = Convert.ToInt64(row[MDkVADateTimeTZ5]);
            if (NotNullAndNotDBNull(row, MDkVATZ6)) dLMS650BillingEntity.MDkVATZ6 = Convert.ToString(row[MDkVATZ6]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ6)) dLMS650BillingEntity.MDkVADateTimeTZ6 = Convert.ToInt64(row[MDkVADateTimeTZ6]);
            if (NotNullAndNotDBNull(row, MDkVATZ7)) dLMS650BillingEntity.MDkVATZ7 = Convert.ToString(row[MDkVATZ7]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ7)) dLMS650BillingEntity.MDkVADateTimeTZ7 = Convert.ToInt64(row[MDkVADateTimeTZ7]);
            if (NotNullAndNotDBNull(row, MDkVATZ8)) dLMS650BillingEntity.MDkVATZ8 = Convert.ToString(row[MDkVATZ8]);
            if (NotNullAndNotDBNull(row, MDkVADateTimeTZ8)) dLMS650BillingEntity.MDkVADateTimeTZ8 = Convert.ToInt64(row[MDkVADateTimeTZ8]);
            if (NotNullAndNotDBNull(row, CumulativeEnergyFraudkWh)) dLMS650BillingEntity.CumulativeEnergyFraudkWh = Convert.ToString(row[CumulativeEnergyFraudkWh]);
            if (NotNullAndNotDBNull(row, CumulativeEnergyFraudkVAh)) dLMS650BillingEntity.CumulativeEnergyFraudkVAh = Convert.ToString(row[CumulativeEnergyFraudkVAh]);

            // User Story - 1000867
            if (NotNullAndNotDBNull(row, MDkVArLagTZ0)) dLMS650BillingEntity.MDkVArLagTZ0 = Convert.ToString(row[MDkVArLagTZ0]);
            if (NotNullAndNotDBNull(row, MDkVArLagDateTimeTZ0)) dLMS650BillingEntity.MDkVArLagDateTimeTZ0 = Convert.ToInt64(row[MDkVArLagDateTimeTZ0]);
            if (NotNullAndNotDBNull(row, MDkVArLeadTZ0)) dLMS650BillingEntity.MDkVArLeadTZ0 = Convert.ToString(row[MDkVArLeadTZ0]);
            if (NotNullAndNotDBNull(row, MDkVArLeadDateTimeTZ0)) dLMS650BillingEntity.MDkVArLeadDateTimeTZ0 = Convert.ToInt64(row[MDkVArLeadDateTimeTZ0]);
            if (NotNullAndNotDBNull(row, kWhLag)) dLMS650BillingEntity.kWhLag = Convert.ToString(row[kWhLag]);
            if (NotNullAndNotDBNull(row, kWhLead)) dLMS650BillingEntity.kWhLead = Convert.ToString(row[kWhLead]);
            if (NotNullAndNotDBNull(row, kVAhLag)) dLMS650BillingEntity.kVAhLag = Convert.ToString(row[kVAhLag]);
            if (NotNullAndNotDBNull(row, kVAhLead)) dLMS650BillingEntity.kVAhLead = Convert.ToString(row[kVAhLead]);

            if (NotNullAndNotDBNull(row, MeterData_ID)) dLMS650BillingEntity.MeterData_ID = Convert.ToInt32(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, DataIndex)) dLMS650BillingEntity.DataIndex = Convert.ToInt32(row[DataIndex]);
            return dLMS650BillingEntity;
        }


        public DataSet GetBillingParameterColumnList(int meterDataId)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT ColumnsNames FROM billingparameter where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID))); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                ds = helper.FillDataSet(request, ds);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingParameterColumnList(int meterDataId)", ex);
            }
            return ds;
        }


        public DataSet GetCumulativeEnergy(int meterDataId,bool isConsuption)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DataIndex,");
                builder.Append("BillingDate,");
                builder.Append("CumulativeEnergykWhTZ0,");
                builder.Append("CumulativeEnergykVAhTZ0,");
                builder.Append("CumulativeEnergykvarhLag,");
                builder.Append("CumulativeEnergykvarhLead,");
                builder.Append("BillingResetType,");
                builder.Append("CumulativeEnergykWhTZ0Import,");
                builder.Append("CumulativeEnergykVAhTZ0Import,");
                builder.Append("CumulativeEnergykWhTZ0Export,");
                builder.Append("CumulativeEnergykVAhTZ0Export,");
                builder.Append("CumulativeEnergykWhTZ0Net,");
                builder.Append("CumulativeEnergykVAhTZ0Net,");
                builder.Append("CumulativeEnergykvarhLagQ1,");
                builder.Append("CumulativeEnergykvarhLeadQ4,");
                builder.Append("CumulativeEnergykvarhLagQ3,");
                builder.Append("CumulativeEnergykvarhLeadQ2,");
                builder.Append("CumEnergykWhRPhase,");
                builder.Append("CumEnergykWhYPhase,");
                builder.Append("CumEnergykWhBPhase,");
                #region JDVVNL
                builder.Append("MinimumVoltageLSIPAcrossDayRPhase,");
                builder.Append("MinimumVoltageLSIPAcrossDayYPhase,");
                builder.Append("MinimumVoltageLSIPAcrossDayBPhase,");
             
                #endregion
                builder.Append("CumulativeEnergyFraudkWh,");
                builder.Append("CumulativeEnergyFraudkVAh,");
                builder.Append("kWhLag,");
                builder.Append("kWhLead,");
                builder.Append("kVAhLag,");
                builder.Append("kVAhLead");             


                if (!isConsuption) // Story - 365971 - 13 billing for Power ON Hours
                    //builder.Append(string.Concat(" from meterdata_billing where DataIndex <13 and ")); // Story - 365971 - 13 billing for Power ON Hours
                    builder.Append(string.Concat(" from meterdata_billing where DataIndex <61 and ")); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                else
                    builder.Append(string.Concat(" from meterdata_billing where "));
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(" order by DataIndex asc");

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCumulativeEnergy(int meterDataId,bool isConsuption)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// Get TOD details meterwise
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetTODDetails(int meterDataId,bool isConsumption)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DataIndex,");
                builder.Append("BillingDate");
                if (!isConsumption) // Story - 365971 - 13 billing for Power ON Hours
                    //builder.Append(" from meterdata_billing where DataIndex < 13 and "); // Story - 365971 - 13 billing for Power ON Hours
                    builder.Append(" from meterdata_billing where DataIndex < 61 and "); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                else
                    builder.Append(" from meterdata_billing where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(" order by DataIndex asc");

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTODDetails(int meterDataId,bool isConsumption)", ex);
                dataSet = null;
            }
            return dataSet;
        }




      


        /// <summary>
        /// Gets the billing month for a particular MeterDataID
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingMonths(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DataIndex,");
                builder.Append("BillingDate,");
                builder.Append("DATE_FORMAT(SUBSTRING(BillingDate, 1, 8),'%b') as BillingMonth");
                builder.Append(" from meterdata_billing where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(" order by DataIndex asc");

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingMonths(int meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// This method is used to fetch the cum power off duration and cum tamper count.
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <param name="historyID"></param>
        /// <returns></returns>
        public DataSet GetMiscellaneous(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //Added dataindex column in query to solve 95898.
                // builder.Append("Select DataIndex,CumTamperCount");
                builder.Append("Select DataIndex,BillingDate,CumTamperCount,CumPowerFailureCount,CumBillingMDResetCount,DeltaTamperCount,ABCCodeBilling"); // Story - 345154
                //builder.Append(" from meterdata_billing where DataIndex < 13 and "); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" from meterdata_billing where DataIndex < 61 and "); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                builder.Append(" order by DataIndex asc");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int32);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMiscellaneous(int meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;

        }
        public DataSet GetMeterData(long meterDataId, int historyID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CumulativeEnergykWhTZ0,CumulativeEnergykWhTZ1,CumulativeEnergykWhTZ2,CumulativeEnergykWhTZ3,CumulativeEnergykWhTZ4,CumulativeEnergykWhTZ5,CumulativeEnergykWhTZ6,CumulativeEnergykWhTZ7,CumulativeEnergykWhTZ8,CumulativeEnergykVAhTZ0,CumulativeEnergykVAhTZ1,CumulativeEnergykVAhTZ2,CumulativeEnergykVAhTZ3,CumulativeEnergykVAhTZ4,CumulativeEnergykVAhTZ5,CumulativeEnergykVAhTZ6,CumulativeEnergykVAhTZ7,CumulativeEnergykVAhTZ8,DataIndex, CumulativeEnergykvarhLagTZ1, CumulativeEnergykvarhLagTZ2,CumulativeEnergykvarhLagTZ3,  CumulativeEnergykvarhLagTZ4, CumulativeEnergykvarhLagTZ5, CumulativeEnergykvarhLagTZ6, CumulativeEnergykvarhLagTZ7, CumulativeEnergykvarhLagTZ8, CumulativeEnergykvarhLeadTZ1, CumulativeEnergykvarhLeadTZ2, CumulativeEnergykvarhLeadTZ3, CumulativeEnergykvarhLeadTZ4,CumulativeEnergykvarhLeadTZ5,CumulativeEnergykvarhLeadTZ6, CumulativeEnergykvarhLeadTZ7, CumulativeEnergykvarhLeadTZ8"
                       + ",CumulativeEnergykWhTZ0Import,CumulativeEnergykWhTZ1Import,CumulativeEnergykWhTZ2Import,CumulativeEnergykWhTZ3Import"
                    + ",CumulativeEnergykWhTZ4Import,CumulativeEnergykWhTZ5Import,CumulativeEnergykWhTZ6Import,CumulativeEnergykWhTZ7Import"
                    + ",CumulativeEnergykWhTZ8Import"
                    + ",CumulativeEnergykVAhTZ0Import,CumulativeEnergykVAhTZ1Import,CumulativeEnergykVAhTZ2Import,CumulativeEnergykVAhTZ3Import,CumulativeEnergykVAhTZ4Import"
                    + ",CumulativeEnergykVAhTZ5Import,CumulativeEnergykVAhTZ6Import,CumulativeEnergykVAhTZ7Import,CumulativeEnergykVAhTZ8Import"
                    + ",CumulativeEnergykWhTZ0Export,CumulativeEnergykWhTZ1Export,CumulativeEnergykWhTZ2Export,CumulativeEnergykWhTZ3Export"
                    + ",CumulativeEnergykWhTZ4Export,CumulativeEnergykWhTZ5Export,CumulativeEnergykWhTZ6Export,CumulativeEnergykWhTZ7Export"
                    + ",CumulativeEnergykWhTZ8Export"
                    + ",CumulativeEnergykVAhTZ0Export,CumulativeEnergykVAhTZ1Export,CumulativeEnergykVAhTZ2Export,CumulativeEnergykVAhTZ3Export,CumulativeEnergykVAhTZ4Export"
                    + ",CumulativeEnergykVAhTZ5Export,CumulativeEnergykVAhTZ6Export,CumulativeEnergykVAhTZ7Export,CumulativeEnergykVAhTZ8Export"
                    + ",CumulativeEnergykvarhLagTZ1Q1, CumulativeEnergykvarhLagTZ2Q1,CumulativeEnergykvarhLagTZ3Q1,  CumulativeEnergykvarhLagTZ4Q1, CumulativeEnergykvarhLagTZ5Q1, CumulativeEnergykvarhLagTZ6Q1, CumulativeEnergykvarhLagTZ7Q1, CumulativeEnergykvarhLagTZ8Q1, CumulativeEnergykvarhLeadTZ1Q4, CumulativeEnergykvarhLeadTZ2Q4, CumulativeEnergykvarhLeadTZ3Q4, CumulativeEnergykvarhLeadTZ4Q4,CumulativeEnergykvarhLeadTZ5Q4,CumulativeEnergykvarhLeadTZ6Q4, CumulativeEnergykvarhLeadTZ7Q4, CumulativeEnergykvarhLeadTZ8Q4,CumulativeEnergykvarhLagTZ1Q3, CumulativeEnergykvarhLagTZ2Q3,CumulativeEnergykvarhLagTZ3Q3,  CumulativeEnergykvarhLagTZ4Q3, CumulativeEnergykvarhLagTZ5Q3, CumulativeEnergykvarhLagTZ6Q3, CumulativeEnergykvarhLagTZ7Q3, CumulativeEnergykvarhLagTZ8Q3, CumulativeEnergykvarhLeadTZ1Q2, CumulativeEnergykvarhLeadTZ2Q2, CumulativeEnergykvarhLeadTZ3Q2, CumulativeEnergykvarhLeadTZ4Q2,CumulativeEnergykvarhLeadTZ5Q2,CumulativeEnergykvarhLeadTZ6Q2, CumulativeEnergykvarhLeadTZ7Q2, CumulativeEnergykvarhLeadTZ8Q2"
                 + ",case when CumulativeEnergykWhTZ0Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ0Net'"
+ ",case when CumulativeEnergykWhTZ1Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ1Net'"
+ ",case when CumulativeEnergykWhTZ2Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ2Net'"
+ ",case when CumulativeEnergykWhTZ3Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ3Net'"
+ ",case when CumulativeEnergykWhTZ4Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ4Net'"
+ ",case when CumulativeEnergykWhTZ5Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ5Net'"
+ ",case when CumulativeEnergykWhTZ6Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ6Net'"
+ ",case when CumulativeEnergykWhTZ7Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ7Net'"
+ ",case when CumulativeEnergykWhTZ8Net IS NULL then '0*' end as 'CumulativeEnergykWhTZ8Net'"
+ ",case when CumulativeEnergykVAhTZ0Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ0Net'"
+ ",case when CumulativeEnergykVAhTZ1Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ1Net'"
+ ",case when CumulativeEnergykVAhTZ2Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ2Net'"
+ ",case when CumulativeEnergykVAhTZ3Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ3Net'"
+ ",case when CumulativeEnergykVAhTZ4Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ4Net'"
+ ",case when CumulativeEnergykVAhTZ5Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ5Net'"
+ ",case when CumulativeEnergykVAhTZ6Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ6Net'"
+ ",case when CumulativeEnergykVAhTZ7Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ7Net'"
+ ",case when CumulativeEnergykVAhTZ8Net IS NULL then '0*' end as 'CumulativeEnergykVAhTZ8Net'"
+ ",CumulativeEnergyFraudkWh"
+ ",CumulativeEnergyFraudkVAh"
                  + ",DATE_FORMAT(SUBSTRING(BillingDate, 1, 8),'%b') as BillingMonth");
                builder.Append(" from meterdata_billing where "); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(DataIndex, "=", ParameterName(DataIndex)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName(DataIndex), historyID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterData(long meterDataId, int historyID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetMaximumDemand(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DataIndex,");
                builder.Append("BillingDate,");
                builder.Append("MDkWTZ0,");
                builder.Append("MDkWDateTimeTZ0,");
                builder.Append("MDkVATZ0,");
                builder.Append("MDkVADateTimeTZ0,");
                builder.Append("RPhaseMDkW,");
                builder.Append("RPhaseMDDateTime,");
                builder.Append("YPhaseMDkW,");
                builder.Append("YPhaseMDDateTime,");
                builder.Append("BPhaseMDkW,");
                builder.Append("BPhaseMDDateTime,");
                builder.Append("MDkWTZ0Import,");
                builder.Append("MDkWDateTimeTZ0Import,");
                builder.Append("MDkVATZ0Import,");
                builder.Append("MDkVADateTimeTZ0Import,");
                builder.Append("MDkWTZ0Export,");
                builder.Append("MDkWDateTimeTZ0Export,");
                builder.Append("MDkVATZ0Export,");
                builder.Append("MDkVADateTimeTZ0Export,");

                // User Story - 1000867
                builder.Append("MDkVArLagTZ0,");
                builder.Append("MDkVArLagDateTimeTZ0,");
                builder.Append("MDkVArLeadTZ0,");
                builder.Append("MDkVArLeadDateTimeTZ0 ");

                //builder.Append(" from meterdata_billing where DataIndex < 13 and "); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" from meterdata_billing where DataIndex < 61 and "); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMaximumDemand(int meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet GetCumulMD(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select DataIndex,");
                builder.Append("BillingDate,");
                builder.Append("CumulativeMDkW,");
                builder.Append("CumulativeMDkva");
                             
                builder.Append(" from meterdata_billing where DataIndex < 61 and "); 
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.UInt32);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetCumulMD(int meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }


        // Added for Billing Report by Swati Chaudhary
        public DataSet GetBillingReport()
        {
            DataSet dataset = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select a.*,b.* from (select a.Consumer_Number,a.Consumer_Name,b.Meter_ID from consumer_master a inner join consumermeter b on a.Consumer_Number = b.Consumer_Number) as a,");
                builder.Append(" (SELECT b.MeterId, a.Billing_ID,a.BillingDate,a.SystemPowerFactorforBillingPeriod,a.CumulativeEnergykWhTZ0,a.CumulativeEnergykvarhLag,a.CumulativeEnergykvarhLead,");
                builder.Append(" a.CumulativeEnergykVAhTZ0,a.MDkWTZ0,a.MDkWDateTimeTZ0,a.MDkVATZ0,a.MDkVADateTimeTZ0,");
                // User Story - 1000867
                builder.Append("a.MDkVArLagTZ0,a.MDkVArLagDateTimeTZ0,a.MDkVArLeadTZ0,a.MDkVArLeadDateTimeTZ0,");
                builder.Append(" a.MeterData_ID,a.DataIndex ");

                builder.Append(" from meterdata_billing a, meterdata b where a.meterdata_id = b.meterdata_id and a.DataIndex = 1) as b where a.Meter_ID = b.MeterId ");
                DataRequest request = new DataRequest(builder.ToString());
                dataset = helper.FillDataSet(request, dataset);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingReport()", ex);
                dataset = null;
            }
            return dataset;


        }
        // Added to get the Billing Data by Group
        public DataSet GetBillingReportByGroup(int GroupID)
        {
            DataSet dataset = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select a. * ,b.* from (select a.Consumer_Number,a.Consumer_Name,b.Meter_ID from consumer_master a inner join consumermeter b on a.Consumer_Number = b.Consumer_Number) as a,");
                builder.Append(" (select c.* from (SELECT b.MeterId, a.Billing_ID,a.BillingDate,a.SystemPowerFactorforBillingPeriod,");
                builder.Append(" a.CumulativeEnergykWhTZ0,a.CumulativeEnergykvarhLag,a.CumulativeEnergykvarhLead,");
                builder.Append(" a.CumulativeEnergykVAhTZ0,a.MDkWTZ0,a.MDkWDateTimeTZ0,a.MDkVATZ0,a.MDkVADateTimeTZ0,");

                // User Story - 1000867
                builder.Append("a.MDkVArLagTZ0,a.MDkVArLagDateTimeTZ0,a.MDkVArLeadTZ0,a.MDkVArLeadDateTimeTZ0,");
                builder.Append(" a.MeterData_ID,a.DataIndex ");

                builder.Append(" FROM meterdata_billing a, meterdata b where a.meterdata_id = b.meterdata_id and a.DataIndex = 1 ) as c");
                builder.Append(" , gsm_group_meters d where ");
                builder.Append(string.Concat(Group_ID, "=", ParameterName(Group_ID)));
                builder.Append(" and  c.MeterId = d.Meter_ID) as b where a.Meter_ID = b.MeterId");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_ID), GroupID, DbType.Int32);
                dataset = helper.FillDataSet(request, dataset);

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingReportByGroup(int GroupID)", ex);
            }
            return dataset;
        }


        public DataSet GetTODMDMeterData(long meterDataId, int historyID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");
                builder.Append("MDkWTZ0,MDkWDateTimeTZ0,MDkWTZ1,MDkWDateTimeTZ1,MDkWTZ2,MDkWDateTimeTZ2,MDkWTZ3,MDkWDateTimeTZ3,");
                builder.Append("MDkWTZ4,MDkWDateTimeTZ4,MDkWTZ5,MDkWDateTimeTZ5,MDkWTZ6,MDkWDateTimeTZ6,MDkWTZ7,MDkWDateTimeTZ7,MDkWTZ8,MDkWDateTimeTZ8,MDkVATZ0,MDkVADateTimeTZ0,");
                builder.Append("MDkVATZ1,MDkVADateTimeTZ1,MDkVATZ2,MDkVADateTimeTZ2,MDkVATZ3,MDkVADateTimeTZ3,MDkVATZ4,MDkVADateTimeTZ4,MDkVATZ5,MDkVADateTimeTZ5,MDkVATZ6,MDkVADateTimeTZ6,");
                builder.Append("MDkVATZ7,MDkVADateTimeTZ7,MDkVATZ8,MDkVADateTimeTZ8,DataIndex,DATE_FORMAT(SUBSTRING(BillingDate, 1, 8),'%b') as BillingMonth,");

                 builder.Append("MDkWTZ0Import,MDkWDateTimeTZ0Import,MDkWTZ1Import,MDkWDateTimeTZ1Import,MDkWTZ2Import,MDkWDateTimeTZ2Import,MDkWTZ3Import,MDkWDateTimeTZ3Import,");

                builder.Append("MDkWTZ4Import,MDkWDateTimeTZ4Import,MDkWTZ5Import,MDkWDateTimeTZ5Import,MDkWTZ6Import,MDkWDateTimeTZ6Import,MDkWTZ7Import,MDkWDateTimeTZ7Import,MDkWTZ8Import,MDkWDateTimeTZ8Import,MDkVATZ0Import,MDkVADateTimeTZ0Import,");

                builder.Append("MDkVATZ1Import,MDkVADateTimeTZ1Import,MDkVATZ2Import,MDkVADateTimeTZ2Import,MDkVATZ3Import,MDkVADateTimeTZ3Import,MDkVATZ4Import,MDkVADateTimeTZ4Import,MDkVATZ5Import,MDkVADateTimeTZ5Import,MDkVATZ6Import,MDkVADateTimeTZ6Import,");


                builder.Append("MDkVATZ7Import,MDkVADateTimeTZ7Import,MDkVATZ8Import,MDkVADateTimeTZ8Import,");



                builder.Append("MDkWTZ0Export,MDkWDateTimeTZ0Export,MDkWTZ1Export,MDkWDateTimeTZ1Export,MDkWTZ2Export,MDkWDateTimeTZ2Export,MDkWTZ3Export,MDkWDateTimeTZ3Export,");
                builder.Append("MDkWTZ4Export,MDkWDateTimeTZ4Export,MDkWTZ5Export,MDkWDateTimeTZ5Export,MDkWTZ6Export,MDkWDateTimeTZ6Export,MDkWTZ7Export,MDkWDateTimeTZ7Export,MDkWTZ8Export,MDkWDateTimeTZ8Export,MDkVATZ0Export,MDkVADateTimeTZ0Export,");
                builder.Append("MDkVATZ1Export,MDkVADateTimeTZ1Export,MDkVATZ2Export,MDkVADateTimeTZ2Export,MDkVATZ3Export,MDkVADateTimeTZ3Export,MDkVATZ4Export,MDkVADateTimeTZ4Export,MDkVATZ5Export,MDkVADateTimeTZ5Export,MDkVATZ6Export,MDkVADateTimeTZ6Export,");
                builder.Append("MDkVATZ7Export,MDkVADateTimeTZ7Export,MDkVATZ8Export,MDkVADateTimeTZ8Export");
                //builder.Append(" from meterdata_billing where DataIndex < 13 and "); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" from meterdata_billing where DataIndex < 61 and "); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(DataIndex, "=", ParameterName(DataIndex)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName(DataIndex), historyID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTODMDMeterData(long meterDataId, int historyID)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        public DataSet GetAveragePowerFactor(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select DataIndex,BillingDate,SystemPowerFactorforBillingPeriod,SystemPowerFactorImportforBillingPeriod,SystemPowerFactorExportforBillingPeriod  from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing Power ON Hours
                builder.Append("Select DataIndex,BillingDate,SystemPowerFactorforBillingPeriod,SystemPowerFactorImportforBillingPeriod,SystemPowerFactorExportforBillingPeriod  from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                //if (dataSet != null && dataSet.Tables != null && dataSet.Tables[0].Rows.Count > 0)
                //{
                //    dataSet.Tables[0].Rows.RemoveAt(0);
                //}
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAveragePowerFactor(int meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet GetMeterCategory(long meterDataId)//pradipta_loadfactor
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                
                builder.Append("Select Category from meterdata_general where ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterCategory(long meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingAverageLoadFactor(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select DataIndex,BillingDate,ifnull(BillingAvgLoadFactor,'---------') as BillingAvgLoadFactor from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing Power ON Hours
                //builder.Append("Select DataIndex,BillingDate,ifnull(BillingAvgLoadFactor,'---------') as BillingAvgLoadFactor from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                //pradipta_start_081018

                builder.Append("Select DataIndex,BillingDate,ifnull(BillingAvgLoadFactor,'---------') as BillingAvgLoadFactor ,ifnull(BillingAvgkWImportLoadFactor,'---------') as BillingAvgkWImportLoadFactor,ifnull(BillingAvgkWExportLoadFactor,'---------') as BillingAvgkWExportLoadFactor,ifnull(BillingAvgkVAImportLoadFactor,'---------') as BillingAvgkVAImportLoadFactor,ifnull(BillingAvgkVAExportLoadFactor,'---------') as BillingAvgkVAExportLoadFactor from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement

                //pradipta_End_081018


                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingAverageLoadFactor(long meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }


        /// <summary>
        /// Average Load for Billing 
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingAverageLoad(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append("Select DataIndex,BillingDate,ifnull(BillingAverageLoad,'---------') as BillingAverageLoad from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing Power ON Hours
                builder.Append("Select DataIndex,BillingDate,ifnull(BillingAverageLoad,'---------') as BillingAverageLoad from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingAverageLoad(long meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }



        private DataRequest GetRequest(IEntity entity)
        {
            DLMS650BillingEntity dLMS650BillingEntity = entity as DLMS650BillingEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into meterdata_billing (BillingDate"
                //Net Start
                + ",CumulativeEnergykWhTZ0Net,CumulativeEnergykWhTZ1Net,CumulativeEnergykWhTZ2Net,CumulativeEnergykWhTZ3Net"
+ ",CumulativeEnergykWhTZ4Net,CumulativeEnergykWhTZ5Net,CumulativeEnergykWhTZ6Net,CumulativeEnergykWhTZ7Net"
+ ",CumulativeEnergykWhTZ8Net"
+ ",CumulativeEnergykVAhTZ0Net,CumulativeEnergykVAhTZ1Net,CumulativeEnergykVAhTZ2Net,CumulativeEnergykVAhTZ3Net,CumulativeEnergykVAhTZ4Net"
+ ",CumulativeEnergykVAhTZ5Net,CumulativeEnergykVAhTZ6Net,CumulativeEnergykVAhTZ7Net,CumulativeEnergykVAhTZ8Net"
+ ",MDkWTZ0Net,MDkWTZ1Net,MDkWTZ2Net,MDkWTZ3Net,MDkWTZ4Net,MDkWTZ5Net,MDkWTZ6Net,MDkWTZ7Net,MDkWTZ8Net"
+ ",MDkVATZ0Net,MDkVATZ1Net,MDkVATZ2Net,MDkVATZ3Net,MDkVATZ4Net,MDkVATZ5Net,MDkVATZ6Net,MDkVATZ7Net,MDkVATZ8Net"
+ ",MDkWDateTimeTZ0Net,MDkWDateTimeTZ1Net,MDkWDateTimeTZ2Net,MDkWDateTimeTZ3Net,MDkWDateTimeTZ4Net,MDkWDateTimeTZ5Net,MDkWDateTimeTZ6Net,MDkWDateTimeTZ7Net,MDkWDateTimeTZ8Net"
+ ",MDkVADateTimeTZ0Net,MDkVADateTimeTZ1Net,MDkVADateTimeTZ2Net,MDkVADateTimeTZ3Net,MDkVADateTimeTZ4Net,MDkVADateTimeTZ5Net,MDkVADateTimeTZ6Net,MDkVADateTimeTZ7Net,MDkVADateTimeTZ8Net"
                //Net End
                //Import Start
                + ",CumulativeEnergykWhTZ0Import,CumulativeEnergykWhTZ1Import,CumulativeEnergykWhTZ2Import,CumulativeEnergykWhTZ3Import"
+ ",CumulativeEnergykWhTZ4Import,CumulativeEnergykWhTZ5Import,CumulativeEnergykWhTZ6Import,CumulativeEnergykWhTZ7Import"
+ ",CumulativeEnergykWhTZ8Import"
+ ",CumulativeEnergykVAhTZ0Import,CumulativeEnergykVAhTZ1Import,CumulativeEnergykVAhTZ2Import,CumulativeEnergykVAhTZ3Import,CumulativeEnergykVAhTZ4Import"
+ ",CumulativeEnergykVAhTZ5Import,CumulativeEnergykVAhTZ6Import,CumulativeEnergykVAhTZ7Import,CumulativeEnergykVAhTZ8Import"
+ ",MDkWTZ0Import,MDkWTZ1Import,MDkWTZ2Import,MDkWTZ3Import,MDkWTZ4Import,MDkWTZ5Import,MDkWTZ6Import,MDkWTZ7Import,MDkWTZ8Import"
+ ",MDkVATZ0Import,MDkVATZ1Import,MDkVATZ2Import,MDkVATZ3Import,MDkVATZ4Import,MDkVATZ5Import,MDkVATZ6Import,MDkVATZ7Import,MDkVATZ8Import"
+ ",MDkWDateTimeTZ0Import,MDkWDateTimeTZ1Import,MDkWDateTimeTZ2Import,MDkWDateTimeTZ3Import,MDkWDateTimeTZ4Import,MDkWDateTimeTZ5Import,MDkWDateTimeTZ6Import,MDkWDateTimeTZ7Import,MDkWDateTimeTZ8Import"
+ ",MDkVADateTimeTZ0Import,MDkVADateTimeTZ1Import,MDkVADateTimeTZ2Import,MDkVADateTimeTZ3Import,MDkVADateTimeTZ4Import,MDkVADateTimeTZ5Import,MDkVADateTimeTZ6Import,MDkVADateTimeTZ7Import,MDkVADateTimeTZ8Import"
                //Import End
                //Export Start
                        + ",CumulativeEnergykWhTZ0Export,CumulativeEnergykWhTZ1Export,CumulativeEnergykWhTZ2Export,CumulativeEnergykWhTZ3Export"
                        + ",CumulativeEnergykWhTZ4Export,CumulativeEnergykWhTZ5Export,CumulativeEnergykWhTZ6Export,CumulativeEnergykWhTZ7Export"
                        + ",CumulativeEnergykWhTZ8Export"
                        + ",CumulativeEnergykVAhTZ0Export,CumulativeEnergykVAhTZ1Export,CumulativeEnergykVAhTZ2Export,CumulativeEnergykVAhTZ3Export,CumulativeEnergykVAhTZ4Export"
                        + ",CumulativeEnergykVAhTZ5Export,CumulativeEnergykVAhTZ6Export,CumulativeEnergykVAhTZ7Export,CumulativeEnergykVAhTZ8Export"
                        + ",MDkWTZ0Export,MDkWTZ1Export,MDkWTZ2Export,MDkWTZ3Export,MDkWTZ4Export,MDkWTZ5Export,MDkWTZ6Export,MDkWTZ7Export,MDkWTZ8Export"
                        + ",MDkVATZ0Export,MDkVATZ1Export,MDkVATZ2Export,MDkVATZ3Export,MDkVATZ4Export,MDkVATZ5Export,MDkVATZ6Export,MDkVATZ7Export,MDkVATZ8Export"
                        + ",MDkWDateTimeTZ0Export,MDkWDateTimeTZ1Export,MDkWDateTimeTZ2Export,MDkWDateTimeTZ3Export,MDkWDateTimeTZ4Export,MDkWDateTimeTZ5Export,MDkWDateTimeTZ6Export,MDkWDateTimeTZ7Export,MDkWDateTimeTZ8Export"
                        + ",MDkVADateTimeTZ0Export,MDkVADateTimeTZ1Export,MDkVADateTimeTZ2Export,MDkVADateTimeTZ3Export,MDkVADateTimeTZ4Export,MDkVADateTimeTZ5Export,MDkVADateTimeTZ6Export,MDkVADateTimeTZ7Export,MDkVADateTimeTZ8Export"
                //Export End
                + ",CumulativeEnergykvarhLagQ1,CumulativeEnergykvarhLagTZ1Q1,CumulativeEnergykvarhLagTZ2Q1,CumulativeEnergykvarhLagTZ3Q1,CumulativeEnergykvarhLagTZ4Q1,CumulativeEnergykvarhLagTZ5Q1,CumulativeEnergykvarhLagTZ6Q1,CumulativeEnergykvarhLagTZ7Q1,CumulativeEnergykvarhLagTZ8Q1"
+ ",CumulativeEnergykvarhLeadQ4,CumulativeEnergykvarhLeadTZ1Q4,CumulativeEnergykvarhLeadTZ2Q4,CumulativeEnergykvarhLeadTZ3Q4,CumulativeEnergykvarhLeadTZ4Q4,CumulativeEnergykvarhLeadTZ5Q4,CumulativeEnergykvarhLeadTZ6Q4,CumulativeEnergykvarhLeadTZ7Q4,CumulativeEnergykvarhLeadTZ8Q4"  
                         + ",CumulativeEnergykvarhLagQ3,CumulativeEnergykvarhLagTZ1Q3,CumulativeEnergykvarhLagTZ2Q3,CumulativeEnergykvarhLagTZ3Q3,CumulativeEnergykvarhLagTZ4Q3,CumulativeEnergykvarhLagTZ5Q3,CumulativeEnergykvarhLagTZ6Q3,CumulativeEnergykvarhLagTZ7Q3,CumulativeEnergykvarhLagTZ8Q3"
                        + ",CumulativeEnergykvarhLeadQ2,CumulativeEnergykvarhLeadTZ1Q2,CumulativeEnergykvarhLeadTZ2Q2,CumulativeEnergykvarhLeadTZ3Q2,CumulativeEnergykvarhLeadTZ4Q2,CumulativeEnergykvarhLeadTZ5Q2,CumulativeEnergykvarhLeadTZ6Q2,CumulativeEnergykvarhLeadTZ7Q2,CumulativeEnergykvarhLeadTZ8Q2"
                        + ",SystemPowerFactorforBillingPeriod,SystemPowerFactorImportforBillingPeriod,SystemPowerFactorExportforBillingPeriod,CumEnergykWhRPhase,CumEnergykWhYPhase,CumEnergykWhBPhase,CumulativeEnergykWhTZ0"
                        + ",CumulativeEnergykWhTZ1,CumulativeEnergykWhTZ2,CumulativeEnergykWhTZ3,CumulativeEnergykWhTZ4,CumulativeEnergykWhTZ5,CumulativeEnergykWhTZ6"
                        + ",CumulativeEnergykWhTZ7,CumulativeEnergykWhTZ8,CumulativeEnergykvarhLag,CumulativeEnergykvarhLead,CumulativeEnergykVAhTZ0,CumulativeEnergykVAhTZ1"
                        + ",CumulativeEnergykVAhTZ2,CumulativeEnergykVAhTZ3,CumulativeEnergykVAhTZ4,CumulativeEnergykVAhTZ5,CumulativeEnergykVAhTZ6,CumulativeEnergykVAhTZ7"
                        + ",CumulativeEnergykVAhTZ8,MDkWTZ0,MDkWDateTimeTZ0,MDkWTZ1,MDkWDateTimeTZ1,MDkWTZ2,MDkWDateTimeTZ2,MDkWTZ3,MDkWDateTimeTZ3"
                        + ",MDkWTZ4,MDkWDateTimeTZ4,MDkWTZ5,MDkWDateTimeTZ5,MDkWTZ6,MDkWDateTimeTZ6,MDkWTZ7,MDkWDateTimeTZ7,MDkWTZ8,MDkWDateTimeTZ8,MDkVATZ0,MDkVADateTimeTZ0"
                        + ",MDkVATZ1,MDkVADateTimeTZ1,MDkVATZ2,MDkVADateTimeTZ2,MDkVATZ3,MDkVADateTimeTZ3,MDkVATZ4,MDkVADateTimeTZ4,MDkVATZ5,MDkVADateTimeTZ5,MDkVATZ6,MDkVADateTimeTZ6"
                        + ",MDkVATZ7,MDkVADateTimeTZ7,MDkVATZ8,MDkVADateTimeTZ8,PowerOffDuration,BillingWisePowerOffDuration,BillingAvgLoadFactor,BillingAvgkWImportLoadFactor,BillingAvgkWExportLoadFactor,BillingAvgkVAImportLoadFactor,BillingAvgkVAExportLoadFactor,PowerOnDuration,CumPowerOnDuration,PowerOnDurationDisplay,CumBillingMDResetCount,DeltaTamperCount" // Story - 345154
                        + ",CumulativeEnergykvarhLagTZ1,CumulativeEnergykvarhLagTZ2,CumulativeEnergykvarhLagTZ3,CumulativeEnergykvarhLagTZ4,CumulativeEnergykvarhLagTZ5,CumulativeEnergykvarhLagTZ6,CumulativeEnergykvarhLagTZ7,CumulativeEnergykvarhLagTZ8"
                        + ",CumulativeEnergykvarhLeadTZ1,CumulativeEnergykvarhLeadTZ2,CumulativeEnergykvarhLeadTZ3,CumulativeEnergykvarhLeadTZ4,CumulativeEnergykvarhLeadTZ5,CumulativeEnergykvarhLeadTZ6,CumulativeEnergykvarhLeadTZ7,CumulativeEnergykvarhLeadTZ8"
            + ",TODAveragePowerFactorTZ1,TODAveragePowerFactorTZ2,TODAveragePowerFactorTZ3,TODAveragePowerFactorTZ4,TODAveragePowerFactorTZ5,TODAveragePowerFactorTZ6,TODAveragePowerFactorTZ7,TODAveragePowerFactorTZ8,TODAverageExportPowerFactorTZ1,TODAverageExportPowerFactorTZ2,TODAverageExportPowerFactorTZ3,TODAverageExportPowerFactorTZ4,TODAverageExportPowerFactorTZ5,TODAverageExportPowerFactorTZ6,TODAverageExportPowerFactorTZ7,TODAverageExportPowerFactorTZ8,RPhaseMDDateTime,YPhaseMDDateTime,BPhaseMDDateTime,RPhaseMDkW,YPhaseMDkW,BPhaseMDkW,MinimumVoltageLSIPAcrossDayRPhase,MinimumVoltageLSIPAcrossDayYPhase,MinimumVoltageLSIPAcrossDayBPhase,BillingAverageLoad,");//story 1024441 Add TOD Export PF
            //if (isMPKWCL)
            //{
            //    builder.Append("CumPowerOffDuration,CumTamperCount,");

            //}

         
            builder.Append("CumTamperCount,CumPowerFailureCount,");
            builder.Append("BillingResetType,");
            builder.Append("MeterData_ID,DataIndex,");
            builder.Append("ABCCodeBilling,CumulativeMDkW,");
            builder.Append("CumulativeMDkva,");
            builder.Append("CumulativeEnergyFraudkWh, CumulativeEnergyFraudkVAh,");
            builder.Append("MDkVArLagTZ0, MDkVArLagDateTimeTZ0, MDkVArLeadTZ0, MDkVArLeadDateTimeTZ0, kWhLag, kWhLead, kVAhLag, kVAhLead"); // User Story - 1000867
            builder.Append(")values(");
            builder.Append(string.Concat(ParameterName(BillingDate), ","));
            //Net Start
            //KWH - Net
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ0Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ1Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ2Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ3Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ4Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ5Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ6Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ7Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ8Net), ","));
            //KVAH - Net
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ0Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ1Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ2Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ3Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ4Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ5Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ6Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ7Net), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ8Net), ","));
            //MD - KW - Net
            builder.Append(string.Concat(ParameterName(MDkWTZ0Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ1Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ2Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ3Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ4Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ5Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ6Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ7Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ8Net), ","));
            //MD - KVA - Net
            builder.Append(string.Concat(ParameterName(MDkVATZ0Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ1Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ2Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ3Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ4Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ5Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ6Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ7Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ8Net), ","));
            //MD - KW - DateTime - Net
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ0Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ1Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ2Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ3Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ4Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ5Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ6Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ7Net), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ8Net), ","));
            //MD - KVA - DateTime - Net
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ0Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ1Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ2Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ3Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ4Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ5Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ6Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ7Net), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ8Net), ","));
            //Net End
            //Import Start
            //KWH - Import
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ0Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ1Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ2Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ3Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ4Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ5Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ6Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ7Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ8Import), ","));
            //KVAH - Import
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ0Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ1Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ2Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ3Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ4Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ5Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ6Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ7Import), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ8Import), ","));
            //MD - KW - Import
            builder.Append(string.Concat(ParameterName(MDkWTZ0Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ1Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ2Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ3Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ4Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ5Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ6Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ7Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ8Import), ","));
            //MD - KVA - Import
            builder.Append(string.Concat(ParameterName(MDkVATZ0Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ1Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ2Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ3Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ4Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ5Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ6Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ7Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ8Import), ","));
            //MD - KW - DateTime - Import
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ0Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ1Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ2Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ3Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ4Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ5Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ6Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ7Import), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ8Import), ","));
            //MD - KVA - DateTime - Import
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ0Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ1Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ2Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ3Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ4Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ5Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ6Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ7Import), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ8Import), ","));
            //Import End
            //Export Start
            //KWH - Export
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ0Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ1Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ2Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ3Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ4Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ5Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ6Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ7Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ8Export), ","));
            //KVAH - Export
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ0Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ1Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ2Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ3Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ4Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ5Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ6Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ7Export), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ8Export), ","));
            //MD - KW - Export
            builder.Append(string.Concat(ParameterName(MDkWTZ0Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ1Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ2Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ3Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ4Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ5Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ6Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ7Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ8Export), ","));
            //MD - KVA - Export
            builder.Append(string.Concat(ParameterName(MDkVATZ0Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ1Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ2Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ3Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ4Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ5Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ6Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ7Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ8Export), ","));
            //MD - KW - DateTime - Export
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ0Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ1Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ2Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ3Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ4Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ5Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ6Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ7Export), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ8Export), ","));
            //MD - KVA - DateTime - Export
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ0Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ1Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ2Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ3Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ4Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ5Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ6Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ7Export), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ8Export), ","));
            //Export End
            //Cumulative-Energy-Lag-Q1
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagQ1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ1Q1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ2Q1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ3Q1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ4Q1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ5Q1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ6Q1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ7Q1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ8Q1), ","));
            //Cumulative-Energy-Lead-Q4
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadQ4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ1Q4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ2Q4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ3Q4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ4Q4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ5Q4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ6Q4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ7Q4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ8Q4), ",")); 
            //Cumulative-Energy-Lag-Q3
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagQ3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ1Q3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ2Q3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ3Q3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ4Q3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ5Q3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ6Q3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ7Q3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ8Q3), ","));
            //Cumulative-Energy-Lead-Q2
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadQ2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ1Q2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ2Q2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ3Q2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ4Q2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ5Q2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ6Q2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ7Q2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ8Q2), ",")); 

            
            builder.Append(string.Concat(ParameterName(SystemPowerFactorforBillingPeriod), ","));
            builder.Append(string.Concat(ParameterName(SystemPowerFactorImportforBillingPeriod), ","));
            builder.Append(string.Concat(ParameterName(SystemPowerFactorExportforBillingPeriod), ","));
            builder.Append(string.Concat(ParameterName(CumEnergykWhRPhase), ","));
            builder.Append(string.Concat(ParameterName(CumEnergykWhYPhase), ","));
            builder.Append(string.Concat(ParameterName(CumEnergykWhBPhase), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ0), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ5), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ6), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ7), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykWhTZ8), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLag), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLead), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ0), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ5), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ6), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ7), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykVAhTZ8), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ0), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ0), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ1), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ1), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ2), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ2), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ3), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ3), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ4), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ4), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ5), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ5), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ6), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ6), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ7), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ7), ","));
            builder.Append(string.Concat(ParameterName(MDkWTZ8), ","));
            builder.Append(string.Concat(ParameterName(MDkWDateTimeTZ8), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ0), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ0), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ1), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ1), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ2), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ2), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ3), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ3), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ4), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ4), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ5), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ5), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ6), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ6), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ7), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ7), ","));
            builder.Append(string.Concat(ParameterName(MDkVATZ8), ","));
            builder.Append(string.Concat(ParameterName(MDkVADateTimeTZ8), ","));
            builder.Append(string.Concat(ParameterName(CUMPOWEROFFDURATION), ","));
            builder.Append(string.Concat(ParameterName(BILLINGWISEPOWEROFFDURATION), ","));
            builder.Append(string.Concat(ParameterName(BillingAverageLoadFactor), ","));
            //pradipta_start_081018

            builder.Append(string.Concat(ParameterName(BillingAvgkWImportLoadFactor), ","));
            builder.Append(string.Concat(ParameterName(BillingAvgkWExportLoadFactor), ","));
            builder.Append(string.Concat(ParameterName(BillingAvgkVAImportLoadFactor), ","));
            builder.Append(string.Concat(ParameterName(BillingAvgkVAExportLoadFactor), ","));
            //pradipta_End_081018


            builder.Append(string.Concat(ParameterName(PowerOnDuration), ","));
            builder.Append(string.Concat(ParameterName(CumPowerOnDuration), ","));
            builder.Append(string.Concat(ParameterName(PowerOnDurationDisplay), ","));
            builder.Append(string.Concat(ParameterName(CUMBILLINGMDRESETCOUNT), ","));
            builder.Append(string.Concat(ParameterName(DELTATAMPERCOUNT), ",")); // Story - 345154

            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ5), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ6), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ7), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLagTZ8), ","));

            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ1), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ2), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ3), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ4), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ5), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ6), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ7), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergykvarhLeadTZ8), ","));

            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ1), ","));
            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ2), ","));
            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ3), ","));
            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ4), ","));
            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ5), ","));
            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ6), ","));
            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ7), ","));
            builder.Append(string.Concat(ParameterName(TODAveragePowerFactorTZ8), ","));

            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ1), ","));//story 1024441 Add TOD Export PF
            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ2), ","));
            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ3), ","));
            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ4), ","));
            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ5), ","));
            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ6), ","));
            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ7), ","));
            builder.Append(string.Concat(ParameterName(TODAverageExportPowerFactorTZ8), ","));
            builder.Append(string.Concat(ParameterName(RPhaseMDDateTime), ","));
            builder.Append(string.Concat(ParameterName(YPhaseMDDateTime), ","));
            builder.Append(string.Concat(ParameterName(BPhaseMDDateTime), ","));
            builder.Append(string.Concat(ParameterName(RPhaseMDkW), ","));
            builder.Append(string.Concat(ParameterName(YPhaseMDkW), ","));
            builder.Append(string.Concat(ParameterName(BPhaseMDkW), ","));

            #region JDVVNL
            builder.Append(string.Concat(ParameterName(MinimumVoltageLSIPAcrossDayRPhase), ","));
            builder.Append(string.Concat(ParameterName(MinimumVoltageLSIPAcrossDayYPhase), ","));
            builder.Append(string.Concat(ParameterName(MinimumVoltageLSIPAcrossDayBPhase), ","));
            
            #endregion
            builder.Append(string.Concat(ParameterName(BillingAverageLoad), ","));

            //if (isMPKWCL)
            //{
            //    builder.Append(string.Concat(ParameterName(CUMPOWEROFFDURATION), ","));
            //    builder.Append(string.Concat(ParameterName(CUMTAMPERCOUNT), ","));
            //}
            //If Utility is PGVCL then add Billing Reset Type
            builder.Append(string.Concat(ParameterName(CUMTAMPERCOUNT), ","));
            builder.Append(string.Concat(ParameterName(CUMPOWERFAILURECOUNT), ","));
            builder.Append(string.Concat(ParameterName(BillingResetType), ","));
            builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
            builder.Append(string.Concat(ParameterName(DataIndex), ","));
            builder.Append(string.Concat(ParameterName(ABCCodeBilling), ","));
            builder.Append(string.Concat(ParameterName(Cumulativemdkw), ","));
            builder.Append(string.Concat(ParameterName(Cumulativemdkva), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergyFraudkWh), ","));
            builder.Append(string.Concat(ParameterName(CumulativeEnergyFraudkVAh), ","));

            // User Story - 1000867
            builder.Append(string.Concat(ParameterName(MDkVArLagTZ0), ","));
            builder.Append(string.Concat(ParameterName(MDkVArLagDateTimeTZ0), ","));
            builder.Append(string.Concat(ParameterName(MDkVArLeadTZ0), ","));
            builder.Append(string.Concat(ParameterName(MDkVArLeadDateTimeTZ0), ","));
            builder.Append(string.Concat(ParameterName(kWhLag), ","));
            builder.Append(string.Concat(ParameterName(kWhLead), ","));
            builder.Append(string.Concat(ParameterName(kVAhLag), ","));
            builder.Append(string.Concat(ParameterName(kVAhLead), ")"));

            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(BillingDate), dLMS650BillingEntity.BillingDate, DbType.Int64);
            //Net Start
            //KWH - Net
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ0Net), dLMS650BillingEntity.CumulativeEnergykWhTZ0Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ1Net), dLMS650BillingEntity.CumulativeEnergykWhTZ1Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ2Net), dLMS650BillingEntity.CumulativeEnergykWhTZ2Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ3Net), dLMS650BillingEntity.CumulativeEnergykWhTZ3Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ4Net), dLMS650BillingEntity.CumulativeEnergykWhTZ4Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ5Net), dLMS650BillingEntity.CumulativeEnergykWhTZ5Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ6Net), dLMS650BillingEntity.CumulativeEnergykWhTZ6Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ7Net), dLMS650BillingEntity.CumulativeEnergykWhTZ7Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ8Net), dLMS650BillingEntity.CumulativeEnergykWhTZ8Net, DbType.String, 40);
            //KVAH - Net
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ0Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ0Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ1Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ1Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ2Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ2Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ3Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ3Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ4Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ4Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ5Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ5Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ6Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ6Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ7Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ7Net, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ8Net), dLMS650BillingEntity.CumulativeEnergykVAhTZ8Net, DbType.String, 40);
            //MD - KW - Net
            request.AddParamter(ParameterName(MDkWTZ0Net), dLMS650BillingEntity.MDkWTZ0Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ1Net), dLMS650BillingEntity.MDkWTZ1Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ2Net), dLMS650BillingEntity.MDkWTZ2Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ3Net), dLMS650BillingEntity.MDkWTZ3Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ4Net), dLMS650BillingEntity.MDkWTZ4Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ5Net), dLMS650BillingEntity.MDkWTZ5Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ6Net), dLMS650BillingEntity.MDkWTZ6Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ7Net), dLMS650BillingEntity.MDkWTZ7Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ8Net), dLMS650BillingEntity.MDkWTZ8Net, DbType.String, 40);
            //MD - KVA - Net
            request.AddParamter(ParameterName(MDkVATZ0Net), dLMS650BillingEntity.MDkVATZ0Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ1Net), dLMS650BillingEntity.MDkVATZ1Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ2Net), dLMS650BillingEntity.MDkVATZ2Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ3Net), dLMS650BillingEntity.MDkVATZ3Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ4Net), dLMS650BillingEntity.MDkVATZ4Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ5Net), dLMS650BillingEntity.MDkVATZ5Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ6Net), dLMS650BillingEntity.MDkVATZ6Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ7Net), dLMS650BillingEntity.MDkVATZ7Net, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ8Net), dLMS650BillingEntity.MDkVATZ8Net, DbType.String, 40);
            //MD - KW - DateTime - Net
            request.AddParamter(ParameterName(MDkWDateTimeTZ0Net), dLMS650BillingEntity.MDkWDateTimeTZ0Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ1Net), dLMS650BillingEntity.MDkWDateTimeTZ1Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ2Net), dLMS650BillingEntity.MDkWDateTimeTZ2Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ3Net), dLMS650BillingEntity.MDkWDateTimeTZ3Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ4Net), dLMS650BillingEntity.MDkWDateTimeTZ4Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ5Net), dLMS650BillingEntity.MDkWDateTimeTZ5Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ6Net), dLMS650BillingEntity.MDkWDateTimeTZ6Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ7Net), dLMS650BillingEntity.MDkWDateTimeTZ7Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ8Net), dLMS650BillingEntity.MDkWDateTimeTZ8Net, DbType.Int64);
            //MD - KVA - DateTime - Net
            request.AddParamter(ParameterName(MDkVADateTimeTZ0Net), dLMS650BillingEntity.MDkVADateTimeTZ0Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ1Net), dLMS650BillingEntity.MDkVADateTimeTZ1Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ2Net), dLMS650BillingEntity.MDkVADateTimeTZ2Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ3Net), dLMS650BillingEntity.MDkVADateTimeTZ3Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ4Net), dLMS650BillingEntity.MDkVADateTimeTZ4Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ5Net), dLMS650BillingEntity.MDkVADateTimeTZ5Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ6Net), dLMS650BillingEntity.MDkVADateTimeTZ6Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ7Net), dLMS650BillingEntity.MDkVADateTimeTZ7Net, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ8Net), dLMS650BillingEntity.MDkVADateTimeTZ8Net, DbType.Int64);
            //Net End
            //Import Start
            //KWH - Import
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ0Import), dLMS650BillingEntity.CumulativeEnergykWhTZ0Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ1Import), dLMS650BillingEntity.CumulativeEnergykWhTZ1Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ2Import), dLMS650BillingEntity.CumulativeEnergykWhTZ2Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ3Import), dLMS650BillingEntity.CumulativeEnergykWhTZ3Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ4Import), dLMS650BillingEntity.CumulativeEnergykWhTZ4Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ5Import), dLMS650BillingEntity.CumulativeEnergykWhTZ5Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ6Import), dLMS650BillingEntity.CumulativeEnergykWhTZ6Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ7Import), dLMS650BillingEntity.CumulativeEnergykWhTZ7Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ8Import), dLMS650BillingEntity.CumulativeEnergykWhTZ8Import, DbType.String, 40);
            //KVAH - Import
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ0Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ0Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ1Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ1Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ2Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ2Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ3Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ3Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ4Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ4Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ5Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ5Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ6Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ6Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ7Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ7Import, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ8Import), dLMS650BillingEntity.CumulativeEnergykVAhTZ8Import, DbType.String, 40);
            //MD - KW - Import
            request.AddParamter(ParameterName(MDkWTZ0Import), dLMS650BillingEntity.MDkWTZ0Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ1Import), dLMS650BillingEntity.MDkWTZ1Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ2Import), dLMS650BillingEntity.MDkWTZ2Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ3Import), dLMS650BillingEntity.MDkWTZ3Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ4Import), dLMS650BillingEntity.MDkWTZ4Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ5Import), dLMS650BillingEntity.MDkWTZ5Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ6Import), dLMS650BillingEntity.MDkWTZ6Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ7Import), dLMS650BillingEntity.MDkWTZ7Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ8Import), dLMS650BillingEntity.MDkWTZ8Import, DbType.String, 40);
            //MD - KVA - Import
            request.AddParamter(ParameterName(MDkVATZ0Import), dLMS650BillingEntity.MDkVATZ0Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ1Import), dLMS650BillingEntity.MDkVATZ1Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ2Import), dLMS650BillingEntity.MDkVATZ2Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ3Import), dLMS650BillingEntity.MDkVATZ3Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ4Import), dLMS650BillingEntity.MDkVATZ4Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ5Import), dLMS650BillingEntity.MDkVATZ5Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ6Import), dLMS650BillingEntity.MDkVATZ6Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ7Import), dLMS650BillingEntity.MDkVATZ7Import, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ8Import), dLMS650BillingEntity.MDkVATZ8Import, DbType.String, 40);
            //MD - KW - DateTime - Import
            request.AddParamter(ParameterName(MDkWDateTimeTZ0Import), dLMS650BillingEntity.MDkWDateTimeTZ0Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ1Import), dLMS650BillingEntity.MDkWDateTimeTZ1Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ2Import), dLMS650BillingEntity.MDkWDateTimeTZ2Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ3Import), dLMS650BillingEntity.MDkWDateTimeTZ3Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ4Import), dLMS650BillingEntity.MDkWDateTimeTZ4Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ5Import), dLMS650BillingEntity.MDkWDateTimeTZ5Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ6Import), dLMS650BillingEntity.MDkWDateTimeTZ6Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ7Import), dLMS650BillingEntity.MDkWDateTimeTZ7Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ8Import), dLMS650BillingEntity.MDkWDateTimeTZ8Import, DbType.Int64);
            //MD - KVA - DateTime - Import
            request.AddParamter(ParameterName(MDkVADateTimeTZ0Import), dLMS650BillingEntity.MDkVADateTimeTZ0Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ1Import), dLMS650BillingEntity.MDkVADateTimeTZ1Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ2Import), dLMS650BillingEntity.MDkVADateTimeTZ2Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ3Import), dLMS650BillingEntity.MDkVADateTimeTZ3Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ4Import), dLMS650BillingEntity.MDkVADateTimeTZ4Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ5Import), dLMS650BillingEntity.MDkVADateTimeTZ5Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ6Import), dLMS650BillingEntity.MDkVADateTimeTZ6Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ7Import), dLMS650BillingEntity.MDkVADateTimeTZ7Import, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ8Import), dLMS650BillingEntity.MDkVADateTimeTZ8Import, DbType.Int64);
            //Import End
            //Export Start
            //KWH - Export
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ0Export), dLMS650BillingEntity.CumulativeEnergykWhTZ0Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ1Export), dLMS650BillingEntity.CumulativeEnergykWhTZ1Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ2Export), dLMS650BillingEntity.CumulativeEnergykWhTZ2Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ3Export), dLMS650BillingEntity.CumulativeEnergykWhTZ3Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ4Export), dLMS650BillingEntity.CumulativeEnergykWhTZ4Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ5Export), dLMS650BillingEntity.CumulativeEnergykWhTZ5Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ6Export), dLMS650BillingEntity.CumulativeEnergykWhTZ6Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ7Export), dLMS650BillingEntity.CumulativeEnergykWhTZ7Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ8Export), dLMS650BillingEntity.CumulativeEnergykWhTZ8Export, DbType.String, 40);           
            //KVAH - Export
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ0Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ0Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ1Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ1Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ2Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ2Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ3Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ3Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ4Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ4Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ5Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ5Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ6Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ6Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ7Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ7Export, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ8Export), dLMS650BillingEntity.CumulativeEnergykVAhTZ8Export, DbType.String, 40);           
            //MD - KW - Export
            request.AddParamter(ParameterName(MDkWTZ0Export), dLMS650BillingEntity.MDkWTZ0Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ1Export), dLMS650BillingEntity.MDkWTZ1Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ2Export), dLMS650BillingEntity.MDkWTZ2Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ3Export), dLMS650BillingEntity.MDkWTZ3Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ4Export), dLMS650BillingEntity.MDkWTZ4Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ5Export), dLMS650BillingEntity.MDkWTZ5Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ6Export), dLMS650BillingEntity.MDkWTZ6Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ7Export), dLMS650BillingEntity.MDkWTZ7Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWTZ8Export), dLMS650BillingEntity.MDkWTZ8Export, DbType.String, 40);
            //MD - KVA - Export
            request.AddParamter(ParameterName(MDkVATZ0Export), dLMS650BillingEntity.MDkVATZ0Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ1Export), dLMS650BillingEntity.MDkVATZ1Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ2Export), dLMS650BillingEntity.MDkVATZ2Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ3Export), dLMS650BillingEntity.MDkVATZ3Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ4Export), dLMS650BillingEntity.MDkVATZ4Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ5Export), dLMS650BillingEntity.MDkVATZ5Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ6Export), dLMS650BillingEntity.MDkVATZ6Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ7Export), dLMS650BillingEntity.MDkVATZ7Export, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVATZ8Export), dLMS650BillingEntity.MDkVATZ8Export, DbType.String, 40);
            //MD - KW - DateTime - Export
            request.AddParamter(ParameterName(MDkWDateTimeTZ0Export), dLMS650BillingEntity.MDkWDateTimeTZ0Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ1Export), dLMS650BillingEntity.MDkWDateTimeTZ1Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ2Export), dLMS650BillingEntity.MDkWDateTimeTZ2Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ3Export), dLMS650BillingEntity.MDkWDateTimeTZ3Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ4Export), dLMS650BillingEntity.MDkWDateTimeTZ4Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ5Export), dLMS650BillingEntity.MDkWDateTimeTZ5Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ6Export), dLMS650BillingEntity.MDkWDateTimeTZ6Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ7Export), dLMS650BillingEntity.MDkWDateTimeTZ7Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkWDateTimeTZ8Export), dLMS650BillingEntity.MDkWDateTimeTZ8Export, DbType.Int64);
            //MD - KVA - DateTime - Export
            request.AddParamter(ParameterName(MDkVADateTimeTZ0Export), dLMS650BillingEntity.MDkVADateTimeTZ0Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ1Export), dLMS650BillingEntity.MDkVADateTimeTZ1Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ2Export), dLMS650BillingEntity.MDkVADateTimeTZ2Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ3Export), dLMS650BillingEntity.MDkVADateTimeTZ3Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ4Export), dLMS650BillingEntity.MDkVADateTimeTZ4Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ5Export), dLMS650BillingEntity.MDkVADateTimeTZ5Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ6Export), dLMS650BillingEntity.MDkVADateTimeTZ6Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ7Export), dLMS650BillingEntity.MDkVADateTimeTZ7Export, DbType.Int64);
            request.AddParamter(ParameterName(MDkVADateTimeTZ8Export), dLMS650BillingEntity.MDkVADateTimeTZ8Export, DbType.Int64);
            //Export End
            //Cumulative-Energy-Lag-Q1
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagQ1), dLMS650BillingEntity.CumulativeEnergykvarhLagQ1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ1Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ1Q1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ2Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ2Q1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ3Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ3Q1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ4Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ4Q1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ5Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ5Q1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ6Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ6Q1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ7Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ7Q1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ8Q1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ8Q1, DbType.String, 40);
            //Cumulative-Energy-Lead-Q4
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadQ4), dLMS650BillingEntity.CumulativeEnergykvarhLeadQ4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ1Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ1Q4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ2Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ2Q4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ3Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ3Q4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ4Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ4Q4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ5Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ5Q4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ6Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ6Q4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ7Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ7Q4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ8Q4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ8Q4, DbType.String, 40);
            //Cumulative-Energy-Lag-Q3
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagQ3), dLMS650BillingEntity.CumulativeEnergykvarhLagQ3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ1Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ1Q3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ2Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ2Q3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ3Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ3Q3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ4Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ4Q3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ5Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ5Q3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ6Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ6Q3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ7Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ7Q3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ8Q3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ8Q3, DbType.String, 40);
            //Cumulative-Energy-Lead-Q2
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadQ2), dLMS650BillingEntity.CumulativeEnergykvarhLeadQ2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ1Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ1Q2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ2Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ2Q2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ3Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ3Q2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ4Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ4Q2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ5Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ5Q2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ6Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ6Q2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ7Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ7Q2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ8Q2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ8Q2, DbType.String, 40);
            
            request.AddParamter(ParameterName(SystemPowerFactorforBillingPeriod), dLMS650BillingEntity.SystemPowerFactorforBillingPeriod, DbType.String, 40);
            request.AddParamter(ParameterName(SystemPowerFactorImportforBillingPeriod), dLMS650BillingEntity.SystemPowerFactorImportforBillingPeriod, DbType.String, 40);
            request.AddParamter(ParameterName(SystemPowerFactorExportforBillingPeriod), dLMS650BillingEntity.SystemPowerFactorExportforBillingPeriod, DbType.String, 40);

            request.AddParamter(ParameterName(CumEnergykWhRPhase), dLMS650BillingEntity.CumEnergykWhRPhase, DbType.String, 40);
            request.AddParamter(ParameterName(CumEnergykWhYPhase), dLMS650BillingEntity.CumEnergykWhYPhase, DbType.String, 40);
            request.AddParamter(ParameterName(CumEnergykWhBPhase), dLMS650BillingEntity.CumEnergykWhBPhase, DbType.String, 40);

            request.AddParamter(ParameterName(CumulativeEnergykWhTZ0), dLMS650BillingEntity.CumulativeEnergykWhTZ0, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ1), dLMS650BillingEntity.CumulativeEnergykWhTZ1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ2), dLMS650BillingEntity.CumulativeEnergykWhTZ2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ3), dLMS650BillingEntity.CumulativeEnergykWhTZ3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ4), dLMS650BillingEntity.CumulativeEnergykWhTZ4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ5), dLMS650BillingEntity.CumulativeEnergykWhTZ5, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ6), dLMS650BillingEntity.CumulativeEnergykWhTZ6, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ7), dLMS650BillingEntity.CumulativeEnergykWhTZ7, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykWhTZ8), dLMS650BillingEntity.CumulativeEnergykWhTZ8, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLag), dLMS650BillingEntity.CumulativeEnergykvarhLag, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLead), dLMS650BillingEntity.CumulativeEnergykvarhLead, DbType.String, 40);

            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ0), dLMS650BillingEntity.CumulativeEnergykVAhTZ0, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ1), dLMS650BillingEntity.CumulativeEnergykVAhTZ1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ2), dLMS650BillingEntity.CumulativeEnergykVAhTZ2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ3), dLMS650BillingEntity.CumulativeEnergykVAhTZ3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ4), dLMS650BillingEntity.CumulativeEnergykVAhTZ4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ5), dLMS650BillingEntity.CumulativeEnergykVAhTZ5, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ6), dLMS650BillingEntity.CumulativeEnergykVAhTZ6, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ7), dLMS650BillingEntity.CumulativeEnergykVAhTZ7, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykVAhTZ8), dLMS650BillingEntity.CumulativeEnergykVAhTZ8, DbType.String, 40);

            request.AddParamter(ParameterName(MDkWTZ0), dLMS650BillingEntity.MDkWTZ0, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ0), dLMS650BillingEntity.MDkWDateTimeTZ0, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ1), dLMS650BillingEntity.MDkWTZ1, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ1), dLMS650BillingEntity.MDkWDateTimeTZ1, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ2), dLMS650BillingEntity.MDkWTZ2, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ2), dLMS650BillingEntity.MDkWDateTimeTZ2, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ3), dLMS650BillingEntity.MDkWTZ3, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ3), dLMS650BillingEntity.MDkWDateTimeTZ3, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ4), dLMS650BillingEntity.MDkWTZ4, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ4), dLMS650BillingEntity.MDkWDateTimeTZ4, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ5), dLMS650BillingEntity.MDkWTZ5, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ5), dLMS650BillingEntity.MDkWDateTimeTZ5, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ6), dLMS650BillingEntity.MDkWTZ6, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ6), dLMS650BillingEntity.MDkWDateTimeTZ6, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ7), dLMS650BillingEntity.MDkWTZ7, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ7), dLMS650BillingEntity.MDkWDateTimeTZ7, DbType.Int64);
            request.AddParamter(ParameterName(MDkWTZ8), dLMS650BillingEntity.MDkWTZ8, DbType.String, 40);
            request.AddParamter(ParameterName(MDkWDateTimeTZ8), dLMS650BillingEntity.MDkWDateTimeTZ8, DbType.Int64);

            request.AddParamter(ParameterName(MDkVATZ0), dLMS650BillingEntity.MDkVATZ0, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ0), dLMS650BillingEntity.MDkVADateTimeTZ0, DbType.Int64);
            request.AddParamter(ParameterName(MDkVATZ1), dLMS650BillingEntity.MDkVATZ1, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ1), dLMS650BillingEntity.MDkVADateTimeTZ1, DbType.Int64);
            request.AddParamter(ParameterName(MDkVATZ2), dLMS650BillingEntity.MDkVATZ2, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ2), dLMS650BillingEntity.MDkVADateTimeTZ2, DbType.Int64);
            request.AddParamter(ParameterName(MDkVATZ3), dLMS650BillingEntity.MDkVATZ3, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ3), dLMS650BillingEntity.MDkVADateTimeTZ3, DbType.Int64);
            request.AddParamter(ParameterName(MDkVATZ4), dLMS650BillingEntity.MDkVATZ4, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ4), dLMS650BillingEntity.MDkVADateTimeTZ4, DbType.Int64);

            request.AddParamter(ParameterName(MDkVATZ5), dLMS650BillingEntity.MDkVATZ5, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ5), dLMS650BillingEntity.MDkVADateTimeTZ5, DbType.Int64);
            request.AddParamter(ParameterName(MDkVATZ6), dLMS650BillingEntity.MDkVATZ6, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ6), dLMS650BillingEntity.MDkVADateTimeTZ6, DbType.Int64);

            request.AddParamter(ParameterName(MDkVATZ7), dLMS650BillingEntity.MDkVATZ7, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ7), dLMS650BillingEntity.MDkVADateTimeTZ7, DbType.Int64);
            request.AddParamter(ParameterName(MDkVATZ8), dLMS650BillingEntity.MDkVATZ8, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVADateTimeTZ8), dLMS650BillingEntity.MDkVADateTimeTZ8, DbType.Int64);
            request.AddParamter(ParameterName(CUMPOWEROFFDURATION), dLMS650BillingEntity.CumPowerOffDuration, DbType.String, 40);
            request.AddParamter(ParameterName(BILLINGWISEPOWEROFFDURATION), dLMS650BillingEntity.BillingWisePowerOffDuration, DbType.String, 40);
            request.AddParamter(ParameterName(BillingAverageLoadFactor), dLMS650BillingEntity.BillingAverageLoadFactor, DbType.String, 40);
            //pradipta_start_081018

            request.AddParamter(ParameterName(BillingAvgkWImportLoadFactor), dLMS650BillingEntity.BillingAveragekWImportLoadFactor, DbType.String, 40);
            request.AddParamter(ParameterName(BillingAvgkWExportLoadFactor), dLMS650BillingEntity.BillingAveragekWExportLoadFactor, DbType.String, 40);
            request.AddParamter(ParameterName(BillingAvgkVAImportLoadFactor), dLMS650BillingEntity.BillingAveragekVAImportLoadFactor, DbType.String, 40);
            request.AddParamter(ParameterName(BillingAvgkVAExportLoadFactor), dLMS650BillingEntity.BillingAveragekVAExportLoadFactor, DbType.String, 40);
            //pradipta_End_081018


            request.AddParamter(ParameterName(PowerOnDuration), dLMS650BillingEntity.PowerOnDuration, DbType.String, 40);
            request.AddParamter(ParameterName(CumPowerOnDuration), dLMS650BillingEntity.CumPowerOnDuration, DbType.String, 40);
            request.AddParamter(ParameterName(PowerOnDurationDisplay), dLMS650BillingEntity.PowerOnDurationDisplay, DbType.Byte);
            request.AddParamter(ParameterName(CUMBILLINGMDRESETCOUNT), dLMS650BillingEntity.CumBillingMDResetCount, DbType.Int64);
            request.AddParamter(ParameterName(DELTATAMPERCOUNT), dLMS650BillingEntity.DeltaTamperCount, DbType.Int64); // Story - 345154

            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ1), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ2), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ3), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ4), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ5), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ5, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ6), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ6, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ7), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ7, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLagTZ8), dLMS650BillingEntity.CumulativeEnergykvarhLagTZ8, DbType.String, 40);

            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ1), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ1, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ2), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ2, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ3), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ3, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ4), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ4, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ5), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ5, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ6), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ6, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ7), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ7, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergykvarhLeadTZ8), dLMS650BillingEntity.CumulativeEnergykvarhLeadTZ8, DbType.String, 40);

            request.AddParamter(ParameterName(TODAveragePowerFactorTZ1), dLMS650BillingEntity.TODAveragePowerFactorTZ1, DbType.String, 40);
            request.AddParamter(ParameterName(TODAveragePowerFactorTZ2), dLMS650BillingEntity.TODAveragePowerFactorTZ2, DbType.String, 40);
            request.AddParamter(ParameterName(TODAveragePowerFactorTZ3), dLMS650BillingEntity.TODAveragePowerFactorTZ3, DbType.String, 40);
            request.AddParamter(ParameterName(TODAveragePowerFactorTZ4), dLMS650BillingEntity.TODAveragePowerFactorTZ4, DbType.String, 40);
            request.AddParamter(ParameterName(TODAveragePowerFactorTZ5), dLMS650BillingEntity.TODAveragePowerFactorTZ5, DbType.String, 40);
            request.AddParamter(ParameterName(TODAveragePowerFactorTZ6), dLMS650BillingEntity.TODAveragePowerFactorTZ6, DbType.String, 40);
            request.AddParamter(ParameterName(TODAveragePowerFactorTZ7), dLMS650BillingEntity.TODAveragePowerFactorTZ7, DbType.String, 40);
            request.AddParamter(ParameterName(TODAveragePowerFactorTZ8), dLMS650BillingEntity.TODAveragePowerFactorTZ8, DbType.String, 40);

            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ1), dLMS650BillingEntity.TODAverageExportPowerFactorTZ1, DbType.String, 40);//story 1024441 Add TOD Export PF
            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ2), dLMS650BillingEntity.TODAverageExportPowerFactorTZ2, DbType.String, 40);
            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ3), dLMS650BillingEntity.TODAverageExportPowerFactorTZ3, DbType.String, 40);
            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ4), dLMS650BillingEntity.TODAverageExportPowerFactorTZ4, DbType.String, 40);
            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ5), dLMS650BillingEntity.TODAverageExportPowerFactorTZ5, DbType.String, 40);
            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ6), dLMS650BillingEntity.TODAverageExportPowerFactorTZ6, DbType.String, 40);
            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ7), dLMS650BillingEntity.TODAverageExportPowerFactorTZ7, DbType.String, 40);
            request.AddParamter(ParameterName(TODAverageExportPowerFactorTZ8), dLMS650BillingEntity.TODAverageExportPowerFactorTZ8, DbType.String, 40);

            request.AddParamter(ParameterName(RPhaseMDDateTime), dLMS650BillingEntity.RPhaseMDDateTime, DbType.Int64);
            request.AddParamter(ParameterName(YPhaseMDDateTime), dLMS650BillingEntity.YPhaseMDDateTime, DbType.Int64);
            request.AddParamter(ParameterName(BPhaseMDDateTime), dLMS650BillingEntity.BPhaseMDDateTime, DbType.Int64);
            request.AddParamter(ParameterName(RPhaseMDkW), dLMS650BillingEntity.RPhaseMDkW, DbType.String, 40);
            request.AddParamter(ParameterName(YPhaseMDkW), dLMS650BillingEntity.YPhaseMDkW, DbType.String, 40);
            request.AddParamter(ParameterName(BPhaseMDkW), dLMS650BillingEntity.BPhaseMDkW, DbType.String, 40);
            //if (isMPKWCL)
            //{
            //    request.AddParamter(ParameterName(CUMPOWEROFFDURATION), dLMS650BillingEntity.CumPowerOffDuration, DbType.String, 40);
            //    request.AddParamter(ParameterName(CUMTAMPERCOUNT), dLMS650BillingEntity.CumTamperCount, DbType.Int64);
            //}

            request.AddParamter(ParameterName(MinimumVoltageLSIPAcrossDayRPhase), dLMS650BillingEntity.MinimumVoltageLSIPAcrossDayRPhase, DbType.String, 40);
            request.AddParamter(ParameterName(MinimumVoltageLSIPAcrossDayYPhase), dLMS650BillingEntity.MinimumVoltageLSIPAcrossDayYPhase, DbType.String, 40);
            request.AddParamter(ParameterName(MinimumVoltageLSIPAcrossDayBPhase), dLMS650BillingEntity.MinimumVoltageLSIPAcrossDayBPhase, DbType.String, 40);
            request.AddParamter(ParameterName(BillingAverageLoad), dLMS650BillingEntity.BillingAverageLoad, DbType.String, 40);

            request.AddParamter(ParameterName(CUMTAMPERCOUNT), dLMS650BillingEntity.CumTamperCount, DbType.Int64);
            request.AddParamter(ParameterName(CUMPOWERFAILURECOUNT), dLMS650BillingEntity.CumPowerFailureCount, DbType.Int64);
            request.AddParamter(ParameterName(BillingResetType), dLMS650BillingEntity.BillingType, DbType.String, 40);
            request.AddParamter(ParameterName(MeterData_ID), dLMS650BillingEntity.MeterData_ID, DbType.Int64);
            request.AddParamter(ParameterName(DataIndex), dLMS650BillingEntity.DataIndex, DbType.Int64);
            request.AddParamter(ParameterName(ABCCodeBilling), dLMS650BillingEntity.ABCCodeBilling, DbType.String,40);
            request.AddParamter(ParameterName(Cumulativemdkw), dLMS650BillingEntity.CumulativeMDkw, DbType.String, 40);
            request.AddParamter(ParameterName(Cumulativemdkva), dLMS650BillingEntity.CumulativeMDkva, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergyFraudkWh), dLMS650BillingEntity.CumulativeEnergyFraudkWh, DbType.String, 40);
            request.AddParamter(ParameterName(CumulativeEnergyFraudkVAh), dLMS650BillingEntity.CumulativeEnergyFraudkVAh, DbType.String, 40);

            // User Story - 1000867
            request.AddParamter(ParameterName(MDkVArLagTZ0), dLMS650BillingEntity.MDkVArLagTZ0, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVArLagDateTimeTZ0), dLMS650BillingEntity.MDkVArLagDateTimeTZ0, DbType.Int64);
            request.AddParamter(ParameterName(MDkVArLeadTZ0), dLMS650BillingEntity.MDkVArLeadTZ0, DbType.String, 40);
            request.AddParamter(ParameterName(MDkVArLeadDateTimeTZ0), dLMS650BillingEntity.MDkVArLeadDateTimeTZ0, DbType.Int64);
            request.AddParamter(ParameterName(kWhLag), dLMS650BillingEntity.kWhLag, DbType.String);
            request.AddParamter(ParameterName(kWhLead), dLMS650BillingEntity.kWhLead, DbType.String);
            request.AddParamter(ParameterName(kVAhLag), dLMS650BillingEntity.kVAhLag, DbType.String);
            request.AddParamter(ParameterName(kVAhLead), dLMS650BillingEntity.kVAhLead, DbType.String);
            return request;
        }

        public override IEntity InsertData(IEntity entity)
        {
            BillingEntity billingEntity = entity as BillingEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = this.GetRequest(entity);
                helper.ExecuteNonQuery(request);
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "IEntity InsertData(IEntity entity)", ex);
            }
            if (Flag)
                billingEntity.Billing_ID = long.Parse(this.GetPK());
            return billingEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            List<DataRequest> requests = new List<DataRequest>();
            foreach (IEntity entity in entities)
            {
                requests.Add(this.GetRequest(entity));
            }
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(requests);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IList<IEntity> entities)", ex);
            }
            return null;
        }

        public DataSet GetBillingDataByMeter(string meterID, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    if (column.ToLower() == "poweroffduration")
                    {
                        builder.Append(string.Concat(",", "ifnull(" + column + ",0) as PowerOffDuration", " "));
                    }
                    else
                    {
                        builder.Append(string.Concat(",", column, " "));
                    }
                }
                builder.Append(",m.MeterData_ID from meterdata_billing b inner join meterdata m on b.MeterData_ID = m.MeterData_ID ");
                //builder.Append("inner join meterdata_tariffinformation ti on m.MeterData_ID = ti.MeterData_ID ");
                //builder.Append("and b.History_ID = ti.HistoryID ");
                //builder.Append("inner join meterdata_tampercountergeneral tc on m.MeterData_ID = tc.MeterData_ID ");
                //builder.Append("and b.History_ID = tc.History_ID and ti.HistoryID = tc.History_ID ");
                //builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                //builder.Append(string.Concat("tc.", RelatedTo, "=", ParameterName(RelatedTo)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String);
                //request.AddParamter(ParameterName(RelatedTo), "B", DbType.String);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data for a specified meter viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingDataByMeter(string meterID, List<string> columns)", ex);
            }
            return dataSet;
        }

        public DataSet GetBillingDataByFileName(string meterID, string fileName, List<string> columns)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select m.MeterID, f.FileName, m.ReadingDateTime ");
                foreach (string column in columns)
                {
                    if (column.ToLower() == "poweroffduration")
                    {
                        builder.Append(string.Concat(",", "ifnull(" + column + ",0) as PowerOffDuration", " "));
                    }
                    else
                    {
                        builder.Append(string.Concat(",", column, " "));
                    }
                }
                builder.Append(",m.MeterData_ID from meterdata_billing b inner join meterdata m on b.MeterData_ID = m.MeterData_ID ");
                //builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 13 and "); // Story - 365971 - 13 billing for Power ON Hours
                builder.Append("inner join fileupload_master f on m.FileUpload_ID = f.FileUpload_ID where b.DataIndex < 61 and "); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat("m.", MeterID, "=", ParameterName(MeterID)));
                builder.Append(string.Concat(" ", "and", " ", "f.", FileName, "=", ParameterName(FileName)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), meterID, DbType.String, 20);
                request.AddParamter(ParameterName(FileName), fileName, DbType.String, 150);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing data for a specified meter viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingDataByFileName(string meterID, string fileName, List<string> columns)", ex);
            }
            return dataSet;
        }



        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_billing where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
        }
        /// <summary>
        /// VBM - fetch power off duration
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetPowerOffDuration(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CAST(ifnull(dataindex,0) as char) as History,BillingDate,ifnull(PowerOffDuration,0) as 'Cumulative (0.0.94.91.8.255;3;2) dd:hh:mm' ,");
                //builder.Append("ifnull(PowerOffDuration,0) as 'Billing Wise(0.0.94.91.8.255;3;2) dd:hh:mm' from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing Power ON Hours
                builder.Append("ifnull(PowerOffDuration,0) as 'Billing Wise(0.0.94.91.8.255;3;2) dd:hh:mm' from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPowerOffDuration(int meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        ///  fetch power on duration
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetPowerOnDuration(int meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CAST(ifnull(dataindex,0) as char) as History,BillingDate as BillingTimeStamp,ifnull(PowerOnDuration,0) as 'PowerOnDuration',");
                builder.Append("ifnull(BillingWisePowerOffDuration,0) as BillingWisePowerOffDuration , ifnull(PowerOffDuration,0) as PowerOffDuration, ");
                builder.Append("PowerOnDurationDisplay, ");
                //builder.Append("ifnull(CumPowerOnDuration,0) as CumPowerOnDuration from meterdata_billing where DataIndex < 13 and "); // Story - 365971 - 13 billing Power ON Hours
                builder.Append("ifnull(CumPowerOnDuration,0) as CumPowerOnDuration from meterdata_billing where DataIndex < 61 and "); // Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPowerOnDuration(int meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// VBM - fetch billing wise power off duration and billing timestamp
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        /// 
       


        public DataSet GetLoadFactorInputData(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            DataSet category = new DataSet();


            category = GetMeterCategory(meterDataId);

            foreach (DataRow dataRow in category.Tables[0].Rows)
            {
                meter_cat = Convert.ToString(dataRow["Category"]);
            }

            try
            {

                if (meter_cat == "B8" || meter_cat == "B2")
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    StringBuilder builder = new StringBuilder();
                    builder.Append(" select CAST(ifnull(dataindex,0) as char) as History,BillingDate as BillingTimeStamp,cumulativeenergykwhtz0 as CumulativeEnergyImportKWH,cumulativeenergykwhtz0Export as CumulativeEnergyExportKWH ,MDKWTZ0 as MDKW,MDKWTZ0Export as MDKWExport,ifnull(BillingWisePowerOffDuration,0) as BillingWisePowerOffDuration  from meterdata_billing where ");
                    builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                    DataRequest request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                    dataSet = helper.FillDataSet(request, dataSet);

                }
                else
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    StringBuilder builder = new StringBuilder();
                    builder.Append(" select CAST(ifnull(dataindex,0) as char) as History,BillingDate as BillingTimeStamp,cumulativeenergykwhtz0 as CumulativeEnergyKWH,MDKWTZ0 as MDKW,ifnull(BillingWisePowerOffDuration,0) as BillingWisePowerOffDuration  from meterdata_billing where ");
                    builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                    DataRequest request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                    dataSet = helper.FillDataSet(request, dataSet);
                }

            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadFactorInputData(long meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }
     
        /// <summary>
        /// VBM - fetch billing wise power off duration and billing timestamp
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingWisePowerOffDuration(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append(" select CAST(ifnull(dataindex,0) as char) as History,BillingDate as BillingTimeStamp,ifnull(BillingWisePowerOffDuration,0) as BillingWisePowerOffDuration  from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" select CAST(ifnull(dataindex,0) as char) as History,BillingDate as BillingTimeStamp,ifnull(BillingWisePowerOffDuration,0) as BillingWisePowerOffDuration  from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingWisePowerOffDuration(long meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// Gets the data for Billing Transaction from the database
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <returns></returns>
        public DataSet GetBillingTransaction(long meterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //builder.Append(" select dataindex as History, BillingDate as BillingTimeStamp, BillingResetType as Transaction from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" select dataindex as History, BillingDate as BillingTimeStamp, BillingResetType as Transaction from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingTransaction(long meterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        /// <summary>
        /// Used to get number of Billing for for meterDataId
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <param name="dateTimeEvent"></param>
        /// <returns></returns>
        public int GetBillingCount(long meterDataId)
        {
            object count = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Count(1) from meterData_billing where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                count = helper.ExecuteScalar(request);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingCount(long meterDataId)", ex);
            }
            return Convert.ToInt32(count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterDataId"></param>
        /// <param name="historyID"></param>
        /// <returns></returns>
        public DataSet GetTODAvgPFMeterData(long meterDataId, int historyID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");
                builder.Append("TODAveragePowerFactorTZ1,TODAveragePowerFactorTZ2,TODAveragePowerFactorTZ3,TODAveragePowerFactorTZ4,TODAveragePowerFactorTZ5,TODAveragePowerFactorTZ6,TODAveragePowerFactorTZ7,TODAveragePowerFactorTZ8,TODAverageExportPowerFactorTZ1,TODAverageExportPowerFactorTZ2,TODAverageExportPowerFactorTZ3,TODAverageExportPowerFactorTZ4,TODAverageExportPowerFactorTZ5,TODAverageExportPowerFactorTZ6,TODAverageExportPowerFactorTZ7,TODAverageExportPowerFactorTZ8,");//story 1024441 Add TOD Export PF
                builder.Append("DataIndex,DATE_FORMAT(SUBSTRING(BillingDate, 1, 8),'%b') as BillingMonth");
                //builder.Append(" from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(DataIndex, "=", ParameterName(DataIndex)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                request.AddParamter(ParameterName(DataIndex), historyID, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTODAvgPFMeterData(long meterDataId, int historyID)", ex);
                dataSet = null;
            }
            return dataSet;
        }






        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }


        public DataSet GetDataByMeterID(long MeterDataId, string billingParameters)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                billingParameters = UpdateColumnNames(billingParameters);
                builder.Append("Select distinct ");
                builder.Append(billingParameters);
                //builder.Append(" from meterdata_billing where DataIndex < 13 and ");// Story - 365971 - 13 billing for Power ON Hours
                builder.Append(" from meterdata_billing where DataIndex < 61 and ");// Story - 581355 - 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), MeterDataId, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper data viewed"));
                //update dataset for datetime format
                UpdateDataSet(dataSet);


            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDataByMeterID(long MeterDataId, string billingParameters)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        private void UpdateDataSet(DataSet dataSet)
        {
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataColumn Col = dataSet.Tables[0].Columns.Add("Type");
                Col.SetOrdinal(0);

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    dr["Billing Date (0.0.0.1.2.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["Billing Date (0.0.0.1.2.255)"])));

                    dr["MD, kW Date and time(1.0.1.6.0.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW Date and time(1.0.1.6.0.255)"])));

                    dr["MD, kW for TZ1 date and time(1.0.1.6.1.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ1 date and time(1.0.1.6.1.255)"])));

                    dr["MD, kW for TZ2 date and time(1.0.1.6.2.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ2 date and time(1.0.1.6.2.255)"])));

                    dr["MD, kW for TZ3 date and time(1.0.1.6.3.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ3 date and time(1.0.1.6.3.255)"])));

                    dr["MD, kW for TZ4 date and time(1.0.1.6.4.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ4 date and time(1.0.1.6.4.255)"])));

                    dr["MD, kW for TZ5 date and time(1.0.1.6.5.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ5 date and time(1.0.1.6.5.255)"])));

                    dr["MD, kW for TZ6 date and time(1.0.1.6.6.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ6 date and time(1.0.1.6.6.255)"])));

                    dr["MD, kW for TZ7 date and time(1.0.1.6.7.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ7 date and time(1.0.1.6.7.255)"])));

                    dr["MD, kW for TZ8 date and time(1.0.1.6.8.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kW for TZ8 date and time(1.0.1.6.8.255)"])));

                    dr["MD, kVA Date and time(1.0.9.6.0.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA Date and time(1.0.9.6.0.255)"])));

                    dr["MD, kVA for TZ1 date and time(1.0.9.6.1.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ1 date and time(1.0.9.6.1.255)"])));

                    dr["MD, kVA for TZ2 date and time(1.0.9.6.2.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ2 date and time(1.0.9.6.2.255)"])));

                    dr["MD, kVA for TZ3 date and time(1.0.9.6.3.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ3 date and time(1.0.9.6.3.255)"])));

                    dr["MD, kVA for TZ4 date and time(1.0.9.6.4.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ4 date and time(1.0.9.6.4.255)"])));

                    dr["MD, kVA for TZ5 date and time(1.0.9.6.5.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ5 date and time(1.0.9.6.5.255)"])));

                    dr["MD, kVA for TZ6 date and time(1.0.9.6.6.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ6 date and time(1.0.9.6.6.255)"])));

                    dr["MD, kVA for TZ7 date and time(1.0.9.6.7.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ7 date and time(1.0.9.6.7.255)"])));

                    dr["MD, kVA for TZ8 date and time(1.0.9.6.8.255)"] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64((dr["MD, kVA for TZ8 date and time(1.0.9.6.8.255)"])));
                }
            }      
        }

        private string UpdateColumnNames(string parameters)
        {
            string convertedColumns = "*";
            try
            {
                parameters = parameters.Replace("BillingDate", "CAST(BillingDate as char(16)) as 'Billing Date (0.0.0.1.2.255)'");

                parameters = parameters.Replace("SystemPowerFactorforBillingPeriod", "SystemPowerFactorforBillingPeriod as 'System power factor for billing period(1.0.13.0.0.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ0", "CumulativeEnergykWhTZ0 as 'Cumulative energy, KWh(1.0.1.8.0.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ1", "CumulativeEnergykWhTZ1 as 'Cumulative energy, KWh for TZ1(1.0.1.8.1.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ2", "CumulativeEnergykWhTZ2 as 'Cumulative energy, KWh for TZ2(1.0.1.8.2.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ3", "CumulativeEnergykWhTZ3 as 'Cumulative energy, KWh for TZ3(1.0.1.8.3.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ4", "CumulativeEnergykWhTZ4 as 'Cumulative energy, KWh for TZ4(1.0.1.8.4.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ5", "CumulativeEnergykWhTZ5 as 'Cumulative energy, KWh for TZ5(1.0.1.8.5.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ6", "CumulativeEnergykWhTZ6 as 'Cumulative energy, KWh for TZ6(1.0.1.8.6.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ7", "CumulativeEnergykWhTZ7 as 'Cumulative energy, KWh for TZ7(1.0.1.8.7.255)'");

                parameters = parameters.Replace("CumulativeEnergykWhTZ8", "CumulativeEnergykWhTZ8 as 'Cumulative energy, KWh for TZ8(1.0.1.8.8.255)'");

                parameters = parameters.Replace("CumulativeEnergykvarhLag", "CumulativeEnergykvarhLag as 'Cumulative energy, kvarh (Lag)(1.0.5.8.0.255)'");
                parameters = parameters.Replace("CumulativeEnergykvarhLead", "CumulativeEnergykvarhLead as 'Cumulative energy, kvarh (Lead)(1.0.8.8.0.255)'");


                parameters = parameters.Replace("CumulativeEnergykVAhTZ0", "CumulativeEnergykVAhTZ0 as 'Cumulative energy, KVAh(1.0.9.8.0.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ1", "CumulativeEnergykVAhTZ1 as 'Cumulative energy, KVAh for TZ1(1.0.9.8.1.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ2", "CumulativeEnergykVAhTZ2 as 'Cumulative energy, KVAh for TZ2(1.0.9.8.2.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ3", "CumulativeEnergykVAhTZ3 as 'Cumulative energy, KVAh for TZ3(1.0.9.8.3.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ4", "CumulativeEnergykVAhTZ4 as 'Cumulative energy, KVAh for TZ4(1.0.9.8.4.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ5", "CumulativeEnergykVAhTZ5 as 'Cumulative energy, KVAh for TZ5(1.0.9.8.5.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ6", "CumulativeEnergykVAhTZ6 as 'Cumulative energy, KVAh for TZ6(1.0.9.8.6.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ7", "CumulativeEnergykVAhTZ7 as 'Cumulative energy, KVAh for TZ7(1.0.9.8.7.255)'");

                parameters = parameters.Replace("CumulativeEnergykVAhTZ8", "CumulativeEnergykVAhTZ8 as 'Cumulative energy, KVAh for TZ8(1.0.9.8.8.255)'");



                parameters = parameters.Replace("MDkWTZ0", "MDkWTZ0 as 'MD, kW(1.0.1.6.0.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ0", "CAST(MDkWDateTimeTZ0 as char(16)) as 'MD, kW Date and time(1.0.1.6.0.255)'");

                parameters = parameters.Replace("MDkWTZ1", "MDkWTZ1 as 'MD, kW for TZ1(1.0.1.6.1.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ1", "CAST(MDkWDateTimeTZ1 as char(16)) 'MD, kW for TZ1 date and time(1.0.1.6.1.255)'");

                parameters = parameters.Replace("MDkWTZ2", "MDkWTZ2 as 'MD, kW for TZ2(1.0.1.6.2.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ2", "CAST(MDkWDateTimeTZ2 as char(16)) as 'MD, kW for TZ2 date and time(1.0.1.6.2.255)'");

                parameters = parameters.Replace("MDkWTZ3", "MDkWTZ3 as 'MD, kW for TZ3(1.0.1.6.3.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ3", "CAST(MDkWDateTimeTZ3 as char(16)) as 'MD, kW for TZ3 date and time(1.0.1.6.3.255)'");

                parameters = parameters.Replace("MDkWTZ4", "MDkWTZ4 as 'MD, kW for TZ4(1.0.1.6.4.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ4", "CAST(MDkWDateTimeTZ4 as char(16)) as 'MD, kW for TZ4 date and time(1.0.1.6.4.255)'");

                parameters = parameters.Replace("MDkWTZ5", "MDkWTZ5 as 'MD, kW for TZ5(1.0.1.6.5.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ5", "CAST(MDkWDateTimeTZ5 as char(16)) as 'MD, kW for TZ5 date and time(1.0.1.6.5.255)'");

                parameters = parameters.Replace("MDkWTZ6", "MDkWTZ6 as 'MD, kW for TZ6(1.0.1.6.6.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ6", "CAST(MDkWDateTimeTZ6 as char(16)) as 'MD, kW for TZ6 date and time(1.0.1.6.6.255)'");

                parameters = parameters.Replace("MDkWTZ7", "MDkWTZ7 as 'MD, kW for TZ7(1.0.1.6.7.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ7", "CAST(MDkWDateTimeTZ7 as char(16)) as 'MD, kW for TZ7 date and time(1.0.1.6.7.255)'");

                parameters = parameters.Replace("MDkWTZ8", "MDkWTZ8 as 'MD, kW for TZ8(1.0.1.6.8.255)'");

                parameters = parameters.Replace("MDkWDateTimeTZ8", "CAST(MDkWDateTimeTZ8 as char(16)) as 'MD, kW for TZ8 date and time(1.0.1.6.8.255)'");


                parameters = parameters.Replace("MDkVATZ0", "MDkVATZ0 as 'MD, kVA(1.0.9.6.0.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ0", "CAST(MDkVADateTimeTZ0 as char(16)) as 'MD, kVA Date and time(1.0.9.6.0.255)'");

                parameters = parameters.Replace("MDkVATZ1", "MDkVATZ1 as 'MD, kVA for TZ1(1.0.9.6.1.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ1", "CAST(MDkVADateTimeTZ1 as char(16)) as 'MD, kVA for TZ1 date and time(1.0.9.6.1.255)'");

                parameters = parameters.Replace("MDkVATZ2", "MDkVATZ2 as 'MD, kVA for TZ2(1.0.9.6.2.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ2", "CAST(MDkVADateTimeTZ2 as char(16)) as 'MD, kVA for TZ2 date and time(1.0.9.6.2.255)'");


                parameters = parameters.Replace("MDkVATZ3", "MDkVATZ3 as 'MD, kVA for TZ3(1.0.9.6.3.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ3", "CAST(MDkVADateTimeTZ3 as char(16)) as 'MD, kVA for TZ3 date and time(1.0.9.6.3.255)'");


                parameters = parameters.Replace("MDkVATZ4", "MDkVATZ4 as 'MD, kVA for TZ4(1.0.9.6.4.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ4", "CAST(MDkVADateTimeTZ4 as char(16)) as 'MD, kVA for TZ4 date and time(1.0.9.6.4.255)'");


                parameters = parameters.Replace("MDkVATZ5", "MDkVATZ5 as 'MD, kVA for TZ5(1.0.9.6.5.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ5", "CAST(MDkVADateTimeTZ5 as char(16)) as 'MD, kVA for TZ5 date and time(1.0.9.6.5.255)'");


                parameters = parameters.Replace("MDkVATZ6", "MDkVATZ6 as 'MD, kVA for TZ6(1.0.9.6.6.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ6", "CAST(MDkVADateTimeTZ6 as char(16)) as 'MD, kVA for TZ6 date and time(1.0.9.6.6.255)'");


                parameters = parameters.Replace("MDkVATZ7", "MDkVATZ7 as 'MD, kVA for TZ7(1.0.9.6.7.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ7", "CAST(MDkVADateTimeTZ7 as char(16)) as 'MD, kVA for TZ7 date and time(1.0.9.6.7.255)'");


                parameters = parameters.Replace("MDkVATZ8", "MDkVATZ1 as 'MD, kVA for TZ8(1.0.9.6.8.255)'");

                parameters = parameters.Replace("MDkVADateTimeTZ8", "CAST(MDkVADateTimeTZ8 as char(16)) as 'MD, kVA for TZ8 date and time(1.0.9.6.8.255)'");

                parameters = parameters.Replace("PowerOffDuration", "PowerOffDuration as 'Cumulative power-failure duration(0.0.94.91.8.255)'");

                parameters = parameters.Replace("CumulativeEnergyFraudkWh", "CumulativeEnergyFraudkWh as 'Cumulative Fraud energy, KWh(0.0.96.1.218.255)'");
                parameters = parameters.Replace("CumulativeEnergyFraudkVAh", "CumulativeEnergyFraudkVAh as 'Cumulative Fraud energy, KVAh(0.0.96.2.189.255)'");
                parameters = parameters.Replace("MDkVATZ8", "MDkVATZ1 as 'MD, kVA for TZ8(1.0.9.6.8.255)'");

                // User Story - 1000867
                parameters = parameters.Replace("MDkVArLagTZ0", "MDkVArLagTZ0 as 'MD, kVAr Lag(1.0.5.6.0.255)'");
                parameters = parameters.Replace("MDkVArLagDateTimeTZ0", "CAST(MDkVArLagDateTimeTZ0 as char(16)) as 'MD, kVAr Lag Date and time(1.0.5.6.0.255)'");
                parameters = parameters.Replace("MDkVArLeadTZ0", "MDkVArLeadTZ0 as 'MD, kVAr Lead(1.0.8.6.0.255)'");
                parameters = parameters.Replace("MDkVArLeadDateTimeTZ0", "CAST(MDkVArLeadDateTimeTZ0 as char(16)) 'MD, kVAr Lead date and time(1.0.8.6.0.255)'");

                convertedColumns = parameters;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateColumnNames(string parameters)", ex);
            }

            return convertedColumns;
        }

        public DataSet GetColumnNames(long MeterDataId)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append(@" select BillingDate,SystemPowerFactorforBillingPeriod,
                                    CumulativeEnergykWhTZ0,
                                    CumulativeEnergykWhTZ1,
                                    CumulativeEnergykWhTZ2,
                                    CumulativeEnergykWhTZ3,
                                    CumulativeEnergykWhTZ4,
                                    CumulativeEnergykWhTZ5,
                                    CumulativeEnergykWhTZ6,
                                    CumulativeEnergykWhTZ7,
                                    CumulativeEnergykWhTZ8,
                                    CumulativeEnergykvarhLag,
                                    CumulativeEnergykvarhLead,
                                    CumulativeEnergykVAhTZ0,
                                    CumulativeEnergykVAhTZ1,
                                    CumulativeEnergykVAhTZ2,
                                    CumulativeEnergykVAhTZ3,
                                    CumulativeEnergykVAhTZ4,
                                    CumulativeEnergykVAhTZ5,
                                    CumulativeEnergykVAhTZ6,
                                    CumulativeEnergykVAhTZ7,
                                    CumulativeEnergykVAhTZ8,
                                    MDkWTZ0,                                    
                                    MDkWDateTimeTZ0,
                                    MDkWTZ1,
                                    MDkWDateTimeTZ1,
                                    MDkWTZ2,
                                    MDkWDateTimeTZ2,
                                    MDkWTZ3,
                                    MDkWDateTimeTZ3,
                                    MDkWTZ4,
                                    MDkWDateTimeTZ4,
                                    MDkWTZ5,
                                    MDkWDateTimeTZ5,
                                    MDkWTZ6,
                                    MDkWDateTimeTZ6,
                                    MDkWTZ7,
                                    MDkWDateTimeTZ7,
                                    MDkWTZ8,
                                    MDkWDateTimeTZ8,
                                    MDkVATZ0,
                                    MDkVADateTimeTZ0,
                                    MDkVATZ1,
                                    MDkVADateTimeTZ1,
                                    MDkVATZ2,
                                    MDkVADateTimeTZ2,
                                    MDkVATZ3,
                                    MDkVADateTimeTZ3,
                                    MDkVATZ4,
                                    MDkVADateTimeTZ4,
                                    MDkVATZ5,
                                    MDkVADateTimeTZ5,
                                    MDkVATZ6,
                                    MDkVADateTimeTZ6,
                                    MDkVATZ7,
                                    MDkVADateTimeTZ7,
                                    MDkVATZ8,
                                    PowerOffDuration,
                                    MDkVADateTimeTZ8,
                                    CumulativeEnergyFraudkWh,
                                    CumulativeEnergyFraudkVAh
                                    from meterdata_billing where DataIndex < 61 and ");// Story - 365971 - 13 billing for Power ON Hours // Story - 581355 - 13 billing overright to 60 months billing for Nepal 1P VIM Tender requirement
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID), " and "));
                builder.Append(string.Concat(DataIndex, "=", ParameterName(DataIndex)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), MeterDataId, DbType.Int64);
                request.AddParamter(ParameterName(DataIndex), 0, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetColumnNames(long MeterDataId)", ex);
                dataSet = null;
            }
            return dataSet;
        }
    }
}
