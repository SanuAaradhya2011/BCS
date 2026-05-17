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
    public class ProgrammingEntity : EntityBase
    {
        private long programming_ID;
        public long Programming_ID
        {
            get { return this.programming_ID; }
            set { this.programming_ID = value; }
        }
        private string totalProgrammingUpdates;
        public string TotalProgrammingUpdates
        {
            get { return this.totalProgrammingUpdates; }
            set { this.totalProgrammingUpdates = value; }
        }
        private string updateSequence;
        public string UpdateSequence
        {
            get { return this.updateSequence; }
            set { this.updateSequence = value; }
        }
        private string lastTimestamp;
        public string LastTimestamp
        {
            get { return this.lastTimestamp; }
            set { this.lastTimestamp = value; }
        }
        private string description1;
        public string Description1
        {
            get { return this.description1; }
            set { this.description1 = value; }
        }
        private string description2;
        public string Description2
        {
            get { return this.description2; }
            set { this.description2 = value; }
        }
        private string description3;
        public string Description3
        {
            get { return this.description3; }
            set { this.description3 = value; }
        }
        private string description4;
        public string Description4
        {
            get { return this.description4; }
            set { this.description4 = value; }
        }
        private string description5;
        public string Description5
        {
            get { return this.description5; }
            set { this.description5 = value; }
        }
        private string description6;
        public string Description6
        {
            get { return this.description6; }
            set { this.description6 = value; }
        }
        private string description7;
        public string Description7
        {
            get { return this.description7; }
            set { this.description7 = value; }
        }
        private string description8;
        public string Description8
        {
            get { return this.description8; }
            set { this.description8 = value; }
        }
        private string description9;
        public string Description9
        {
            get { return this.description9; }
            set { this.description9 = value; }
        }
        private string description10;
        public string Description10
        {
            get { return this.description10; }
            set { this.description10 = value; }
        }
        private string description11;
        public string Description11
        {
            get { return this.description11; }
            set { this.description11 = value; }
        }
        private string description12;
        public string Description12
        {
            get { return this.description12; }
            set { this.description12 = value; }
        }
        private string description13;
        public string Description13
        {
            get { return this.description13; }
            set { this.description13 = value; }
        }
        private string description14;
        public string Description14
        {
            get { return this.description14; }
            set { this.description14 = value; }
        }
        private string description15;
        public string Description15
        {
            get { return this.description15; }
            set { this.description15 = value; }
        }
        private string description16;
        public string Description16
        {
            get { return this.description16; }
            set { this.description16 = value; }
        }
        private string description17;
        public string Description17
        {
            get { return this.description17; }
            set { this.description17 = value; }

        }
        private string description18;
        public string Description18
        {
            get { return this.description18; }
            set { this.description18 = value; }

        }
        private string description19;
        public string Description19
        {
            get { return this.description19; }
            set { this.description19 = value; }

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
