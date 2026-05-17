using CAB.IECFramework.Entity;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											Entity calss for anamoly parameters for single Phase																						|
 * |											Author : Maruti Shrivastava						|
 * |											Date   : 08-dec-2015										|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


namespace CAB.Entity
{
   public class AnomalyEntityForSP :EntityBase
    {
        private long anomalyId;
        private long meterDataId;
        private int flash;
        private int eepRam;
        private int smps;
        private int rtc;
        private int mainBattery;
        private int rtcBattery;

        public long AnomalyId
        {
            get
            {
                return anomalyId;
            }
            set
            {
                anomalyId = value;
            }
        }

        public long MeterDataId
        {
            get
            {
                return meterDataId;
            }
            set
            {
                meterDataId = value;
            }
        }
        public int Flash
        {
            get
            {
                return flash;
            }
            set
            {
                flash = value;
            }
        }
        public int EeProm
        {
            get
            {
                return eepRam;
            }
            set
            {
                eepRam = value;
            }
        }
        public int Smps
        {
            get
            {
                return smps;
            }
            set
            {
                smps = value;
            }
        }
        public int Rtc
        {
            get
            {
                return rtc;
            }
            set
            {
                rtc = value;
            }
        }

        public int RTCBattery
        {
            get
            {
                return rtcBattery;
            }
            set
            {
                rtcBattery = value;
            }
        }

        public int MainBattery
        {
            get
            {
                return mainBattery;
            }
            set
            {
                mainBattery = value;
            }
        }
    }
}
