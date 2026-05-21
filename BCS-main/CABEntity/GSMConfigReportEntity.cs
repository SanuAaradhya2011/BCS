using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class GSMConfigReportEntity : EntityBase
    {
        private int meter_ID;
        private string sim_No;

        private string status;
        private string reason;
        private string task;
        private string reading_DateTime;
       

        public int Meter_ID
        {
            get { return meter_ID; }
            set { meter_ID = value; }
        }

        public string Sim_No
        {
            get { return sim_No; }
            set { sim_No = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }
        public string TaskName
        {
            get { return task; }
            set { task = value; }
        }
        public string Reading_DateTime
        {
            get { return reading_DateTime; }
            set { reading_DateTime = value; }
        }

       
    }
}
