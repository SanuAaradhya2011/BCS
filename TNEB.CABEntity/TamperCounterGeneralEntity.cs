/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 29/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the entities of meterdata_tampercountergeneral Table.
    /// </summary>
    public class TamperCounterGeneralEntity : EntityBase
    {
        /// Private variable for Group ID.
        private long tamperCounterGeneral_ID;
        private int voltageImbalanceRPhaseTamperCounter;
        private int voltageImbalanceYPhaseTamperCounter;
        private int voltageImbalanceBPhaseTamperCounter;
        private int missingPotentialRPhaseTamperCounter;
        private int missingPotentialYPhaseTamperCounter;
        private int missingPotentialBPhaseTamperCounter;
        private int lowUnderVoltageRPhaseTamperCounter;
        private int lowUnderVoltageYPhaseTamperCounter;
        private int lowUnderVoltageBPhaseTamperCounter;
        private int highOverVoltageRPhaseTamperCounter;
        private int highOverVoltageYPhaseTamperCounter;
        private int highOverVoltageBPhaseTamperCounter;
        private int ctShortTamperCounter;
        private int ctOpenRPhaseTamperCounter;
        private int ctOpenYPhaseTamperCounter;
        private int ctOpenBPhaseTamperCounter;
        private int currentWithoutVoltageRPhaseTamperCounter;
        private int currentWithoutVoltageYPhaseTamperCounter;
        private int currentWithoutVoltageBPhaseTamperCounter;
        private int lowPowerFactorRPhaseTamperCounter;
        private int lowPowerFactorYPhaseTamperCounter;
        private int lowPowerFactorBPhaseTamperCounter;
        private int onePhaseNeutralAbsentTamperCounter;
        private int currentPhaseReversalTamperCounter;
        private int voltagePhaseReversalTamperCounter;
        private int currentImbalanceRPhaseTamperCounter;
        private int currentImbalanceYPhaseTamperCounter;
        private int currentImbalanceBPhaseTamperCounter;
        private int currentReversalRPhaseTamperCounter;
        private int currentReversalYPhaseTamperCounter;
        private int currentReversalBPhaseTamperCounter;
        private int magneticInfluenceTamperCounter;
        private int neutralDisturbanceTamperCounter;
        private int frontCoverOpeningTamperCounter;
        private int terminalCoverOpeningTamperCounter;
        private long history_ID;
        private long meterData_ID;
        private long billingTimeStamp;
        private string relatedTo;
        /// <summary>
        /// This property is used to get and set the value of TamperCounterGeneral ID. 
        /// This field is autoincremented in database table.
        /// </summary>
        public long TamperCounterGeneral_ID
        {
            get
            {
                return tamperCounterGeneral_ID;
            }
            set
            {
                tamperCounterGeneral_ID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Voltage Imbalance R Phase tamper counter.
        /// </summary>
        public int VoltageImbalanceRPhaseTamperCounter
        {
            get
            {
                return voltageImbalanceRPhaseTamperCounter;
            }
            set
            {
                voltageImbalanceRPhaseTamperCounter = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Voltage Imbalance Y Phase tamper counter.
        /// </summary>
        public int VoltageImbalanceYPhaseTamperCounter
        {
            get
            {
                return voltageImbalanceYPhaseTamperCounter;
            }
            set
            {
                voltageImbalanceYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Voltage Imbalance B Phase tamper counter.
        /// </summary>
        public int VoltageImbalanceBPhaseTamperCounter
        {
            get
            {
                return voltageImbalanceBPhaseTamperCounter;
            }
            set
            {
                voltageImbalanceBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Missing Potential R Phase tamper counter.
        /// </summary>
        public int MissingPotentialRPhaseTamperCounter
        {
            get
            {
                return missingPotentialRPhaseTamperCounter;
            }
            set
            {
                missingPotentialRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Missing Potential Y Phase tamper counter.
        /// </summary>
        public int MissingPotentialYPhaseTamperCounter
        {
            get
            {
                return missingPotentialYPhaseTamperCounter;
            }
            set
            {
                missingPotentialYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Missing Potential B Phase tamper counter.
        /// </summary>
        public int MissingPotentialBPhaseTamperCounter
        {
            get
            {
                return missingPotentialBPhaseTamperCounter;
            }
            set
            {
                missingPotentialBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Low/Under Voltage R Phase tamper counter.
        /// </summary>
        public int LowUnderVoltageRPhaseTamperCounter
        {
            get
            {
                return lowUnderVoltageRPhaseTamperCounter;
            }
            set
            {
                lowUnderVoltageRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Low/Under Voltage Y Phase tamper counter.
        /// </summary>
        public int LowUnderVoltageYPhaseTamperCounter
        {
            get
            {
                return lowUnderVoltageYPhaseTamperCounter;
            }
            set
            {
                lowUnderVoltageYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Low/Under Voltage B Phase tamper counter.
        /// </summary>
        public int LowUnderVoltageBPhaseTamperCounter
        {
            get
            {
                return lowUnderVoltageBPhaseTamperCounter;
            }
            set
            {
                lowUnderVoltageBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of High/Over Voltage R Phase tamper counter.
        /// </summary>
        public int HighOverVoltageRPhaseTamperCounter
        {
            get
            {
                return highOverVoltageRPhaseTamperCounter;
            }
            set
            {
                highOverVoltageRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of High/Over Voltage Y Phase tamper counter.
        /// </summary>
        public int HighOverVoltageYPhaseTamperCounter
        {
            get
            {
                return highOverVoltageYPhaseTamperCounter;
            }
            set
            {
                highOverVoltageYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of High/Over Voltage B Phase tamper counter.
        /// </summary>
        public int HighOverVoltageBPhaseTamperCounter
        {
            get
            {
                return highOverVoltageBPhaseTamperCounter;
            }
            set
            {
                highOverVoltageBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of CT Short Tamper tamper counter.
        /// </summary>
        public int CTShortTamperCounter
        {
            get
            {
                return ctShortTamperCounter;
            }
            set
            {
                ctShortTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of CT Open R Phase tamper counter.
        /// </summary>
        public int CTOpenRPhaseTamperCounter
        {
            get
            {
                return ctOpenRPhaseTamperCounter;
            }
            set
            {
                ctOpenRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of CT Open Y Phase tamper counter.
        /// </summary>
        public int CTOpenYPhaseTamperCounter
        {
            get
            {
                return ctOpenYPhaseTamperCounter;
            }
            set
            {
                ctOpenYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of CT Open B Phase tamper counter.
        /// </summary>
        public int CTOpenBPhaseTamperCounter
        {
            get
            {
                return ctOpenBPhaseTamperCounter;
            }
            set
            {
                ctOpenBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Without Voltage R Phase tamper counter.
        /// </summary>
        public int CurrentWithoutVoltageRPhaseTamperCounter
        {
            get
            {
                return currentWithoutVoltageRPhaseTamperCounter;
            }
            set
            {
                currentWithoutVoltageRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Without Voltage Y Phase tamper counter.
        /// </summary>
        public int CurrentWithoutVoltageYPhaseTamperCounter
        {
            get
            {
                return currentWithoutVoltageYPhaseTamperCounter;
            }
            set
            {
                currentWithoutVoltageYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Without Voltage B Phase tamper counter.
        /// </summary>
        public int CurrentWithoutVoltageBPhaseTamperCounter
        {
            get
            {
                return currentWithoutVoltageBPhaseTamperCounter;
            }
            set
            {
                currentWithoutVoltageBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Low Power Factor R Phase tamper counter.
        /// </summary>
        public int LowPowerFactorRPhaseTamperCounter
        {
            get
            {
                return lowPowerFactorRPhaseTamperCounter;
            }
            set
            {
                lowPowerFactorRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Low Power Factor Y Phase tamper counter.
        /// </summary>
        public int LowPowerFactorYPhaseTamperCounter
        {
            get
            {
                return lowPowerFactorYPhaseTamperCounter;
            }
            set
            {
                lowPowerFactorYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Low Power Factor B Phase tamper counter.
        /// </summary>
        public int LowPowerFactorBPhaseTamperCounter
        {
            get
            {
                return lowPowerFactorBPhaseTamperCounter;
            }
            set
            {
                lowPowerFactorBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of One Phase Neutral Absent tamper counter.
        /// </summary>
        public int OnePhaseNeutralAbsentTamperCounter
        {
            get
            {
                return onePhaseNeutralAbsentTamperCounter;
            }
            set
            {
                onePhaseNeutralAbsentTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Phase Reversal tamper counter.
        /// </summary>
        public int CurrentPhaseReversalTamperCounter
        {
            get
            {
                return currentPhaseReversalTamperCounter;
            }
            set
            {
                currentPhaseReversalTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Voltage Phase Reversal tamper counter.
        /// </summary>
        public int VoltagePhaseReversalTamperCounter
        {
            get
            {
                return voltagePhaseReversalTamperCounter;
            }
            set
            {
                voltagePhaseReversalTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Imbalance R Phase tamper counter.
        /// </summary>
        public int CurrentImbalanceRPhaseTamperCounter
        {
            get
            {
                return currentImbalanceRPhaseTamperCounter;
            }
            set
            {
                currentImbalanceRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Imbalance Y Phase tamper counter.
        /// </summary>
        public int CurrentImbalanceYPhaseTamperCounter
        {
            get
            {
                return currentImbalanceYPhaseTamperCounter;
            }
            set
            {
                currentImbalanceYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Imbalance B Phase tamper counter.
        /// </summary>
        public int CurrentImbalanceBPhaseTamperCounter
        {
            get
            {
                return currentImbalanceBPhaseTamperCounter;
            }
            set
            {
                currentImbalanceBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Reversal R Phase tamper counter.
        /// </summary>
        public int CurrentReversalRPhaseTamperCounter
        {
            get
            {
                return currentReversalRPhaseTamperCounter;
            }
            set
            {
                currentReversalRPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Reversal Y Phase tamper counter.
        /// </summary>
        public int CurrentReversalYPhaseTamperCounter
        {
            get
            {
                return currentReversalYPhaseTamperCounter;
            }
            set
            {
                currentReversalYPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Current Reversal B Phase tamper counter.
        /// </summary>
        public int CurrentReversalBPhaseTamperCounter
        {
            get
            {
                return currentReversalBPhaseTamperCounter;
            }
            set
            {
                currentReversalBPhaseTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Magnetic Influence tamper counter.
        /// </summary>
        public int MagneticInfluenceTamperCounter
        {
            get
            {
                return magneticInfluenceTamperCounter;
            }
            set
            {
                magneticInfluenceTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Neutral Disturbance tamper counter.
        /// </summary>
        public int NeutralDisturbanceTamperCounter
        {
            get
            {
                return neutralDisturbanceTamperCounter;
            }
            set
            {
                neutralDisturbanceTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Front Cover Opening tamper counter.
        /// </summary>
        public int FrontCoverOpeningTamperCounter
        {
            get
            {
                return frontCoverOpeningTamperCounter;
            }
            set
            {
                frontCoverOpeningTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Terminal Cover Opening tamper counter.
        /// </summary>
        public int TerminalCoverOpeningTamperCounter
        {
            get
            {
                return terminalCoverOpeningTamperCounter;
            }
            set
            {
                terminalCoverOpeningTamperCounter = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of history ID.
        /// </summary>
        public long History_ID
        {
            get
            {
                return history_ID;
            }
            set
            {
                history_ID = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of MeterData ID.
        /// </summary>
        public long MeterData_ID
        {
            get
            {
                return meterData_ID;
            }
            set
            {
                meterData_ID = value;
            }
        }
        public long BillingTimeStamp
        {
            get
            {
                return billingTimeStamp;
            }
            set
            {
                billingTimeStamp = value;
            }
        }
        private long billingCounter;
        public long BillingCounter
        {
            get
            {
                return billingCounter;
            }
            set
            {
                billingCounter = value;
            }
        }
        public string RelatedTo
        {
            get
            {
                return relatedTo;
            }
            set
            {
                relatedTo = value;
            }
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
                cmriID = value;
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