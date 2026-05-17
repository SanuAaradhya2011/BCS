using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework.Entity;

namespace CABEntity
{
    public class IECMeterAccuracyCheckEntity : EntityBase
    {
        public decimal kWh
        {get; set;}

        public decimal kVAh
        { get; set; }
        public decimal kvarhLag
        { get; set; }
        public decimal kvarhLead
        { get; set; }

        public decimal ReversekWh
        { get; set; }

        public decimal ReversekVAh
        { get; set; }
        public decimal ReversekvarhLag
        { get; set; }
        public decimal ReversekvarhLead
        { get; set; }


    }
}
