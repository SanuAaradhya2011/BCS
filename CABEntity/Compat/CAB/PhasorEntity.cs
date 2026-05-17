using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;
namespace LNG.Entity
{
    /// <summary>
    /// This class is entity for phasor data.
    /// </summary>
    public class PhasorEntity : EntityBase
    {
        private string rPhaseCurrent;
        private string yPhaseCurrent;
        private string bPhaseCurrent;
        private string rPhaseVoltage;
        private string yPhaseVoltage;
        private string bPhaseVoltage;
        private string rPhasePowerFactor;
        private string yPhasePowerFactor;
        private string bPhasePowerFactor;
        private long currentDateTime;
        private string totalPhasePowerFactor;
        private string frequency;
        private string apparentPower;
        private string activePower;
        private string reActivePower;
        private string numberOfPowerFailMin;
        private string cumulativePowerFailMin;
        private string cumulativeTamperCounter;
        private string cumulativeBillingCounter;
        private string cumulativeProgrammingCounter;
        private string cumulativeActiveEnergy;
        private string cumulativeReactiveLagEnergy;
        private string cumulativeReactiveLeadEnergy;
        private string cumulativeApparentEnergy;
        private string mdOneKWData;
        private long mdOneKWTimeStamp;
        private string mdTwoKVAData;
        private long mdTwoKVATimeStamp;
        private long billingDate;
        private string rPhaseNegativePowerFlag;
        private string yPhaseNegativePowerFlag;
        private string bPhaseNegativePowerFlag;
        private string rPhaseCapacitiveInductiveFlag;
        private string yPhaseCapacitiveInductiveFlag;
        private string bPhaseCapacitiveInductiveFlag;
        private string angleYR;
        private string angleBR;
        private string angleBetweenTwo;
        private string rPhaseChannel;
        private string yPhaseChannel;
        private string bPhaseChannel;
        private long meterDataId;
        private long phasorId;
        private double tempVoltage;
        private string phaseSequence;
        private string totalActivePower;
        private string totalInductivePower;
        private string totalCapacitivePower;
        private string totalApparentPower;
        private string rPhasePF;
        private string yPhasePF;
        private string bPhasePF;
        private string totalInstantaneousPF;
        private string totalkWDirection;
        private string rPhasekWDirection;
        private string yPhasekWDirection;
        private string bPhasekWDirection;
        private string rPhaseLagLead;
        private string yPhaseLagLead;
        private string bPhaseLagLead;

        public long CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }
            set
            {
                currentDateTime = value;
            }
        }
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
        public string RPhasePowerFactor
        {
            get
            {
                return rPhasePowerFactor;
            }
            set
            {
                rPhasePowerFactor = value;
            }
        }
        public string YPhasePowerFactor
        {
            get
            {
                return yPhasePowerFactor;
            }
            set
            {
                yPhasePowerFactor = value;
            }
        }
        public string BPhasePowerFactor
        {
            get
            {
                return bPhasePowerFactor;
            }
            set
            {
                bPhasePowerFactor = value;
            }
        }
        public string TotalPhasePowerFactor
        {
            get
            {
                return totalPhasePowerFactor;
            }
            set
            {
                totalPhasePowerFactor = value;
            }
        }
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
        public string ApparentPower
        {
            get
            {
                return apparentPower;
            }
            set
            {
                apparentPower = value;
            }
        }
        public string ReActivePower
        {
            get
            {
                return reActivePower;
            }
            set
            {
                reActivePower = value;
            }
        }
        public string NumberOfPowerFailMin
        {
            get
            {
                return numberOfPowerFailMin;
            }
            set
            {
                numberOfPowerFailMin = value;
            }
        }
        public string CumulativePowerFailMin
        {
            get
            {
                return cumulativePowerFailMin;
            }
            set
            {
                cumulativePowerFailMin = value;
            }
        }
        public string CumulativeTamperCounter
        {
            get
            {
                return cumulativeTamperCounter;
            }
            set
            {
                cumulativeTamperCounter = value;
            }
        }
        public string CumulativeBillingCounter
        {
            get
            {
                return cumulativeBillingCounter;
            }
            set
            {
                cumulativeBillingCounter = value;
            }
        }
        public string CumulativeProgrammingCounter
        {
            get
            {
                return cumulativeProgrammingCounter;
            }
            set
            {
                cumulativeProgrammingCounter = value;
            }
        }
        public long BillingDate
        {
            get
            {
                return billingDate;
            }
            set
            {
                billingDate = value;
            }
        }
        public string CumulativeActiveEnergy
        {
            get
            {
                return cumulativeActiveEnergy;
            }
            set
            {
                cumulativeActiveEnergy = value;
            }
        }
        public string CumulativeReactiveLagEnergy
        {
            get
            {
                return cumulativeReactiveLagEnergy;
            }
            set
            {
                cumulativeReactiveLagEnergy = value;
            }
        }
        public string CumulativeReactiveLeadEnergy
        {
            get
            {
                return cumulativeReactiveLeadEnergy;
            }
            set
            {
                cumulativeReactiveLeadEnergy = value;
            }
        }
        public string CumulativeApparentEnergy
        {
            get
            {
                return cumulativeApparentEnergy;
            }
            set
            {
                cumulativeApparentEnergy = value;
            }
        }
        public string MDOneKWData
        {
            get
            {
                return mdOneKWData;
            }
            set
            {
                mdOneKWData = value;
            }
        }
        public long MDOneKWTimeStamp
        {
            get
            {
                return mdOneKWTimeStamp;
            }
            set
            {
                mdOneKWTimeStamp = value;
            }
        }


        public string MDTwoKVAData
        {
            get
            {
                return mdTwoKVAData;
            }
            set
            {
                mdTwoKVAData = value;
            }
        }
        public long MDTwoKVATimeStamp
        {
            get
            {
                return mdTwoKVATimeStamp;
            }
            set
            {
                mdTwoKVATimeStamp = value;
            }
        }
        public string RPhaseNegativePowerFlag
        {
            get
            {
                return rPhaseNegativePowerFlag;
            }
            set
            {
                rPhaseNegativePowerFlag = value;
            }
        }
        public string YPhaseNegativePowerFlag
        {
            get
            {
                return yPhaseNegativePowerFlag;
            }
            set
            {
                yPhaseNegativePowerFlag = value;
            }
        }
        public string BPhaseNegativePowerFlag
        {
            get
            {
                return bPhaseNegativePowerFlag;
            }
            set
            {
                bPhaseNegativePowerFlag = value;
            }

        }
        public string RPhaseCapacitiveInductiveFlag
        {
            get
            {
                return rPhaseCapacitiveInductiveFlag;
            }
            set
            {
                rPhaseCapacitiveInductiveFlag = value;
            }
        }

        public string YPhaseCapacitiveInductiveFlag
        {
            get
            {
                return yPhaseCapacitiveInductiveFlag;
            }
            set
            {
                yPhaseCapacitiveInductiveFlag = value;
            }
        }
        public string BPhaseCapacitiveInductiveFlag
        {
            get
            {
                return bPhaseCapacitiveInductiveFlag;
            }
            set
            {
                bPhaseCapacitiveInductiveFlag = value;
            }
        }
        public string AngleYR
        {
            get
            {
                return angleYR;
            }
            set
            {
                angleYR = value;
            }
        }
        public string AngleBR
        {
            get
            {
                return angleBR;
            }
            set
            {
                angleBR = value;
            }
        }
        public string AngleBetweenTwo
        {
            get
            {
                return angleBetweenTwo;
            }
            set
            {
                angleBetweenTwo = value;
            }
        }
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

        //public string RPhaseChannel
        //{

        //    get
        //    {
        //        rPhaseChannel = "Absent";
        //        tempVoltage = 0;
        //        if (double.TryParse(rPhaseVoltage, out tempVoltage))
        //        {
        //            if (tempVoltage > 0)
        //            {
        //                rPhaseChannel = "Present";
        //            }
        //        }

        //        return rPhaseChannel;
        //    }
        //}

        //public string YPhaseChannel
        //{
        //    get
        //    {
        //        tempVoltage = 0;
        //        yPhaseChannel = "Absent";
        //        if (double.TryParse(yPhaseVoltage, out tempVoltage))
        //        {
        //            if (tempVoltage > 0)
        //            {
        //                yPhaseChannel = "Present";
        //            }
        //        }

        //        return yPhaseChannel;
        //    }

        //}

        //public string BPhaseChannel
        //{
        //    get
        //    {
        //        tempVoltage = 0;
        //        bPhaseChannel = "Absent";
        //        if (double.TryParse(bPhaseVoltage, out tempVoltage))
        //        {
        //            if (tempVoltage > 0)
        //            {
        //                bPhaseChannel = "Present";
        //            }
        //        }

        //        return bPhaseChannel;
        //    }
        //}

        public string ActivePower
        {
            get
            {
                return activePower;
            }
            set
            {
                activePower = value;
            }
        }

        public long MeterDataId
        {
            get
            {
                return meterDataId;
            }
            set
            {
                meterDataId = value;
            }
        }

        public long PhasorId
        {
            get
            {
                return phasorId;
            }
            set
            {
                phasorId = value;
            }
        }

        public string PhaseSequence
        {
            get { return phaseSequence; }
            set { phaseSequence = value; }
        }
    }
}

