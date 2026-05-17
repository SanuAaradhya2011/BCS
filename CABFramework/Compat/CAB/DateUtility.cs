/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Text;

namespace LNG.Framework.Utility
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
        //Added to add specific values on basis of type value- from date or to date.
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
            string number = value.ToString();
            bool isContains24 = false;

            try
            {
                if (number.Length < 14)
                    number = number + "00";
                string year = number.Substring(0, 4);
                if (year.Trim().ToUpper().Equals("9999"))
                    year = "0000";

                string month = number.Substring(4, 2);
                if (month.Trim().ToUpper().Equals("99"))
                    month = "00";

                string day = number.Substring(6, 2);
                if (day.Trim().ToUpper().Equals("99"))
                    day = "00";


                string hour = number.Substring(8, 2);
                if (hour.Trim().ToUpper().Equals("99"))
                    hour = "00";

                string minute = "00";
                if (number.Length >= 12)
                {
                    minute = number.Substring(10, 2);
                    if (minute.Trim().ToUpper().Equals("99"))
                    {
                        minute = "00";
                    }
                }

                string second = "00";
                if (number.Length >= 14)
                {
                    second = number.Substring(12, 2);
                    if (second.Trim().ToUpper().Equals("99"))
                    {
                        second = "00";
                    }
                }

                if (month == "00" || day == "00" || year == "0000")
                {
                    return Convert.ToDateTime("01/01/1900");
                }
                // Story - 349654 - Hours in time format is containing 24, which is not valid
                // if (hour == "24") // Story - 354382 - Time 24:00:00 is equivalant to 23:59:59 of same day
                if (hour == "24" && minute == "00" && second == "00")
                {
                    hour = "00";
                    isContains24 = true;
                }
                DateTime dateTime = new DateTime(
                 Convert.ToInt32(year),
                 Convert.ToInt32(month),
                 Convert.ToInt32(day),
                 Convert.ToInt32(hour),
                 Convert.ToInt32(minute),
                 Convert.ToInt32(second));
                //return dateTime;
                if (isContains24)
                {
                    DateTime dateTime1 = dateTime.AddDays(1);
                    return dateTime1;
                }
                else
                {
                    return dateTime;
                }
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }
        public static string LongToStringDateFormat(long value)
        {
            string dmeter;
            bool isDayFirst = false;
            string format = ConfigInfo.DateFormat();
            if (format.Substring(0, 2).ToUpper() == "DD")
                isDayFirst = true;
            if (format.IndexOf('-') > 0)
                dmeter = "-";
            else
                dmeter = "/";
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            year1 = (year1.ToString().Length == 2) ? string.Concat("20", year1.ToString()) : year1.ToString();
            month1 = (month1.ToString().Length == 1) ? string.Concat("0", month1.ToString()) : month1.ToString();
            day1 = (day1.ToString().Length == 1) ? string.Concat("0", day1.ToString()) : day1.ToString();
            return string.Concat(year1, dmeter, month1, dmeter, day1);
            //if (isDayFirst)
            //    return string.Concat(day1, dmeter, month1, dmeter, year1);
            //else
            //    return string.Concat(month1, dmeter, day1, dmeter, year1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LongToStringDate(long value)
        {
            string dmeter = "/";
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            year1 = (year1.ToString().Length == 2) ? string.Concat("20", year1.ToString()) : year1.ToString();
            month1 = (month1.ToString().Length == 1) ? string.Concat("0", month1.ToString()) : month1.ToString();
            day1 = (day1.ToString().Length == 1) ? string.Concat("0", day1.ToString()) : day1.ToString();
            return string.Concat(day1, dmeter, month1, dmeter, year1);
        }

        /// <summary>
        /// Convert the long value to date value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LongToStringMIOSDateFormat(long value)
        {
            string dmeter = "-"; ;
            bool isDayFirst = false;
            string format = "dd-MM-yyyy";
            if (format.Substring(0, 2).ToUpper() == "DD")
                isDayFirst = true;
            string number = value.ToString();
            string year1 = number.Substring(0, 4).Trim();
            string month1 = number.Substring(4, 2).Trim();
            string day1 = number.Substring(6, 2).Trim();
            year1 = (year1.ToString().Length == 2) ? string.Concat("20", year1.ToString()) : year1.ToString();
            month1 = (month1.ToString().Length == 1) ? string.Concat("0", month1.ToString()) : month1.ToString();
            day1 = (day1.ToString().Length == 1) ? string.Concat("0", day1.ToString()) : day1.ToString();
            return string.Concat(day1, dmeter, month1, dmeter, year1);
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
        public static string LongToMIOSStringDateTimeWithSecFormat(long value)
        {
            string data = "---------";
            if (value == 0 || value == 99)
                return data;
            string dmeter;
            string[] sequence;
            string format = "dd-MM-yyyy";
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
            //BhardwajG : 138702 NDPL : Wrong date in billing
            //Mark the wrong date to default value "---------"
            if (month1 != "00" && day1 != "00" && month1 != "99" && day1 != "99" & year1 != "9999")
            {
                if (sequence[0].ToLower().Equals("dd") && sequence[1].ToLower().Equals("mm") && sequence[2].ToLower().Equals("yyyy"))
                {
                    if (!second.Equals("99"))
                    {
                        data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, ":", minute, ":", second);
                    }
                    else
                    {
                        data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, ":", minute);
                    }
                }

                if (sequence[0].ToLower().Equals("mm") && sequence[1].ToLower().Equals("dd") && sequence[2].ToLower().Equals("yyyy"))
                    data = string.Concat(month1, dmeter, day1, dmeter, year1, " ", hour, ":", minute, ":", second);
            }
            return data;
        }

        /// <summary>
        ///  This method used to get WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTamperOccurDateTimeMinusFiveMinute(long value)
        {
            string resultString = string.Empty;
            long val = 0;
            string strDate = Convert.ToString(value);
            if (strDate.Length >= 14)
            {
                int year = Convert.ToInt32(strDate.Substring(0, 4));
                int month = Convert.ToInt32(strDate.Substring(4, 2));
                int day = Convert.ToInt32(strDate.Substring(6, 2));
                int hour = Convert.ToInt32(strDate.Substring(8, 2));
                int minute = Convert.ToInt32(strDate.Substring(10, 2));
                int seconds = Convert.ToInt32(strDate.Substring(12, 2));
                DateTime parsedDate = new DateTime(year, month, day, hour, minute, seconds);
                parsedDate = parsedDate.AddMinutes(-5);
                val = long.Parse(parsedDate.ToString("yyyyMMddHHmmss"));
            }
            return resultString = LongToStringDateTimeFormat(val);
        }


        public static string LongToStringDateTimeFormat(long value)
        {
            string data = "---------";
            if (value == 0)
                return data;
            if (value == 99999999999999)
                return data;
            if (value.ToString().Length < 10)
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
            return data;
        }
        public static string LongToStringMIOSDateTimeFormat(long value)
        {
            string data = "---------";
            if (value == 0)
                return data;
            if (value == 99999999999999)
                return data;
            if (value.ToString().Length < 10)
                return data;
            string dmeter = "-";
            string[] sequence;
            string format = "dd-MM-yyyy";
            if (format.IndexOf('-') > 0)
            {
                sequence = format.Split('-');

            }
            else
            {
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
                    data = string.Concat(day1, dmeter, month1, dmeter, year1, " ", hour, ":", minute);
                if (sequence[0].ToLower().Equals("mm") && sequence[1].ToLower().Equals("dd") && sequence[2].ToLower().Equals("yyyy"))
                    data = string.Concat(month1, dmeter, day1, dmeter, year1, " ", hour, ":", minute);
            }
            return data;
        }

        /// <summary>
        /// Converts date time to long format
        /// </summary>
        /// <param name="value"></param> input format "12/3/2013 6:18:38 PM"
        /// <returns></returns> 
        public static string StringToLongDateTimeFormatDLMS(string value)
        {

            StringBuilder result = new StringBuilder("");
            if (value != "0")
            {

                string[] fullDateTime = value.Split(' ');
                char separator = '/';
                if (fullDateTime.Length > 1)
                {
                    separator = fullDateTime[0].Contains("/") ? '/' : fullDateTime[0].Contains("-") ? '-' : fullDateTime[0].Contains(".") ? '.' : separator;
                    string[] dateComponent = fullDateTime[0].Split(separator);
                    string[] timeComponent = fullDateTime[1].Split(':');
                    string day = Convert.ToInt32(dateComponent[0]).ToString("D2");
                    string month = Convert.ToInt32(dateComponent[1]).ToString("D2");
                    string year = dateComponent[2];
                    string hour = Convert.ToInt32(timeComponent[0]).ToString("D2");
                    string minute = Convert.ToInt32(timeComponent[1]).ToString("D2");
                    string second = Convert.ToInt32(timeComponent[2]).ToString("D2");
                    result.Append(year).Append(month).Append(day).Append(hour).Append(minute).Append(second);
                    // NO billing so need to inser t0 in database 
                    if (result.ToString() == "00010101120000" || result.ToString() == "00010101000000")
                    {
                        result = new StringBuilder("0");
                    }

                }
            }
            else
            {
                result.Append("0");
            }
            return result.ToString();
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
        /// <summary>
        /// This method converts the Date time values(Hexadecimal format) for a parameter into proper date time string
        /// </summary>
        /// <param name="DateTimeValue"></param>
        /// <returns></returns>
        public static DateTime ConvertHexStringToDateTime(string dateTimeValue)
        {
            int num = 0;
            int Year = 0;
            int Month = 0;
            int Day = 0;
            int Hour = 0;
            int Minute = 0;
            int Seconds = 0;
            DateTime dateTime = DateTime.MinValue;
            try
            {
                // Extracting the year value
                num += 4;
                string data = dateTimeValue.Substring(num, 4);
                Year = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
                num += 4;
                // Extracting the month value
                Month = ConvertHexToDecimal(dateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Day value
                Day = ConvertHexToDecimal(dateTimeValue.Substring(num, 2), 0);
                num += 4;
                // Extracting the Hour value
                Hour = ConvertHexToDecimal(dateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Minutes value
                Minute = ConvertHexToDecimal(dateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Seconds value
                Seconds = ConvertHexToDecimal(dateTimeValue.Substring(num, 2), 0);
                num += 2;
                dateTime = new DateTime(Year, Month, Day, Hour, Minute, Seconds);
                return dateTime;
            }
            catch
            {
                dateTime = DateTime.Now;
                return dateTime;
            }
        }
        /// <summary>
        /// Converts hex to decimal
        /// </summary>
        /// <param name="dataInStringFormat"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        private static byte ConvertHexToDecimal(string dataInStringFormat, int dataIndex)
        {
            string data = dataInStringFormat.Substring(dataIndex, 2);
            return byte.Parse(data, System.Globalization.NumberStyles.HexNumber);
        }
        public static string GetFormatedDateTme(string data)
        {
            string result = string.Empty;
            try
            {
                string[] value = data.Split(';');
                string[] date = value[1].Split('-');
                string[] time = value[0].Split(':');
                //TimeSpan span = TimeSpan.FromHours(Convert.ToDouble(time[0]));
                result = string.Concat(string.Concat("20", date[2]), date[1], date[0], time[0], time[1], "00");
            }
            catch
            { }
            return result;
        }
        /// <summary>
        /// This method is used to calculate hours from the time format day:hour:minute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetHours(string value)
        {
            string[] hourArray = null;
            double totalHours = 0;
            hourArray = value.Split(':');
            if (hourArray.Length > 2)
                totalHours = Convert.ToDouble(hourArray[0]) * 24 + Convert.ToDouble(hourArray[1]) + Convert.ToDouble(hourArray[2]) / 60;
            totalHours = Math.Truncate(totalHours * 100) / 100;
            return totalHours;
        }
    }
}

