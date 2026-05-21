#region nameSpaces
using LNG.Framework.Entity;
#endregion
namespace LNGEntity
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

