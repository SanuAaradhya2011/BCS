using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class GSMReportEntity : EntityBase
    {
        private int consumer_ID;
        private string consumer_Name;
        private string meter_ID;
        private string sim_ID;
        private string success_Rate;
        //private string failure_Rate;
        private string reading_DateTime;
        private string success_Attempt;
        private string failure_Attempt;
        
        public int Consumer_ID
        {
            get { return consumer_ID; }
            set { consumer_ID = value; }
        }

        public string Consumer_Name
        {
            get { return consumer_Name; }
            set { consumer_Name = value; }
        }

        public string Meter_ID
        {
            get { return meter_ID; }
            set { meter_ID = value; }
        }

        public string Sim_ID
        {
            get { return sim_ID; }
            set { sim_ID = value; }
        }

        public string Success_Rate
        {
            get { return success_Rate; }
            set { success_Rate = value; }
        }

        //public string Failure_Rate
        //{
        //    get { return failure_Rate; }
        //    set { failure_Rate = value; }
        //}

        public string Reading_DateTime
        {
            get { return reading_DateTime; }
            set { reading_DateTime = value; }
        }

        public string Failure_Attempt
        {
            get { return failure_Attempt; }
            set { failure_Attempt = value; }
        }

        public string Success_Attempt
        {
            get { return success_Attempt; }
            set { success_Attempt = value; }
        }
    }
}
