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
    public class RTCUpdateEntity : EntityBase
    {
        private long rTCUpdate_ID;
        public long RTCUpdate_ID
        {
            get { return this.rTCUpdate_ID; }
            set { this.rTCUpdate_ID = value; }
        }
        private string totalRTCUpdates;
        public string TotalRTCUpdates
        {
            get { return this.totalRTCUpdates; }
            set { this.totalRTCUpdates = value; }
        }



        private string currentRTC1;
        public string CurrentRTC1
        {
            get { return this.currentRTC1; }
            set { this.currentRTC1 = value; }
        }

        private string previousRTC1;
        public string PreviousRTC1
        {
            get { return this.previousRTC1; }
            set { this.previousRTC1 = value; }
        }

        private string currentRTC2;
        public string CurrentRTC2
        {
            get { return this.currentRTC2; }
            set { this.currentRTC2 = value; }
        }

        private string previousRTC2;
        public string PreviousRTC2
        {
            get { return this.previousRTC2; }
            set { this.previousRTC2 = value; }
        }

        private string currentRTC3;
        public string CurrentRTC3
        {
            get { return this.currentRTC3; }
            set { this.currentRTC3 = value; }
        }

        private string previousRTC3;
        public string PreviousRTC3
        {
            get { return this.previousRTC3; }
            set { this.previousRTC3 = value; }
        }

        private string currentRTC4;
        public string CurrentRTC4
        {
            get { return this.currentRTC4; }
            set { this.currentRTC4 = value; }
        }

        private string previousRTC4;
        public string PreviousRTC4
        {
            get { return this.previousRTC4; }
            set { this.previousRTC4 = value; }
        }

        private string currentRTC5;
        public string CurrentRTC5
        {
            get { return this.currentRTC5; }
            set { this.currentRTC5 = value; }
        }

        private string previousRTC5;
        public string PreviousRTC5
        {
            get { return this.previousRTC5; }
            set { this.previousRTC5 = value; }
        }

        private string currentRTC6;
        public string CurrentRTC6
        {
            get { return this.currentRTC6; }
            set { this.currentRTC6 = value; }
        }

        private string previousRTC6;
        public string PreviousRTC6
        {
            get { return this.previousRTC6; }
            set { this.previousRTC6 = value; }
        }

        private string currentRTC7;
        public string CurrentRTC7
        {
            get { return this.currentRTC7; }
            set { this.currentRTC7 = value; }
        }

        private string previousRTC7;
        public string PreviousRTC7
        {
            get { return this.previousRTC7; }
            set { this.previousRTC7 = value; }
        }

        private string currentRTC8;
        public string CurrentRTC8
        {
            get { return this.currentRTC8; }
            set { this.currentRTC8 = value; }
        }

        private string previousRTC8;
        public string PreviousRTC8
        {
            get { return this.previousRTC8; }
            set { this.previousRTC8 = value; }
        }

        private string currentRTC9;
        public string CurrentRTC9
        {
            get { return this.currentRTC9; }
            set { this.currentRTC9 = value; }
        }

        private string previousRTC9;
        public string PreviousRTC9
        {
            get { return this.previousRTC9; }
            set { this.previousRTC9 = value; }
        }

        private string currentRTC10;
        public string CurrentRTC10
        {
            get { return this.currentRTC10; }
            set { this.currentRTC10 = value; }
        }

        private string previousRTC10;
        public string PreviousRTC10
        {
            get { return this.previousRTC10; }
            set { this.previousRTC10 = value; }
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
