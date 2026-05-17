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
    public class TariffDemandEntity : EntityBase
    {
        private long tariffDemand_ID;
        public long TariffDemand_ID
        {
            get { return this.tariffDemand_ID; }
            set { this.tariffDemand_ID = value; }
        }
        private string demandMD1;
        public string DemandMD1
        {
            get { return this.demandMD1; }
            set { this.demandMD1 = value; }
        }
        private string demandMD1TimeStamp;
        public string DemandMD1TimeStamp
        {
            get { return this.demandMD1TimeStamp; }
            set { this.demandMD1TimeStamp = value; }
        }
        private string demandMD2;
        public string DemandMD2
        {
            get { return this.demandMD2; }
            set { this.demandMD2 = value; }
        }
        private string demandMD2TimeStamp;
        public string DemandMD2TimeStamp
        {
            get { return this.demandMD2TimeStamp; }
            set { this.demandMD2TimeStamp = value; }
        }
        private string demandMD3;
        public string DemandMD3
        {
            get { return this.demandMD3; }
            set { this.demandMD3 = value; }
        }
        private string demandMD3TimeStamp;
        public string DemandMD3TimeStamp
        {
            get { return this.demandMD3TimeStamp; }
            set { this.demandMD3TimeStamp = value; }
        }
        private long tariffInformation_ID;
        public long TariffInformation_ID
        {
            get { return this.tariffInformation_ID; }
            set { this.tariffInformation_ID = value; }
        }
        private long billingTariffInformation_ID;
        public long BillingTariffInformation_ID
        {
            get { return this.billingTariffInformation_ID; }
            set { this.billingTariffInformation_ID = value; }
        }
    }
}
