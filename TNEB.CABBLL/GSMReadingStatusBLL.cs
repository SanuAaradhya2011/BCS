using System.Data;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;

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
        public DataSet GetSearchData(string columnName, int value)
        {
            return gSMReadingStatusDAL.ListDataSet(columnName, value);
        }
        public DataSet GetSearchData(string columnName, string value)
        {
            return gSMReadingStatusDAL.ListDataSet(columnName, value);
        }
    }
}
