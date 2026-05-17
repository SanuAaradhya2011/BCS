/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;


namespace LNG.Entity
{
    public class ConsumerImportEntity : EntityBase 
    {
        public ConsumerMasterEntity ConsumerMaster { get; set; }
        public MeterMasterEntity MeterMaster { get; set; }
        public ConsumerMeterEntity ConsumerMeter { get; set; }
        // following three entites added to resolve bug 73549; 12th April 2012
        public RegionEntity Region { get; set; }
        public CircleEntity Circle { get; set; }
        public DivisionEntity Division { get; set; }
    }
}

