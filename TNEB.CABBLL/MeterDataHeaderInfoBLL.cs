using System;
using System.Collections.Generic;
using System.Text;
using CAB.IECFramework;
using CAB.DALC.Data;
using CAB.IECFramework.Entity;
using CAB.Entity;
using System.Data.Common;
using System.Data;
using CAB.IECFramework.Utility;

namespace CAB.BLL
{
    public class MeterDataHeaderInfoBLL : IBLL  
    {
        MeterDataHeaderInfoDAL meterDataHeaderInfoDAL;

        public MeterDataHeaderInfoBLL()
        {
            meterDataHeaderInfoDAL = new MeterDataHeaderInfoDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return meterDataHeaderInfoDAL.InsertData(entity);
        }

        public bool DeleteData(string meterDataId)
        {
            return meterDataHeaderInfoDAL.DeleteData(meterDataId);
        }

        public DataSet ListDataSet()
        {
            return meterDataHeaderInfoDAL.ListDataSet();
        }

        public DataSet GetMeterDataHeaderInfo(long activeMeterData_ID)
        {
            return meterDataHeaderInfoDAL.GetMeterdataHeaderInfo(activeMeterData_ID);
        }

        public DataSet GetMeterdataHeaderInfo(string meterID)
        {
            return meterDataHeaderInfoDAL.GetMeterdataHeaderInfo(meterID);
        }

        public DataSet GetMeterDataHeaderInfoForAnalysisReport(long activeMeterData_ID)
        {
            DataSet dataForAnalysisReport = PrepareDataSet();
            DataSet dataSet = GetMeterDataHeaderInfo(activeMeterData_ID);
            if (dataSet != null&& dataSet.Tables.Count>0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    for (int columnCounter = 0; columnCounter < dataSet.Tables[0].Columns.Count; columnCounter++)
                    {
                        DataRow rowToAdd = dataForAnalysisReport.Tables[0].NewRow();
                        rowToAdd[0] = dataSet.Tables[0].Columns[columnCounter].ColumnName;
                        rowToAdd[1] = row[columnCounter];
                        dataForAnalysisReport.Tables[0].Rows.Add(rowToAdd);

                    }
                }
            }
            return dataForAnalysisReport;
        }
        private DataSet PrepareDataSet()
        {
            DataSet dataSet = new DataSet();
            DataTable table = new DataTable();
            DataColumn column = new DataColumn();
            column.ColumnName = "Description";
            table.Columns.Add(column);
            DataColumn valueColumn = new DataColumn();
            valueColumn.ColumnName = "Value";
            table.Columns.Add(valueColumn);
            dataSet.Tables.Add(table);
            return dataSet;

        }
    }
}
