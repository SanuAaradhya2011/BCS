using CAB.IECFramework.Entity;

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
    }
}
