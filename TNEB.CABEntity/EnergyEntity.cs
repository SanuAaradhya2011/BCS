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
    public class EnergyEntity : EntityBase
    {
        private long energy_ID;
        public long Energy_ID
        {
            get { return this.energy_ID; }
            set { this.energy_ID = value; }
        }
        private string totalFundamentalActiveEnergy;
        public string TotalFundamentalActiveEnergy
        {
            get { return this.totalFundamentalActiveEnergy; }
            set { this.totalFundamentalActiveEnergy = value; }
        }
        private string totalActiveEnergy;
        public string TotalActiveEnergy
        {
            get { return this.totalActiveEnergy; }
            set { this.totalActiveEnergy = value; }
        }
        private string totalReactiveLagEnergy;
        public string TotalReactiveLagEnergy
        {
            get { return this.totalReactiveLagEnergy; }
            set { this.totalReactiveLagEnergy = value; }
        }
        private string totalReactiveLeadEnergy;
        public string TotalReactiveLeadEnergy
        {
            get { return this.totalReactiveLeadEnergy; }
            set { this.totalReactiveLeadEnergy = value; }
        }
        private string totalApparentEnergy;
        public string TotalApparentEnergy
        {
            get { return this.totalApparentEnergy; }
            set { this.totalApparentEnergy = value; }
        }
    }
}
