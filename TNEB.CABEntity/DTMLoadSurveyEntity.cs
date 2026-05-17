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
    public class DTMLoadSurveyEntity : EntityBase
    {
        private long dTMDailyProfile_ID;
        public long DTMDailyProfile_ID
        {
            get { return this.dTMDailyProfile_ID; }
            set { this.dTMDailyProfile_ID = value; }
        }
        private long dTMLoadSurvey_ID;
        public long DTMLoadSurvey_ID
        {
            get { return this.dTMLoadSurvey_ID; }
            set { this.dTMLoadSurvey_ID = value; }
        }
        private long dTMDateTime;
        public long DTMDateTime
        {
            get { return this.dTMDateTime; }
            set { this.dTMDateTime = value; }
        }
        private string kWh;
        public string KWh
        {
            get { return this.kWh; }
            set { this.kWh = value; }
        }
        private string kVAh;
        public string KVAh
        {
            get { return this.kVAh; }
            set { this.kVAh = value; }
        }
        private string rPhaseKW;
        public string RPhaseKW
        {
            get { return this.rPhaseKW; }
            set { this.rPhaseKW = value; }
        }
        private string yPhaseKW;
        public string YPhaseKW
        {
            get { return this.yPhaseKW; }
            set { this.yPhaseKW = value; }
        }
        private string bPhaseKW;
        public string BPhaseKW
        {
            get { return this.bPhaseKW; }
            set { this.bPhaseKW = value; }
        }
        private string rPhaseKVAr;
        public string RPhaseKVAr
        {
            get { return this.rPhaseKVAr; }
            set { this.rPhaseKVAr = value; }
        }
        private string rPhaseType;
        public string RPhaseType
        {
            get { return this.rPhaseType; }
            set { this.rPhaseType = value; }
        }
        private string yPhaseKVAr;
        public string YPhaseKVAr
        {
            get { return this.yPhaseKVAr; }
            set { this.yPhaseKVAr = value; }
        }
        private string yPhaseType;
        public string YPhaseType
        {
            get { return this.yPhaseType; }
            set { this.yPhaseType = value; }
        }
        private string bPhaseKVAr;
        public string BPhaseKVAr
        {
            get { return this.bPhaseKVAr; }
            set { this.bPhaseKVAr = value; }
        }
        private string bPhaseType;
        public string BPhaseType
        {
            get { return this.bPhaseType; }
            set { this.bPhaseType = value; }
        }
        private string rPhaseVoltage;
        public string RPhaseVoltage
        {
            get { return this.rPhaseVoltage; }
            set { this.rPhaseVoltage = value; }
        }
        private string yPhaseVoltage;
        public string YPhaseVoltage
        {
            get { return this.yPhaseVoltage; }
            set { this.yPhaseVoltage = value; }
        }
        private string bPhaseVoltage;
        public string BPhaseVoltage
        {
            get { return this.bPhaseVoltage; }
            set { this.bPhaseVoltage = value; }
        }
        private string powerDownTime;
        public string PowerDownTime
        {
            get { return this.powerDownTime; }
            set { this.powerDownTime = value; }
        }
        private long meterData_ID;
        public long MeterData_ID
        {
            get { return this.meterData_ID; }
            set { this.meterData_ID = value; }
        } 
    }
}
