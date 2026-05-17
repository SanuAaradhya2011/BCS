/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 						|
 * | 																										|
 * |											Author : Piyush Singh. 	 										|
 * | 																										|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic; 
using System.Text;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class IECPhasorEntity : EntityBase
    {
        private long phasor_ID;       
        public long Phasor_ID
        {
            get
            {
                return phasor_ID;
            }
            set
            {
                phasor_ID = value;
            }
        }
        private string rPhaseVoltage;
        public string RPhaseVoltage
        {
            get
            {
                return rPhaseVoltage;
            }
            set
            {
                rPhaseVoltage = value;
            }
        }
        private string yPhaseVoltage;
        public string YPhaseVoltage
        {
            get
            {
                return yPhaseVoltage;
            }
            set
            {
                yPhaseVoltage = value;
            }
        }
        private string bPhaseVoltage;
        public string BPhaseVoltage
        {
            get
            {
                return bPhaseVoltage;
            }
            set
            {
                bPhaseVoltage = value;
            }
        }
        private string rPhaseCurrent;
        public string RPhaseCurrent
        {
            get
            {
                return rPhaseCurrent;
            }
            set
            {
                rPhaseCurrent = value;
            }
        }
        private string yPhaseCurrent;
        public string YPhaseCurrent
        {
            get
            {
                return yPhaseCurrent;
            }
            set
            {
                yPhaseCurrent = value;
            }
        }
        private string bPhaseCurrent;
        public string BPhaseCurrent
        {
            get
            {
                return bPhaseCurrent;
            }
            set
            {
                bPhaseCurrent = value;
            }
        }

        private string totalActivePower;
        public string TotalActivePower
        {
            get
            {
                return totalActivePower;
            }
            set
            {
                totalActivePower = value;
            }
        }
        private string totalInductivePower;
        public string TotalInductivePower
        {
            get
            {
                return totalInductivePower;
            }
            set
            {
                totalInductivePower = value;
            }
        }
        private string totalCapacitivePower;
        public string TotalCapacitivePower
        {
            get
            {
                return totalCapacitivePower;
            }
            set
            {
                totalCapacitivePower = value;
            }
        }
        private string totalApparentPower;
        public string TotalApparentPower
        {
            get
            {
                return totalApparentPower;
            }
            set
            {
                totalApparentPower = value;
            }
        }
        private string rPhasePF;
        public string RPhasePF
        {
            get
            {
                return rPhasePF;
            }
            set
            {
                rPhasePF = value;
            }
        }
        private string yPhasePF;
        public string YPhasePF
        {
            get
            {
                return yPhasePF;
            }
            set
            {
                yPhasePF = value;
            }
        }
        private string bPhasePF;
        public string BPhasePF
        {
            get
            {
                return bPhasePF;
            }
            set
            {
                bPhasePF = value;
            }
        }
        private string totalInstantaneousPF;
        public string TotalInstantaneousPF
        {
            get
            {
                return totalInstantaneousPF;
            }
            set
            {
                totalInstantaneousPF = value;
            }
        }
        private string frequency;
        public string Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }
        private string phaseSequence;
        public string PhaseSequence
        {
            get
            {
                return phaseSequence;
            }
            set
            {
                phaseSequence = value;
            }
        }
        private string totalkWDirection;
        public string TotalkWDirection
        {
            get
            {
                return totalkWDirection;
            }
            set
            {
                totalkWDirection = value;
            }
        }
        private string rPhasekWDirection;
        public string RPhasekWDirection
        {
            get
            {
                return rPhasekWDirection;
            }
            set
            {
                rPhasekWDirection = value;
            }
        }
        private string yPhasekWDirection;
        public string YPhasekWDirection
        {
            get
            {
                return yPhasekWDirection;
            }
            set
            {
                yPhasekWDirection = value;
            }
        }
        private string bPhasekWDirection;
        public string BPhasekWDirection
        {
            get
            {
                return bPhasekWDirection;
            }
            set
            {
                bPhasekWDirection = value;
            }
        }
        private string rPhaseChannel;
        public string RPhaseChannel
        {
            get
            {
                return rPhaseChannel;
            }
            set
            {
                rPhaseChannel = value;
            }
        }
        private string yPhaseChannel;
        public string YPhaseChannel
        {
            get
            {
                return yPhaseChannel;
            }
            set
            {
                yPhaseChannel = value;
            }
        }
        private string bPhaseChannel;
        public string BPhaseChannel
        {
            get
            {
                return bPhaseChannel;
            }
            set
            {
                bPhaseChannel = value;
            }
        }
        private string rPhaseLagLead;
        public string RPhaseLagLead
        {
            get
            {
                return rPhaseLagLead;
            }
            set
            {
                rPhaseLagLead = value;
            }
        }
        private string yPhaseLagLead;
        public string YPhaseLagLead
        {
            get
            {
                return yPhaseLagLead;
            }
            set
            {
                yPhaseLagLead = value;
            }
        }
        private string bPhaseLagLead;
        public string BPhaseLagLead
        {
            get
            {
                return bPhaseLagLead;
            }
            set
            {
                bPhaseLagLead = value;
            }
        }
        private string total;
        public string Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
            }
        }
        private string yPhaseAngleWithRPhase;
        public string YPhaseAngleWithRPhase
        {
            get
            {
                return yPhaseAngleWithRPhase;
            }
            set
            {
                yPhaseAngleWithRPhase = value;
            }
        }
        private string bPhaseAngleWithRPhase;
        public string BPhaseAngleWithRPhase
        {
            get
            {
                return bPhaseAngleWithRPhase;
            }
            set
            {
                bPhaseAngleWithRPhase = value;
            }
        }
        private string angleBWAnyPhasePresent;
        public string AngleBWAnyPhasePresent
        {
            get
            {
                return angleBWAnyPhasePresent;
            }
            set
            {
                angleBWAnyPhasePresent = value;
            }
        }
        private long meterData_ID;
        public long MeterData_ID
        {
            get { return this.meterData_ID; }
            set { this.meterData_ID = value; }
        }
        public long ReadingDateTime { get; set; }
        public string MeterID { get; set; }
        private string cmriID;
        public string CMRIID
        {
        
           get
           {
               return cmriID;
           }
           set
           {
              cmriID=value;
           }
        }
        private string cmriType;
        public string CMRIType
        {
            get
            { 
               return cmriType;
            }
            set
            {
                cmriType = value;            
            }
        }     
       
       
    }
}