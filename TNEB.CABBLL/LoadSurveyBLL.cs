
using System;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using CAB.DALC.Data;
using System.Collections.Generic;
using System.Collections;
using CAB.IECFramework.Utility;
using CAB.Entity;
using System.Data.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using CABEntity;
namespace CAB.BLL
{
    public class LoadSurveyBLL : IBLL
    {

        private LoadSurveyDAL loadSurveyDAL;
        private int lengthOfArray = 0;
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
        private DataTable GetDaySpecificEnergyTable(long MeterDataId, string mode)
        {
            IECLoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterBLL().GetColumn(MeterDataId) as IECLoadSurveyParameterEntity;
            if (loadSurveyParameterEntity == null)
                return null;
            DataTable table = new DataTable();
            string[] columnName = loadSurveyParameterEntity.ColumnsNames.Split(',');
            table.Columns.Add(new DataColumn("Date", typeof(System.String)));

                 
            foreach (string colval in columnName)
            {
                if (colval.Trim().Equals("RPhaseVoltage as 'Voltage R Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Voltage R Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Voltage R Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals( "YPhaseVoltage as 'Voltage Y Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Voltage Y Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Voltage Y Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("BPhaseVoltage as 'Voltage B Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Voltage B Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Voltage B Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("RPhaseCurrent as 'Current R Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Current R Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Current R Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("YPhaseCurrent as 'Current Y Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Current Y Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Current Y Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("BPhaseCurrent as 'Current B Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Current B Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Current B Phase", typeof(System.String)));
                }  
                if (colval.Trim().Equals("AvgVoltage as 'Average Voltage'"))
                {
                    table.Columns.Add(new DataColumn("Max Average Voltage", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Average Voltage", typeof(System.String)));
                }
                if (colval.Trim().Equals("AvgCurrent as 'Average Current'"))
                {
                    table.Columns.Add(new DataColumn("Max Average Current", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Average Current", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKVARLead as 'Demand kvar (lead)'"))
                {
                    table.Columns.Add(new DataColumn("Max Energy kvarh(Lead)", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Energy kvarh(Lead)", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKVA as 'Demand kVA'"))
                {
                    table.Columns.Add(new DataColumn("Max Energy kVAh", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Energy kVAh", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKW as 'Demand kW'"))
                {
                    table.Columns.Add(new DataColumn("Max Energy kWh", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Energy kWh", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKVARLag as 'Demand kvar (lag)'"))
                {
                    table.Columns.Add(new DataColumn("Max Energy kvarh(Lag)", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Energy kvarh(Lag)", typeof(System.String)));
                }
                if (colval.Trim().Equals("PowerFactor"))
                {
                    table.Columns.Add(new DataColumn("Max Power Factor", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Power Factor", typeof(System.String)));
                }
            }
            return table;
        }
        private DataTable GetDaySpecificDemandTable(long MeterDataId, string mode)
        {
            IECLoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterBLL().GetColumn(MeterDataId) as IECLoadSurveyParameterEntity;
            if (loadSurveyParameterEntity == null)
                return null;
            DataTable table = new DataTable();
            string[] columnName = loadSurveyParameterEntity.ColumnsNames.Split(',');
            table.Columns.Add(new DataColumn("Date", typeof(System.String)));  
 
            foreach (string colval in columnName)
            {
                if (colval.Trim().Equals("RPhaseVoltage as 'Voltage R Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Voltage R Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Voltage R Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("YPhaseVoltage as 'Voltage Y Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Voltage Y Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Voltage Y Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("BPhaseVoltage as 'Voltage B Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Voltage B Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Voltage B Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("RPhaseCurrent as 'Current R Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Current R Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Current R Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("YPhaseCurrent as 'Current Y Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Current Y Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Current Y Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("BPhaseCurrent as 'Current B Phase'"))
                {
                    table.Columns.Add(new DataColumn("Max Current B Phase", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Current B Phase", typeof(System.String)));
                }
                if (colval.Trim().Equals("AvgVoltage as 'Average Voltage'"))
                {
                    table.Columns.Add(new DataColumn("Max Average Voltage", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Average Voltage", typeof(System.String)));
                }
                if (colval.Trim().Equals("AvgCurrent as 'Average Current'"))
                {
                    table.Columns.Add(new DataColumn("Max Average Current", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Average Current", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKVARLead as 'Demand kvar (lead)'"))
                {
                    table.Columns.Add(new DataColumn("Max Demand kvar(Lead)", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Demand kvar(Lead)", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKVA as 'Demand kVA'"))
                {
                    table.Columns.Add(new DataColumn("Max Demand kVA", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Demand kVA", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKW as 'Demand kW'"))
                {
                    table.Columns.Add(new DataColumn("Max Demand kW", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Demand kW", typeof(System.String)));
                }
                if (colval.Trim().Equals("DemandKVARLag as 'Demand kvar (lag)'"))
                {
                    table.Columns.Add(new DataColumn("Max Demand kvar(Lag)", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Demand kvar(Lag)", typeof(System.String)));
                }
                if (colval.Trim().Equals("PowerFactor"))
                {
                    table.Columns.Add(new DataColumn("Max Power Factor", typeof(System.String)));
                    table.Columns.Add(new DataColumn("Min Power Factor", typeof(System.String)));
                }
            }
            return table;
        }
        public DataTable ConvertDemandtoEnergy(DataTable tableForGrid, int energyIndex)
        {
            int columnIndex = 0;
            decimal itemValue;
            foreach (DataColumn column in tableForGrid.Columns)
            {
                
                if (column.ColumnName.Contains("Energy"))
                {
                    foreach (DataRow row in tableForGrid.Rows)
                    {
                       itemValue = Convert.ToDecimal(row[columnIndex]);
                       itemValue = itemValue / energyIndex;
                       row[columnIndex] = itemValue;
                    }
                }
                columnIndex++;
            }
            return tableForGrid;
        }
         public DataSet GetMaxMinDayLoadsurvey(long meterDataId,long fromDate,long toDate,string mode)
        {
       
            int energyIndex = 1;
            IECLoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterBLL().GetColumn(meterDataId) as IECLoadSurveyParameterEntity;
            if (loadSurveyParameterEntity == null)
                return null;
            DataSet lsData = loadSurveyDAL.ListDataSet(meterDataId, loadSurveyParameterEntity.ColumnsNames, fromDate, toDate);
            if (lsData == null)
                return null;
            if (lsData.Tables.Count == 0 || lsData.Tables[0].Rows.Count==0)
                return null;
            else
                if (lsData.Tables[0].Rows.Count <= 0)
                    return null;            
            string[] columnName = loadSurveyParameterEntity.ColumnsNames.Split(',');
            List<List<decimal>> colsList = new List<List<decimal>>();
          
            int intervalPeriod = Convert.ToInt32(lsData.Tables[0].Rows[0]["MDIntervalPeriod"]); 
             if(intervalPeriod==15)
             {
                 if (mode == "Energy")
                     energyIndex = 4;
                 lengthOfArray = 96;
             }
             else if (intervalPeriod == 30)
             {
                 lengthOfArray = 48;
                 energyIndex = 2;
             }
             else
             {
                 energyIndex = 1;
                 lengthOfArray = 24;
             }
             for (int p = 0; p < 13; p++)
                 colsList.Add(new List<decimal>());
            
             DataTable tableForGrid;
            if(mode=="Demand")
                tableForGrid = GetDaySpecificDemandTable(meterDataId, mode);
             else 
                  tableForGrid = GetDaySpecificEnergyTable(meterDataId, mode);
            long datevalue = 0; 
             int counter = 0;
             int rowIndex = 0;
             foreach (DataRow row in lsData.Tables[0].Rows)
             { 
             Refetch:
                 string dateString = Convert.ToString(row["loadsurveydatetime"]); 
                 long newDate = Convert.ToInt32(dateString.Substring(0, 8));
                 if (datevalue == 0)
                     datevalue = newDate;
                 if (datevalue == newDate)
                 {
                     int colCounter = 0;
                     foreach (DataColumn col in  lsData.Tables[0].Columns)
                     {
                         if (col.ColumnName.Equals("TamperStatus"))
                         {
                             colCounter++;
                             continue;
                         }
                         if (colCounter != 0)
                             colsList[colCounter - 1].Add(Convert.ToDecimal(row[colCounter]));
                         
                         colCounter++;
                     } 
                     counter++; 
                 }
                 else
                 { 
                     int totalRow = counter;
                
                     tableForGrid = AddRowToMaxMinTable(tableForGrid, colsList, datevalue);
                     datevalue = 0;
                     counter = 0;
                     goto Refetch;
                    
                 }
                 rowIndex++;
             }
             // for last row ----- 
        
             tableForGrid = AddRowToMaxMinTable(tableForGrid, colsList, datevalue);
             if (mode == "Energy")
                 ConvertDemandtoEnergy(tableForGrid, energyIndex);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(tableForGrid);
            return dataSet;
        }
         private DataTable AddRowToMaxMinTable(DataTable tableForGrid,List<List<decimal>> colsList, long datevalue)
         {
             DataRow rowForGrid = tableForGrid.NewRow();
             int j = 0;
             for (int i = 0; i <= tableForGrid.Columns.Count / 2 ; i++)
             {

                 // if it is first column ans date is not equalt to empty
                 if (i == 0 && DateUtility.LongToStringDateFormat(datevalue) != string.Empty)
                 {
                    rowForGrid[0] = DateUtility.LongToStringDateFormat(datevalue).Substring(0, 10);
                 }
                 if (i > 0)
                 {
                     rowForGrid[++j] = colsList[i - 1].Max();
                     rowForGrid[++j] = colsList[i - 1].Min();
                 }
             }

             tableForGrid.Rows.Add(rowForGrid);
         
             for (int l = 0; l < colsList.Count;l++ )
             {
                 colsList[l] = new List<decimal>();
             }
             return tableForGrid;
         }

        public DataSet GetDates(long meterDataID)
        {
            return loadSurveyDAL.GetDates(meterDataID);
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

       

        public ArrayList GetColumnsListAvailable(string parameterName, string meterDataID)//string fileName, string meterID)
        {
            return loadSurveyDAL.GetColumnsListAvailable(parameterName, meterDataID);//fileName, meterID);
        }

        public DataSet ListDataSet(long meterDataId, long fromDate, long toDate,string dataType)
        {
            IECLoadSurveyParameterEntity loadSurveyParameterEntity = new LoadSurveyParameterBLL().GetColumn(meterDataId) as IECLoadSurveyParameterEntity;
            if (loadSurveyParameterEntity == null)
                return null;
            DataSet dataSet=loadSurveyDAL.ListDataSet(meterDataId, loadSurveyParameterEntity.ColumnsNames, fromDate, toDate);
            DataSet ds = ConvertTamper(dataSet);
            return CommonBLL.ConvertLoadSurveyTabular(ds, dataType, fromDate, toDate);
        }
        public DataSet ListDataSetForGraph(long meterDataId, long fromDate, long toDate, string dataType)
        {
            string parVal=",RPhaseVoltage as 'Voltage R Phase',YPhaseVoltage as 'Voltage Y Phase',BPhaseVoltage as 'Voltage B Phase',RPhaseCurrent as 'Current R Phase',YPhaseCurrent as 'Current Y Phase',BPhaseCurrent as 'Current B Phase',AvgVoltage as 'Average Voltage',AvgCurrent as 'Average Current',DemandKVARLead as 'Demand kvar (lead)',DemandKVA as 'Demand kVA',DemandKW as 'Demand kW',DemandKVARLag as 'Demand kvar (lag)',PowerFactor";
            return CommonBLL.ConvertLoadSurvey(loadSurveyDAL.ListDataSet(meterDataId, parVal, fromDate, toDate), dataType);
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
            loadSurveyColumns.Add("Demand kvar (lead)", "DemandKVARLead");
            loadSurveyColumns.Add("Demand kVA", "DemandKVA");
            loadSurveyColumns.Add("Demand  kW", "DemandKW");
            loadSurveyColumns.Add("Demand kvar (lag)", "DemandKVARLag");
            loadSurveyColumns.Add("Power Factor", "PowerFactor");
            loadSurveyColumns.Add("DateTime", "LoadSurveyDateTime");
            loadSurveyColumns.Add("Interval Period", "MDIntervalPeriod");
            return loadSurveyColumns;
        }

        public DataSet GetLoadSurveyDataByParameter(string value, List<string> columnList, string reportType)
        {
            DataSet ds = new DataSet();
            List<string> columns = GetDatabaseColumns(columnList);
            if (reportType == "CAB")
                ds = loadSurveyDAL.GetLoadSurveyDataByFile(value, columns);
            else if (reportType == "Meter")
                ds = loadSurveyDAL.GetLoadSurveyDataByMeter(value, columns);

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

        private DataSet ConvertTamper(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count <= 0)
                return null;
            if (ds.Tables[0].Rows.Count <= 0)
                return null;
            DataTable table = new DataTable();
            foreach (DataColumn col in ds.Tables[0].Columns)
                table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
            bool isDLMS = false;
            string val = string.Empty;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DataRow dr = table.NewRow();
                int counter = 0;
                isDLMS = Convert.ToInt32(row["IsDLMS"])>0?true:false;
                foreach (DataColumn col in ds.Tables[0].Columns)
                {                    
                    if (col.ColumnName.Equals("TamperStatus"))
                    {                        
                        if (isDLMS)
                        {
                            val = Convert.ToString(row[counter]);
                            dr[counter] = GetTamperDLMSName(val);
                        }
                        else
                        {
                            val = Convert.ToString(row[counter]);
                            dr[counter] = GetTamperNames(val);
                        }
                    }
                    else
                        dr[counter] = row[counter];
                    counter++;
                }
                table.Rows.Add(dr);
            }
            DataSet dataSet = new DataSet();
            //if IsDLMS is present remove it as if present this will be displayed in grids or reports.
            if (table.Columns.Contains("IsDLMS"))
            {
                table.Columns.Remove("IsDLMS");
            }
            dataSet.Tables.Add(table);
            return dataSet;
        }
        /// <summary>
        /// This function converts decimal string to hex and then to binary and make a call to gettampertype method.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetTamperDLMSName(string value)
        {
            int number;
            string hexString = string.Empty;
            string tamperType = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(value))
                    number = int.Parse(value);
                else
                    number = 0;
                // Getting hex value
                hexString = number.ToString("x").PadLeft(8, '0');
                if (!string.IsNullOrEmpty(hexString))
                {
                    // Getting binary value
                    tamperType = Convert.ToString(Convert.ToInt32(hexString, 16), 2).PadLeft(24, '0');
                    tamperType = GetTamperType(tamperType);
                }
                if (string.IsNullOrEmpty(tamperType))
                {
                    tamperType = "NA";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tamperType;


        }
        /// <summary>
        /// This function matches the bits values to 1 is tamper is present and assign tamper accordingly.
        /// </summary>
        /// <param name="tamperBits"></param>
        /// <returns></returns>
        private string GetTamperType(string tamperBits)
        {
            string tamperType = string.Empty;
            try
            {
                // Checking the status of tamper.
                if (!string.IsNullOrEmpty(tamperBits))
                {
                    // Change the seqence of tamper bytes  to solve bug 72926.
                    if (tamperBits[4] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.LowPF) + ",";
                    if (tamperBits[5] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.OverCurrent) + ",";
                    if (tamperBits[6] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.UnderVoltage) + ",";
                    if (tamperBits[7] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.OverVoltage) + ",";
                    if (tamperBits[8] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.VoltImbalance) + ",";
                    if (tamperBits[9] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.CurrentImbalance) + ",";
                    if (tamperBits[10] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.NeutralDisturbance) + ",";
                    if (tamperBits[11] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.RPhaseCTReversal) + ",";
                    if (tamperBits[12] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.YPhaseCTReversal) + ",";
                    if (tamperBits[13] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.BPhaseCTReversal) + ",";
                    if (tamperBits[14] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.RPhaseCTOpen) + ",";
                    if (tamperBits[15] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.YPhaseCTOpen) + ",";
                    if (tamperBits[16] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.BPhaseCTOpen) + ",";
                    if (tamperBits[17] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.CTByPass) + ",";
                    if (tamperBits[18] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.RPhaseMissingPotential) + ",";
                    if (tamperBits[19] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.YPhaseMissingPotential) + ",";
                    if (tamperBits[20] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.BPhaseMissingPotential) + ",";
                    if (tamperBits[21] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.FrontCover) + ",";
                    // Removed terminal cover for bug 75298.
                    //if (tamperBits[22] == '1')
                    //    tamperType += EnumUtil.stringValueOf(CAB.Entity.TamperTypeEntity.GetTamperTypes.TerminalCover) + ",";
                    if (tamperBits[23] == '1')
                        tamperType += EnumUtil.StringValue(CAB.Entity.TamperTypeEntity.GetTamperTypes.Magnet) + ",";

                }
                if (!string.IsNullOrEmpty(tamperType))
                {
                    tamperType = tamperType.Substring(0, tamperType.Length - 1);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tamperType;

        }
        public string GetTamperNames(string tamperByte)
        {
            if (string.IsNullOrEmpty(tamperByte))
                return "-----";
            else if (tamperByte.Equals("000000"))
                return "-----";
            else
                return GetTamperNamesForDisplay(Convert.ToString(Convert.ToInt32(tamperByte, 16), 2));
        }

        public string GetTamperNamesForDisplay(string binaryValue)
        {
            string tamperNames = "";
            for(int count=0;count<=binaryValue.Length-1;count++)
            {
                if (binaryValue.Substring(count, 1) == "1")
                {
                    tamperNames += TamperNames()[count];
                    if (count < binaryValue.Length - 1)
                        tamperNames += ",";
                }
            }
            tamperNames = tamperNames.Substring(0, tamperNames.Length - 1);
            return tamperNames;
        }

        public string[] TamperNames()
        {
            string[] tampers = new string[24];
            tampers[0] = "UF(r)";
            tampers[1] = "UF(y)";
            tampers[2] = "UF(b)";
            tampers[3] = "PSR";
            tampers[4] = "2Pn";
            tampers[5] = "FCO";
            tampers[6] = "TCO";
            tampers[7] = "mt";
            tampers[8] = "nd ";
            tampers[9] = "CR(r)";
            tampers[10] = "CR(y)";
            tampers[11] = "CR(b)";
            tampers[12] = "CO(r)";   
            tampers[13] = "CO(y)";
            tampers[14] = "CO(b)";
            tampers[15] = "CS";
            tampers[16] = "UU(r)";
            tampers[17] = "UU(y)";
            tampers[18] = "UU(b)";
            tampers[19] = "CU(r)";
            tampers[20] = "CU(y)";
            tampers[21] = "CU(b)";
            tampers[22] = "";
            tampers[23] = "";
            return tampers;
        }
    }
}
