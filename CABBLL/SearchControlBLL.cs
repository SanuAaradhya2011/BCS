using System;
using System.Data;
using CAB.DALC.Data;
using CAB.Framework;

namespace CAB.BLL
{
    public class SearchControlBLL : IBLL
    {
        private bool Flag = false;

        public DataSet GetFilterData(string filter, bool ShowAll)
        {
            Flag = ShowAll;
            return GetFilterData(filter);
        }

        public DataSet GetFilterData(string filter)
        {
            SearchControlDAL searchControlDAL = new SearchControlDAL();
            DataSet dataSet = searchControlDAL.ListDataSet(filter);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            if (Flag)
                AddNewRow(dataTable, "All", "-");
			else
				AddNewRow(dataTable, "     ", "-");
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    AddNewRow(dataTable, Convert.ToString(row[0]), Convert.ToString(row[1]));
                }
            }
            dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        private void AddNewRow(DataTable dataTable,string displayMember, string valueMember)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow[0] = displayMember;
            dataRow[1] = valueMember;
            dataTable.Rows.Add(dataRow);
        }
    }
}
