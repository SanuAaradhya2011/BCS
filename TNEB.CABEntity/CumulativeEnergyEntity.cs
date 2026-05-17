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
    public class CumulativeEnergyEntity : EntityBase
    {
        private long cumulativeEnergy_ID;
        public long CumulativeEnergy_ID
        {
            get { return this.cumulativeEnergy_ID; }
            set { this.cumulativeEnergy_ID = value; }
        }
        private string cumulativeEnergyKWH;
        public string CumulativeEnergyKWH
        {
            get { return this.cumulativeEnergyKWH; }
            set { this.cumulativeEnergyKWH = value; }
        }
        private string cumulativeEnergyKVARHLag;
        public string CumulativeEnergyKVARHLag
        {
            get { return this.cumulativeEnergyKVARHLag; }
            set { this.cumulativeEnergyKVARHLag = value; }
        }
        private string cumulativeEnergyKVARHLead;
        public string CumulativeEnergyKVARHLead
        {
            get { return this.cumulativeEnergyKVARHLead; }
            set { this.cumulativeEnergyKVARHLead = value; }
        }
        private string cumulativeEnergyKVAH;
        public string CumulativeEnergyKVAH
        {
            get { return this.cumulativeEnergyKVAH; }
            set { this.cumulativeEnergyKVAH = value; }
        }
    }
}