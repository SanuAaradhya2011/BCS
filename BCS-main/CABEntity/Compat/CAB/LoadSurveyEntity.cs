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
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class LoadSurveyEntity : EntityBase
    {
        private long loadSurvey_ID;
        public long LoadSurvey_ID
        {
            get { return this.loadSurvey_ID; }
            set { this.loadSurvey_ID = value; }
        }
        private long meterReadingDatetime;
        public long MeterReadingDatetime
        {
            get { return this.meterReadingDatetime; }
            set { this.meterReadingDatetime = value; }
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
        private string rPhaseCurrent;
        public string RPhaseCurrent
        {
            get { return this.rPhaseCurrent; }
            set { this.rPhaseCurrent = value; }
        }
        private string yPhaseCurrent;
        public string YPhaseCurrent
        {
            get { return this.yPhaseCurrent; }
            set { this.yPhaseCurrent = value; }
        }
        private string bPhaseCurrent;
        public string BPhaseCurrent
        {
            get { return this.bPhaseCurrent; }
            set { this.bPhaseCurrent = value; }
        }
        private string avgVoltage;
        public string AvgVoltage
        {
            get { return this.avgVoltage; }
            set { this.avgVoltage = value; }
        }
        private string avgCurrent;
        public string AvgCurrent
        {
            get { return this.avgCurrent; }
            set { this.avgCurrent = value; }
        }
        private string demandKVARLead;
        public string DemandKVARLead
        {
            get { return this.demandKVARLead; }
            set { this.demandKVARLead = value; }
        }
        private string demandKVA;
        public string DemandKVA
        {
            get { return this.demandKVA; }
            set { this.demandKVA = value; }
        }
        private string demandKW;
        public string DemandKW
        {
            get { return this.demandKW; }
            set { this.demandKW = value; }
        }
        private string demandKVARLag;
        public string DemandKVARLag
        {
            get { return this.demandKVARLag; }
            set { this.demandKVARLag = value; }
        }
        private string powerFactor;
        public string PowerFactor
        {
            get { return this.powerFactor; }
            set { this.powerFactor = value; }
        }
        private string tamperStatus;
        public string TamperStatus
        {
            get { return this.tamperStatus; }
            set { this.tamperStatus = value; }
        }
        private long loadSurveyDateTime;
        public long LoadSurveyDateTime
        {
            get { return this.loadSurveyDateTime; }
            set { this.loadSurveyDateTime = value; }
        }
        private int mDIntervalPeriod;
        public int MDIntervalPeriod
        {
            get { return this.mDIntervalPeriod; }
            set { this.mDIntervalPeriod = value; }
        }
        private long meterData_ID;
        public long MeterData_ID
        {
            get { return this.meterData_ID; }
            set { this.meterData_ID = value; }
        }

        public long NeutralCurrent//add pradipta_load_neu
        {
            get { return this.NeutralCurrent; }
            set { this.NeutralCurrent = value; }
        }


        private string parameters;
        public string Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }
        public long ReadingDateTime { get; set; }
        public string MeterID { get; set; }
    }
}

