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
    public class TariffEnergyEntity : EntityBase
    {
        private long tariffEnergy_ID;
        public long TariffEnergy_ID
        {
            get { return this.tariffEnergy_ID; }
            set { this.tariffEnergy_ID = value; }
        }
        private string kwh;
        public string Kwh
        {
            get { return this.kwh; }
            set { this.kwh = value; }
        }
        private string kVARhLag;
        public string KVARhLag
        {
            get { return this.kVARhLag; }
            set { this.kVARhLag = value; }
        }
        private string kVARhLead;
        public string KVARhLead
        {
            get { return this.kVARhLead; }
            set { this.kVARhLead = value; }
        }
        private string kVAh;
        public string KVAh
        {
            get { return this.kVAh; }
            set { this.kVAh = value; }
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
