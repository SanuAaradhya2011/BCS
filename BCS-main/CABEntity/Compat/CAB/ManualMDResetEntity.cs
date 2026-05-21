#region nameSpaces
using LNG.Framework.Entity;
#endregion
namespace LNGEntity
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

