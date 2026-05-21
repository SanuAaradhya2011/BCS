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
    public class CurrentTimeStampEntity : EntityBase
    {
        private long currentTimeStamp_ID;
        public long CurrentTimeStamp_ID
        {
            get { return this.currentTimeStamp_ID; }
            set { this.currentTimeStamp_ID = value; }
        }
        private string currentMD1;
        public string CurrentMD1
        {
            get { return this.currentMD1; }
            set { this.currentMD1 = value; }
        }
        private string currentMD1TimeStamp;
        public string CurrentMD1TimeStamp
        {
            get { return this.currentMD1TimeStamp; }
            set { this.currentMD1TimeStamp = value; }
        }
        private string currentMD2;
        public string CurrentMD2
        {
            get { return this.currentMD2; }
            set { this.currentMD2 = value; }
        }
        private string currentMD2TimeStamp;
        public string CurrentMD2TimeStamp
        {
            get { return this.currentMD2TimeStamp; }
            set { this.currentMD2TimeStamp = value; }
        }
        private string currentMD3;
        public string CurrentMD3
        {
            get { return this.currentMD3; }
            set { this.currentMD3 = value; }
        }
        private string currentMD3TimeStamp;
        public string CurrentMD3TimeStamp
        {
            get { return this.currentMD3TimeStamp; }
            set { this.currentMD3TimeStamp = value; }
        }
    }
}
