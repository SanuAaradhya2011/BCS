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
    public class BillingFactorEntity : EntityBase
    {
        private long billingFactor_ID;
        public long BillingFactor_ID
        {
            get { return this.billingFactor_ID; }
            set { this.billingFactor_ID = value; }
        }
        private string cTRatio;
        public string CTRatio
        {
            get { return this.cTRatio; }
            set { this.cTRatio = value; }
        }
        private string averagePowerFactor;
        public string AveragePowerFactor
        {
            get { return this.averagePowerFactor; }
            set { this.averagePowerFactor = value; }
        }
        private string powerOnHours;
        public string PowerOnHours
        {
            get { return this.powerOnHours; }
            set { this.powerOnHours = value; }
        }
        private string loadFactor;
        public string LoadFactor
        {
            get { return this.loadFactor; }
            set { this.loadFactor = value; }
        }
        private long history_ID;
        public long History_ID
        {
            get { return this.history_ID; }
            set { this.history_ID = value; }
        }
    }
}
