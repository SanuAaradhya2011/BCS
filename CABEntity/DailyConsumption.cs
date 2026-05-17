using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.Entity
{
    public class DailyConsumption
    {
        private double kWh;

        public double KWh
        {
            get { return kWh; }
            set { kWh = value; }
        }

        private double kVAh;

        public double KVAh
        {
            get { return kVAh; }
            set { kVAh = value; }
        }

        private double kvarh_Lag;

        public double Kvarh_Lag
        {
            get { return kvarh_Lag; }
            set { kvarh_Lag = value; }
        }

        private double kvarh_Lead;

        public double Kvarh_Lead
        {
            get { return kvarh_Lead; }
            set { kvarh_Lead = value; }
        }
    }
}