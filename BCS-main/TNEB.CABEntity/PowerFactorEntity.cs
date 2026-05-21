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
    public class PowerFactorEntity : EntityBase
    {
        private long powerFactor_ID;
        public long PowerFactor_ID
        {
            get { return this.powerFactor_ID; }
            set { this.powerFactor_ID = value; }
        }
        private string totalPowerFactor;
        public string TotalPowerFactor
        {
            get { return this.totalPowerFactor; }
            set { this.totalPowerFactor = value; }
        }
        private string powerFactorRPhase;
        public string PowerFactorRPhase
        {
            get { return this.powerFactorRPhase; }
            set { this.powerFactorRPhase = value; }
        }
        private string powerFactorYPhase;
        public string PowerFactorYPhase
        {
            get { return this.powerFactorYPhase; }
            set { this.powerFactorYPhase = value; }
        }
        private string powerFactorBPhase;
        public string PowerFactorBPhase
        {
            get { return this.powerFactorBPhase; }
            set { this.powerFactorBPhase = value; }
        }
        private string averagePowerFactor;
        public string AveragePowerFactor
        {
            get { return this.averagePowerFactor; }
            set { this.averagePowerFactor = value; }
        }
    }
}
