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
    public class VoltagePhaseEntity : EntityBase
    {
        private long voltagePhase_ID;
        public long VoltagePhase_ID
        {
            get { return this.voltagePhase_ID; }
            set { this.voltagePhase_ID = value; }
        }
        private string voltagePhaseSequence;
        public string VoltagePhaseSequence
        {
            get { return this.voltagePhaseSequence; }
            set { this.voltagePhaseSequence = value; }
        }
        private string voltageRPhase;
        public string VoltageRPhase
        {
            get { return this.voltageRPhase; }
            set { this.voltageRPhase = value; }
        }
        private string voltageYPhase;
        public string VoltageYPhase
        {
            get { return this.voltageYPhase; }
            set { this.voltageYPhase = value; }
        }
        private string voltageBPhase;
        public string VoltageBPhase
        {
            get { return this.voltageBPhase; }
            set { this.voltageBPhase = value; }
        }
    }
}
