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
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class BillingEntity : EntityBase
    {
        private long billing_ID;
        private string cTRatio;
        private string cumulativeEnergyKWH;
        private string cumulativeEnergyKVARHLag;
        private string cumulativeEnergyKVARHLead;
        private string cumulativeEnergyKVAH;
        private string cumulativeMD1;
        private string cumulativeMD1TimeStamp;
        private string cumulativeMD2;
        private string cumulativeMD2TimeStamp;
        private string cumulativeMD3;
        private string cumulativeMD3TimeStamp;
        private string averagePowerFactor;
        private string powerOnHours;
        private string loadFactor; 
        private long meterData_ID;
        private long history_ID;
         public long History_ID
        {
            get { return history_ID; }
            set { history_ID = value; }
        }
        public long MeterData_ID
        {
            get { return meterData_ID; }
            set { meterData_ID = value; }
        }
        public long Billing_ID
        {
            get { return billing_ID; }
            set { billing_ID = value; }
        }
        public string CTRatio
        {
            get { return cTRatio; }
            set { cTRatio = value; }
        }
        public string CumulativeEnergyKWH
        {
            get { return cumulativeEnergyKWH; }
            set { cumulativeEnergyKWH = value; }
        }
        public string CumulativeEnergyKVARHLag
        {
            get { return cumulativeEnergyKVARHLag; }
            set { cumulativeEnergyKVARHLag = value; }
        }
        public string CumulativeEnergyKVARHLead
        {
            get { return cumulativeEnergyKVARHLead; }
            set { cumulativeEnergyKVARHLead = value; }
        }
        public string CumulativeEnergyKVAH
        {
            get { return cumulativeEnergyKVAH; }
            set { cumulativeEnergyKVAH = value; }
        }
        public string CumulativeMD1
        {
            get { return cumulativeMD1; }
            set { cumulativeMD1 = value; }
        }
        public string CumulativeMD1TimeStamp
        {
            get { return cumulativeMD1TimeStamp; }
            set { cumulativeMD1TimeStamp = value; }
        }
        public string CumulativeMD2
        {
            get { return cumulativeMD2; }
            set { cumulativeMD2 = value; }
        }
        public string CumulativeMD2TimeStamp
        {
            get { return cumulativeMD2TimeStamp; }
            set { cumulativeMD2TimeStamp = value; }
        }
        public string CumulativeMD3
        {
            get { return cumulativeMD3; }
            set { cumulativeMD3 = value; }
        }
        public string CumulativeMD3TimeStamp
        {
            get { return cumulativeMD3TimeStamp; }
            set { cumulativeMD3TimeStamp = value; }
        }
        public string AveragePowerFactor
        {
            get { return averagePowerFactor; }
            set { averagePowerFactor = value; }
        }
        public string PowerOnHours
        {
            get { return powerOnHours; }
            set { powerOnHours = value; }
        }
        public string LoadFactor
        {
            get { return loadFactor; }
            set { loadFactor = value; }
        }
 
          
    }
}

