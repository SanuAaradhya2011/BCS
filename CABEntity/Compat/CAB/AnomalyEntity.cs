using System;
using LNG.Framework.Entity;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											Entity calss for anamoly parameters 																						|
 * |											Author : Vidya BHooshan Mishra       									|
 * |											Date   : 18-dec-2012											|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


namespace LNG.Entity
{
   public class AnomalyEntity :EntityBase
    {
        private long anomalyId;
        private long meterDataId;
        private int flash;        
        private int eepRam;       
        private int smps;
        private int rtc;
        private int mainBattery;
        private int rtcBattery;
        private int error5;
        private int MBatterryVal;


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

        public int Error
        {
            get
            {
                return error5;
            }
            set
            {
                error5 = value;
            }
        }
        public int MainBatteryValue
        {
            get
            {
                return MBatterryVal;
            }
            set
            {
                MBatterryVal = value;
            }
        }
       
    }
}

