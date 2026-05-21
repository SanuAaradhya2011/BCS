using System;
using System.Data;
using CAB.DALC.Data;
using CAB.Framework;

namespace CAB.BLL
{
    public class GPRSReportBLL : IBLL
    {
        private GPRSReportDAL gprsDAL;

        public GPRSReportBLL()
        {
            gprsDAL = new GPRSReportDAL();
        }

        public DataSet GetGRPSSearchResult(string searchType, DateTime fromDate, DateTime toDate)
        {
            return gprsDAL.GetGRPSSearchResult(searchType, fromDate, toDate);
        }

        public DataSet GetGRPSTaskLog(DateTime fromDate, DateTime toDate)
        {
            return gprsDAL.GetGRPSTaskLog(fromDate, toDate);
        }


    }
}
