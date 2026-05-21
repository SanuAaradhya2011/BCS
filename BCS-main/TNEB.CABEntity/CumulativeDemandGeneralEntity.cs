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
    public class CumulativeDemandGeneralEntity : EntityBase
    {
        private long cumulativeDemandGeneral_ID;
        public long CumulativeDemandGeneral_ID
        {
            get { return this.cumulativeDemandGeneral_ID; }
            set { this.cumulativeDemandGeneral_ID = value; }
        }
        private string cumulativeMD1;
        public string CumulativeMD1
        {
            get { return this.cumulativeMD1; }
            set { this.cumulativeMD1 = value; }
        }
        private string cumulativeMD2;
        public string CumulativeMD2
        {
            get { return this.cumulativeMD2; }
            set { this.cumulativeMD2 = value; }
        }
        private string cumulativeMD3;
        public string CumulativeMD3
        {
            get { return this.cumulativeMD3; }
            set { this.cumulativeMD3 = value; }
        }
    }
}
