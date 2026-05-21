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
    public class CounterEntity : EntityBase
    {
         private long counter_ID;
        public long Counter_ID
        {
            get { return this.counter_ID; }
            set { this.counter_ID = value; }
        }
        private string mDResetCounter;
        public string MDResetCounter
        {
            get { return this.mDResetCounter; }
            set { this.mDResetCounter = value; }
        }
   private string readoutCounter;
        public string ReadoutCounter
        {
            get { return this.readoutCounter; }
            set { this.readoutCounter = value; }
        }
    private string programmingCounter;
        public string ProgrammingCounter
        {
            get { return this.programmingCounter; }
            set { this.programmingCounter = value; }
        }
         private string cTRatioProgrammingCounter;
        public string CTRatioProgrammingCounter
        {
            get { return this.CTRatioProgrammingCounter; }
            set { this.CTRatioProgrammingCounter = value; }
        }  
    }
}
