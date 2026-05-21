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
using System.Text;

namespace CAB.IECFramework.Utility
{
    /// <summary>
    /// This class is used to manage the date & time for diffrent -different Database.
    /// </summary>
    public class DateUtility
    {
        /// <summary>
        /// This method is used to convert the Date time to long integer.
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>long</returns>
        public static long DateTimeToLong(DateTime value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, ConvertToDigit(value.Hour));
            number = string.Concat(number, ConvertToDigit(value.Minute));
            number = string.Concat(number, ConvertToDigit(value.Second));
            return Convert.ToInt64(number);
        }
        /// <summary>
        /// This method is used to convert the Date time to long integer in ddmmyyyymmhh format.
        /// This is required for RTC update mapping to IEC. 
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>long</returns>
        public static string DateTimeToLongInDDMMYYYYMMHH(DateTime value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Year).Substring(2, 2));
            number = string.Concat(number, ConvertToDigit(value.Hour));
            number = string.Concat(number, ConvertToDigit(value.Minute));
            return number;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isStartingDate"></param>
        /// <returns></returns>
        public static long DateTimeToLong(DateTime value, bool isStartingDate)
        {
            string number = string.Empty;
            string hour = string.Empty;
            string minute = string.Empty;
            string second = string.Empty;
            if (isStartingDate)
            {
                hour = "00";
                minute = "00";
                second = "00";
            }
            else
            {
                hour = "23";
                minute = "59";
                second = "59";
            }
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, hour);
            number = string.Concat(number, minute);
            number = string.Concat(number, second);
            return Convert.ToInt64(number);
        }

        public static long LoadSurveyDateTimeToLong(DateTime value, bool isStartingDate)
        {
            string number = string.Empty;
            string hour = string.Empty;
            string minute = string.Empty;
            string second = string.Empty;
            if (isStartingDate)
            {
                hour = "00";
                minute = "00";
                second = "00";
            }
            else
            {
                hour = "23";
                minute = "30";
                second = "00";
            }
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, hour);
            number = string.Concat(number, minute);
            number = string.Concat(number, second);
            return Convert.ToInt64(number);
        }
        public static long DateToLong(DateTime value)
        {
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, "00");
            number = string.Concat(number, "00");
            number = string.Concat(number, "00");
            return Convert.ToInt64(number);
        }
        /// <summary>
        /// This method is used to convert the DateTime for start time(000000 hours) and last time(235959 hours)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startEndDateValue"></param>
        /// <returns></returns>
        public static long ConvertSearchDateTimeToLong(long date, string timeString)
        {
            DateTime value = LongToDateTime(date);
            string number = string.Empty;
            number = string.Concat(number, ConvertToDigit(value.Year));
            number = string.Concat(number, ConvertToDigit(value.Month));
            number = string.Concat(number, ConvertToDigit(value.Day));
            number = string.Concat(number, timeString);
            return Convert.ToInt64(number);
        }

        /// <summary>
        /// This method is used to convert the long integer value to datetime.
        /// </summary>
        /// <param name="value">long</param>
        /// <returns>DateTime</returns>
        public static DateTime LongToDateTime(long value)
        {
            try
            {
                string number = value.ToString();
                if (number.Length < 14)
                    number = number + "00";
                DateTime dateTime = new DateTime(
                 Convert.ToInt32(number.Substring(0, 4)),
                 Convert.ToInt32(number.Substring(4, 2)),
                 Convert.ToInt32(number.Substring(6, 2)),
                 Convert.ToInt32(number.Substring(8, 2)),
                 Convert.ToInt32(number.Substring(10, 2)),
                 Convert.ToInt32(number.Substring(12, 2)));
                return dateTime;
            }
            catch (Exception)
            {
                return System.DateTime.Now;
            }
        }
        public static string LongToStringDateFormat(long value)
        {
            string data = "---------";
            if (value == 0)
                return data;
            string dmeter;
            string[] sequence;
            string format = ConfigInfo.DateFormat();
            if (format.IndexOf('-') > 0)
            {
                sequence = format.Split('-');
                dmeter = "-";
            }
            else
            {
                dmeter = "/";
                sequence = format.Split('/');
            }
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            if (year1.Equals("1900"))
                return data;
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            string hour = "0";
            string minute = "0";
            string second = "0";
            if (number.Length >= 10)
                hour = number.Substring(8, 2).Trim();
            if (number.Length >= 12)
                minute = number.Substring(10, 2).Trim();
            if (number.Length >= 14)
                second = number.Substring(12, 2).Trim();
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            second = (second.Length == 1) ? string.Concat("0", second) : second;
            if (month1 != "00" && day1 != "00")
            {
                if (sequence[0].ToLower().Equals("dd") && sequence[1].ToLower().Equals("mm") && sequence[2].ToLower().Equals("yyyy"))
                    data = string.Concat(day1, dmeter, month1, dmeter, year1);//data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, " : ", minute);
                if (sequence[0].ToLower().Equals("mm") && sequence[1].ToLower().Equals("dd") && sequence[2].ToLower().Equals("yyyy"))
                    data = string.Concat(month1, dmeter, day1, dmeter, year1, " ", hour, " : ", minute);
            }
            return data;
        }
        public static string LongToStringDateTimeWithSecFormat(long value)
        {
            string data = "---------";
            if (value == 0)
                return data;
            string dmeter;
            string[] sequence;
            string format = ConfigInfo.DateFormat();
            if (format.IndexOf('-') > 0)
            {
                sequence = format.Split('-');
                dmeter = "-";
            }
            else
            {
                dmeter = "/";
                sequence = format.Split('/');
            }
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            if (year1.Equals("1900"))
                return data;
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            string hour = "0";
            string minute = "0";
            string second = "0";
            if (number.Length >= 10)
                hour = number.Substring(8, 2).Trim();
            if (number.Length >= 12)
                minute = number.Substring(10, 2).Trim();
            if (number.Length >= 14)
                second = number.Substring(12, 2).Trim();
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            second = (second.Length == 1) ? string.Concat("0", second) : second;
            if (month1 != "00" && day1 != "00")
            {
                if (sequence[0].ToLower().Equals("dd") && sequence[1].ToLower().Equals("mm") && sequence[2].ToLower().Equals("yyyy"))
                    data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, ":", minute, ":", second);
                if (sequence[0].ToLower().Equals("mm") && sequence[1].ToLower().Equals("dd") && sequence[2].ToLower().Equals("yyyy"))
                    data = string.Concat(month1, dmeter, day1, dmeter, year1, " ", hour, ":", minute, ":", second);
            }
            return data;
        }
        public static string LongToStringDateTimeFormat(long value)
        {
            string data = "---------";
            try
            {

                if (value == 0)
                    return data;
                string dmeter;
                string[] sequence;
                string format = ConfigInfo.DateFormat();
                if (format.IndexOf('-') > 0)
                {
                    sequence = format.Split('-');
                    dmeter = "-";
                }
                else
                {
                    dmeter = "/";
                    sequence = format.Split('/');
                }
                string number = value.ToString();
                string year1 = number.Substring(0, 4).Trim();
                if (year1.Equals("1900"))
                    return data;
                string month1 = number.Substring(4, 2).Trim();
                string day1 = number.Substring(6, 2).Trim();
                string hour = "0";
                string minute = "0";
                string second = "0";
                if (number.Length >= 10)
                    hour = number.Substring(8, 2).Trim();
                if (number.Length >= 12)
                    minute = number.Substring(10, 2).Trim();
                if (number.Length >= 14)
                    second = number.Substring(12, 2).Trim();
                year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
                month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
                day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
                hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
                minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
                second = (second.Length == 1) ? string.Concat("0", second) : second;
                if (month1 != "00" && day1 != "00")
                {
                    if (sequence[0].ToLower().Equals("dd") && sequence[1].ToLower().Equals("mm") && sequence[2].ToLower().Equals("yyyy"))
                        data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, " : ", minute);
                    if (sequence[0].ToLower().Equals("mm") && sequence[1].ToLower().Equals("dd") && sequence[2].ToLower().Equals("yyyy"))
                        data = string.Concat(month1, dmeter, day1, dmeter, year1, " ", hour, " : ", minute);
                }
            }
            catch (Exception)
            {
                return String.Empty;
            }
            return data;
        }
        public static string StringToLongDateTimeFormat(string value)
        {
            string data = "0";
            if (value == "0")
                return data;
            string year1 = value.Substring(0, 2).Trim();
            string month1 = value.Substring(3, 2).Trim();
            string day1 = value.Substring(6, 2).Trim();
            string hour = "0";
            string minute = "0";
            string second = "0";
            if (value.Length == 10)
                hour = value.Substring(8, 2).Trim();
            if (value.Length == 12)
                minute = value.Substring(10, 2).Trim();
            if (value.Length == 14)
                second = value.Substring(12, 2).Trim();
            year1 = (year1.Length == 2) ? string.Concat("20", year1) : year1;
            month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
            day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            second = (second.Length == 1) ? string.Concat("0", second) : second;
            if (month1 != "00" && day1 != "00")
                data = string.Concat(year1, month1, day1, hour, minute, second);
            return data;
        }
        public static long DateToLong(string value, bool IsStartTime)
        {
            string startTime = "";
            if (IsStartTime)
                startTime = "000000";
            else
                startTime = "235959";
            if (value.IndexOf('-') > 0)
                value = value.Replace('-', '/');
            string[] number = value.Split('/');
            string longDate = "";
            string formate = ConfigInfo.DateFormat().ToUpper();
            if (formate.IndexOf('-') > 0)
                formate = formate.Replace('-', '/');
            if (formate.Equals("MM/DD/YYYY"))
                longDate = string.Concat(number[2], number[0], number[1], startTime);
            else
                longDate = string.Concat(number[2], number[1], number[0], startTime);
            return Convert.ToInt64(longDate);
        }

        /// <summary>
        /// This method is used to convert the single digit number to double digit.
        /// </summary>
        /// <param name="value">int</param>
        /// <returnsstring></returns>
        private static string ConvertToDigit(int value)
        {
            string number = string.Empty;
            if (value <= 9)
                number = string.Concat("0", value.ToString());
            else
                number = value.ToString();
            return number;
        }

        public static DateTime ConvertIntToDate(int day, int month, int year)
        {
            DateTime dt = new DateTime(year, month, day);
            return dt;
        }

        public static string GetFormatedDateTme(string data)
        {
            string result = string.Empty;
            try
            {
                string[] value = data.Split(';');
                string date = value[1].Replace('-', '/');
                string time = value[0];
                double minutes = Convert.ToDouble(time.Split(':')[0]) * 60;
                double totalMinutes = (minutes + Convert.ToDouble(time.Split(':')[1]));
                TimeSpan span = TimeSpan.FromMinutes(totalMinutes);
                result = string.Concat(date, " ", string.Concat((span.Hours.ToString("00")), ":", span.Minutes.ToString("00"), ":", span.Seconds.ToString("00")));
            }
            catch
            { }
            return result;
        }
        public static string GetFormatedDateTme1(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;
            string result = string.Empty;
            try
            {
                string[] value = data.Split(';');
                string[] date = value[1].Split('-');//.Replace('-', '/');
                string time = value[0];
                double minutes = Convert.ToDouble(time.Split(':')[0]) * 60;
                double totalMinutes = (minutes + Convert.ToDouble(time.Split(':')[1]));
                TimeSpan span = TimeSpan.FromMinutes(totalMinutes);
                result = string.Concat(date[0], "/", date[1], "/", string.Concat(20, date[2]), " ", string.Concat((span.Hours.ToString("00")), ":", span.Minutes.ToString("00")));
            }
            catch
            { }
            return result;
        }
        public static string GetFormatedDateTmeForSPhase(string data)
        {
            string result = string.Empty;
            try
            {
                string[] value = data.Split(';');
                string[] date = value[1].Split('-');
                string[] time = value[0].Split(':');
                //TimeSpan span = TimeSpan.FromHours(Convert.ToDouble(time[0]));
                int temp;
                //Condition for valid dateTime parsing
                if (int.TryParse(date[2],out temp) && int.TryParse(date[1],out temp) && int.TryParse(date[0],out temp) && int.TryParse(time[1],out temp) && int.TryParse(time[0],out temp))
                {
                    result = string.Concat(string.Concat("20", date[2]), date[1], date[0], time[0], time[1], "00");
                }
                else
                {
                    result = string.Concat(string.Concat("20000000000000"));
                }
            }
            catch
            { }
            return result;
        }

        /// <summary>
        /// This method is used to get the Restortation Data time 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetFormatedDateTmeForSPhaseRestoration(string data)
        {
            string result = string.Empty;
            try
            {
                string[] value = data.Split(';');
                string[] date = value[3].Split('-');
                string[] time = value[2].Split(':');
                int temp;
                //Condition for valid dateTime parsing
                if (int.TryParse(date[2], out temp) && int.TryParse(date[1], out temp) && int.TryParse(date[0], out temp) && int.TryParse(time[1], out temp) && int.TryParse(time[0], out temp))
                {
                    result = string.Concat(string.Concat("20", date[2]), date[1], date[0], time[0], time[1], "00");
                }
                else
                {
                    result = string.Concat(string.Concat("20000000000000"));
                }
            }
            catch
            { }
            return result;
        }

        /// <summary>
        /// This method is used to remove the string (alphabet) from the data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetFormatedDaysHrsMinutes(string data)
        {
            string result = string.Empty;
            try
            {
                foreach (char item in data)
                {
                    if ((item >= 'A' && item <= 'Z') || (item >= 'a' && item <= 'z'))
                    {
                        // No need to add data
                    }
                    else
                    {
                        result = result + item;
                    }

                }
            }
            catch
            { }
            return result;
        }
    }
}
