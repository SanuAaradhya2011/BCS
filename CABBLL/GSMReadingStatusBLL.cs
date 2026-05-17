using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;

namespace CAB.BLL
{
    public class GSMReadingStatusBLL : IBLL 
    {
        private GSMReadingStatusDAL gSMReadingStatusDAL;
        public GSMReadingStatusBLL()
        {
           gSMReadingStatusDAL = new GSMReadingStatusDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return gSMReadingStatusDAL.InsertData(entity);
        }
        public bool DeleteData(IEntity entity)
        {
            return gSMReadingStatusDAL.DeleteData(entity);
        }
        public IEntity GetDetailData(int id)
        {
            return gSMReadingStatusDAL.GetDetailData(id);
        }
        public bool IsRead(string meterId, string period)
        {
            long currDate = DateUtility.DateTimeToLong(System.DateTime.Now);
            long sDate = DateUtility.ConvertSearchDateTimeToLong(currDate, "000000");
            long eDate = DateUtility.ConvertSearchDateTimeToLong(currDate, "235959");
            return gSMReadingStatusDAL.IsRead(meterId, period, sDate, eDate);
        }
        public DataSet ListDataSet()
        {
            return gSMReadingStatusDAL.ListDataSet();
        }
        public DataSet ConvertData(DataSet dataSet)
        {
            return gSMReadingStatusDAL.ConvertData(dataSet);
        }
        public DataSet GetDateWiseReadingStatus(long fromDate, long toDate)
        {
            return ConvertData(gSMReadingStatusDAL.ListDataSet(fromDate, toDate));
        } 
    }
}
