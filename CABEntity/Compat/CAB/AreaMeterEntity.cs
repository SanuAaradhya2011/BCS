using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class AreaMeterEntity : EntityBase
    {
        private long area_ID;
        private string meter_ID;
        private long areameter_ID;
        public long AreaMeter_ID
        {
            get
            {
                return areameter_ID;
            }
            set
            {
                areameter_ID = value;
            }
        }
        public long Area_ID
        {
            get
            {
                return area_ID;
            }
            set
            {
                area_ID = value;
            }

        }
        public string Meter_ID
        {
            get
            {
                return meter_ID;
            }
            set
            {
                meter_ID = value;
            }
        }
    }
}

