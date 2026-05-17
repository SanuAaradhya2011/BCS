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
    public class UserLogActivityBLL : IBLL
    {
		UserLogActivityDAL userLogActivityDAL = new UserLogActivityDAL();

		public void InsertData(IEntity entity)
        {
			userLogActivityDAL.InsertData(entity);
        }

		public DataSet GetAllLogActivity()
		{
			return ConvertData(userLogActivityDAL.ListDataSet());
		}

		public DataSet GetDateWiseLogActivity(long fromDate, long toDate)
		{
			return ConvertData(userLogActivityDAL.ListDataSet(fromDate, toDate));
		}



		private DataSet ConvertData(DataSet dataSet)
		{ 
			 
			if (dataSet.Tables.Count < 1)
				return new DataSet();
			DataTable table = new DataTable();
			table.Columns.Add(new DataColumn("SL. Number", typeof(System.Int32)));
			table.Columns.Add(new DataColumn("User Name", typeof(System.String)));
			table.Columns.Add(new DataColumn("Activity DateTime", typeof(System.String)));
			table.Columns.Add(new DataColumn("Activities", typeof(System.String))); 
			int slno = 1;
			DataRow newRow;
			foreach (DataRow row in dataSet.Tables[0].Rows)
			{
				newRow = table.NewRow();
				newRow["SL. Number"] = slno;
				newRow["User Name"] = row["User Name"];
                newRow["Activity DateTime"] = DateUtility.LongToStringDateTimeFormat(long.Parse(row["Activity_DateTime"].ToString())).ToString();
				newRow["Activities"] = row["Activities"];
				table.Rows.Add(newRow);
				slno++;
			}
			dataSet.Tables.Remove("table");
			dataSet.Tables.Add(table);
			return dataSet;
		} 
 
    }
}
