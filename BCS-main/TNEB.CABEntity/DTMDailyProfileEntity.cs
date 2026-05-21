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
    public class DTMDailyProfileEntity : EntityBase
    {
        private string powerOnHours = string.Empty;
        public string PowerOnHours
        {
            get { return this.powerOnHours; }
            set { this.powerOnHours = value; }
        }
        private long dTMDailyProfile_ID=0;
        public long DTMDailyProfile_ID
        {
            get { return this.dTMDailyProfile_ID; }
            set { this.dTMDailyProfile_ID = value; }
        }
        private long dailyProfileDate=0;
        public long DailyProfileDate
        {
            get { return this.dailyProfileDate; }
            set { this.dailyProfileDate = value; }
        }
        private string cumulativeFundamentalkWh=string.Empty;
        public string CumulativeFundamentalkWh
        {
            get { return this.cumulativeFundamentalkWh; }
            set { this.cumulativeFundamentalkWh = value; }
        }
        private string cumulativekWh = string.Empty;
        public string CumulativekWh
        {
            get { return this.cumulativekWh; }
            set { this.cumulativekWh = value; }
        }
        private string cumulativekVArh_lag = string.Empty;
        public string CumulativekVArh_lag
        {
            get { return this.cumulativekVArh_lag; }
            set { this.cumulativekVArh_lag = value; }
        }
        private string cumulativekVArh_lead = string.Empty;
        public string CumulativekVArh_lead
        {
            get { return this.cumulativekVArh_lead; }
            set { this.cumulativekVArh_lead = value; }
        }
        private string cumulativekVAh = string.Empty;
        public string CumulativekVAh
        {
            get { return this.cumulativekVAh; }
            set { this.cumulativekVAh = value; }
        }
        private string dailyMD1 = string.Empty;
        public string DailyMD1
        {
            get { return this.dailyMD1; }
            set { this.dailyMD1 = value; }
        }
        private long mD1TimeStamp=0;
        public long MD1TimeStamp
        {
            get { return this.mD1TimeStamp; }
            set { this.mD1TimeStamp = value; }
        }
        private string dailyMD2 = string.Empty;
        public string DailyMD2
        {
            get { return this.dailyMD2; }
            set { this.dailyMD2 = value; }
        }
        private long mD2TimeStamp=0;
        public long MD2TimeStamp
        {
            get { return this.mD2TimeStamp; }
            set { this.mD2TimeStamp = value; }
        }
        private string dailyMD3 = string.Empty;
        public string DailyMD3
        {
            get { return this.dailyMD3; }
            set { this.dailyMD3 = value; }
        }
        private long mD3TimeStamp=0;
        public long MD3TimeStamp
        {
            get { return this.mD3TimeStamp; }
            set { this.mD3TimeStamp = value; }
        }
        private string maxAvgVoltage = string.Empty;
        public string MaxAvgVoltage
        {
            get { return this.maxAvgVoltage; }
            set { this.maxAvgVoltage = value; }
        }
        private string minAvgVoltage = string.Empty;
        public string MinAvgVoltage
        {
            get { return this.minAvgVoltage; }
            set { this.minAvgVoltage = value; }
        }
        private string maxAvgCurrent = string.Empty;
        public string MaxAvgCurrent
        {
            get { return this.maxAvgCurrent; }
            set { this.maxAvgCurrent = value; }
        }
        private string minAvgCurrent = string.Empty;
        public string MinAvgCurrent
        {
            get { return this.minAvgCurrent; }
            set { this.minAvgCurrent = value; }
        }
        private string availableDays = string.Empty;
        public string AvailableDays
        {
            get { return this.availableDays; }
            set { this.availableDays = value; }
        }
        private string maximumDays = string.Empty;
        public string MaximumDays
        {
            get { return this.maximumDays; }
            set { this.maximumDays = value; }
        }
        private long meterData_ID=0;
        public long MeterData_ID
        {
            get { return this.meterData_ID; }
            set { this.meterData_ID = value; }
        }
        private string parameters = string.Empty;
        public string Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }
        public long ReadingDateTime { get; set; }
        public string MeterID { get; set; }

        private string PowerFactor = string.Empty;
        public string POWERFACTOR
        {
            get { return this.PowerFactor; }
            set { this.PowerFactor = value; }
        }
        private string AvgVoltage = string.Empty;
        public string AVERAGeVoltage
        {
            get { return this.AvgVoltage; }
            set { this.AvgVoltage = value; }
        }
        private string AvgCurrent = string.Empty;
        public string AVERAGECURRENT
        {
            get { return this.AvgCurrent; }
            set { this.AvgCurrent = value; }
        }
        private string AvgNeutralCurrent = string.Empty;
        public string AVERAGEneautralCURRENT
        {
            get { return this.AvgNeutralCurrent; }
            set { this.AvgNeutralCurrent = value; }
        }
    }
}
