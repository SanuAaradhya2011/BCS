using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using CAB.Framework.Entity;

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

    public class kvarSelectionEntity : EntityBase
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
        public long MeterDataID
        {
            get;
            set;
        }
    }
}
