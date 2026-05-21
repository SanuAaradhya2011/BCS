/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using CAB.IECFramework.Utility;


namespace CAB.BLL
{
    public class ApplicationLogBLL : IBLL
    {
        ApplicationLogDAL applicationLogDAL = new ApplicationLogDAL();

		public long InsertData()
        {
           return applicationLogDAL.GenerateAndSaveLog();
        }
        public void UpdateData()
        {
            if(ConfigInfo.LogID>0)
            applicationLogDAL.UpdateData(ConfigInfo.LogID);
        }
		public DataSet GetAllLogActivity()
		{
            return ConvertData(applicationLogDAL.ListDataSet());
		}

		public DataSet GetDateWiseLogActivity(long fromDate, long toDate)
		{
            return ConvertData(applicationLogDAL.ListDataSet(fromDate, toDate));
		}
 
		private DataSet ConvertData(DataSet dataSet)
		{ 
			 
			if (dataSet.Tables.Count < 1)
				return new DataSet();
			DataTable table = new DataTable();
			table.Columns.Add(new DataColumn("SL. Number", typeof(System.Int32)));
			table.Columns.Add(new DataColumn("Login ID", typeof(System.String)));
			table.Columns.Add(new DataColumn("Login DateTime", typeof(System.String)));
            table.Columns.Add(new DataColumn("Logout DateTime", typeof(System.String)));
			table.Columns.Add(new DataColumn("Duration (DD:HH:MM:SS)", typeof(System.String))); 
			int slno = 1;
			DataRow newRow;
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				newRow = table.NewRow();
				newRow["SL. Number"] = slno;
                newRow["Login ID"] = row["User Name"];
                long startDateTime = long.Parse(row["StartDateTime"].ToString());
                string endTimes = row["EndDateTime"].ToString();
                long endDateTime = 0;
                DateTime fromDateTime = DateUtility.LongToDateTime(startDateTime);
                DateTime toDateTime = System.DateTime.Now;
                if (!String.IsNullOrEmpty(endTimes))
                {
                    endDateTime = long.Parse(endTimes);
                    toDateTime = DateUtility.LongToDateTime(endDateTime);
                }
                 string differences = "----";
                endTimes = "----";
                if (endDateTime > 0)
                {
                    endTimes = DateUtility.LongToDateTime(endDateTime).ToString();
                    TimeSpan timeSpan = toDateTime - fromDateTime;
                    string days = timeSpan.Days.ToString();
                    string hour = timeSpan.Hours.ToString();
                    string minute = timeSpan.Minutes.ToString();
                    string second = timeSpan.Seconds.ToString();
                    if (Convert.ToInt32(hour) <= 0)
                        hour = "00";
                    if (Convert.ToInt32(minute) <= 0)
                        minute = "00";
                    if (Convert.ToInt32(days) <= 0)
                        days = "00";
                    if (hour.Length == 1)
                        hour = "0" + hour;
                    if (minute.Length == 1)
                        minute = "0" + minute;
                    if (second.Length == 1)
                        second = "0" + second;
                    if (days.Length == 1)
                        days = "0" + days;
                      differences = string.Concat(days, ":", hour, ":", minute,":",second);
                      if(differences.IndexOf("-")>0)
                          differences = "----";
                }
                newRow["Login DateTime"] = DateUtility.LongToDateTime(startDateTime).ToString();
                newRow["Logout DateTime"] = endTimes;
                newRow["Duration (DD:HH:MM:SS)"] = differences;
				table.Rows.Add(newRow);
				slno++;
			}
			dataSet.Tables.Remove("table");
			dataSet.Tables.Add(table);
			return dataSet;
		} 
 
    }
}

