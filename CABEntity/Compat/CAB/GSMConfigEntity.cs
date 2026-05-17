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
    public class GSMConfigEntity : EntityBase
    {
        public string Meter_ID { get; set; }
        public long SIM_Number { get; set; }
    }
}

