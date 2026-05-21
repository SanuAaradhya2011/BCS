using System;
using System.Collections.Generic;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.Framework.Utility;

namespace CAB.BLL
{
    public class ExceptionLogDetailsBLL : IBLL 
    {
        ExceptionLogDetailsDAL exceptionLogDetailsDAL = new ExceptionLogDetailsDAL();
        public void InsertData(IEntity entity)
        {
            exceptionLogDetailsDAL.InsertData(entity);
        }

        public DataSet GetAllLogActivity()
        {
            return ConvertData(exceptionLogDetailsDAL.ListDataSet());
        }

        public DataSet GetDateWiseLogActivity(long fromDate, long toDate)
        {
            return ConvertData(exceptionLogDetailsDAL.ListDataSet(fromDate, toDate));
        }

        private DataSet ConvertData(DataSet dataSet)
        {

            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL.", typeof(System.Int32)));
            //table.Columns.Add(new DataColumn("Exception ID", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Date", typeof(System.String)));
            table.Columns.Add(new DataColumn("Source", typeof(System.String)));
            table.Columns.Add(new DataColumn("Message", typeof(System.String)));
            //table.Columns.Add(new DataColumn("Machine ID", typeof(System.String)));
            //table.Columns.Add(new DataColumn("User ID", typeof(System.Int32)));
            //table.Columns.Add(new DataColumn("Exception", typeof(System.String)));

            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL."] = slno;
               // newRow["Exception ID"] = row["Exception ID"];
                newRow["Date"] = DateUtility.LongToDateTime(long.Parse(row["Date"].ToString())).ToString();
                newRow["Source"] = row["Source"];
                newRow["Message"] = row["Message"];
                //newRow["Machine ID"] = row["Machine ID"];
                //newRow["User ID"] = row["User ID"];
                //newRow["Exception"] = row["Exception"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        } 
    }
}
