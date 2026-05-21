
using System;
using CAB.Framework.Entity;
using System.Collections.Generic;
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
    public class TOUEntity : EntityBase
    {
        public long MeterDataId { get; set; }
        public List<TOU> tou {get; set;}
    }
}
/* GKG JVVNL Current TOU Read */