#region nameSpaces
using CAB.Framework.Entity;
#endregion
namespace CABEntity
{
    public class SoftwareBillingEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public string SoftwareBillingStatus
        {
            get;
            set;
        }
        public long MeterDataID
        {
            get;
            set;
        }

    }
}
