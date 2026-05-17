using CAB.IECFramework.Entity;
using System.ComponentModel;

namespace CABEntity
{
    public class BillingResetEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }
        public IECBillingMode ModeOfBilling
        { get; set; }
        public BillingPeriod1 BillingPeriod
        { get; set; }
        public string Day
        { get; set; }
        public string Hours
        { get; set; }
        public string Minutes
        { get; set; }
        public string ResetLockOutDays
        { get; set; }

    }
    public enum IECBillingMode
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
