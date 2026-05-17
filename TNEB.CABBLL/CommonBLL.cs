using System;
using System.Data;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Collections.Generic;
using LTCTBLL;
using System.Reflection;
using System.ComponentModel;
namespace CAB.BLL
{
    public class CommonBLL : IBLL
    {
        static bool isWBExportVCL = false;
        private const string IMPORT = "";
        private const string EXPORT = " (E)";
        private const string NOTAPPLICABLE = "NA";
        static CommonBLL()
        {
            if (UtilityDetails.UtilityName == CAB.Entity.IECUtilityEntity.WBEXPORTVCL)
            {
                isWBExportVCL = true;
            }
        }
        private string SNo = "SNo";
        public static string GetUnit(string unit)
        {
            if (unit.ToUpper().Equals("KW"))
                return "kW";
            else if (unit.ToUpper().Equals("KWH"))
                return "kWh";
            else if (unit.ToUpper().Equals("KVAH"))
                return "kVAh";
            else if (unit.ToUpper().Equals("KVARH"))
                return "kvarh";
            else if (unit.ToUpper().Equals("KVA"))
                return "kVA";
            else if (unit.ToUpper().Equals("KVAR"))
                return "kvar";
            else if (unit.ToUpper().Equals("KVAR(LAG)"))
                return "kvar(lag)";
            else if (unit.ToUpper().Equals("KVAR(LEAD)"))
                return "kvar(lead)";
            else
                return unit;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static DataSet ConvertDemandToEnergy(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            int intervalPeriod = 60 / Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["MDIntervalPeriod"]));
            DataTable table = new DataTable();
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                string columnName = col.ColumnName.Replace("Demand", "Energy");
                if (columnName == "Energy kW")
                    columnName = "Energy kWh";
                if (columnName == "Energy kVA")
                    columnName = "Energy kVAh";
                if (columnName == "Energy kvar (lag)")
                    columnName = "Energy kvarh (lag)";
                if (columnName == "Energy kvar (lead)")
                    columnName = "Energy kvarh (lead)";
                
                table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
            }
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                DataRow dr = table.NewRow();
                for (int col = 0; col < ds.Tables[0].Columns.Count; col++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[row][col]);
                    int decPoint = val.Length - 1 - val.IndexOf('.');
                    string colname = ds.Tables[0].Columns[col].ColumnName;
                    if (colname.IndexOf("Demand") >= 0)
                    {
                        if (string.IsNullOrEmpty(val))
                            val = "0";
                        decimal num = decimal.Parse(val);
                        if (num <= -1)
                            dr[col] = val;
                        else
                        {
                            val = Convert.ToString(num / intervalPeriod);
                            int dotpos = val.IndexOf('.') + decPoint + 1;
                            dr[col] = val.Substring(0, dotpos);
                        }
                    }
                    else
                        dr[col] = val;

                }
                table.Rows.Add(dr);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        /// <summary>
        /// Convert fraud energy rows to columns
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static DataSet ConvertFraudEnergyRowToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Descriptions", typeof(System.String)));
            table.Columns.Add(new DataColumn("Value", typeof(System.String)));
            table.Columns.Add(new DataColumn("Unit", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                try
                {
                    if (col.ColumnName.Equals("FraudEnergy_ID") || col.ColumnName.Equals("MeterData_ID"))
                    {
                        continue; // NO deen to display these rows .
                    }
                    row = table.NewRow();
                    row[0] = Convert.ToString(col.ColumnName);
                    row[1] = Convert.ToString(dataRow[col.ColumnName]);
                    if (col.ColumnName.ToUpper().Contains("KWH"))
                    {
                         row[2] = "kWh";
                    }
                    else if (col.ColumnName.ToUpper().Contains("KVAH"))
                    {
                         row[2] = "kVAh";
                    }
                    else if (col.ColumnName.ToUpper().Contains("KVARH"))
                    {
                        row[2] = "kVArh";
                    }
                    table.Rows.Add(row);
                }
                catch
                {

                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public static DataSet ConvertRowToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Descriptions", typeof(System.String)));
            table.Columns.Add(new DataColumn("Value", typeof(System.String)));
            table.Columns.Add(new DataColumn("Unit", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                row = table.NewRow();
                row[0] = Convert.ToString(col.ColumnName);
                string val = Convert.ToString(dataRow[col.ColumnName]);
                if (val.ToUpper().IndexOf("PF") > 0)
                    val = val.Substring(0, val.ToUpper().IndexOf("PF") - 1);
                if (col.ColumnName.Equals("Latest Tamper Occurrence")) 
                {
                    if (val.ToUpper().IndexOf("LOW") > 0)
                        val = val + " PF";
                }
                if (col.ColumnName.Equals("Latest Tamper Restoration"))
                {
                    if (val.ToUpper().Contains("PHASE LOW"))
                        val = val + " PF";
                }
                if (col.ColumnName.Equals("Meter Constant"))
                    val = val+"*Imp/kWh; Imp/kVArh";
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = dat[0];
                    if ((col.ColumnName.IndexOf("On Hour") > 0) || col.ColumnName.Equals("Elapsed Time kW") || col.ColumnName.Equals("ElapsedTime kVA"))
                    {
                        string[] values = val.Split(':');
                        if (values.Length > 1)
                        {
                            val = decimal.Parse(values[0]).ToString() + ":" + values[1].ToString();
                        }
                        else
                        {

                            val = FormatPowerOnHours(values[0],false);
                        }
                        if (val.Equals("0:0"))
                            val = "00:00";
                        row[1] = val;
                    }
                    else
                    {
                        decimal val1 = 0;
                        if (val.IndexOf(' ') > 0)
                            row[1] = val;
                        else
                        {
                            if (decimal.TryParse(val, out val1))
                            {
                                row[1] = decimal.Parse(val).ToString();
                            }
                        }
                    }
                    if (dat[1].IndexOf("(") > 0)
                    {
                        if ((dat[1].IndexOf(")") == -1))
                            dat[1] = dat[1] + ")";
                    }
                    row[2] = GetUnit(dat[1]);
                }
                else
                {
                    if (col.ColumnName.Equals("Meter DateTime") || col.ColumnName.Equals("Restoration Time") || (col.ColumnName.IndexOf("Time Stamp") > 0) || col.ColumnName.Equals("Occurrence Time"))
                    {
                        row[1] = SplitWithOutDateUnit(val);
                        row[2] = ConfigInfo.DateFormat().ToUpper() + " HH : MM";
                    }
                    else if (col.ColumnName.Equals("Voltage Phase Sequence") || col.ColumnName.Equals("Latest Tamper Occurrence") || col.ColumnName.Equals("Latest Tamper Restoration") || col.ColumnName.Equals("Restoration Time") || col.ColumnName.Equals("Meter Id") || col.ColumnName.Equals("Error Code"))
                        row[1] = val;
                    else
                    {
                        try
                        {
                            row[1] = decimal.Parse(val).ToString();
                        }
                        catch (Exception)
                        {
                            row[1] = val;
                        }
                    }
                }
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        /// <summary>
        /// Used to format poweronhours value(comes in minute )
        /// into HH:MM format like :- 90 = 01:30
        /// </summary>
        /// <returns></returns>
        public static string FormatPowerOnHours(string minute,bool isWithUnit)
        {
            
            if (isWithUnit && minute.IndexOf("*") > 0)
            {
                string[] hourMinuteUnit = minute.Split('*');
                if (decimal.Parse(hourMinuteUnit[0]) > 59)
                {                   
                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(hourMinuteUnit[0]));
                    string strHour = ((int)timeSpan.TotalHours).ToString("00");
                    string strMinute = ((int)timeSpan.Minutes).ToString("00");
                    return strHour + ":" + strMinute;
                }
                else if (decimal.Parse(hourMinuteUnit[0]) > 0)
                {
                    return "00:" + hourMinuteUnit[0]; 
                }
            }
            else
            {
                if (decimal.Parse(minute) > 59)
                {
                    decimal powerOnHour = decimal.Parse(minute) / 60;
                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(minute));                    
                    string strHour = ((int)timeSpan.TotalHours).ToString("00");
                    string strMinute = ((int)timeSpan.Minutes).ToString("00");
                    return strHour + ":" + strMinute;

                }
                else if (decimal.Parse(minute) > 0)
                {
                    return "00:" + decimal.Parse(minute).ToString();
                }

            }
            return "00:00";
        }
        public static string ColText(int num)
        {
            if (num == 0)
                return "Last Time Stamp";
            else if (num == 1)
                return "2nd Last Time Stamp";
            else if (num == 2)
                return "3rd Last Time Stamp";
            else
                return string.Concat((num + 1).ToString(), "th Last Time Stamp");
        }

        public static DataSet ConvertProgramming(DataSet ds)
        {
            DataTable table = new DataTable();
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                string columnName = col.ColumnName;
                table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
            }
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            int noUpdates = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
            if (noUpdates > 10) noUpdates = 10;
            DataRow[] rowArray = ds.Tables[0].Select();
            int index = 9;
            for (int i=0; i<=noUpdates-1; i++)
            {

                DataRow drow = rowArray[index];
                index--;
                DataRow rowx = table.NewRow();
                rowx[1] = ColText(i);
                if (Convert.ToString(drow[2]) == "") rowx[2] = "--------";
                else rowx[2] = drow[2];
                for (int count = 3; count <= drow.ItemArray.Length - 1; count++)
                    rowx[count] = drow[count];
                table.Rows.Add(rowx);
            }
            DataSet dataset = new DataSet();
            dataset.Tables.Add(table);
            return dataset;
        }
        //public static DataSet ConvertProgramming(DataSet ds)
        //{
        //    DataTable table = new DataTable();
        //    foreach (DataColumn col in ds.Tables[0].Columns)
        //    {
        //        string columnName = col.ColumnName;
        //        table.Columns.Add(new DataColumn(columnName, typeof(System.String)));
        //    }
        //    int noUpdates = Convert.ToInt32(ds.Tables[0].Rows.Count);
        //    DataRow[] rowArray = ds.Tables[0].Select();
        //    int i = 0;
        //    for (int j = ds.Tables[0].Rows.Count - 1; j >= 0; j--)
        //    {
        //        DataRow drow = rowArray[noUpdates - (i + 1)];
        //        DataRow rowx = table.NewRow();
        //        rowx[1] = ColText(i);
        //        rowx[2] = drow[2];
        //        rowx[3] = drow[3];
                
        //        table.Rows.Add(rowx);
        //        i++;



        //        DataRow dr = ds.Tables[0].Rows[j];
        //        DataRow row = table.NewRow();
        //        row[0] = dr[0];
        //        row[1] = ColText(j);
        //        row[2] = dr[2];
        //        row[3] = dr[3];
        //        row[4] = dr[4];
        //        row[5] = dr[5];
        //        row[6] = dr[6];
        //        row[7] = dr[7];
        //        row[8] = dr[8];
        //        row[9] = dr[9];
        //        row[10] = dr[10];
        //        row[11] = dr[11];
        //        row[12] = dr[12];
        //        row[13] = dr[13];
        //        row[14] = dr[14];
        //        row[15] = dr[15];
        //        row[16] = dr[16];
        //        row[17] = dr[17];
        //        row[18] = dr[18];
        //        row[19] = dr[19];
        //        table.Rows.Add(row);
        //    }
        //    DataSet dataset = new DataSet();
        //    dataset.Tables.Add(table);
        //    return dataset;
        //}
        public static DataSet ConvertRTCUpdatesRowToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("RTC Updates", typeof(System.String)));
            table.Columns.Add(new DataColumn("Old Time Stamp", typeof(System.String)));
            table.Columns.Add(new DataColumn("Updated Time Stamp", typeof(System.String)));
            DataTable tmpTable = new DataTable();
            tmpTable.Columns.Add(new DataColumn("VAL", typeof(System.Int64)));
            tmpTable.Columns.Add(new DataColumn("VALX", typeof(System.Int64)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0]; 
            for (int counter = 0; counter < 10; counter++)
            {
                string colName = "PreviousRTC" + (counter + 1).ToString();
                row = tmpTable.NewRow(); 
                row[0] = SplitDateUnit(Convert.ToString(dataRow[colName]));
                colName = "CurrentRTC" + (counter + 1).ToString();
                row[1] = SplitDateUnit(Convert.ToString(dataRow[colName]));
                tmpTable.Rows.Add(row); 
            }
            DataRow[] rowArray = tmpTable.Select();
            int noUpdates = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]);
            //dataIndex += 2;
            int count = 0;
            if (noUpdates > 10)
                noUpdates = 10;
            int i = 0;
            while (i < noUpdates)
            {
                DataRow drow = rowArray[noUpdates - (i + 1)];
                DataRow rowx = table.NewRow();
                rowx[0] = ColText(i);
                if (Convert.ToInt64(drow[0]) == 10000000000000)
                    rowx[1] = "--------";
                else
                    rowx[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[0]));
                if (Convert.ToInt64(drow[1]) == 10000000000000)
                    rowx[2] = "--------";
                else
                    rowx[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1]));
                table.Rows.Add(rowx);
                i++;
            }
             //for (int i = 0; i < 10; i++)
             //{
            //DataRow drow = rowArray[10 - (i + 1)];
            //DataRow rowx = table.NewRow();
            //rowx[0] = ColText(i);
            //if (Convert.ToInt64(drow[0]) == 10000000000000)
            //    rowx[1] = "--------";
            //else
            //    rowx[1] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[0]));
            //if (Convert.ToInt64(drow[1]) == 10000000000000)
            //    rowx[2] = "--------";
            //else
            //    rowx[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(drow[1]));
            //table.Rows.Add(rowx);
             //} 

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public static long SplitDateUnit(string data)
        {
            data = data.Trim();
            if (string.IsNullOrEmpty(data))
                return 10000000000000;
            if (data.Length != 16)
            {
                if (data.IndexOf(' ') > 0)
                    return 10000000000000;
            }
			if (data.Length <= 15)
				data = data + "0000000000";
            data = data.Replace(" : ",":");
            string day = data.Substring(0,2);
            string month = data.Substring(3, 2);
            string year = data.Substring(6, 4);
            string hour = data.Substring(11, 2);
            string minute = data.Substring(14, 2);
            data=string.Concat( year,month,day,hour,minute,"00"); 
            return Int64.Parse(data);
        }
        public static long SplitLoadsurveyDateUnit(string data)
        {
            bool isDayFirst = false;
          string format=  ConfigInfo.DateFormat();
          if (format.Substring(0, 2).ToUpper() == "DD")
              isDayFirst = true;
            if (string.IsNullOrEmpty(data))
                return 10000000000000;
            if (data.Length <= 15)
                data = data + "0000000000";
            data = data.Replace(" : ", ":");
            string day = data.Substring(0, 2);
            string month = data.Substring(3, 2);
            string year = data.Substring(6, 4);
            string hour = data.Substring(11, 2);
            string minute = data.Substring(14, 2);
            if(isDayFirst)
            data = string.Concat(year, month, day, hour, minute, "00");
            else
                data = string.Concat(year, day, month, hour, minute, "00");
            return Int64.Parse(data);
        }
        public static DataSet ConvertCTRatioColumn(DataSet ds, string ColumnName)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn(ColumnName, typeof(System.String)));
            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                int rowVal = Convert.ToInt32(dataRow[0]);
                if (rowVal == 0)
                    row[0] = "Current";
                else
                    row[0] = "History - " + rowVal.ToString();

                    string val = SplitCTRatio(Convert.ToString(dataRow[1]));
                    if (val.Length >= 7)
                        if (val.Substring(0, 2) == "00")
                            val = val.Substring(2, val.Length - 2);
                    row[1] = val;
                    if (rowVal != 0)  // condition added on 10 Aug 2010 by Shivangy as per Ruby Specifications
                    {
                        table.Rows.Add(row);
                    }
                    else
                    {
                        row[1] = "------";
                        table.Rows.Add(row);
                    }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertPowerOnHour(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;   
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Values (HH:MM)", typeof(System.String)));
            DataRow row;
            long currHour, currMinute, hour, minute;
            currHour = currMinute = hour = minute = 0;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                int rowVal = Convert.ToInt32(dataRow[0]);
                if (rowVal == 0)
                    row[0] = "Current";
                else
                    row[0] = "History - " + rowVal.ToString();
                string val = SplitWithOutUnit(Convert.ToString(dataRow[1]));


                if (val.IndexOf(':') > 0)
                {
                    if (row[0] == "Current")
                    {
                        currHour = long.Parse(val.Substring(0, val.IndexOf(':')));
                        currMinute = long.Parse(val.Substring(val.IndexOf(':') + 1, (val.Length - val.IndexOf(':')) - 1));
                    }
                    else
                    {
                        hour = hour + long.Parse(val.Substring(0, val.IndexOf(':')));
                        minute = minute + long.Parse(val.Substring(val.IndexOf(':') + 1, (val.Length - val.IndexOf(':')) - 1));
                    }
                }
                /* GKG 21/01/2013 TANGEDCO ISSUE*/
                if (UtilityDetails.ShowPowerOnHoursInMinutes)
                {
                    row[1] = FormatPowerOnHours(val, false);
                }
                else
                {
                    row[1] = val;
                }
                /* GKG 21/01/2013 TANGEDCO ISSUE*/
                table.Rows.Add(row);
            }
            if (!UtilityDetails.ShowPowerOnHoursInMinutes)
            {
                currHour = currHour * 60 + currMinute;
                hour = hour * 60 + minute;
                currHour = currHour - hour;
                hour = currHour / 60;
                minute = currHour - (hour * 60);
                if (minute.ToString().Length == 1)
                    table.Rows[0][1] = hour.ToString() + ":0" + minute.ToString();
                else
                    table.Rows[0][1] = hour.ToString() + ":" + minute.ToString();
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        
        public static DataSet ConvertHistoryWithSingleColumn(DataSet ds, string ColumnName, bool currValue)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;   
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn(ColumnName, typeof(System.String)));
            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                int rowVal = Convert.ToInt32(dataRow[0]);
                if (rowVal == 0)
                    row[0] = "Current";
                else
                    row[0] = "History - " + rowVal.ToString();
                string val = SplitWithOutUnit(Convert.ToString(dataRow[1]));
                if (string.IsNullOrEmpty(val))
                    val = "0";
                if (val.Length >= 7)
                    if (val.Substring(0, 2) == "00")
                        val = val.Substring(2, val.Length - 2);
                if (val == "----------")
                    row[1] = val;
                else
                {
                    if (val.IndexOf(':') > 0)
                        row[1] = val;
                    else
                    {
                        try
                        {
                            row[1] = Convert.ToDecimal(val).ToString();
                        }
                        catch (Exception)
                        { 
                            row[1] = val;
                        }
                    }
                }
                if (!currValue)
                {
                    if (rowVal != 0)  // condition added on 10 Aug 2010 by Shivangy as per Ruby Specifications
                        table.Rows.Add(row);
                }
                else
                {
                    table.Rows.Add(row);
                }
            }
           
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        private static string SplitCTRatio(string data)
        {
            string value = data;
            if (data.IndexOf('-') > 0)
            {
                string[] val = data.Split('-');
                value = val[0];
            }
            value = value.Trim();
            int Num;
            if (int.TryParse(value, out Num))
            {
                if (string.IsNullOrEmpty(value))
                    value = "0";
                return Convert.ToUInt32(value).ToString();
            }
            else
            {
                return value;
            }
        }
        private static string SplitWithOutUnit(string data)
        {
            string value = data;
            if (data.IndexOf('*') > 0)
            {
                string[] val = data.Split('*');
                value = val[0];
            }
            if (value.IndexOf(':') > 0)
            {
                string[] result = value.Split(':');
                value = decimal.Parse(result[0]).ToString() + ":" + result[1].ToString();
            }
            //decimal.Parse(values[0]).ToString() + ":" + values[1].ToString();
            return value;// return value.Replace("000000.00", "0.000");//changed by Shivangy on 11 Aug 2010
        }
        public static DataSet ConvertCumulativeEnergyCalculatedToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWh" + IMPORT, typeof(System.String)));
            table.Columns.Add(new DataColumn("kVAh" + IMPORT, typeof(System.String)));            
            table.Columns.Add(new DataColumn("kvarh (Lag)", typeof(System.String)));
            table.Columns.Add(new DataColumn("kvarh (Lead)", typeof(System.String)));
            if (isWBExportVCL)
            {
                table.Columns.Add(new DataColumn("kWh" + EXPORT, typeof(System.String)));
                table.Columns.Add(new DataColumn("kVAh" + EXPORT, typeof(System.String)));
            }
            DataRow row;
            int counter = 0;
            int counterInner = 1;
            string frInterval = "";
            string toInterval = "";
            for (counter = 0; counter <= 12; counter++, counterInner++)
            {
                if (counter == 0) { frInterval = "Current"; toInterval = "History " + Convert.ToString(counterInner); }
                else { frInterval  = "History " + Convert.ToString(counterInner-1); toInterval="History " + Convert.ToString(counterInner);}
                if (ds.Tables[0].Rows.Count > counter)
                {
                    DataRow firstRow = ds.Tables[0].Rows[counter];
                    if (counterInner == 13)
                        break;
                    if (ds.Tables[0].Rows.Count > counterInner)
                    {
                        DataRow secondRow = ds.Tables[0].Rows[counterInner];
                        row = table.NewRow();
                        row[0] = frInterval  + " - " + toInterval;
                        row[1] = (Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[2]))) - Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[2])))).ToString();
                        row[2] = (Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[3]))) - Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[3])))).ToString();
                        row[3] = (Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[4]))) - Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[4])))).ToString();
                        row[4] = (Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[5]))) - Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[5])))).ToString();
                        if (isWBExportVCL && row.Table.Columns.Count > 5)
                        {
                            if (((firstRow[6].ToString()== NOTAPPLICABLE) && (firstRow[7].ToString()== NOTAPPLICABLE)))
                            {
                                row[5] = NOTAPPLICABLE;
                                row[6] = NOTAPPLICABLE;
                            }
                            else if (((secondRow[6].ToString() == NOTAPPLICABLE) && (secondRow[7].ToString() == NOTAPPLICABLE)))
                            {
                                row[5] = NOTAPPLICABLE;
                                row[6] = NOTAPPLICABLE;
                            }
                            else
                            {
                                row[5] = (Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[6]))) - Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[6])))).ToString();
                                row[6] = (Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(firstRow[7]))) - Convert.ToDecimal(SplitWithOutUnit(Convert.ToString(secondRow[7])))).ToString();

                            }
                        }
                        table.Rows.Add(row);
                    }
                }
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertCumulativeEnergyToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));

                table.Columns.Add(new DataColumn("kWh" + IMPORT, typeof(System.String)));
                table.Columns.Add(new DataColumn("kVAh" + IMPORT, typeof(System.String)));
           
            table.Columns.Add(new DataColumn("kvarh (Lag)", typeof(System.String)));
            table.Columns.Add(new DataColumn("kvarh (Lead)", typeof(System.String)));
            if (isWBExportVCL)
            {
                table.Columns.Add(new DataColumn("kWh" + EXPORT, typeof(System.String)));
                table.Columns.Add(new DataColumn("kVAh" + EXPORT, typeof(System.String)));
            }
            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                int rowVal = Convert.ToInt32(dataRow[0]);
                if (rowVal == 0)
                    row[0] = "Current";
                else
                    row[0] = "History - " + rowVal.ToString();
                string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dataRow[1]));
                //if (val.IndexOf(":") > 0)
                //    val = val.Substring(0, 10);
                row[1] = val;
                row[2] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[2]))).ToString();
                row[3] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[3]))).ToString();
                row[4] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[4]))).ToString();
                row[5] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[5]))).ToString();
                if (isWBExportVCL)
                {
                    if (row.Table.Columns.Count > 6)
                    {
                        if ((string.IsNullOrEmpty(dataRow[6].ToString())) && (string.IsNullOrEmpty(dataRow[7].ToString())))
                        {
                            row[6] = NOTAPPLICABLE;
                            row[7] = NOTAPPLICABLE;
                        }
                        else
                        {
                            row[6] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[6]))).ToString();
                            row[7] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[7]))).ToString();
                        }
                    }
                }
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertCumulativeEnergyToColumnTNEB(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ResetNo", typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing DateTime", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWh", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVAh", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVArh (Lag)", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVArh (Lead)", typeof(System.String)));
            table.Columns.Add(new DataColumn("KW(MD)", typeof(System.String)));
            table.Columns.Add(new DataColumn("KW(OccTime)", typeof(System.String)));
            table.Columns.Add(new DataColumn("KVA(MD)", typeof(System.String)));
            table.Columns.Add(new DataColumn("KVA(Occtime)", typeof(System.String)));
            table.Columns.Add(new DataColumn("AvgP.F", typeof(System.String)));
            DataRow row;
            int i = ds.Tables[0].Rows.Count;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                
                    row = table.NewRow();

                    if (!string.IsNullOrEmpty(dataRow[1].ToString()))
                    {

                        row[0] = i;

                        string val = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(dataRow[1]));
                        //if (val.IndexOf(":") > 0)
                        //    val = val.Substring(0, 10);
                        row[1] = val;
                        row[2] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[2]))).ToString();
                        row[3] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[3]))).ToString();
                        row[4] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[4]))).ToString();
                        row[5] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[5]))).ToString();
                        row[6] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[6]))).ToString();
                        val = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(dataRow[7]));
                        row[7] = val;
                        row[8] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[8]))).ToString();
                        val = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(dataRow[9]));
                        row[9] = val;
                        if (!string.IsNullOrEmpty(dataRow[10].ToString()))
                        {
                            row[10] = decimal.Parse(SplitWithOutUnit(Convert.ToString(dataRow[10]))).ToString();
                        }
                        else
                        {
                            row[10] = "---------";
                        }
                        table.Rows.Add(row);
                        i--;
                    }


                    
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertMaximumDemandToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Billing Date Time", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD1", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD1 Time Stamp", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD2", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD2 Time Stamp", typeof(System.String)));
            //table.Columns.Add(new DataColumn("MD3", typeof(System.String)));
            //table.Columns.Add(new DataColumn("MD3 Time Stamp", typeof(System.String))); 
            DataRow row;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                int rowVal = Convert.ToInt32(dataRow[0]);
                if (rowVal == 0)
                    row[0] = "Current";
                else
                    row[0] = "History - " + rowVal.ToString();
                string val = SplitWithOutDateUnit(Convert.ToString(dataRow[1]));
                //if (val.IndexOf(':') > 0)
                //    val = val.Substring(0, val.IndexOf(':') - 4);
                row[1] = val;


                string[] result = dataRow[2].ToString().Split('.');
                string value = decimal.Parse(result[0]).ToString() + ":" + result[1].ToString();


                row[2] =Convert.ToString(dataRow[2]).Replace("*","  ").Replace("("," ").Replace(")",""); 
                val = SplitWithOutDateUnit(Convert.ToString(dataRow[3]));
                row[3] = val;
                row[4] = Convert.ToString(dataRow[4]).Replace("*", "  ").Replace("(", " ").Replace(")", "");
                if (Convert.ToString(row[4]).IndexOf("KVA") < 0)
                    row[4] = "------";
                val = SplitWithOutDateUnit(Convert.ToString(dataRow[5])); 

                row[5] = val;
                //row[6] = Convert.ToString(dataRow[6]).Replace("*", "  ").Replace("(", " ").Replace(")", "");
                //if (Convert.ToString(row[6]).IndexOf("KVAr") < 0)
                //    row[6] = "------";
                //val = SplitWithOutDateUnit(Convert.ToString(dataRow[7]));

                //row[7] = val;
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static string SplitWithOutDateUnit(string data)
        {
            string value = "------";
            if (data == "0")
                return value;
			if (data == "")
				return value;
            if (data.IndexOf('*') > 0)
            {
                string[] val = data.Split('*');
                if (val[1] == "YYYY/MM/DD :MM:SS")
                {
                    value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(val[0]));
                }
            }
            else if (data.IndexOf('-') > 0)
            {
                long dateval = Convert.ToInt64(DateUtility.StringToLongDateTimeFormat(data));
                if (dateval != 0)
                    value = DateUtility.LongToStringDateTimeFormat(dateval);
            }
            else
                value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(data));
            return value;
        }
        public static DataSet RemoveGarbageColumns(DataSet dataSet)
        {
            int counter =0;
            string date = string.Empty;
            if (dataSet != null)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    date = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row["DailyProfileDate"]));
                    if ((date == string.Empty) || (date.StartsWith("-")))
                    {
                        dataSet.Tables[0].Rows[counter].Delete();
                    }
                    counter++;
                   
                }
                dataSet.AcceptChanges();
            }
            return dataSet;
        }
        public static DataSet ConvertDTMDailyProfileRowToColumn(DataSet dataSet)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            dataSet = RemoveGarbageColumns(dataSet);
            DataTable table = new DataTable();
            int counter = 0;
            int DailyProfileDateIndex = -1;
            int MD1TimeStampIndex = -1;
            int MD2TimeStampIndex = -1;
            int MD3TimeStampIndex = -1;
            int PowerOnHours = -1;
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            { 
                if (col.ColumnName.Equals("AvailableDays"))
                    continue;
                if (col.ColumnName.Equals("Daily Profile Date"))
                    DailyProfileDateIndex = counter;
                if (col.ColumnName.Equals("MD1 Time Stamp"))
                    MD1TimeStampIndex = counter;
                if (col.ColumnName.Equals("MD2 Time Stamp"))
                    MD2TimeStampIndex = counter;
                if (col.ColumnName.Equals("MD3 Time Stamp"))
                    MD3TimeStampIndex = counter;
                if (col.ColumnName.Equals("Power On Hours (HH)"))
                    PowerOnHours = counter;
                table.Columns.Add(col.ColumnName);
                counter++;
            }
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < counter ; i++)
                {
                     if (i == 0)
                    {
                        string value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[i]));
                        if (value.IndexOf(' ') > 0)
                            value = value.Substring(0, value.IndexOf(' '));
                        row[i] = value;
                    }
                    else if (DailyProfileDateIndex == i)
                    {
                        string value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[i]));
                        if(value.Length>10)
                           value =value.Substring(0, 10);
                        row[i] = value;
                    }
                    else if (MD1TimeStampIndex == i)
                    {
                        string value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[i]));
                        row[i] = value;
                    }
                    else if (MD2TimeStampIndex == i)
                    {
                        string value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[i]));
                        row[i] = value;
                    }
                    else if (MD3TimeStampIndex == i)
                    {
                        string value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[i]));
                        row[i] = value;
                    }
                     else if (PowerOnHours == i && dr[i].ToString().Length < 2)
                     {
                         row[i] = "0" + dr[i].ToString();
                     }  
                    else                         
                         row[i] = dr[i];
                }
                table.Rows.Add(row);
            }
            DataSet fDataSet = new DataSet();
            fDataSet.Tables.Add(table);
            return fDataSet;
        }
        public static DataSet ExportData(DataSet dataSet)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            foreach (DataColumn col in dataSet.Tables[0].Columns)
                table.Columns.Add(col.ColumnName);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                DataRow dr = table.NewRow();
                dr[0] = row["S.No"];
                dr[1] = row["MeterData_ID"];
                dr[2] = row["Meter Number"];
                dr[3] = row["File Name"];
                dr[4] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row["Reading DateTime"]));
                table.Rows.Add(dr);
            }
            DataSet tmpData = new DataSet();
            tmpData.Tables.Add(table);
            return tmpData;
        }
        public static DataSet ExportListData(DataSet dataSet)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count == 0)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            foreach (DataColumn col in dataSet.Tables[0].Columns)
                table.Columns.Add(col.ColumnName);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                DataRow dr = table.NewRow();
                dr[0] = row["S.No"];
                dr[1] = row["File Name"];
                dr[2] = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(row["Uploading DateTime"]));
                table.Rows.Add(dr);
            }
            DataSet tmpData = new DataSet();
            tmpData.Tables.Add(table);
            return tmpData;
        }
        public static DataSet ConvertTariffAveragePowerFactorToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("Value", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariff = 1;
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                row = table.NewRow();
                row[0] = tariff.ToString();
                row[1] = SplitWithOutUnit(Convert.ToString(dataRow[col.ColumnName]));
                table.Rows.Add(row);
                tariff++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertNamePlateDetailRowToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Parameters", typeof(System.String)));
            table.Columns.Add(new DataColumn("Values", typeof(System.String)));
            DataRow row = table.NewRow();
            DataRow dataRow = ds.Tables[0].Rows[0];
            row[0] = "Meter Id";
            row[1] = dataRow[0];
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "Meter Type";
            row[1] = dataRow[1];
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "Current Rating";
            row[1] = dataRow[2];
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "Voltage Rating";
            row[1] = dataRow[3];
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "Meter Constant";
            row[1] = dataRow[4];
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "Manufacturing Year";
            row[1] = dataRow[5];
            table.Rows.Add(row);

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertPhasorToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Parameters", typeof(System.String)));
            table.Columns.Add(new DataColumn("Values", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];

            if (dataRow != null)
            {
                string val = Convert.ToString(dataRow["R Phase kW Direction"]);
                int exIm=0;
                if (val == "Import")
                    exIm = 0;
                else
                    exIm = 1; 
                val = Convert.ToString(dataRow["R Phase Lag/Lead"]);
                int lagLead=0;
                if (val == "Lag")
                    lagLead = 0;
                else
                    lagLead = 1;

                if (exIm ==  lagLead )
                    dataRow["R Phase Lag/Lead"] = "Lag";
                else
                    dataRow["R Phase Lag/Lead"] = "Lead";


                val = Convert.ToString(dataRow["Y Phase kW Direction"]);
                  exIm = 0;
                if (val == "Import")
                    exIm = 0;
                else
                    exIm = 1;
                val = Convert.ToString(dataRow["Y Phase Lag/Lead"]);
                  lagLead = 0;
                if (val == "Lag")
                    lagLead = 0;
                else
                    lagLead = 1;

                if (exIm ==   lagLead )
                    dataRow["Y Phase Lag/Lead"] = "Lag";
                else
                    dataRow["Y Phase Lag/Lead"] = "Lead";


                val = Convert.ToString(dataRow["B Phase kW Direction"]);
                exIm = 0;
                if (val == "Import")
                    exIm = 0;
                else
                    exIm = 1;
                val = Convert.ToString(dataRow["B Phase Lag/Lead"]);
                lagLead = 0;
                if (val == "Lag")
                    lagLead = 0;
                else
                    lagLead = 1;

                if (exIm == lagLead )
                    dataRow["B Phase Lag/Lead"] = "Lag";
                else
                    dataRow["B Phase Lag/Lead"] = "Lead";
                 
            }
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                row = table.NewRow();
                row[0] = col.ColumnName;
                row[1] = Convert.ToString(dataRow[col.ColumnName]);
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertamperCounterToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("History", typeof(System.String)));
            table.Columns.Add(new DataColumn("Counter", typeof(System.String)));
            DataRow row;
            int counter = 0;
            foreach (DataRow dataRow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                if (counter == 0)
                    row[0] = "Current";
                else
                    row[0] = "History - " + counter.ToString();
                row[1] = Convert.ToString(dataRow[0]);
                table.Rows.Add(row);
                counter++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertTariffEnergyTODMDToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable(); 
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD1(KW)", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD1 Time Stamp", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD2(KVA)", typeof(System.String)));
            table.Columns.Add(new DataColumn("MD2 Time Stamp", typeof(System.String)));
            //table.Columns.Add(new DataColumn("MD3", typeof(System.String)));
            //table.Columns.Add(new DataColumn("MD3 Time Stamp", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariff = 1;
            int tariffNumber = 1;
            row = table.NewRow();
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                string val = Convert.ToString(dataRow[col.ColumnName]);
                if (tariff == 1)
                    row[0] = tariffNumber.ToString();
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = dat[0];
                }
                if (col.ColumnName.IndexOf("Time") > 0)
                {
                    val = SplitWithOutDateUnit(val);
                    row[tariff] = val;
                }
                else
                    row[tariff] = decimal.Parse(val).ToString();
                if (tariff == 4)
                {
                    table.Rows.Add(row);
                    row = table.NewRow();
                    tariff = 0;
                    tariffNumber++;
                }
                tariff++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertTODConsumptionToColumn(DataSet History1, DataSet History2)
        {
            if (History1 == null || History2 == null)
                return null;
            if (History1.Tables[0].Rows.Count == 0 || History1.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Tariff", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWh", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVAh", typeof(System.String)));
            table.Columns.Add(new DataColumn("kvarh (Lag)", typeof(System.String)));
            table.Columns.Add(new DataColumn("kvarh (Lead)", typeof(System.String)));
            DataRow row;
            for (int counter = 0; counter < 8; counter++)
            {
                DataRow Row1 = History1.Tables[0].Rows[counter];
                DataRow Row2 = History2.Tables[0].Rows[counter];
                row = table.NewRow();
                row[0] = Row1[0];
                row[1] = (Convert.ToDecimal(Convert.ToString(Row1[1])) - Convert.ToDecimal(Convert.ToString(Row2[1]))).ToString();
                row[2] = (Convert.ToDecimal(Convert.ToString(Row1[2])) - Convert.ToDecimal(Convert.ToString(Row2[2]))).ToString();
                row[3] = (Convert.ToDecimal(Convert.ToString(Row1[3])) - Convert.ToDecimal(Convert.ToString(Row2[3]))).ToString();
                row[4] = (Convert.ToDecimal(Convert.ToString(Row1[4])) - Convert.ToDecimal(Convert.ToString(Row2[4]))).ToString();
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertTariffEnergyToColumn(DataSet ds)
        {
            if (ds == null)
                return null;
            if (ds.Tables.Count == 0)
                return null;
            if (ds.Tables[0].Rows.Count == 0)
                return null;
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Tariff Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("kWh", typeof(System.String)));
            table.Columns.Add(new DataColumn("kVAh", typeof(System.String)));
            table.Columns.Add(new DataColumn("kvarh (Lag)", typeof(System.String)));
            table.Columns.Add(new DataColumn("kvarh (Lead)", typeof(System.String)));
            DataRow row;
            DataRow dataRow = ds.Tables[0].Rows[0];
            int tariff = 1;
            int tariffNumber = 1;
            row = table.NewRow();
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                string val = Convert.ToString(dataRow[col.ColumnName]);
                if (tariff == 1)
                    row[0] = tariffNumber.ToString();
                if (val.IndexOf('*') > 0)
                {
                    string[] dat = val.Split('*');
                    val = decimal.Parse(dat[0]).ToString();
                }
                row[tariff] = val;
                if (tariff == 4)
                {
                    table.Rows.Add(row);
                    row = table.NewRow();
                    tariff = 0;
                    tariffNumber++;
                }
                tariff++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet History()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            table.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            for (int counter = 0; counter <= 12; counter++)
            {
                row = table.NewRow();
                string val = "History ";
                if (counter == 0)
                    val = "Current";
                else
                    val = val + counter.ToString();
                row[0] = val;
                row[1] = counter.ToString();

                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet TODHistory()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            table.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            for (int counter = 0; counter <= 12; counter++)
            {
                row = table.NewRow();
                string val = "";
                if (counter == 0)
                    val = "Current    - History  1";
                else
                {
                    if (counter == 12)
                        break;
                    val = "History ";
                    if (counter <= 9)
                        val = val + " " + counter.ToString() + " - History ";
                    else
                        val = val + counter.ToString() + " - History ";
                    if (counter + 1 <= 9)
                        val = val + "  " + (counter + 1).ToString();
                    else
                        val = val + (counter + 1).ToString();
                }
                row[0] = val;
                row[1] = counter.ToString();

                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet Tamper()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            table.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));

            DataRow row = table.NewRow();
            row[0] = "  1. Voltage Imbalance R Phase Tamper Counter";
            row[1] = "VoltageImbalanceRPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  2. Voltage Imbalance Y Phase Tamper Counter";
            row[1] = "VoltageImbalanceYPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  3. Voltage Imbalance B Phase Tamper Counter";
            row[1] = "VoltageImbalanceBPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  4. Missing Potential R Phase Tamper Counter";
            row[1] = "MissingPotentialRPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  5. Missing Potential Y Phase Tamper Counter";
            row[1] = "MissingPotentialYPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  6. Missing Potential B Phase Tamper Counter";
            row[1] = "MissingPotentialBPhaseTamperCounter";
            table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = " 7.    Low / Under Voltage R Phase Tamper Counter";
            //row[1] = "LowUnderVoltageRPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = " 8.    Low / Under Voltage Y Phase Tamper Counter";
            //row[1] = "LowUnderVoltageYPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = " 9.    Low / Under Voltage B Phase Tamper Counter";
            //row[1] = "LowUnderVoltageBPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "10.    High / Over Voltage R Phase Tamper Counter";
            //row[1] = "HighOverVoltageRPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "11.    High / Over Voltage Y Phase Tamper Counter";
            //row[1] = "HighOverVoltageYPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "12.    High / Over Voltage B Phase Tamper Counter";
            //row[1] = "HighOverVoltageBPhaseTamperCounter";
            //table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  7. CT Short Tamper Counter";
            row[1] = "CTShortTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  8. CT Open R Phase Tamper Counter";
            row[1] = "CTOpenRPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "  9. CT Open Y Phase Tamper Counter";
            row[1] = "CTOpenYPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "10. CT Open B Phase Tamper Counter";
            row[1] = "CTOpenBPhaseTamperCounter";
            table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "17.    Current Without Voltage R Phase Tamper Counter";
            //row[1] = "CurrentWithoutVoltageRPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "18.    Current Without Voltage Y Phase Tamper Counter";
            //row[1] = "CurrentWithoutVoltageYPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "19.    Current Without Voltage B Phase Tamper Counter";
            //row[1] = "CurrentWithoutVoltageBPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "20.    Low Power Factor R Phase Tamper Counter";
            //row[1] = "LowPowerFactorRPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "21.    Low Power Factor Y Phase Tamper Counter";
            //row[1] = "LowPowerFactorYPhaseTamperCounter";
            //table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "22.    Low Power Factor B Phase Tamper Counter";
            //row[1] = "LowPowerFactorBPhaseTamperCounter";
            //table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "11. One Phase Neutral Absent Tamper Counter";
            row[1] = "OnePhaseNeutralAbsentTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "12. Voltage Phase Reversal Tamper Counter";
            row[1] = "VoltagePhaseReversalTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "13. Current Imbalance R Phase Tamper Counter";
            row[1] = "CurrentImbalanceRPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "14. Current Imbalance Y Phase Tamper Counter";
            row[1] = "CurrentImbalanceYPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "15. Current Imbalance B Phase Tamper Counter";
            row[1] = "CurrentImbalanceBPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "16. Current Reversal R Phase Tamper Counter";
            row[1] = "CurrentReversalRPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "17. Current Reversal Y Phase Tamper Counter";
            row[1] = "CurrentReversalYPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "18. Current Reversal B Phase Tamper Counter";
            row[1] = "CurrentReversalBPhaseTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "19. Magnetic Influence Tamper Counter";
            row[1] = "MagneticInfluenceTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "20. Neutral Disturbance Tamper Counter";
            row[1] = "NeutralDisturbanceTamperCounter";
            table.Rows.Add(row);

            row = table.NewRow();
            row[0] = "21. Front Cover Opening Tamper Counter";
            row[1] = "FrontCoverOpeningTamperCounter";
            table.Rows.Add(row);

            //row = table.NewRow();
            //row[0] = "22.    Terminal Cover Opening Tamper Counter";
            //row[1] = "TerminalCoverOpeningTamperCounter";
            //table.Rows.Add(row);


            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertDTMLoadSurvey(DataSet dataSet)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            DataTable table = new DataTable();
            int counter = 0;
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
                counter++;
            }
            int intervalPeriod = 30;
            DataRow secondRow = null;
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                DataRow firstRow = dataSet.Tables[0].Rows[counter];
                if ((counter + 1) < dataSet.Tables[0].Rows.Count)
                    secondRow = dataSet.Tables[0].Rows[counter + 1];
                else
                    secondRow = firstRow;

                DateTime firstRowDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow["Date Time"]));
                DateTime secondRowDate = DateUtility.LongToDateTime(Convert.ToInt64(secondRow["Date Time"]));

                DataRow row = table.NewRow();
                string val = SplitWithOutDateUnit(firstRow[0].ToString());
                row[0] = val;
                for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
                    row[i] = Convert.ToString(firstRow[i]);
                table.Rows.Add(row);
                bool flag = true;
                do
                {
                    if (firstRowDate == secondRowDate)
                        break;
                    firstRowDate = firstRowDate.AddMinutes(intervalPeriod);
                    if (!firstRowDate.Equals(secondRowDate))
                    {
                        row = table.NewRow();
                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                        row[0] = val;
                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                            row[j] = "-1";
                        table.Rows.Add(row);
                    }
                    else
                        flag = false;
                } while (flag);
            }
            //DataView sortView = table.DefaultView;  
            //sortView.AllowEdit = true; 
            //sortView.Sort = "Date Time DESC"; 

            //DataSet ds = new DataSet();
            //ds.Tables.Add(sortView.Table);
            //return ds;
            DataSet ds = new DataSet();
            ds.Tables.Add( table);
            return ds;
        }

        // this function was derived from ConvertLoadSurvey() for displaying padded data for tabular display
        public static DataSet ConvertLoadSurveyTabular(DataSet dataSet, string dataType, long fromDate, long toDate)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            if (dataType.Equals("Energy"))
                dataSet = ConvertDemandToEnergy(dataSet);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Date Time", typeof(System.String)));
            int counter = 0;
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                if (counter != 0)
                    table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
                counter++;
            }
            int intervalPeriod = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]);
            DataRow secondRow = null;

            DateTime fDate = DateUtility.LongToDateTime(fromDate);
            DateTime lDate = DateUtility.LongToDateTime(toDate);
            counter = 0;
            DataRow firstRow = dataSet.Tables[0].Rows[counter];
            DataRow row = table.NewRow();
            string val = "";

            firstRow = dataSet.Tables[0].Rows[0];
            DateTime firstRowDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow["LoadSurveyDateTime"]));

            if (firstRowDate.Month == fDate.Month)
            {
                if (firstRowDate.Day > fDate.Day)
                {
                    while (DateUtility.DateTimeToLong(firstRowDate) > DateUtility.DateTimeToLong(fDate))
                    {
                        row = table.NewRow();
                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(fDate).ToString());
                        row[0] = val;
                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                            row[j] = "-1";
                        table.Rows.Add(row);
                        fDate = fDate.AddMinutes(intervalPeriod);
                    }
                }
            }

            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                firstRow = dataSet.Tables[0].Rows[counter];
                if ((counter + 1) < dataSet.Tables[0].Rows.Count)
                    secondRow = dataSet.Tables[0].Rows[counter + 1];
                else
                    secondRow = firstRow;

                firstRowDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow["LoadSurveyDateTime"]));
                DateTime secondRowDate = DateUtility.LongToDateTime(Convert.ToInt64(secondRow["LoadSurveyDateTime"]));

                row = table.NewRow();
                val = SplitWithOutDateUnit(firstRow[0].ToString());
                row[0] = val;
                for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    string valuesofCell = Convert.ToString(firstRow[i]);
                    if (string.IsNullOrEmpty(valuesofCell))
                        valuesofCell = "0";
                    row[i] = valuesofCell;
                }
                table.Rows.Add(row);
                bool flag = true;
                do
                {
                    if (firstRowDate == secondRowDate)
                        break;
                    firstRowDate = firstRowDate.AddMinutes(intervalPeriod);
                    if (!firstRowDate.Equals(secondRowDate))
                    {
                        row = table.NewRow();
                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                        row[0] = val;
                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                            row[j] = "-1";
                        table.Rows.Add(row);
                    }
                    else
                        flag = false;
                } while (flag);
            }


            DataRow lastRow = dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count - 1];
            DateTime lastRowDate = DateUtility.LongToDateTime(Convert.ToInt64(lastRow["LoadSurveyDateTime"]));

            if (lastRowDate.Month == lDate.Month)
            {
                if (lastRowDate.Day < lDate.Day)
                {
                    while (DateUtility.DateTimeToLong(lastRowDate) < DateUtility.DateTimeToLong(lDate))
                    {
                        lastRowDate = lastRowDate.AddMinutes(intervalPeriod);
                        row = table.NewRow();
                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(lastRowDate).ToString());
                        row[0] = val;
                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                            row[j] = "-1";
                        table.Rows.Add(row);
                    }
                }
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ds;
        }

        public static DataSet ConvertLoadSurvey(DataSet dataSet, string dataType)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            if (dataType.Equals("Energy"))
                dataSet = ConvertDemandToEnergy(dataSet);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Date Time", typeof(System.String)));
            int counter = 0;
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                if (counter != 0) 
                    table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String))); 
                counter++;
            }
            int intervalPeriod = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MDIntervalPeriod"]);
            DataRow secondRow = null;
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                DataRow firstRow = dataSet.Tables[0].Rows[counter];
                if ((counter + 1) < dataSet.Tables[0].Rows.Count)
                    secondRow = dataSet.Tables[0].Rows[counter + 1];
                else
                    secondRow = firstRow;

                DateTime firstRowDate = DateUtility.LongToDateTime(Convert.ToInt64(firstRow["LoadSurveyDateTime"]));
                DateTime secondRowDate = DateUtility.LongToDateTime(Convert.ToInt64(secondRow["LoadSurveyDateTime"]));

                DataRow row = table.NewRow();
                string val = SplitWithOutDateUnit(firstRow[0].ToString());
                row[0] = val;
                for (int i = 1; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    string valuesofCell=Convert.ToString(firstRow[i]);
                    if(string.IsNullOrEmpty(valuesofCell))
                       valuesofCell="0";
                    row[i] = valuesofCell;
                }
                table.Rows.Add(row);
                bool flag = true;
                do
                {
                    if (firstRowDate == secondRowDate)
                        break;
                    firstRowDate = firstRowDate.AddMinutes(intervalPeriod);
                    if (!firstRowDate.Equals(secondRowDate))
                    {
                        row = table.NewRow();
                        val = SplitWithOutDateUnit(DateUtility.DateTimeToLong(firstRowDate).ToString());
                        row[0] = val;
                        for (int j = 1; j < dataSet.Tables[0].Columns.Count; j++)
                            row[j] = "-1";
                        table.Rows.Add(row);
                    }
                    else
                        flag = false;
                } while (flag);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(table);
            return ds;
        }
        public static int TotalTamperCounter(DataSet ds)
        { 
            int i = 0;
            foreach (DataRow tmpRow in ds.Tables[0].Rows)
            {
                if (i == ds.Tables[0].Rows.Count - 1) 
                    return Int32.Parse(tmpRow[2].ToString());
                i++;
            }
            return 0;
        }
        public static DataSet TamperCounter(DataSet ds)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Data", typeof(System.String)));
            table.Columns.Add(new DataColumn("Tamper", typeof(System.String)));
            table.Columns.Add(new DataColumn("Status", typeof(System.String)));
            int i = 0;
            foreach (DataRow tmpRow in ds.Tables[0].Rows)
            {
                if (i == ds.Tables[0].Rows.Count - 1)
                    break;
                i++;
                DataRow row = table.NewRow();
                row[0] = tmpRow[0].ToString();
                row[1] = tmpRow[1].ToString();
                int val = Int32.Parse(tmpRow[2].ToString());
                if (val == 0)
                    row[2] = "Absent";
                else
                    row[2] = "Present";
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet TamperCounter(int meterDataId)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Data", typeof(System.String)));
            table.Columns.Add(new DataColumn("Tamper", typeof(System.String)));
            table.Columns.Add(new DataColumn("Counter", typeof(System.String)));
            TamperGeneralBLL tamperBLL = new TamperGeneralBLL();
            TamperCounterBLL counterBLL = new TamperCounterBLL();
            TamperTypeBLL tamperTypeBLL = new TamperTypeBLL();
            DataSet dataTmp = tamperTypeBLL.ExistOrInsert();
            foreach (DataRow tmpRow in dataTmp.Tables[0].Rows)
            {
                DataRow row = table.NewRow();
                string val = tmpRow[1].ToString();
                if (val.Equals("Total Tamper"))
                    row[1] = val;
                else
                    row[1] = val.Replace("Tamper", "").Trim();
                val = val.Replace(" ", "");
                val = val.Replace("/", "");
                val = val + "Counter";
                row[0] = tmpRow[0].ToString(); //meterDataId.ToString();
                if (Convert.ToInt32(tmpRow[0]) < 225)
                    row[2] = tamperBLL.GetTamperCount(meterDataId, val);
                else
                    row[2] = counterBLL.GetTamperCount(meterDataId, val);
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet GetTamperOccurRestoreDetail(long shnapshotId, long meterDataID)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Description", typeof(System.String)));
            table.Columns.Add(new DataColumn("Occurrence Value", typeof(System.String)));
            table.Columns.Add(new DataColumn("Restoration Value", typeof(System.String)));
            table.Columns.Add(new DataColumn("TamperCode", typeof(System.String)));
            TamperSnapShotBLL tamperSnapShotBLL = new TamperSnapShotBLL();
            DataSet dataSet = tamperSnapShotBLL.DetailData(shnapshotId, meterDataID);
            if (dataSet == null)
                return null;
            if (dataSet.Tables[0].Rows.Count == 0)
                return null;
            DataRow row = dataSet.Tables[0].Rows[0];

            DataRow tableRow = table.NewRow();
            tableRow[0] = "R Voltage";
            tableRow[1] = Convert.ToString(row[0]);
            tableRow[2] = Convert.ToString(row[12]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Y Voltage";
            tableRow[1] = Convert.ToString(row[1]);
            tableRow[2] = Convert.ToString(row[13]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "B Voltage";
            tableRow[1] = Convert.ToString(row[2]);
            tableRow[2] = Convert.ToString(row[14]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "R Current";
            tableRow[2] = Convert.ToString(row[15]);
            string val = Convert.ToString(row[3]);
           
            //if (!Convert.ToString(row[24]).Trim().Equals("198"))
            //{
            //    if (val.IndexOf("-") < 0)
            //        val = "-" + val;
            //    val = val.Replace('-', ' ').Trim();
            //}
             tableRow[1]=val;
             
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Y Current";
            tableRow[2] = Convert.ToString(row[16]);
            val = Convert.ToString(row[4]);

            //if (!Convert.ToString(row[24]).Trim().Equals("199"))
            //{
            //    if (val.IndexOf("-") < 0)
            //        val = "-" + val;
            //    val = val.Replace('-', ' ').Trim();
            //}
            tableRow[1] = val;
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "B Current";
            tableRow[2] = Convert.ToString(row[17]);
            val = Convert.ToString(row[5]);

            //if (!Convert.ToString(row[24]).Trim().Equals("200"))
            //{
            //    if (val.IndexOf("-") < 0)
            //        val = "-" + val;
            //    val = val.Replace('-', ' ').Trim();
            //}
            tableRow[1] = val;
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "R PF";
            tableRow[1] = Convert.ToString(row[6]);
            tableRow[2] = Convert.ToString(row[18]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Y PF";
            tableRow[1] = Convert.ToString(row[7]);
            tableRow[2] = Convert.ToString(row[19]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "B PF";
            tableRow[1] = Convert.ToString(row[8]);
            tableRow[2] = Convert.ToString(row[20]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "Total PF";
            tableRow[1] = Convert.ToString(row[9]);
            tableRow[2] = Convert.ToString(row[21]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "kWh";
            tableRow[1] = Convert.ToString(row[10]);
            tableRow[2] = Convert.ToString(row[22]);
            table.Rows.Add(tableRow);

            tableRow = table.NewRow();
            tableRow[0] = "kVAh";
            tableRow[1] = Convert.ToString(row[11]);
            tableRow[2] = Convert.ToString(row[23]);
            table.Rows.Add(tableRow);
            foreach (DataRow dr in table.Rows)
                dr[3] = Convert.ToString(row[24]);
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertTamperOccurRestore(long meterDataId,long tamperId)
        {
            TamperSnapShotBLL tamperSnapShotBLL = new TamperSnapShotBLL();
            DataSet dataSet = tamperSnapShotBLL.ListData(meterDataId, tamperId);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Sl.", typeof(System.Int16)));
            foreach (DataColumn col in dataSet.Tables[0].Columns)
                table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
            table.Columns.Add(new DataColumn("Duration (Days:HH:MM)", typeof(System.String)));
            DataRow row;
            int cc = 1;
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                row = table.NewRow();
                row[0] = cc; cc++;
                row[1] = Convert.ToString(dr[0]);
                string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[1]));
                row[2] = val.Trim();

                if (val.IndexOf(":") > 0)
                    val = (val.Substring(val.IndexOf(":") - 3, 7).Replace(" ", "")).Replace(":", "");
                else
                    val = "0";
                string val1 = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[2]));
                row[3] = val1.Trim();
                if (val1.IndexOf(":") > 0)
                    val1 = (val1.Substring(val1.IndexOf(":") - 3, 7).Replace(" ", "")).Replace(":", "");
                else
                    val1 = "0";
                row[4] = Convert.ToInt32(dr[3]);
                if (val1 != "0")
                {
                    DateTime fromDateTime = DateUtility.LongToDateTime(Convert.ToInt64(dr[1]));
                    DateTime toDateTime = DateUtility.LongToDateTime(Convert.ToInt64(dr[2]));
                    TimeSpan timeSpan = toDateTime - fromDateTime;
                    string days = timeSpan.Days.ToString();
                    string hour = timeSpan.Hours.ToString();
                    string minute = timeSpan.Minutes.ToString();
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
                    if (days.Length == 1)
                        days = "0" + days;
                    row[5] = string.Concat(days, ":", hour, ":", minute);
                }
                else
                    row[5] = "-----";
                table.Rows.Add(row);
            }
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet ConvertTamperOccurRestore(long meterDataId)
        {
            TamperSnapShotBLL tamperSnapShotBLL = new TamperSnapShotBLL();
            DataSet dataSet = tamperSnapShotBLL.ListData(meterDataId);
            DataTable table = new DataTable();
            foreach (DataColumn col in dataSet.Tables[0].Columns)
                table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
            table.Columns.Add(new DataColumn("Duration (Days:HH:MM)", typeof(System.String)));
            DataRow row;
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                row = table.NewRow();
                row[0] = Convert.ToString(dr[0]);
                string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[1]));
                row[1] = val.Trim();
                   
                if (val.IndexOf(":") > 0)
                     val = (val.Substring(val.IndexOf(":") - 3, 7).Replace(" ", "")).Replace(":", "");
                else
                    val = "0";
                string val1 = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[2]));
                row[2] = val1.Trim();
                if (val1.IndexOf(":") > 0) 
                    val1 = (val1.Substring(val1.IndexOf(":") - 3, 7).Replace(" ", "")).Replace(":", "");
                else
                    val1 = "0";
                row[3] = Convert.ToInt32(dr[3]);
                if (val1 != "0")
                {
                   DateTime fromDateTime =DateUtility.LongToDateTime( Convert.ToInt64(dr[1]));
                   DateTime toDateTime = DateUtility.LongToDateTime(Convert.ToInt64(dr[2]));
                   TimeSpan timeSpan = toDateTime - fromDateTime;
                   string days = timeSpan.Days.ToString();
                   string hour = timeSpan.Hours.ToString();
                   string minute = timeSpan.Minutes.ToString(); 
                    if (Convert.ToInt32(hour) <= 0)
                        hour = "00";
                    if (Convert.ToInt32(minute) <= 0)
                        minute = "00";
                    if (Convert.ToInt32(days) <= 0)
                        days = "00";
                    if (hour.Length==1)
                        hour = "0" + hour;
                    if (minute.Length == 1)
                        minute = "0" + minute;
                    if (days.Length == 1)
                        days = "0" + days;
                    row[4] = string.Concat(days, ":", hour, ":", minute);
                }
                else
                    row[4] = "-----";
                table.Rows.Add(row);
            }
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        // Added for TNEB
        public static DataSet ConvertTamperOccurRestoreTNEB(DataSet ds)
        {

            DataTable table = new DataTable();
            foreach (DataColumn col in ds.Tables[0].Columns)
                table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));
            table.Columns.Add(new DataColumn("Duration (Days:HH:MM)", typeof(System.String)));
            DataRow row;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                row[0] = Convert.ToString(dr[0]);
                //row[1] = Convert.ToString(dr[1]);
                //row[2] = Convert.ToString(dr[2]);
                string val = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[1]));
                row[1] = val.Trim();

                if (val.IndexOf(":") > 0)
                    val = (val.Substring(val.IndexOf(":") - 3, 7).Replace(" ", "")).Replace(":", "");
                else
                    val = "0";
                string val1 = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[2]));
                row[2] = val1.Trim();
                if (val1.IndexOf(":") > 0)
                    val1 = (val1.Substring(val1.IndexOf(":") - 3, 7).Replace(" ", "")).Replace(":", "");
                else
                    val1 = "0";
                for (int i = 3; i <= 26; i++)
                    row[i] = Convert.ToString(dr[i]);
                if (val1 != "0")
                {
                    DateTime fromDateTime = DateUtility.LongToDateTime(Convert.ToInt64(dr[1]));
                    DateTime toDateTime = DateUtility.LongToDateTime(Convert.ToInt64(dr[2]));
                    TimeSpan timeSpan = toDateTime - fromDateTime;
                    string days = timeSpan.Days.ToString();
                    string hour = timeSpan.Hours.ToString();
                    string minute = timeSpan.Minutes.ToString();
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
                    if (days.Length == 1)
                        days = "0" + days;
                    row[27] = string.Concat(days, ":", hour, ":", minute);
                }
                else
                    row[27] = "-----";
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public DataTable AutoNumberedTable(DataTable SourceTable)
        {
            DataTable ResultTable = new DataTable();
            DataColumn AutoNumberColumn = new DataColumn();
            AutoNumberColumn.ColumnName = SNo;
            AutoNumberColumn.DataType = typeof(int);
            AutoNumberColumn.AutoIncrement = true;
            AutoNumberColumn.AutoIncrementSeed = 1;
            AutoNumberColumn.AutoIncrementStep = 1;
            ResultTable.Columns.Add(AutoNumberColumn);
            ResultTable.Merge(SourceTable);
            return ResultTable;
        }
        public static string GetFormattedData(string unformatttedData)
        {
            decimal result = 0;
            string tempString = string.Empty;
            string[] resultData = unformatttedData.Split('*');

            if (resultData.Length > 1)
                resultData[1] = GetUnit(resultData[1]);
            if (resultData[0].IndexOf(':') > 0)
            {
                string[] values = resultData[0].Split(':');
                if (decimal.TryParse(values[0], out result))
                    resultData[0] = decimal.Parse(values[0]).ToString() + ":" + values[1];
            }
            else
            {
                if (decimal.TryParse(resultData[0], out result))
                    resultData[0] = decimal.Parse(resultData[0]).ToString();
            }
            foreach (string sTemp in resultData)
                tempString += sTemp + " ";
            return tempString.Trim();
        }
        public static string RemoveUnit(string data)
        {
            if (data.IndexOf('*') > 0)
            {
                string[] resultData = data.Split('*', ' ');
                return resultData[0];
            }
            /*GKG 25/02/2013 : added case for Removing unit*/ 
            // GKG : This case may create problem for other data in which space
            // is expected.Right now all the cases where remove unit is referenced
            // dont have space in data.So be carefull!!!
            else if (data.IndexOf(' ') > 0)
            {
                return data.Substring(0, data.IndexOf(' '));
            }
            /*GKG 25/02/2013 : added case for Removing unit*/ 
            else
                return data;
        }

        public static string RemoveUnitForReport(string data)
        {
            if (data.Contains("*"))
            {
                return data.Substring(0, data.IndexOf('*'));//data.Split('*', ' ');
                //return resultData[0];
            }
            else
            {
                return data;
            }
        }
        public static List<string> GetColumns(string paramType)
        {
            Dictionary<string, string> columnDictionary = new Dictionary<string, string>();

            List<string> columns = new List<string>();

            if (paramType == "General")
                columnDictionary = new GeneralBLL().CreateGeneralDictionary();
            else if (paramType == "Instant")
                columnDictionary = new InstantPowerBLL().CreateInstantDictionary();
            else if (paramType == "Billing")
                columnDictionary = new BillingBLL().CreateBillingDictionary();
            else if (paramType == "LoadSurvey")
                columnDictionary = new LoadSurveyBLL().CreateLoadSurveyDictionary();
            else if (paramType == "TamperSnapshot")
                columnDictionary = new TamperSnapShotBLL().CreateSnapshotDictionary();
            else if (paramType == "PowerOnOffSnapShot")
                columnDictionary = new TamperSnapShotBLL().CreatePowerOnOffDictionary();


            foreach (KeyValuePair<string, string> kvp in columnDictionary)
                columns.Add(kvp.Key);

            return columns;
        }
        public static string GetFormattedHourData(string hourMinutes)
        {
            int result = 0;
            string tempString = string.Empty;
            string[] resultData = hourMinutes.Split(':');

            if (int.TryParse(resultData[0], out result))
                resultData[0] = int.Parse(resultData[0]).ToString("d2");
            else
                resultData[0] = "00";

            if (int.TryParse(resultData[1], out result))
                resultData[1] = int.Parse(resultData[1]).ToString("d2");
            else
                resultData[1] = "00";

            return string.Concat(resultData[0], ":", resultData[1]);
        }

        public static bool IsTimeColumn(string columnName)
        {
            bool isTimeColumn = false;
            switch (columnName)
            {
                case "ElapsedTimeKVA":
                    isTimeColumn = false;
                    break;
                case "ElapsedTimeKW":
                    isTimeColumn = false;
                    break;
                default:
                    if (columnName.ToLower().Contains("time") || columnName.ToLower().Contains("date"))
                        isTimeColumn = true;
                    else
                        isTimeColumn = false;
                    break;
            }
            return isTimeColumn;
        }
        public static DataSet ConvertMainEnergyConsumption(DataSet dataSet)
        {
            if (dataSet == null)
                return null;
            if (dataSet.Tables.Count <= 0)
                return null;
            if (dataSet.Tables[0].Rows.Count <= 0)
                return null;
            DataTable table = new DataTable();                               
            foreach (DataColumn col in dataSet.Tables[0].Columns)
                table.Columns.Add(new DataColumn(col.ColumnName, typeof(System.String)));        
            DataRow row;     
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                row = table.NewRow();
                for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    row[i] = RemoveUnit(Convert.ToString(dr[i]));
                }
                table.Rows.Add(row);
            }
            dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }        

    }
}