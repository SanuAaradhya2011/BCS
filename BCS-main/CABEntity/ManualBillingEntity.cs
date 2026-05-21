#region nameSpaces
using CAB.Framework.Entity;
#endregion
namespace CABEntity
{
    public class ManualBillingEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public string ManualBillingStatus
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
