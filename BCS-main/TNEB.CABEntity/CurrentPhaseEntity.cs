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
    public class CurrentPhaseEntity : EntityBase
    {
        private long currentPhase_ID;
        public long CurrentPhase_ID
        {
            get { return this.currentPhase_ID; }
            set { this.currentPhase_ID = value; }
        }
        private string currentPhaseSequence;
        public string CurrentPhaseSequence
        {
            get { return this.currentPhaseSequence; }
            set { this.currentPhaseSequence = value; }
        }
        private string currentRPhase;
        public string CurrentRPhase
        {
            get { return this.currentRPhase; }
            set { this.currentRPhase = value; }
        }
        private string currentYPhase;
        public string CurrentYPhase
        {
            get { return this.currentYPhase; }
            set { this.currentYPhase = value; }
        }
        private string currentBPhase;
        public string CurrentBPhase
        {
            get { return this.currentBPhase; }
            set { this.currentBPhase = value; }
        }
    }
}
