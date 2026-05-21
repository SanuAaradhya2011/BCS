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
    public class GeneralEntity : EntityBase
    {
        private long general_ID;
        private string meterID;
        private long meterDateTime;
        private string errorCode;
        private string meterConstant;
        private string firmwareVersion;
        private string voltagePhaseSequence;
        private string totalActiveEnergy;
        private string cumulativeMD1;
        private string cumulativeMD2;
        private string risingDemandKW;
        private string elapsedTimeKW;
        private string risingDemandKVA;
        private string elapsedTimeKVA;
        private string totalPowerOnHours;
        private string currentMonthPowerOnHours;
        private string mDResetCounter;
        private string readoutCounter;
        private string programmingCounter;
        private string latestTamperOccurrenceID;
        private string occurrenceTime;
        private string latestTamperRestorationID;
        private string restorationTime;
        private long meterData_ID;
        private string bateryModePowerOnHour;
        private string powerOffDays;
        private string cumulativeExportEnergyKWH;
        private string cumulativeExportEnergyKVAH;
        public string BateryModePowerOnHour
        {
            get { return bateryModePowerOnHour; }
            set { bateryModePowerOnHour = value; }
        }
        public string PowerOffDays
        {
            get { return powerOffDays; }
            set { powerOffDays = value; }
        }
        public long MeterData_ID
        {
            get { return meterData_ID; }
            set { meterData_ID = value; }
        }
        public long General_ID
        {
            get { return general_ID; }
            set { general_ID = value; }
        }
        public string MeterID
        {
            get { return meterID; }
            set { meterID = value; }
        }
        public long MeterDateTime
        {
            get { return meterDateTime; }
            set { meterDateTime = value; }
        }
        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        public string MeterConstant
        {
            get { return meterConstant; }
            set { meterConstant = value; }
        }
        public string FirmwareVersion
        {
            get { return firmwareVersion; }
            set { firmwareVersion = value; }
        }

        public string VoltagePhaseSequence
        {
            get { return voltagePhaseSequence; }
            set { voltagePhaseSequence = value; }
        }
        public string TotalActiveEnergy
        {
            get { return totalActiveEnergy; }
            set { totalActiveEnergy = value; }
        }
        public string CumulativeMD1
        {
            get { return cumulativeMD1; }
            set { cumulativeMD1 = value; }
        }
        public string CumulativeMD2
        {
            get { return cumulativeMD2; }
            set { cumulativeMD2 = value; }
        }
        public string RisingDemandKW
        {
            get { return risingDemandKW; }
            set { risingDemandKW = value; }
        }
        public string ElapsedTimeKW
        {
            get { return elapsedTimeKW; }
            set { elapsedTimeKW = value; }
        }
        public string RisingDemandKVA
        {
            get { return risingDemandKVA; }
            set { risingDemandKVA = value; }
        }
        public string ElapsedTimeKVA
        {
            get { return elapsedTimeKVA; }
            set { elapsedTimeKVA = value; }
        }
        public string TotalPowerOnHours
        {
            get { return totalPowerOnHours; }
            set { totalPowerOnHours = value; }
        }
        public string CurrentMonthPowerOnHours
        {
            get { return currentMonthPowerOnHours; }
            set { currentMonthPowerOnHours = value; }
        }

        public string MDResetCounter
        {
            get { return mDResetCounter; }
            set { mDResetCounter = value; }
        }
        public string ReadoutCounter
        {
            get { return readoutCounter; }
            set { readoutCounter = value; }
        }
        public string ProgrammingCounter
        {
            get { return programmingCounter; }
            set { programmingCounter = value; }
        }

        public string LatestTamperOccurrenceID
        {
            get { return latestTamperOccurrenceID; }
            set { latestTamperOccurrenceID = value; }
        }
        public string OccurrenceTime
        {
            get { return occurrenceTime; }
            set { occurrenceTime = value; }
        }
        public string LatestTamperRestorationID
        {
            get { return latestTamperRestorationID; }
            set { latestTamperRestorationID = value; }
        }
        public string RestorationTime
        {
            get { return restorationTime; }
            set { restorationTime = value; }
        }
        public string CumulativeExportEnergyKWH
        {
            get { return cumulativeExportEnergyKWH; }
            set { cumulativeExportEnergyKWH = value; }
        }
        public string CumulativeExportEnergyKVAH
        {
            get { return cumulativeExportEnergyKVAH; }
            set { cumulativeExportEnergyKVAH = value; }
        }

        public string LastManualDemandReset { get; set; }

        public string AccuracyClass { get; set; }
        public string VoltageRating { get; set; }
        public string CurrentRating { get; set; }
        public string TotalPowerOnMinutes { get; set; }
        public string TotalPowerOnMinutesCreepLevel { get; set; }
        public string RTCBatteryVoltage { get; set; }
        public string DisplayBatteryVoltage { get; set; }
        public string MeterManufacturing { get; set; }
        public string LegalEnergy { get; set; }
        public string FraudEnergy { get; set; }
        public string BillingDateTime { get; set; }       
        public string ApparentEnergy { get; set; }
        public string ReactiveEnergy { get; set; }      
        public string CumulativeMD { get; set; }
        public string TamperDurationMinutes { get; set; }
        public string TotalMagTamperDurationMin { get; set; }
        public string TotalNDDurationMinutes { get; set; }
        public string TotalReverseTamperDurationMinutes { get; set; }
        public string TotalEarthTamperDurationMinutes { get; set; }

    }
}
