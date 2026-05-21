#region nameSpaces
using LNG.Framework.Entity;
#endregion
namespace LNGEntity
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

