using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.Entity;
using CAB.DALC.Data;
using System.Collections.Generic;
using CAB.Framework.Utility;
using System;

namespace CAB.BLL
{
    public class GSMGroupScheduleBLL : IBLL
    {
        private GSMGroupScheduleDAL gsmGroupScheduleDAL;
        public GSMGroupScheduleBLL()
        {
            gsmGroupScheduleDAL = new GSMGroupScheduleDAL();
        }
        public DataSet ListDataSet()
        {
            return gsmGroupScheduleDAL.ListDataSet();
        }
        public bool GetDetailData(string groupName)
        {
            return gsmGroupScheduleDAL.GetDetailData(groupName);
        }
        public IEntity GetDetailData(int id)
        {
            return gsmGroupScheduleDAL.GetDetailData(id);
        }
        public bool DeleteData(IEntity entity)
        {
            return gsmGroupScheduleDAL.DeleteData(entity);
        }
        public bool UpdateData(IEntity entity)
        {
            return gsmGroupScheduleDAL.UpdateData(entity);
        }
        public IEntity InsertData(IEntity entity)
        {
            return gsmGroupScheduleDAL.InsertData(entity);
        }
        public DataSet GetSearchData(string columnName, string value)
        {
            return gsmGroupScheduleDAL.ListDataSet(columnName, value);
        }
        public DataSet GetSearchData(long fromDate, long toDate)
        {
            return gsmGroupScheduleDAL.ListDataSet(fromDate, toDate);
        }
        public DataSet GetSearchData(string columnName, int value)
        {
            return gsmGroupScheduleDAL.ListDataSet(columnName, value);
        }

        public bool ValidateGroup(IEntity entity)
        {
            return gsmGroupScheduleDAL.ValidateGroup(entity);
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
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                if (col.ColumnName.Equals("Reading Start Date"))
                    table.Columns.Add("Group Creation Date");
                else
                    table.Columns.Add(col.ColumnName);
            }
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                DataRow row = table.NewRow();
                row[0] = dr[0];
                row[1] = dr[1];
                row[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[2])).Substring(0, 10);
                row[3] = dr[3];
                row[4] = dr[4];
                table.Rows.Add(row);
            }
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        
    }
}
