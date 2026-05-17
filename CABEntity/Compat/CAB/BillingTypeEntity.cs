using System.ComponentModel;
using LNG.Framework.Entity;

namespace LNGEntity
{
    public class BillingTypeEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }
        public BillingMode ModeOfBilling
        { get; set; }
        public BillingPeriod1 BillingPeriod
        { get; set; }
        public string Day
        { get; set; }
        public string Hours
        { get; set; }
        public string Minutes
        { get; set; }
        public string BillingType
        { get; set; }
        public string ResetLockOutDays
        { get; set; }
        public long MeterDataID
        {
            get;
            set;
        }
    }
    public enum BillingMode
    {
        [DescriptionAttribute("393039303930")]
        EndofMonth,
        UserDefined
    }
    public enum BillingPeriod1
    {
        Monthly,
        OddMonth,
        EvenMonth
    }
}

