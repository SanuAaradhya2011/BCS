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
    public class InstantPowerEntity : EntityBase
    {


        private long instantPower_ID;
        public long InstantPower_ID
        {
            get { return this.instantPower_ID; }
            set { this.instantPower_ID = value; }
        }
        private string meterID;
        public string MeterID
        {
            get { return this.meterID; }
            set { this.meterID = value; }
        }
        private long meterDateTime;
        public long MeterDateTime
        {
            get { return this.meterDateTime; }
            set { this.meterDateTime = value; }
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

        private string instantActivepower;
        public string InstantActivepower
        {
            get { return this.instantActivepower; }
            set { this.instantActivepower = value; }
        }
        private string instantReactiveLagPower;
        public string InstantReactiveLagPower
        {
            get { return this.instantReactiveLagPower; }
            set { this.instantReactiveLagPower = value; }
        }
        private string instantReactiveLeadPower;
        public string InstantReactiveLeadPower
        {
            get { return this.instantReactiveLeadPower; }
            set { this.instantReactiveLeadPower = value; }
        }
        private string instantApparentPower;
        public string InstantApparentPower
        {
            get { return this.instantApparentPower; }
            set { this.instantApparentPower = value; }
        }
        private string totalPowerFactor;
        public string TotalPowerFactor
        {
            get { return this.totalPowerFactor; }
            set { this.totalPowerFactor = value; }
        }

        private string powerFactor;
        public string PowerFactor
        {
            get { return this.powerFactor; }
            set { this.powerFactor = value; }
        }

        private string presentMonthAveragePF;
        public string PresentMonthAveragePF
        {
            get { return this.presentMonthAveragePF; }
            set { this.presentMonthAveragePF = value; }
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

        private string frequency;
        public string Frequency
        {
            get { return this.frequency; }
            set { this.frequency = value; }
        }
        private string totalFundamentalActiveEnergy;
        public string TotalFundamentalActiveEnergy
        {
            get { return this.totalFundamentalActiveEnergy; }
            set { this.totalFundamentalActiveEnergy = value; }
        }
        private long meterData_ID;
        public long MeterData_ID
        {
            get { return meterData_ID; }
            set { meterData_ID = value; }
        }

        // Story - 365960 - More instant parameters for single phase non DLMS integration
        private string manufactureDateTime;
        public string ManufactureDateTime
        {
            get { return this.manufactureDateTime; }
            set { this.manufactureDateTime = value; }
        }
        private string totalPowerOffMinutes;
        public string TotalPowerOffMinutes
        {
            get { return this.totalPowerOffMinutes; }
            set { this.totalPowerOffMinutes = value; }
        }
        private string singleWireTamperDuration;
        public string SingleWireTamperDuration
        {
            get { return this.singleWireTamperDuration; }
            set { this.singleWireTamperDuration = value; }
        }
        private string magnetTamperDuration;
        public string MagnetTamperDuration
        {
            get { return this.magnetTamperDuration; }
            set { this.magnetTamperDuration = value; }
        }
        private string neutralDisturbanceTamperDuration;
        public string NeutralDisturbanceTamperDuration
        {
            get { return this.neutralDisturbanceTamperDuration; }
            set { this.neutralDisturbanceTamperDuration = value; }
        }
        private string earthTamperDuration;
        public string EarthTamperDuration
        {
            get { return this.earthTamperDuration; }
            set { this.earthTamperDuration = value; }
        }
        private string reverseTamperDuration;
        public string ReverseTamperDuration
        {
            get { return this.reverseTamperDuration; }
            set { this.reverseTamperDuration = value; }
        }

        private string esdTamperDuration;
        public string ESDTamperDuration
        {
            get { return this.esdTamperDuration; }
            set { this.esdTamperDuration = value; }
        }
        //newly added

        public string InstantActivepowerRPhase
        {
            get;
            set;
        }
        public string InstantActivepowerYPhase
        {
            get;
            set;
        }
        public string InstantActivepowerBPhase
        {
            get;
            set;
        }
        public string InstantReactivepowerRPhase
        {
            get;
            set;
        }
        public string InstantReactivepowerYPhase
        {
            get;
            set;
        }
        public string InstantReactivepowerBPhase
        {
            get;
            set;
        }
        public string InstantApparentpowerRPhase
        {
            get;
            set;
        }
        public string InstantApparentpowerYPhase
        {
            get;
            set;
        }
        public string InstantApparentpowerBPhase
        {
            get;
            set;
        }
        public string PhaseVoltage { get; set; }
        public string PhaseCurrent { get; set; }
        public string NeutralCurrent { get; set; }

        public string NeutralPowerKW { get; set; }
        public string ActivePowerKW { get; set; }
        public string TotalPowerOnMinutes { get; set; }
        public string MDResetCounter { get; set; }
        public string ReadoutCounter { get; set; }
        public string ProgrammingCounter { get; set; }
        public string ProgrammedBillDayTime { get; set; }
        public string PowerfailCount { get; set; }
        public string FraudEnergy { get; set; }
        public string LegalEnergy { get; set; }
        public string LineFrequency { get; set; }

        public string SignedPowerFactor { get; set; }
        public string ReactivePower { get; set; }
        public string ReactivePowerNeutral { get; set; }
        public string AveragePowerFactor { get; set; }
        public string TamperResetCounter { get; set; }

        public string CumulativeEnergyKVAh { get; set; }
        public string CumulativeEnergyKWh { get; set; }
        public string CumulativeEnergyKVArh { get; set; }
        public string ABC { get; set; }
        public string ABCType2Bill1 { get; set; }
        public string ABCType2Bill2 { get; set; }
        public string MeterDateAndTime { get; set; }
        public string CumulativeActiveMDKWh { get; set; }

        #region Tamper counters
        public string EarthTamperCounter { get; set; }
        public string MagnetTamperCounter { get; set; }
        public string NeutralDisturbanceTamperCounter { get; set; }
        public string SingleWireTamperCounter { get; set; }
        public string ESDTamperCounter { get; set; }
        public string CoverOpenTamperCounter { get; set; }
        public string TotalTamperCounter { get; set; }
        public string OverLoadTamperCounter { get; set; }
        public string LowVoltageTamperCounter { get; set; }
        public string LowPFTamperCounter { get; set; }
        public string ReverseTamperCounter { get; set; }
        //public string TotalTransactionCounter { get; set; }// ProgrammingCounter is same as TotalTransactionCounter
        #endregion      

    }
}
