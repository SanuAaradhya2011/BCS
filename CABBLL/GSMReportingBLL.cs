using System.Data;
using CAB.DALC.Data;
using CAB.Framework.Entity;
using CAB.Entity;
using System.Collections.Generic;
using System;
namespace CAB.BLL
{
    public class GSMReportingBLL
    {
        GSMReportingDAL gsmRepotingDal;

        public GSMReportingBLL()
        {
            gsmRepotingDal = new GSMReportingDAL();
        }

        public List<GSMReportEntity> GetReportData(DateTime fromDate, DateTime toDate,out decimal saTotal,out decimal faTotal,out string totalSuccess)
        {
            return gsmRepotingDal.GetReportData(fromDate, toDate, out saTotal,out faTotal,out totalSuccess);
        }
    }
}
