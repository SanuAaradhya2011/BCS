using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using CAB.IECFramework.Utility;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Globalization;

namespace CAB.IECChannel.Programming
{
    public class ProgrammingCommon
    {
        public static string futureActivationDate;
        private const byte dayProfileCount = 1;
        private const byte weekProfileCount = 1;
        private const byte seasonProfileCount = 1;
        public static string GetASCIIValue(string inputString)
        {
            string tempStr = string.Empty;
            futureActivationDate = string.Empty;
            foreach (char ch in inputString)
            {
                tempStr += String.Format("{0:x2}", Convert.ToInt32(ch));
            }
            return tempStr;
        }

        public static DateTime GetDate(string dateValue, bool Flag)
        {
            DateTime dt = new DateTime();
            try
            {
                dt = DateTime.Parse(dateValue);
            }
            catch (Exception ex)
            {                
                throw;
            }
            return dt;
        }

        public static DateTime GetDate_Old(string dateValue, bool Flag)
        {
            DateTime dt = new DateTime();
            try
            {
                if (string.IsNullOrEmpty(dateValue))
                    return dt;
                if (dateValue.IndexOf(' ') > 0)
                {
                    string val = dateValue.Substring(0, dateValue.IndexOf(' '));
                    if (val.Length < 10)
                        Flag = false;
                }
                if (dateValue.Length == 10 && Flag == false)
                    Flag = true;
                if (Flag)
                {
                    string tDay, tMonth, tYear;
                    tDay = dateValue.Substring(0, 2);
                    tMonth = dateValue.Substring(3, 2);
                    tYear = dateValue.Substring(6, 4);
                    return new DateTime(Int32.Parse(tYear), Int32.Parse(tMonth), Int32.Parse(tDay));
                }
                else
                {
                    return Convert.ToDateTime(dateValue);
                }
            }
            catch (Exception ex)
            {                
                throw;
            }            
            return dt;
        }
        public static DateTime GetDateWithTime(string dateValue)
        {
            DateTime dt = new DateTime();
            try
            {
                if (string.IsNullOrEmpty(dateValue))
                    return dt;
                string tDay, tMonth, tYear, hr, min, sec;
                tDay = dateValue.Substring(0, 2);
                tMonth = dateValue.Substring(3, 2);
                tYear = "20" + dateValue.Substring(6, 2);
                hr = dateValue.Substring(9, 2);
                min = dateValue.Substring(12, 2);
                sec = dateValue.Substring(15, 2);
                dt = new DateTime(Int32.Parse(tYear), Int32.Parse(tMonth), Int32.Parse(tDay), Int32.Parse(hr), Int32.Parse(min), Int32.Parse(sec));
            }
            catch (Exception ex)
            {
                dt = new DateTime();
            }
            return dt;
        }
        public static string GetASCIIValue(DateTime selectedRTC)
        {
            DayOfWeek weekDay = selectedRTC.DayOfWeek;
            int tempDay = (int)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(DayOfWeek), weekDay));

            string dateStr = string.Empty;
            string tempStr = string.Empty;

            dateStr = string.Concat(selectedRTC.ToString("ssmmHH"), tempDay.ToString("d2"), selectedRTC.ToString("ddMMyy"));

            foreach (char ch in dateStr)
            {
                tempStr += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(ch));
            }
            return tempStr;
        }
        public static string GetASCIIValueSP(DateTime selectedRTC)
        {
            DayOfWeek weekDay = selectedRTC.DayOfWeek;
            int tempDay = (int)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(DayOfWeek), weekDay));
            if (tempDay == 0)
                tempDay = 6;
            else
                tempDay = tempDay - 1;
            string dateStr = string.Empty;
            string tempStr = string.Empty;

            dateStr = string.Concat(selectedRTC.ToString("ssmmHH"), selectedRTC.ToString("ddMMyy"),tempDay.ToString("d2"));

            foreach (char ch in dateStr)
            {
                tempStr += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(ch));
            }
            return tempStr;
        }
        #region DLMS TOU data
        /// <summary>
        /// Returns the dataset of tou dlms data 
        /// </summary>
        /// <param name="touData"></param>
        /// <param name="touType"></param>
        /// <param name="seasonTable"></param>
        /// <param name="holidayTable"></param>
        /// <param name="dayAssignmentTable"></param>
        /// <param name="activationDayTable"></param>
        /// <returns></returns>
        public DataSet GetTOUDLMSData(string touData, string touType, DataTable[,] seasonTable, DataTable[] holidayTable,
            DataTable[] dayAssignmentTable, DataTable activationDayTable)
        {
            DataSet ds = new DataSet();
            try
            {
                string[] data = touData.Split('\\');
                if (data != null && data.Length > 7)
                {
                    if (touType == "Future")
                    {
                        //Passive
                        FillSeasonProfileParameters(SoapHexBinary.Parse(data[1]).Value, dayAssignmentTable);
                        FillDayProfileParameters(SoapHexBinary.Parse(data[3]).Value, seasonTable);
                        FillHoliday(SoapHexBinary.Parse(data[3]).Value, holidayTable);
                    }
                    else
                    {
                        //Active
                        FillSeasonProfileParameters(SoapHexBinary.Parse(data[4]).Value, dayAssignmentTable);
                        FillDayProfileParameters(SoapHexBinary.Parse(data[6]).Value, seasonTable);
                        FillHoliday(SoapHexBinary.Parse(data[6]).Value, holidayTable);
                    }
                    FillDayAssignmentForDLMS(dayAssignmentTable);
                    FillActivationDate(SoapHexBinary.Parse(data[7]).Value, holidayTable, touType);
                    for (int i = 0; i < seasonTable.GetLength(0); i++)
                        for (int j = 0; j < seasonTable.GetLength(1); j++)
                            ds.Tables.Add(seasonTable[i, j]);
                    ds.Tables.AddRange(holidayTable);
                    ds.Tables.AddRange(dayAssignmentTable);
                    ds.Tables.Add(activationDayTable);
                }
            }
            catch
            {

            }
            return ds;
        }       
        /// <summary>
        /// Fills the activation date for DLMS
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="holidayTable"></param>
        /// <param name="touType"></param>
        private void FillActivationDate(byte[] buffer, DataTable[] holidayTable,string touType)
        {
            string holidayActivationDate = string.Empty;
            futureActivationDate = string.Empty;
            int nIndex = 0x02;
            int activationYear = 0;
            try
            {
                activationYear = (activationYear | (int)buffer[nIndex++]) << 8;
                activationYear = (activationYear | (int)buffer[nIndex++]);
                int activationMonth = buffer[nIndex++];
                int activationDay = buffer[nIndex];
                holidayActivationDate = Convert.ToDateTime(activationDay.ToString() + "/" + activationMonth.ToString() + "/"
                    + activationYear.ToString(), new CultureInfo("en-GB")).ToString("dd/MM/yyyy");                
            }
            catch (Exception ex)
            {
                holidayActivationDate = DateTime.MinValue.ToString("dd/MM/yyyy");
                throw ex;
            }
            for (int holidayIndex = 0; holidayIndex < holidayTable.Length; holidayIndex++)
            {
                foreach (DataRow row in holidayTable[holidayIndex].Rows)
                    row["Activation Date"] = holidayActivationDate;
            }
            if (touType == "Future")
            {
                futureActivationDate = holidayActivationDate;                 
            }
        }
        /// <summary>
        /// This method is used for filling season profile details in datatable array
        /// </summary>
        private void FillSeasonProfileParameters(byte[] buffer, DataTable[] dayAssignmentTable)
        {
            try
            {
                int nIndex = 0;
                for (int counter = 0; counter < dayAssignmentTable.Length; counter++)
                {
                    for (byte seasonCount = 0; seasonCount < dayAssignmentTable[counter].Rows.Count; seasonCount++)
                    {
                        nIndex = 6;
                        dayAssignmentTable[counter].Rows[seasonCount][1] = buffer[nIndex++].ToString("00");
                        nIndex += 4;
                        dayAssignmentTable[counter].Rows[seasonCount][1] = buffer[nIndex++].ToString("00");
                        dayAssignmentTable[counter].Rows[seasonCount][1] = buffer[nIndex++].ToString("00");
                        nIndex += 11;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Fills the holidays TOU data 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="holidayTable"></param>
        private void FillHoliday(byte[] buffer, DataTable[] holidayTable)
        {
            try
            {
                for (int rowIndex = 0; rowIndex < 10; rowIndex++)
                {

                    int nIndex = 2;
                    for (byte dayCount = 0; dayCount < dayProfileCount; dayCount++)
                    {
                        nIndex += 6;
                        for (byte rowCount = 0; rowCount < 10; rowCount++)
                        {
                            nIndex += 4;
                            string startHour = buffer[nIndex++].ToString("d2");
                            string startMin = buffer[nIndex++].ToString("d2");
                            nIndex += 12;
                            int tariff = buffer[nIndex++];
                            DataRow row = holidayTable[rowIndex].NewRow();
                            if (tariff == 0)
                            {
                                row[0] = rowCount + 1;
                                row[1] = "00";
                                row[2] = "00";
                                row[3] = "00";
                            }
                            else
                            {
                                row[0] = rowCount + 1;
                                row[2] = startHour;
                                row[3] = startMin;
                                row[1] = "T" + tariff.ToString();
                            }
                            holidayTable[rowIndex].Rows.Add(row);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       /// <summary>
        /// Fill day profile parameters of TOU data for DLMS
       /// </summary>
       /// <param name="buffer"></param>
       /// <param name="seasonTable"></param>
        public void FillDayProfileParameters(byte[] buffer, DataTable[,] seasonTable)
        {
            try
            {

                for (int rowIndex = 0; rowIndex < 4; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < 6; columnIndex++)
                    {
                        int nIndex = 2;
                        for (byte dayCount = 0; dayCount < dayProfileCount; dayCount++)
                        {
                            nIndex += 6;
                            for (byte rowCount = 0; rowCount < 10; rowCount++)
                            {
                                nIndex += 4;
                                string startHour = buffer[nIndex++].ToString("d2");
                                string startMin = buffer[nIndex++].ToString("d2");
                                nIndex += 12;
                                int tariff = buffer[nIndex++];
                                DataRow row = seasonTable[rowIndex, columnIndex].NewRow();
                                if (tariff == 0)
                                {
                                    row[0] = rowCount + 1;
                                    row[1] = "00";
                                    row[2] = "00";
                                    row[3] = "00";
                                }
                                else
                                {
                                    row[0] = rowCount + 1;
                                    row[2] = startHour;
                                    row[3] = startMin;
                                    row[1] = "T" + tariff.ToString();
                                }
                                seasonTable[rowIndex, columnIndex].Rows.Add(row);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Fills the day assignment tables for DLMS
        /// </summary>
        /// <param name="tempTable"></param>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private DataTable[] FillDayAssignmentForDLMS(DataTable[] dayAssignmentTable)
        {
            foreach (DataTable table in dayAssignmentTable)
            {
                for (int day = 0; day < 7; day++)
                {
                    table.Rows[day][1] = string.Concat("Day Table 1");
                }
            }
           return  dayAssignmentTable;
        }
        #endregion
        /// <summary>
        /// Fills the day assignment tables
        /// </summary>
        /// <param name="tempTable"></param>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private DataTable FillDayAssignment(DataTable tempTable, string data, int index)
        {
            for (int day = 0; day < 7; day++)
            {
                DataRow dataRow = tempTable.Rows[day];
                string dayTable = Convert.ToInt32(data.Substring(index, 2)).ToString();
                if (dayTable == "1" || dayTable == "2" || dayTable == "3" || dayTable == "4" || dayTable == "5" || dayTable == "6" || dayTable == "7")
                {
                    dataRow["Day Table"] = string.Concat("Day Table", " ", dayTable);
                }
                else
                {
                    dataRow["Day Table"] = string.Concat("Day Table 1");
                }
                index += 2;
            }
            return tempTable;
        }
        public static DataSet DisplayTOUData(string touData, string touType)
        {
            ProgrammingCommon programmingCommon = new ProgrammingCommon();
            DataSet ds = new DataSet();
            int index = 0;
            int rowIndex = 0;
            int colIndex = 0;
            int holidayIndex = 0;

            string data = string.Empty;

            List<string> TOUParameters = new List<string>();
            DataTable[,] seasonTable = new DataTable[4, 6];
            DataTable[] holidayTable = new DataTable[10];
            DataTable[] dayAssignmentTable = new DataTable[4];
            DataTable activationDayTable = new DataTable();
            try
            {
                //Set data tables
                programmingCommon.SetSeasonColumns(seasonTable);
                programmingCommon.SetHolidayColumns(holidayTable);
                programmingCommon.SetDayAssignmentColumns(dayAssignmentTable);
                programmingCommon.SetActivationDayColumns(activationDayTable);
                if (touData.StartsWith("DLMS"))
                {
                    ds = programmingCommon.GetTOUDLMSData(touData, touType, seasonTable, holidayTable,
                        dayAssignmentTable, activationDayTable);
                }
                else
                {
                    TOUParameters = programmingCommon.GetTOUParameters(touData);
                    colIndex = -1;
                    rowIndex = 0;
                    for (int paramCount = 0; paramCount < TOUParameters.Count; paramCount++)
                    {
                        index = 0;
                        data = TOUParameters[paramCount].Substring(1, TOUParameters[paramCount].Length - 2);
                        if (paramCount == 6 || paramCount == 13 || paramCount == 20 || paramCount == 27)
                        {
                            //fill day assigment tables
                            programmingCommon.FillDayAssignment(dayAssignmentTable[paramCount % 6], data, index);
                        }
                        else if (paramCount < TOUParameters.Count - 1)
                        {
                            //fill holiday and season tables
                            if (paramCount < 28)
                            {
                                if (colIndex == seasonTable.GetLength(1) - 1)
                                {
                                    colIndex = 0;
                                    rowIndex++;
                                }
                                else
                                {
                                    colIndex++;
                                }
                                index += 2;
                                programmingCommon.FillTouSlots(seasonTable[rowIndex, colIndex], data, index);
                            }
                            else
                            {
                                DateTime dt = new DateTime();
                                string holidayActivationDate = string.Empty;
                                if (DateTime.TryParse(string.Concat(data.Substring(4, 2), "/", data.Substring(2, 2), "/", data.Substring(0, 2)), new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out dt))
                                    holidayActivationDate = dt.ToString("dd/MM/yyyy");//(ConfigInfo.DateFormat());
                                else
                                    holidayActivationDate = DateTime.Now.ToString("dd/MM/yyyy");
                                //return null; //Invalid TOU
                                index += 8;
                                programmingCommon.FillTouSlots(holidayTable[holidayIndex], data, index);
                                foreach (DataRow row in holidayTable[holidayIndex].Rows)
                                    row["Activation Date"] = holidayActivationDate;
                                holidayIndex++;
                            }
                        }
                        else
                        {
                            //future TOU activation date
                            programmingCommon.FillFutureTou(activationDayTable, data, index, touType);
                        }
                    }
                    for (int i = 0; i < seasonTable.GetLength(0); i++)
                        for (int j = 0; j < seasonTable.GetLength(1); j++)
                            ds.Tables.Add(seasonTable[i, j]);
                    ds.Tables.AddRange(holidayTable);
                    ds.Tables.AddRange(dayAssignmentTable);
                    ds.Tables.Add(activationDayTable);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        private DataTable FillTouSlots(DataTable tempTable, string data, int index)
        {
            int serialNumber = 0;
            //index += 2;
            int slots = Convert.ToInt32(data.Substring(index, 2)); index += 2;
            try
            {
                while (slots != 0)
                {
                    DataRow dataRow = tempTable.NewRow();
                    int startHour = Convert.ToInt32(data.Substring(index, 2)); index += 2;  //4
                    int endHour = Convert.ToInt32(data.Substring(index, 2)); index += 2;    //6
                    int rate = Convert.ToInt32(data.Substring(index, 2)); index += 2;       //8
                    dataRow["S No"] = (++serialNumber).ToString();
                    dataRow["Rate"] = string.Concat("T", rate.ToString());
                    dataRow["Start Hour"] = String.Format("{0:00}", startHour);
                    dataRow["Start Minute"] = String.Format("{0:00}", endHour);
                    tempTable.Rows.Add(dataRow);
                    slots--;
                }

                while (tempTable.Rows.Count < 10)
                {
                    DataRow dataRow = tempTable.NewRow();
                    dataRow["S No"] = (++serialNumber).ToString();
                    dataRow["Rate"] = "00";
                    dataRow["Start Hour"] = "00";
                    dataRow["Start Minute"] = "00";
                    tempTable.Rows.Add(dataRow);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return tempTable;
        }

        private DataTable FillFutureTou(DataTable tempTable, string data, int index, string touType)
        {
            futureActivationDate = "";
            DateTime dt = new DateTime();
            if (touType == "Future")
            {
                if (DateTime.TryParse(string.Concat(data.Substring(index, 2), "/", data.Substring(index + 2, 2), "/", data.Substring(index + 4, 2)), new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out dt))
                {
                    futureActivationDate = dt.ToString("dd/MM/yyyy");// (ConfigInfo.DateFormat());
                }
                index += 6;
            }
            for (int i = 0; i < 4; i++)
            {
                DataRow row = tempTable.NewRow();
                if (DateTime.TryParse(string.Concat(data.Substring(index, 2), "/", data.Substring(index + 2, 2), "/", DateTime.Now.Year), new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out dt))
                    row["Season Activation Date"] = dt.ToString("dd/MM/yyyy");// (ConfigInfo.DateFormat());
                string seasonNo = data.Substring(index + 4, 2);
                if (seasonNo == "01" || seasonNo == "02" || seasonNo == "03" || seasonNo == "04")
                    row["Season Number"] = seasonNo;
                else
                    row["Season Number"] = "0" + Convert.ToString(i + 1);
                index += 6;
                tempTable.Rows.Add(row);
            }
            return tempTable;
        }

      


        private void SetSeasonColumns(DataTable[,] seasonTable)
        {
            string[] seasonColumn = GetSeasonColumns();
            for (int rowIndex = 0; rowIndex < seasonTable.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < seasonTable.GetLength(1); colIndex++)
                {
                    seasonTable[rowIndex, colIndex] = new DataTable();
                    for (int col = 0; col < seasonColumn.Length; col++)
                        seasonTable[rowIndex, colIndex].Columns.Add(new DataColumn(seasonColumn[col], typeof(System.String)));
                }
            }
        }
        private void SetHolidayColumns(DataTable[] holidayTable)
        {
            string[] holidayColumn = GetHolidayColumns();
            for (int rowIndex = 0; rowIndex < holidayTable.GetLength(0); rowIndex++)
            {
                holidayTable[rowIndex] = new DataTable();
                for (int col = 0; col < holidayColumn.Length; col++)
                    holidayTable[rowIndex].Columns.Add(new DataColumn(holidayColumn[col], typeof(System.String)));
            }
        }

        private void SetDayAssignmentColumns(DataTable[] dayAssignmentTable)
        {
            int rowIndex = 0;
            int col = 0;
            string[] dayAssignmentColumn = GetDayAssigmentColumns();
            string[] dayAssignmentRow = GetDayAssigmentRows();
            for (rowIndex = 0; rowIndex < dayAssignmentTable.GetLength(0); rowIndex++)
            {
                dayAssignmentTable[rowIndex] = new DataTable();
                for (col = 0; col < dayAssignmentColumn.Length; col++)
                    dayAssignmentTable[rowIndex].Columns.Add(new DataColumn(dayAssignmentColumn[col], typeof(System.String)));
            }

            for (rowIndex = 0; rowIndex < dayAssignmentTable.GetLength(0); rowIndex++)
            {
                for (int rowCount = 0; rowCount < dayAssignmentRow.Length; rowCount++)
                {
                    DataRow dataRow = dayAssignmentTable[rowIndex].NewRow();
                    for (col = 0; col < dayAssignmentColumn.Length; col++)
                    {
                        if (col == 0)
                            dataRow[col] = dayAssignmentRow[rowCount];
                        else
                            dataRow[col] = string.Empty;
                    }
                    dayAssignmentTable[rowIndex].Rows.Add(dataRow);
                }
            }
        }

        private void SetActivationDayColumns(DataTable activationDayTable)
        {
            string[] activationDayColumn = GetActivationDayColumns();
            for (int col = 0; col < activationDayColumn.Length; col++)
                activationDayTable.Columns.Add(new DataColumn(activationDayColumn[col], typeof(System.String)));
        }

        public List<string> GetTOUParameters(string touData)
        {
            List<string> touParameters = new List<string>();
            MatchCollection matches = Regex.Matches(touData, ValidationConstant.TOUExpression, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                touParameters.Add(groups[0].Value);
            }
            return touParameters;
        }

        private string[] GetSeasonColumns()
        {
            string[] tempArray = new string[4];
            tempArray[0] = "S No";
            tempArray[1] = "Rate";
            tempArray[2] = "Start Hour";
            tempArray[3] = "Start Minute";
            return tempArray;
        }

        private string[] GetHolidayColumns()
        {
            string[] tempArray = new string[5];
            tempArray[0] = "S No";
            tempArray[1] = "Rate";
            tempArray[2] = "Start Hour";
            tempArray[3] = "Start Minute";
            tempArray[4] = "Activation Date";
            return tempArray;
        }

        private string[] GetDayAssigmentColumns()
        {
            string[] tempArray = new string[2];
            tempArray[0] = "Day";
            tempArray[1] = "Day Table";
            return tempArray;
        }


        public string[] GetDayAssigmentRows()
        {
            string[] tempArray = new string[7];
            tempArray[0] = "Sunday";
            tempArray[1] = "Monday";
            tempArray[2] = "Tuesday";
            tempArray[3] = "Wednesday";
            tempArray[4] = "Thursday";
            tempArray[5] = "Friday";
            tempArray[6] = "Saturday";
            return tempArray;
        }

        private string[] GetActivationDayColumns()
        {
            string[] tempArray = new string[2];
            tempArray[0] = "Season Activation Date";
            tempArray[1] = "Season Number";
            return tempArray;
        }
    }
}
