using CAB.Framework.Entity;

namespace CAB.Entity
{
     public class GSMReadingStatusEntity : EntityBase
    {
        private long gSMReadingStatus_ID;
        public long GSMReadingStatus_ID
        {
            get { return gSMReadingStatus_ID; }
            set { gSMReadingStatus_ID = value;}
        }

        private long readingDateTime;
        public long ReadingDateTime
        {
            get { return readingDateTime; }
            set { readingDateTime = value; }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get { return statusMessage; }
            set { statusMessage = value; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private long gsmSchedule_ID;
        public long GSMScheduleID
        {
            get { return gsmSchedule_ID; }
            set { gsmSchedule_ID = value; }
        }

        private long gsmGroupSchedule_ID;
        public long GSMGroupScheduleID
        {
            get { return gsmGroupSchedule_ID; }
            set { gsmGroupSchedule_ID = value; }
        }

        private string meter_ID;
        public string MeterID
        {
            get { return meter_ID; }
            set { meter_ID = value; }
        }
          private string schedulePeriod;
        public string SchedulePeriod
        {
            get { return schedulePeriod; }
            set { schedulePeriod = value; }
        } 
    }
}
