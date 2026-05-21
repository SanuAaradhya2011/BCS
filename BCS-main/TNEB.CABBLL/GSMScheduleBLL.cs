/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh										|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System;
using CAB.IECFramework.Utility;

namespace CAB.BLL
{
    public class GSMScheduleBLL : IBLL
    {
        GSMScheduleDAL gsmScheduleDAL;

         public GSMScheduleBLL()
        {
            gsmScheduleDAL = new GSMScheduleDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return gsmScheduleDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return gsmScheduleDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return gsmScheduleDAL.DeleteData(entity);
        }
     
        public IEntity GetDetailData(int id)
        {
            return gsmScheduleDAL.GetDetailData(id);
        }
        
        public DataSet ListDataSet()
        {
            return gsmScheduleDAL.ListDataSet(); 
        }
        public DataSet ComboListDataSet()
        {
            return gsmScheduleDAL.ComboListDataSet(); 
        }
        public DataSet GetSearchData(string columnName, string value)
        {
            return gsmScheduleDAL.ListDataSet(columnName, value); 
        }
        public DataSet GetSearchData(long fromDate, long toDate)
        {
            return gsmScheduleDAL.ListDataSet(fromDate, toDate);
        }
        public DataSet GetSearchData(string columnName, int value)
        {
            return gsmScheduleDAL.ListDataSet(columnName, value);
        }
        public DataSet ListScheduleDataSet()
        {
            return gsmScheduleDAL.ListScheduleDataSet();
        }
        public long ValidateData(long scheduleID,string consumerNumber, string meterNumber, string meterSimNumber)
        {
            return gsmScheduleDAL.ValidateData(scheduleID,consumerNumber, meterNumber, meterSimNumber); 
        }
        public bool IsPortFree(long scheduleID, long datetimes, string portName)
        {
            DateTime dateTime = DateUtility.LongToDateTime(datetimes);
            long startDateTime = DateUtility.DateTimeToLong(dateTime);
            dateTime = dateTime.AddMinutes(-10);
            long endDateTime = DateUtility.DateTimeToLong(dateTime);
            return gsmScheduleDAL.IsPortFree(scheduleID, endDateTime,startDateTime , portName);
        }
        public DataSet GetCustomerMeterInformationList()
        {
            return gsmScheduleDAL.GetCustomerMeterInformationList(); 
        }
        public DataSet ConvertData(DataSet dataSet)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add("gsmSchedule_ID");
            table.Columns.Add("Name"); 
            table.Columns.Add("Creation Date");
            table.Columns.Add("Period");
            table.Columns.Add("Day Name");
            table.Columns.Add("Day Number");
            table.Columns.Add("Start Time (HH:MM)");
            table.Columns.Add("Parameter's"); 
            table.Columns.Add("Status");
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                DataRow row = table.NewRow();
                row[0] = dr[0];
                row[1] = dr[1];
                row[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[5])).Substring(0, 10);

                if (Convert.ToString(dr[2]) == "D")
                    row[3] = "Daily";
                else if (Convert.ToString(dr[2]) == "M")
                    row[3] = "Monthly";
                else if (Convert.ToString(dr[2]) == "W")
                    row[3] = "Weekly";
                string val = Convert.ToString(dr[3]);
                if (string.IsNullOrEmpty(val))
                    val = "------";
                row[4] = val;
                if (Convert.ToString(dr[4]) == "0")
                    row[5] = "-----";
                else
                    row[5] = ColText(Convert.ToInt32(dr[4]));
                row[6] = dr[6];
                  val = Convert.ToString(dr[7]);
                if (!string.IsNullOrEmpty(val))
                    val = val.Replace("|", ",");
                else
                    val = "----";
                row[7] = val;
                if (Convert.ToInt16(dr[8]) == 1)
                    row[8] = "Active";
                else
                    row[8] = "Inactive";
                table.Rows.Add(row);
            }
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        private string ColText(int num)
        {
            if (num == 1)
                return "1st Day";
            else if (num == 2)
                return "2nd Day";
            else if (num == 3)
                return "3rd Day";
            else
                return string.Concat(num.ToString(), "th Day");
        }
        public bool ValidateSchedule(IEntity entity)
        {
            return gsmScheduleDAL.ValidateSchedule(entity);
        }

        public DataSet GroupScheduleDataList()
        {
            return gsmScheduleDAL.GroupScheduleDataList();
        }
    }
}
