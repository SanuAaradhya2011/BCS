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
    public class DemandElapsedTimeEntity : EntityBase
    {
        private long demandElapsedTime_ID;
        public long DemandElapsedTime_ID
        {
            get { return this.demandElapsedTime_ID; }
            set { this.demandElapsedTime_ID = value; }
        }
        private string risingDemandKW;
        public string RisingDemandKW
        {
            get { return this.risingDemandKW; }
            set { this.risingDemandKW = value; }
        }
        private string elapsedTimeKW;
        public string ElapsedTimeKW
        {
            get { return this.elapsedTimeKW; }
            set { this.elapsedTimeKW = value; }
        }
        private string risingDemandKVA;
        public string RisingDemandKVA
        {
            get { return this.risingDemandKVA; }
            set { this.risingDemandKVA = value; }
        }
        private string elapsedTimeKVA;
        public string ElapsedTimeKVA
        {
            get { return this.elapsedTimeKVA; }
            set { this.elapsedTimeKVA = value; }
        }
    }
}
