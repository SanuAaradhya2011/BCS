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
using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class GSMConfigEntity : EntityBase
    {
        public string Meter_ID { get; set; }
        public long SIM_Number { get; set; }
    }
}
