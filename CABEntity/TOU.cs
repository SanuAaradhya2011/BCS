using System;
using CAB.Framework.Entity;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											Entity class for TOU parameters 																						|
 * |											Author : Gopal Krishna Gupta    									|
 * |											Date   : 05-Feb-2013	      										|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */
/* GKG JVVNL Current TOU Read */
namespace CAB.Entity
{
    public class TOU :EntityBase
    {
        public long Tou_Id { get; set; } 
        public byte StartHour { get; set; }
        public byte StartMin { get; set; }
        public byte Tariff { get; set; }
        public byte SeasonNumber { get; set; }
        public long MeterData_ID { get; set; } 
    }
}
/* GKG JVVNL Current TOU Read */