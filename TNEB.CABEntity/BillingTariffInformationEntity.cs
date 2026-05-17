/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic; 
using System.Text;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class BillingTariffInformationEntity : EntityBase
    {
        private long billingTariffInformation_ID;
        public long BillingTariffInformation_ID
        {
            get { return this.billingTariffInformation_ID; }
            set { this.billingTariffInformation_ID = value; }
        }
        private string averagePowerFactor;
        public string AveragePowerFactor
        {
            get { return this.averagePowerFactor; }
            set { this.averagePowerFactor = value; }
        }
        private int tariff_Number;
        public int Tariff_Number
        {
            get { return this.tariff_Number; }
            set { this.tariff_Number = value; }
        }
        private long billingFactor_ID;
        public long BillingFactor_ID
        {
            get { return this.billingFactor_ID; }
            set { this.billingFactor_ID = value; }
        }
        private long cumulativeDemandBillingTimeStamp_ID;
        public long CumulativeDemandBillingTimeStamp_ID
        {
            get { return this.cumulativeDemandBillingTimeStamp_ID; }
            set { this.cumulativeDemandBillingTimeStamp_ID = value; }
        }
        private long cumulativeEnergy_ID;
        public long CumulativeEnergy_ID
        {
            get { return this.cumulativeEnergy_ID; }
            set { this.cumulativeEnergy_ID = value; }
        }
        private long meterData_ID;
        public long MeterData_ID
        {
            get { return this.meterData_ID; }
            set { this.meterData_ID = value; }
        }
        private long history_ID;
        public long History_ID
        {
            get { return this.history_ID; }
            set { this.history_ID = value; }
        }
    }
}
