using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class GSMReportingDAL
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMReportingDAL).ToString());
        
        public List<GSMReportEntity> GetReportData(DateTime fromDate, DateTime toDate, out decimal saTotal, out decimal faTotal, out string totalSuccess)
        {
            string meterIDCheck = string.Empty;
            string meterIDSwap = string.Empty;
            decimal successAttempt = 0;
            decimal failureAttempt = 0;
            decimal totalAttempts = 0;
            
            saTotal = 0;
            faTotal = 0;
            totalSuccess = string.Empty;

            List<GSMReportEntity> lstGsmReport = new List<GSMReportEntity>();
            GSMReportEntity gsmReportEntity;
            IDataHelper helper = DatabaseFactory.GetHelper();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");

            StringBuilder builder = new StringBuilder();
            builder.Append("select CM.Consumer_Number as 'Consumer ID'," +
                " C.Consumer_Name as 'Consumer Name'," +
                " CM.Meter_ID as 'Meter ID'," +
                " GT.Status as 'Status'," +
                " GT.creationDateTime as 'Last reading date'," +
                " M.Meter_Phone as 'Meter SIM No.' from consumer_master C" +
                " join consumermeter CM on C.Consumer_Number = CM.Consumer_Number" +
                " join meter_master M on CM.Meter_ID = M.Meter_ID" +
                " join gsm_task_logs GT on GT.Meter_ID = M.Meter_ID" +
                // Added function str_to_date to convert varchar to date.
                " where STR_TO_DATE(creationDateTime, '%d/%m/%Y %H:%i:%s') between STR_TO_DATE('" + fromDate + "', '%d/%m/%Y %H:%i:%s') and STR_TO_DATE('" + toDate + "'" +
                " ,'%d/%m/%Y %H:%i:%s')" +
                " order by GT.Meter_ID,GT.creationDateTime");

            DataRequest request = new DataRequest(builder.ToString());
            DataSet ds = new DataSet();
            ds = helper.FillDataSet(request, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count + 1; i++)
                {
                    //extracting meter ID
                    if (i < ds.Tables[0].Rows.Count)
                        meterIDCheck = ds.Tables[0].Rows[i]["Meter ID"].ToString();

                    //enter if it is different meter id or always enter if it is last row
                    if ((meterIDCheck != meterIDSwap && i != 0) || i == ds.Tables[0].Rows.Count)
                    {
                        try
                        {
                            //this is different meter id
                            gsmReportEntity = new GSMReportEntity();
                            gsmReportEntity.Consumer_ID = Convert.ToInt32(ds.Tables[0].Rows[i - 1]["Consumer ID"]);
                            gsmReportEntity.Consumer_Name = ds.Tables[0].Rows[i - 1]["Consumer Name"].ToString();
                            gsmReportEntity.Meter_ID = ds.Tables[0].Rows[i - 1]["Meter ID"].ToString();
                            gsmReportEntity.Sim_ID = ds.Tables[0].Rows[i - 1]["Meter SIM No."].ToString();
                            gsmReportEntity.Success_Rate = ((successAttempt / totalAttempts) * 100).FormatToTwoDigit();
                            gsmReportEntity.Success_Attempt = successAttempt.ToString();
                            gsmReportEntity.Failure_Attempt = failureAttempt.ToString();
                            gsmReportEntity.Reading_DateTime = (Convert.ToDateTime(ds.Tables[0].Rows[i - 1]["Last reading date"].ToString(), new System.Globalization.CultureInfo("en-GB"))).ToString();
                            lstGsmReport.Add(gsmReportEntity);
                        }
                        catch (DivideByZeroException ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "GetReportData(DateTime fromDate, DateTime toDate, out decimal saTotal, out decimal faTotal, out s", ex);
                        }

                        successAttempt = 0;
                        failureAttempt = 0;
                        totalAttempts = 0;
                    }

                    totalAttempts++;

                    if ((i < ds.Tables[0].Rows.Count))
                    {
                        if ((ds.Tables[0].Rows[i]["Status"].ToString() == "C"))
                        {
                            // if the attempt is successfull
                            successAttempt++;
                        }
                        else
                            failureAttempt++;
                    }
                    meterIDSwap = meterIDCheck;
                }

                if (lstGsmReport.Count > 0)
                {
                    for (int i = 0; i < lstGsmReport.Count; i++)
                    {
                        saTotal = saTotal + Convert.ToInt16(lstGsmReport[i].Success_Attempt);
                        faTotal = faTotal + Convert.ToInt16(lstGsmReport[i].Failure_Attempt);
                    }
                    totalSuccess = (((saTotal) / (saTotal + faTotal)) * 100).FormatToTwoDigit();
                }
            }
            return lstGsmReport;
        }
    }
}