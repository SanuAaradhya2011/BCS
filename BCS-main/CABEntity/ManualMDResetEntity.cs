#region nameSpaces
using CAB.Framework.Entity;
#endregion
namespace CABEntity
{
    public class ManualMDResetEntity : EntityBase
    {
        public string MeterID
        {
            get;
            set;
        }

        public string ManualMDResetStatus
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
