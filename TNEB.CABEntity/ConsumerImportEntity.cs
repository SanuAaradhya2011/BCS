/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;


namespace CAB.Entity
{
    public class IECConsumerImportEntity : EntityBase 
    {
       // public ConsumerMasterEntity ConsumerMaster { get; set; }
        public IECMeterMasterEntity MeterMaster { get; set; }
       // public ConsumerMeterEntity ConsumerMeter { get; set; } 
    }
}
