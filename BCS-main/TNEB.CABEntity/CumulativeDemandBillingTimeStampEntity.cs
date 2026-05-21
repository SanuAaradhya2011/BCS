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
    public class CumulativeDemandBillingTimeStampEntity : EntityBase
    {
        private long cumulativeDemandBillingTimeStamp_ID;
        public long CumulativeDemandBillingTimeStamp_ID
        {
            get { return this.cumulativeDemandBillingTimeStamp_ID; }
            set { this.cumulativeDemandBillingTimeStamp_ID = value; }
        }
        private string cumulativeMD1;
        public string CumulativeMD1
        {
            get { return this.cumulativeMD1; }
            set { this.cumulativeMD1 = value; }
        }
        private string cumulativeMD1TimeStamp;
        public string CumulativeMD1TimeStamp
        {
            get { return this.cumulativeMD1TimeStamp; }
            set { this.cumulativeMD1TimeStamp = value; }
        }
        private string cumulativeMD2;
        public string CumulativeMD2
        {
            get { return this.cumulativeMD2; }
            set { this.cumulativeMD2 = value; }
        }
        private string cumulativeMD2TimeStamp;
        public string CumulativeMD2TimeStamp
        {
            get { return this.cumulativeMD2TimeStamp; }
            set { this.cumulativeMD2TimeStamp = value; }
        }
        //private string cumulativeMD3;
        //public string CumulativeMD3
        //{
        //    get { return this.cumulativeMD3; }
        //    set { this.cumulativeMD3 = value; }
        //}
        //private string cumulativeMD3TimeStamp;
        //public string CumulativeMD3TimeStamp
        //{
        //    get { return this.cumulativeMD3TimeStamp; }
        //    set { this.cumulativeMD3TimeStamp = value; }
        //}
    }
}
