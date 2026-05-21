using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework.Entity;
using System.ComponentModel;
using System.Reflection;

namespace CAB.Entity
{
    public enum kVArSelectionParameters
    {
        [DescriptionAttribute("00")]
        LagOnly,

        [DescriptionAttribute("01")]
        LagandLead,

        [DescriptionAttribute("1")]
        SelectedParameterValue,
    }

    public class IECkvarSelectionEntity : EntityBase
    {
        public string MeterID
        {
            get;set;
        }
        public string LagOnly
        {
            get; set;
        }
        public string LagandLead
        {
            get;set;
        }
    }
}
