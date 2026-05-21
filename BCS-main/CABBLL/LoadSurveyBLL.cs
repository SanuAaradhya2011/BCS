using System;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.DALC.Data;
using System.Collections.Generic;
using System.Collections;
using CAB.Framework.Utility;
using CAB.Entity;

namespace CAB.BLL
{
    public class LoadSurveyBLL : IBLL
    {

        private LoadSurveyDAL loadSurveyDAL;

        public LoadSurveyBLL()
        {
            loadSurveyDAL = new LoadSurveyDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return loadSurveyDAL.DeleteData(meterDataId);
        }
        
        public IEntity InsertData(IEntity entity)
        {
            return loadSurveyDAL.InsertData(entity);
        }
        public IEntity InsertData(List<IEntity> entities)
        {
            return loadSurveyDAL.InsertData(entities);
        }
        public List<string> GetColumnsListAvailable(long meterDataID)
        {
            return loadSurveyDAL.GetColumnListAvailable(meterDataID);
        }
        public long GetFromDate(long meterDataID)
        {
            return loadSurveyDAL.GetFromDate(meterDataID);
        }
        public long GetToDate(long meterDataID)
        {
            return loadSurveyDAL.GetToDate(meterDataID);
        }

		public DataSet LoadPaddingData(string meterDataID, GraphView chartType) 
        {
            DataSet dataSet = new DataSet();
            dataSet = loadSurveyDAL.ListDataSet(meterDataID); 
			if ((dataSet == null) || (dataSet.Tables.Count == 0) || (dataSet.Tables[0].Rows.Count == 0))
			{
				return null;
			}
            DateTime firstDate, lastDate; 
			string nextDate = "";
            int intervalPeriod = Convert.ToInt16(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"].ToString()); 
            firstDate = DateUtility.LongToDateTime(Convert.ToInt64(dataSet.Tables[0].Rows[0]["loadSurveyDateTime"].ToString()));
			DateTime CheckFirstDate = new DateTime(firstDate.Year, firstDate.Month, 1, 00, 15, 00);
			if (CheckFirstDate != firstDate)
			{
				nextDate = DateUtility.DateTimeToLong(CheckFirstDate).ToString();
				if (chartType == GraphView.Monthly)
				AddRows(dataSet, nextDate, intervalPeriod);
			}
            lastDate = DateUtility.LongToDateTime(Convert.ToInt64(dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count - 1]["loadSurveyDateTime"].ToString()));
			int getEndDay = GetEndDay(lastDate);
			DateTime CheckLastDate = new DateTime(lastDate.Year, lastDate.Month, getEndDay, 00, 00, 00);
			if (CheckLastDate != lastDate)
			{
				nextDate = DateUtility.DateTimeToLong(CheckLastDate).ToString();
				if (chartType == GraphView.Monthly)
				AddRows(dataSet, nextDate, intervalPeriod);
			} 
			firstDate = DateUtility.LongToDateTime(Convert.ToInt64(dataSet.Tables[0].Rows[0]["loadSurveyDateTime"].ToString()));
			lastDate = DateUtility.LongToDateTime(Convert.ToInt64(dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count - 1]["loadSurveyDateTime"].ToString()));

			while (firstDate < lastDate)
			{
				nextDate = DateUtility.DateTimeToLong(firstDate.AddMinutes(intervalPeriod)).ToString(); 
				DataRow[] foundRow = dataSet.Tables[0].Select("loadSurveyDateTime = " + nextDate);
				DateTime currentDate=DateUtility.LongToDateTime(Convert.ToInt64(nextDate));
				if (foundRow.Length == 0)
				{
					if (firstDate != currentDate)
						AddRows(dataSet, nextDate, intervalPeriod);
				}
				firstDate = DateUtility.LongToDateTime(Convert.ToInt64(nextDate));
			}
			dataSet.AcceptChanges();
			DataSet dsTemp = dataSet.Clone();
			DataRow[] rowArray = dataSet.Tables[0].Select("", "loadsurveydatetime asc");
			foreach (DataRow row in rowArray) 
				dsTemp.Tables[0].ImportRow(row); 
			return dsTemp;
        }
		public int GetEndDay(DateTime lastDate)
		{
			int month = lastDate.Month; 
			int year=lastDate.Year/4;
			int year1=lastDate.Year%4;
			switch (month)
			{
				case 1:
					return 31;
				case 2:
					if (year1 == year)
						return 29;
					else
						return 28;
				case 3:
					return 31;
				case 4:
					return 30;
				case 5:
					return 31;
				case 6:
					return 30;
				case 7:
					return 31;
				case 8:
					return 31;
				case 9:
					return 30;
				case 10:
					return 31;
				case 11:
					return 30;
				case 12:
					return 31;
			}
			return 0;
		}
		private DataSet AddRows(DataSet dataSet, string nextDate, int intervalPeriod)
		{
			int defaultValue = -1;
			DataRow drow; 
			drow = dataSet.Tables[0].NewRow();
			drow["RPhaseVoltage"] = defaultValue;
			drow["YPhaseVoltage"] = defaultValue;
			drow["BPhaseVoltage"] = defaultValue;
			drow["RPhaseCurrent"] = defaultValue;
			drow["YPhaseCurrent"] = defaultValue;
			drow["BPhaseCurrent"] = defaultValue;
			drow["AvgVoltage"] = defaultValue;
			drow["AvgCurrent"] = defaultValue;
			drow["DemandKVARLead"] = defaultValue;
			drow["DemandKVA"] = defaultValue;
			drow["DemandKW"] = defaultValue;
			drow["DemandKVARLag"] = defaultValue;
			drow["PowerFactor"] = defaultValue - 1;
			drow["LoadSurveyDateTime"] = nextDate;
			drow["MDIntervalPeriod"] = intervalPeriod;
			dataSet.Tables[0].Rows.Add(drow);
			return dataSet;
		}
        public DataSet SelectColumnsFromPaddedData(DataSet dataSet, ArrayList parameterList)
        {

            DataSet finalDset = new DataSet();
            DataTable dTable = new DataTable();
            DataColumn dColumn;
            DataRow dr;
            for (int count = 0; count < parameterList.Count; count++)
            {
				if (parameterList[count] != null)
				{
					dColumn = new DataColumn(parameterList[count].ToString());
					dColumn.DataType = System.Type.GetType("System.Double");
					dTable.Columns.Add(dColumn);
				}
            }
            dColumn = new DataColumn("loadSurveyDateTime");
            dColumn.DataType = System.Type.GetType("System.DateTime");
            dTable.Columns.Add(dColumn);
			DateTime dateTimes=System.DateTime.Now; 
            foreach (DataRow drow in dataSet.Tables[0].Rows)
            { 
                dr = dTable.NewRow();
				if (DateUtility.LongToDateTime(Convert.ToInt64(drow["loadSurveyDateTime"].ToString())) == dateTimes)
					continue;
                for (int colCount = 0; colCount < parameterList.Count; colCount++)
                {
					if (parameterList[colCount] != null)
					{
                        string val = Convert.ToString(parameterList[colCount]).Replace(" ", "");
                        dr[Convert.ToString(parameterList[colCount])] = drow[val];
					}
                }
				dateTimes=DateUtility.LongToDateTime(Convert.ToInt64(drow["loadSurveyDateTime"].ToString()));
				dr["loadSurveyDateTime"] = dateTimes;
                dTable.Rows.Add(dr);
            }
            finalDset.Tables.Add(dTable);
            return finalDset;
        }

        public DataSet ListDataSet(ArrayList parameterList, string fileName, string meterID, string graphType)
        {
            DataSet dataSet = new DataSet();
            dataSet = loadSurveyDAL.ListDataSet(parameterList, fileName, meterID);
            if (graphType == "Monthly")
            {
                if (dataSet.Tables.Count < 1)
                    return new DataSet();
                int counter = 0;
                DataTable table = new DataTable();
                for (counter = 0; counter < parameterList.Count; counter++)
                    table.Columns.Add(new DataColumn(parameterList[counter].ToString()));
                table.Columns.Add(new DataColumn("MonthSequence", typeof(System.Int32)));
                DataRow newRow;
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    newRow = table.NewRow();
                    for (counter = 0; counter < parameterList.Count; counter++)
                        newRow[parameterList[counter].ToString()] = row[parameterList[counter].ToString()];

                    DateTime dt = DateUtility.LongToDateTime(Int64.Parse(row["loadsurveydatetime"].ToString()));
                    newRow["MonthSequence"] = dt.Month;
                    table.Rows.Add(newRow);
                }
                dataSet.Tables.Remove("table");
                dataSet.Tables.Add(table);
            }
            return dataSet;
        }

        //public DataSet PaddedDataSet(long meterDataID, List<string> validColumns)
        //{
        //    DataSet ds;
        //    DateTime previousDate, nextDate;

        //    DataRow padRow, row;
        //    TimeSpan tsDay = new TimeSpan(1, 0, 0, 0);
        //    bool flag = false;

        //    int intervalPeriod = 0;
        //    int defaultValue = -1;

        //    ds = new DataSet();
        //    ds = loadSurveyDAL.ListDataSet(Convert.ToInt64(meterDataID), validColumns);
        //    intervalPeriod = Convert.ToInt32(ds.Tables[0].Rows[0]["MDIntervalPeriod"].ToString());
        //    previousDate = DateTime.Now;

        //    for (int rowCount = 0; rowCount < ds.Tables[0].Rows.Count; rowCount++)
        //    {
        //        row = ds.Tables[0].Rows[rowCount];
        //        if (flag == false)
        //        {
        //            previousDate = DateUtility.LongToDateTime(Convert.ToInt64(row["loadSurveyDateTime"].ToString()));
        //            flag = true;
        //            continue;
        //        }

        //        nextDate = DateUtility.LongToDateTime(Convert.ToInt64(row["loadSurveyDateTime"]));
        //        //20100506000000

        //        while (previousDate.AddMinutes(intervalPeriod) < nextDate)
        //        {

        //            padRow = ds.Tables[0].NewRow();
        //            previousDate = previousDate.AddMinutes(intervalPeriod);

        //            padRow["MeterID"] = ds.Tables[0].Rows[0]["MeterID"];

        //            if (ds.Tables[0].Columns.Contains("RPhaseVoltage"))
        //                padRow["RPhaseVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("YPhaseVoltage"))
        //                padRow["YPhaseVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("BPhaseVoltage"))
        //                padRow["BPhaseVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("RPhaseCurrent"))
        //                padRow["RPhaseCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("YPhaseCurrent"))
        //                padRow["YPhaseCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("BPhaseCurrent"))
        //                padRow["BPhaseCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("AvgVoltage"))
        //                padRow["AvgVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("AvgCurrent"))
        //                padRow["AvgCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKVARLead"))
        //                padRow["DemandKVARLead"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKVA"))
        //                padRow["DemandKVA"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKW"))
        //                padRow["DemandKW"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKVARLag"))
        //                padRow["DemandKVARLag"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("PowerFactor"))
        //                padRow["PowerFactor"] = defaultValue - 1;

        //            if (previousDate.ToString("HH:mm").Contains("00:00"))
        //                padRow["LoadSurveyDateTime"] = DateUtility.DateTimeToLong(previousDate.Subtract(tsDay));
        //            else
        //                padRow["LoadSurveyDateTime"] = DateUtility.DateTimeToLong(previousDate);

        //            padRow["MDIntervalPeriod"] = intervalPeriod;
        //            padRow["MeterData_ID"] = row["MeterData_ID"];
        //            ds.Tables[0].Rows.Add(padRow);
        //        }
        //        previousDate = nextDate;
        //        if (nextDate.ToString("HH:mm").Contains("00:00"))
        //            row["LoadSurveyDateTime"] = DateUtility.DateTimeToLong(nextDate.Subtract(tsDay));

        //    }

        //    ds.AcceptChanges();
        //    DataSet dsTemp = ds.Clone();
        //    DataRow[] rowArray = ds.Tables[0].Select("", "loadsurveydatetime asc");
        //    foreach (DataRow tempRow in rowArray)
        //    {
        //        dsTemp.Tables[0].ImportRow(tempRow);
        //    }
        //    return dsTemp;
        //}

        //public DataSet PaddedDataSet(long meterDataID, List<string> validColumns)
        //{
        //    DataSet ds = new DataSet();
        //    ds = loadSurveyDAL.ListDataSet(Convert.ToInt64(meterDataID), validColumns);
        //    DateTime firstDate, lastDate;

        //    DataRow drow;
        //    int defaultValue = -1;
        //    int intervalPeriod = Convert.ToInt32(ds.Tables[0].Rows[0]["MDIntervalPeriod"].ToString());
        //    firstDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow["loadSurveyDateTime"].ToString()));
        //    lastDate = DateUtility.LongToDateTime(Convert.ToInt64(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["loadSurveyDateTime"].ToString()));
        //    TimeSpan tsDay = new TimeSpan(1, 0, 0, 0);
        //    int counter = 0;
        //    while (firstDate < lastDate)
        //    {
        //        DateTime nextDate = firstDate.AddMinutes(intervalPeriod);
        //        DataRow[] foundRow = ds.Tables[0].Select("loadSurveyDateTime = " + DateUtility.DateTimeToLong(nextDate));
        //        if (foundRow.Length == 0)
        //        {

        //            drow = ds.Tables[0].NewRow();

        //            drow["MeterID"] = ds.Tables[0].Rows[0]["MeterID"];

        //            if (ds.Tables[0].Columns.Contains("RPhaseVoltage"))
        //                drow["RPhaseVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("YPhaseVoltage"))
        //                drow["YPhaseVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("BPhaseVoltage"))
        //                drow["BPhaseVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("RPhaseCurrent"))
        //                drow["RPhaseCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("YPhaseCurrent"))
        //                drow["YPhaseCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("BPhaseCurrent"))
        //                drow["BPhaseCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("AvgVoltage"))
        //                drow["AvgVoltage"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("AvgCurrent"))
        //                drow["AvgCurrent"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKVARLead"))
        //                drow["DemandKVARLead"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKVA"))
        //                drow["DemandKVA"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKW"))
        //                drow["DemandKW"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("DemandKVARLag"))
        //                drow["DemandKVARLag"] = defaultValue;

        //            if (ds.Tables[0].Columns.Contains("PowerFactor"))
        //                drow["PowerFactor"] = defaultValue - 1;

        //            if (nextDate.ToString("HH:mm").Contains("00:00"))
        //                drow["LoadSurveyDateTime"] = DateUtility.DateTimeToLong(nextDate.Subtract(tsDay));
        //            else
        //                drow["LoadSurveyDateTime"] = DateUtility.DateTimeToLong(nextDate);

        //            drow["MDIntervalPeriod"] = intervalPeriod;
        //            drow["MeterData_ID"] = ds.Tables[0].Rows[0]["MeterData_ID"]; ;

        //            ds.Tables[0].Rows.Add(drow);

        //            System.Diagnostics.Debug.WriteLine(counter.ToString());
        //            counter++;
        //        }
        //        else
        //        {
        //            if (nextDate.ToString("HH:mm").Contains("00:00"))
        //                ds.Tables[0].Select("loadSurveyDateTime = " + DateUtility.DateTimeToLong(nextDate));
        //                drow["LoadSurveyDateTime"] = DateUtility.DateTimeToLong(nextDate.Subtract(tsDay));
        //            else
        //                drow["LoadSurveyDateTime"] = DateUtility.DateTimeToLong(nextDate);
        //        }
        //        firstDate = nextDate;
        //    }
        //    ds.AcceptChanges();
        //    DataSet  dsTemp = ds.Clone();
        //    DataRow[] rowArray = ds.Tables[0].Select("", "loadsurveydatetime asc");
        //    foreach (DataRow row in rowArray)
        //    {
        //        dsTemp.Tables[0].ImportRow(row);
        //    }
        //    return dsTemp;
        //}

        public ArrayList GetColumnsListAvailable(string parameterName, string meterDataID)//string fileName, string meterID)
        {
            return loadSurveyDAL.GetColumnsListAvailable(parameterName, meterDataID);//fileName, meterID);
        }

        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate,string type)
        {
             LoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterBLL().GetColumn(meterDataId) as LoadSurveyParameterEntity;
            if (loadSurveyParameterEntity == null)
                return null;
            return CommonBLL.ConvertLoadSurvey(loadSurveyDAL.ListDataSet(meterDataId, loadSurveyParameterEntity.ColumnsNames, fromDate, toDate), type);
        }

        public Dictionary<string, string> CreateLoadSurveyDictionary()
        {
            Dictionary<string, string> loadSurveyColumns = new Dictionary<string, string>();
            loadSurveyColumns.Add("Voltage R Phase", "RPhaseVoltage");
            loadSurveyColumns.Add("Voltage Y Phase", "YPhaseVoltage");
            loadSurveyColumns.Add("Voltage B Phase", "BPhaseVoltage");
            loadSurveyColumns.Add("Current R Phase", "RPhaseCurrent");
            loadSurveyColumns.Add("Current Y Phase", "YPhaseCurrent");
            loadSurveyColumns.Add("Current B Phase", "BPhaseCurrent");
            loadSurveyColumns.Add("Average Voltage", "AvgVoltage");
            loadSurveyColumns.Add("Average Current", "AvgCurrent");
            loadSurveyColumns.Add("Demand kVAr Lead", "DemandKVARLead");
            loadSurveyColumns.Add("Demand kVA", "DemandKVA");
            loadSurveyColumns.Add("Demand  kW", "DemandKW");
            loadSurveyColumns.Add("Demand kVAr Lag", "DemandKVARLag");
            loadSurveyColumns.Add("Power Factor", "PowerFactor");
            loadSurveyColumns.Add("DateTime", "LoadSurveyDateTime");
            loadSurveyColumns.Add("Interval Period", "MDIntervalPeriod");
            return loadSurveyColumns;
        }

        public DataSet GetLoadSurveyDataByParameter(string value, List<string> columnList, string reportType)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            //if (reportType == "CAB")
            //    ds = loadSurveyDAL.GetLoadSurveyDataByFile(id,value, columns);
            if (reportType == "Meter")
                ds = loadSurveyDAL.GetLoadSurveyDataByMeter(value, columns);
            return ds;
        }

        public DataSet GetLoadSurveyDataByFileName(string meterID, string fileName, List<string> columnList, string reportType)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            if (reportType == "CAB")
                ds = loadSurveyDAL.GetLoadSurveyDataByFile(meterID, fileName, columns);
            return ds;
        }

        private List<string> GetDatabaseColumns(List<string> columnList)
        {
            Dictionary<string, string> lsColumns = CreateLoadSurveyDictionary();
            List<string> columns = new List<string>();
            string tempStr = string.Empty;
            foreach (string key in columnList)
            {
                if (lsColumns.TryGetValue(key, out tempStr))
                    columns.Add(tempStr);
            }
            return columns;
        }
    }
}
