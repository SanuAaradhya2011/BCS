/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using System.Globalization;

namespace CAB.IECChannel.Formatter
{

    public class FormatterCommon
    {
        public static MatchCollection ValidateData(string data)
        {
            return Regex.Matches(data, FormatterConstant.BILLING, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        }
        public static Regex GetExpression(string commandExpression)
        {
            return new Regex(commandExpression, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);

        }
        public static MatchCollection ValidateData(string data, string expression)
        {
            return Regex.Matches(data, expression, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        public static string ParseData(string data)
        {
            int startindex = 0, endindex = 0;
            if (data != null)
            {
                startindex = data.IndexOf("(");

                if (((endindex = data.IndexOf("pf")) != -1) || ((endindex = data.IndexOf("PF")) != -1) || (endindex = data.IndexOf(")")) != -1)
                    if (endindex != -1) data = data.Substring(startindex + 1, endindex - 1);
            }
            return data;
        }
        public static string ParseDataFor1Phase(Dictionary<string, string> dicWithOBIS, string data)
        {
            string value = string.Empty;
            try
            {
                if (data != string.Empty && dicWithOBIS.ContainsKey(data))
                {
                    value = dicWithOBIS[data];
                }
            }
            catch
            {
            }
            return value;
        }
        public static int ParseIntData(string data)
        {
            int value = 0;
            data = ParseData(data);
            data = data.Trim();
            if (data != "")
            {
                try
                {
                    value = Int32.Parse(data);
                }
                catch (Exception)
                {
                    value = 0;
                }
            }
            return value;
        }

        public static long ParseDate(string data)
        {
            try
            {
                data = ParseData(data);
                if (data == "----------")
                    return 0;

                data = data.Trim();
                data = data.Replace("  ", " ");
                if (data.Length < 8)
                    return 0;
                string[] datetime = data.Split(' ');
                string[] date = datetime[0].Split('-');
                string[] time = datetime[1].Split(':');

                string year1 = date[2].Trim();
                string month1 = date[1].Trim();
                string day1 = date[0].Trim();

                //commented by dhirendra on 5 may 2010 becoz billing timestamp was inserting incorrectly

                year1 = (year1.Length == 2 && year1 != "00") ? string.Concat("20", year1) : year1;
                month1 = (month1.Length == 1) ? string.Concat("0", month1) : month1;
                day1 = (day1.Length == 1) ? string.Concat("0", day1) : day1;

                if (time.Length == 3)
                    data = string.Concat(year1, month1, day1, DateValueInNumber(time[0]), DateValueInNumber(time[1]), DateValueInNumber(time[2]));
                else if (time.Length == 2)
                    data = string.Concat(year1, month1, day1, DateValueInNumber(time[0]), DateValueInNumber(time[1]), "");
                else if (time.Length == 1)
                    data = string.Concat(year1, month1, day1, DateValueInNumber(time[0]), "", "");
                else
                    data = string.Concat(year1, month1, day1, "", "", "");

                long i = Convert.ToInt64(data);
                if (i == 0)
                    return 0;
                else
                {
                    if (data.Length != 12 && data.Length != 14)
                        data = string.Concat("20", data);
                }
                return Convert.ToInt64(data);

            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static long DTMParseDate(string data)
        {
            data = data.Trim();
            if (data == "----------")
                return 0;
            string minute = data.Substring(8, 2);
            string hour = data.Substring(6, 2);
            string year = data.Substring(4, 2);
            string month = data.Substring(2, 2);
            string day = data.Substring(0, 2);
            year = (year.Length == 2 && year != "00") ? string.Concat("20", year) : year;
            month = (month.Length == 1) ? string.Concat("0", month) : month;
            day = (day.Length == 1) ? string.Concat("0", day) : day;
            hour = (hour.Length == 1) ? string.Concat("0", hour) : hour;
            minute = (minute.Length == 1) ? string.Concat("0", minute) : minute;
            data = string.Concat(year, month, day, hour, minute, "00");
            try
            {
                return Convert.ToInt64(data);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private static string DateValueInNumber(string number)
        {
            number = number.Trim();
            if (number == "")
                return "00";
            if (number.Length == 1)
                return string.Concat("0", number);
            else
                return number;
        }
        private static string YearValueInNumber(string number)
        {
            number = number.Trim();
            if (number == "")
                return "0000";
            if (number.Length == 1)
                return string.Concat("200", number);
            else if (number.Length == 2)
                return string.Concat("20", number);
            else if (number.Length == 3)
                return string.Concat("2", number);
            else
                return number;
        }
        public static string[] RemoveDuplicateData(string[] fileContent)
        {
            int counter = 0;
            List<string> resultData = new List<string>();
            foreach (string strTemp in fileContent)
            {
                counter = 0;
                if (strTemp.Length == 0)
                    continue;
                bool foundFirstTime = false;
                MatchCollection matches = ValidateData(strTemp, FormatterConstant.MeterIDEXPRESSION);
                if (matches.Count == 0)
                    continue;
                string[] matchData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    matchData[counter++] = groups[0].Value;
                }
                for (counter = fileContent.GetUpperBound(0); counter >= 0; counter--)
                {
                    if (fileContent[counter].Length == 0)
                        continue;

                    MatchCollection matchtemp = ValidateData(fileContent[counter], string.Concat(FormatterConstant.METERIDPART1, matchData[0].Substring(5, matchData[0].Length - 6), FormatterConstant.METERIDPART2));
                    if (matchtemp.Count == 0)
                        continue;
                    else
                    {
                        //if (!foundFirstTime)
                        //{
                        //   foundFirstTime = true;
                        resultData.Add(fileContent[counter]);
                        //}
                        fileContent[counter] = string.Empty;
                    }
                }
            }
            return resultData.ToArray();
        }

        public static string[] RemoveDuplicateDataForSPhase(string[] fileContent)
        {
            int counter = 0;
            List<string> resultData = new List<string>();
            foreach (string strTemp in fileContent)
            {
                counter = 0;
                if (strTemp.Length == 0)
                    continue;

                MatchCollection matches = ValidateData(strTemp, FormatterConstant.MeterIDEXPRESSIONFOR1PHASE);
                if (matches.Count == 0)
                    continue;
                string[] matchData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    matchData[counter++] = groups[0].Value;
                }
                for (counter = fileContent.GetUpperBound(0); counter >= 0; counter--)
                {
                    if (fileContent[counter].Length == 0)
                        continue;

                    MatchCollection matchtemp = ValidateData(fileContent[counter], string.Concat(FormatterConstant.METERIDPART1FOR1PHASE, matchData[0].Substring(5, matchData[0].Length - 6))); //, FormatterConstant.METERIDPART2FOR1PHASE
                    if (matchtemp.Count == 0)
                        continue;
                    else
                    {
                        resultData.Add(fileContent[counter]);
                        fileContent[counter] = string.Empty;
                    }
                }
            }
            return resultData.ToArray();
        }

        public static bool IsFileNullOrEmpty(string fileContent)
        {
            MatchCollection matches = ValidateData(fileContent, FormatterConstant.VALIDFILECONTENT);
            if (matches.Count != 0 || fileContent.Contains("/RTC"))
                return true;
            else
                return false;
        }
        private static bool ValueExist(string value)
        {
            int number;
            return Int32.TryParse(value, out number);
        }
        public static string ConvertBitTo8Digit(string data)
        {
            while (data.Length < 8)
                data = string.Concat("0", data);
            return data;
        }
        public static void SetDescription(string data, ProgrammingEntity entity)
        {
            try
            {
                int ch;
                int i = 0;
                string firstBit = FormatterCommon.ConvertBitTo8Digit(Convert.ToString(Convert.ToInt32(Convert.ToInt32(data.Substring(0, 2), 16).ToString(), 10), 2));
                string midBit = FormatterCommon.ConvertBitTo8Digit(Convert.ToString(Convert.ToInt32(Convert.ToInt32(data.Substring(2, 2), 16).ToString(), 10), 2));
                string lastBit = FormatterCommon.ConvertBitTo8Digit(Convert.ToString(Convert.ToInt32(Convert.ToInt32(data.Substring(4, 2), 16).ToString(), 10), 2));
                string totalBit = string.Concat(firstBit, midBit, lastBit);
                while (i < totalBit.Length)
                {
                    if (totalBit.Substring(i, 1) == "1")
                        ch = i + 1;
                    else ch = 0;
                    //switch (ch)
                    //{
                    //    case 17:
                    //        entity = Programmings(entity, "Meter ID");
                    //        break;
                    //    case 16:
                    //        entity=Programmings(entity, "LCD Display Parameters");
                    //        break;
                    //    case 15:
                    //        entity=Programmings(entity,  "Maximum Demand");
                    //        break;
                    //    case 14:
                    //        entity=Programmings(entity,  "Billing Date & Time");
                    //        break;
                    //    case 13:
                    //        entity=Programmings(entity,  "Number of Billing");
                    //        break;
                    //    case 12:
                    //        entity=Programmings(entity,  "Future TOU");
                    //        break;
                    //    case 11:
                    //        entity=Programmings(entity,  "Load Survey");
                    //        break;
                    //    case 10:
                    //        entity=Programmings(entity, "Tampers");
                    //        break;
                    //    case 9:
                    //        entity=Programmings(entity,  "Resolution Parameters");
                    //        break;
                    //    case 8:
                    //        entity=Programmings(entity,  "LCD Backlight");
                    //        break;
                    //    case 7:
                    //        entity=Programmings(entity,  "MD Reset Lock Out");
                    //        break;
                    //    case 6:
                    //        entity=Programmings(entity,  "CT Ratio");
                    //        break;
                    //    case 5:
                    //        entity=Programmings(entity,  "kVAh Selection");
                    //        break;
                    //    case 4:
                    //        entity=Programmings(entity, "Baud Rate");
                    //        break;
                    //    case 3:
                    //        entity=Programmings(entity, "Access Permission");
                    //        break;
                    //    case 2:
                    //        entity=Programmings(entity, "Daily Log Parameters");
                    //        break;
                    //    case 1:
                    //        entity=Programmings(entity, "LPR Parameters");
                    //        break;
                    //}



                    switch (ch)
                    {


                        case 24:
                            entity = Programmings(entity, "Meter ID");
                            break;
                        case 23:
                            entity = Programmings(entity, "LCD Display Parameters");
                            break;
                        case 22:
                            entity = Programmings(entity, "Maximum Demand");
                            break;
                        case 21:
                            entity = Programmings(entity, "Billing Date & Time");
                            break;
                        case 20:
                            entity = Programmings(entity, "Number of Billing");
                            break;
                        case 19:
                            entity = Programmings(entity, "Future TOU");
                            break;
                        case 18:
                            entity = Programmings(entity, "Load Survey");
                            break;
                        case 17:
                            entity = Programmings(entity, "Tampers");
                            break;
                        case 16:
                            entity = Programmings(entity, "Resolution Parameters");
                            break;
                        case 15:
                            entity = Programmings(entity, "LCD Backlight");
                            break;
                        case 14:
                            entity = Programmings(entity, "MD Reset Lock Out");
                            break;
                        case 13:
                            entity = Programmings(entity, "CT Ratio");
                            break;
                        case 12:
                            entity = Programmings(entity, "kVAh Selection");
                            break;
                        case 11:
                            entity = Programmings(entity, "Baud Rate");
                            break;
                        case 10:
                            entity = Programmings(entity, "Access Permission");
                            break;
                        case 9:
                            entity = Programmings(entity, "Daily Log Parameters");
                            break;
                        case 8:
                            entity = Programmings(entity, "LPR Parameters");
                            break;
                        case 7:
                            entity = Programmings(entity, "Odd/Even Billing");
                            break;
                        case 6:
                            entity = Programmings(entity, "RS232 Lock/Unlock");
                            break;
                    }
                    i += 1;
                }
            }
            catch (Exception)
            {
                entity = null;
            }
        }
       
        private static ProgrammingEntity Programmings(ProgrammingEntity entity, string message)
        {
            if (string.IsNullOrEmpty(message))
                return entity;
            else if (string.IsNullOrEmpty(entity.Description1))
                entity.Description1 = message;
            else if (string.IsNullOrEmpty(entity.Description2))
                entity.Description2 = message;
            else if (string.IsNullOrEmpty(entity.Description3))
                entity.Description3 = message;
            else if (string.IsNullOrEmpty(entity.Description4))
                entity.Description4 = message;
            else if (string.IsNullOrEmpty(entity.Description5))
                entity.Description5 = message;
            else if (string.IsNullOrEmpty(entity.Description6))
                entity.Description6 = message;
            else if (string.IsNullOrEmpty(entity.Description7))
                entity.Description7 = message;
            else if (string.IsNullOrEmpty(entity.Description8))
                entity.Description8 = message;
            else if (string.IsNullOrEmpty(entity.Description9))
                entity.Description9 = message;
            else if (string.IsNullOrEmpty(entity.Description10))
                entity.Description10 = message;
            else if (string.IsNullOrEmpty(entity.Description11))
                entity.Description11 = message;
            else if (string.IsNullOrEmpty(entity.Description12))
                entity.Description12 = message;
            else if (string.IsNullOrEmpty(entity.Description13))
                entity.Description13 = message;
            else if (string.IsNullOrEmpty(entity.Description14))
                entity.Description14 = message;
            else if (string.IsNullOrEmpty(entity.Description15))
                entity.Description15 = message;
            else if (string.IsNullOrEmpty(entity.Description16))
                entity.Description16 = message;
            else if (string.IsNullOrEmpty(entity.Description17))
                entity.Description17 = message;
            return entity;
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
        public static string GetTimeStamp(string data, int position)
        {
            int day, month, year, hour, minute;
            day = month = year = hour = minute = 0;
            if (ValueExist(data.Substring(position, 2)))
                day = Convert.ToInt32(data.Substring(position, 2));
            if (ValueExist(data.Substring(position + 2, 2)))
                month = Convert.ToInt32(data.Substring(position + 2, 2));
            if (ValueExist(data.Substring(position + 4, 2)))
                year = Convert.ToInt32(data.Substring(position + 4, 2));
            if (ValueExist(data.Substring(position + 6, 2)))
                hour = Convert.ToInt32(data.Substring(position + 6, 2));
            if (ValueExist(data.Substring(position + 8, 2)))
                minute = Convert.ToInt32(data.Substring(position + 8, 2));
            if ((day >= 1 && day <= 31) && (month >= 1 && month <= 12) && (hour <= 23 && minute <= 59))
            {
                string year1 = (year.ToString().Length == 2) ? "20" + year.ToString() : year.ToString();
                string month1 = (month.ToString().Length == 1) ? "0" + month.ToString() : month.ToString();
                string day1 = (day.ToString().Length == 1) ? "0" + day.ToString() : day.ToString();
                string hour1 = (hour.ToString().Length == 1) ? "0" + hour.ToString() : hour.ToString();
                string minute1 = (minute.ToString().Length == 1) ? "0" + minute.ToString() : minute.ToString();

                return day1 + "/" + month1 + "/" + year1 + " " + hour1.ToString() + ":" + minute1.ToString();
            }
            else
            {
                if (day == 0 && month == 0 && year == 0)
                    return string.Empty;
                else
                    return Convert.ToDateTime("1/1/1900").ToString("dd/MM/yyyy");
            }
        }
        public static string FilterData(string data, int start, int end, int div, string format)
        {           
            return (Convert.ToDouble((Int64.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString()) / div).ToString(format); 
        }
        public static string FilterDataNR(string data, int start, int end, int div, string format)
        {
            string value = "0";
            switch ((end - start) / 2)
            {
                case 1:
                    value = (Int16.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString();
                    break;
                case 2:
                    value = (Int16.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString();
                    break;
                case 4:
                    value = (Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString();
                    break;
                case 8:
                    value = (Int64.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString();
                    break;
            }
            return string.Format(format, Math.Truncate((Convert.ToDouble(value) / div) * 100) / 100);
            //return string.Format(format, Math.Truncate((Convert.ToDouble((Int64.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString()) / div) * 100) / 100); 
        }

        public static string FilterData(string data, int start, int end)
        {
            return ((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString());
        }
        public static string FilterData(string data, int start, int end, double div)
        {
            return ((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber) * div).ToString());
        }
        public static string ParseData(string data, int start, int end, int div, string format)
        {
            int value = Convert.ToInt32(data.Substring(start, end), 16);
            return string.Format(format, (Convert.ToDecimal(value) / div)).ToString();
        }
        public static long DTMDailyProfileDate(string data, int position, bool isDateOnly)
        {
            int day, month, year, hour, minute, second;
            day = month = year = hour = minute = second = 0;
            if (!isDateOnly)
            {
                if (ValueExist(data.Substring(position, 2)))
                    minute = Convert.ToInt32(data.Substring(position, 2));
                if (ValueExist(data.Substring(position + 2, 2)))
                    hour = Convert.ToInt32(data.Substring(position + 2, 2));
                if (ValueExist(data.Substring(position + 4, 2)))
                    day = Convert.ToInt32(data.Substring(position + 4, 2));
                if (ValueExist(data.Substring(position + 6, 2)))
                    month = Convert.ToInt32(data.Substring(position + 6, 2));
                if (ValueExist(data.Substring(position + 8, 2)))
                    year = Convert.ToInt32(data.Substring(position + 8, 2));
            }
            else
            {
                if (ValueExist(data.Substring(position, 2)))
                    day = Convert.ToInt32(data.Substring(position, 2));
                if (ValueExist(data.Substring(position + 2, 2)))
                    month = Convert.ToInt32(data.Substring(position + 2, 2));
                if (ValueExist(data.Substring(position + 4, 2)))
                    year = Convert.ToInt32(data.Substring(position + 4, 2));
            }
            if ((day >= 1 && day <= 31) && (month >= 1 && month <= 12) && (hour <= 23 && minute <= 59))
            {
                string year1 = (year.ToString().Length == 2) ? string.Concat("20", year.ToString()) : year.ToString();
                string month1 = (month.ToString().Length == 1) ? string.Concat("0", month.ToString()) : month.ToString();
                string day1 = (day.ToString().Length == 1) ? string.Concat("0", day.ToString()) : day.ToString();
                string hour1 = (hour.ToString().Length == 1) ? string.Concat("0", hour.ToString()) : hour.ToString();
                string minute1 = (minute.ToString().Length == 1) ? string.Concat("0", minute.ToString()) : minute.ToString();
                string second1 = (second.ToString().Length == 1) ? string.Concat("0", second.ToString()) : second.ToString();
                return Convert.ToInt64(string.Concat(year1, month1, day1, hour1, minute1, second1));
            }
            else
            {
                if (day == 0 && month == 0 && year == 0)
                    return 0;
            }
            return 0;
        }
        public static long DTMDailyProfileDateSP(string data)
        {
            long dateInLong = 0;
            try
            {
                int day, month, year, hour, minute, second;
                day = month = year = hour = minute = second = 0;

                string month2 = Convert.ToString(Convert.ToInt32(data.Substring(0, 2), 16), 2);
                string year2 = Convert.ToString(Convert.ToInt32(data.Substring(2, 2), 16));


                while (month2.Length < 8) { month2 = "0" + month2; }
                month2 = "0" + month2.Substring(1);
                month2 = Convert.ToInt32(month2, 2).ToString();

                string time = Convert.ToString(Convert.ToInt32(data.Substring(6, 2) + data.Substring(4, 2), 16), 2);
                while (time.Length < 16) { time = "0" + time; }
                string temp = time.Substring(0, 5);
                while (temp.Length < 8) { temp = "0" + temp; }
                string day2 = Convert.ToInt32(temp, 2).ToString();

                string hour2 = time.Substring(5, 5);
                while (hour2.Length < 8) { hour2 = "0" + hour2; }
                string min2 = time.Substring(10, 6);
                while (min2.Length < 8) { min2 = "0" + min2; }
                string tempHour = Convert.ToInt32(hour2, 2).ToString();
                if (tempHour.Length < 2) { tempHour = "0" + tempHour; }
                string tempMin = Convert.ToInt32(min2, 2).ToString();
                if (tempMin.Length < 2) { tempMin = "0" + tempMin; }
      


                if (day2 == "00")
                {
                }
                else
                {
                    string year1 = (year2.Length == 2) ? string.Concat("20", year2) : year2;
                    string month1 = (month2.Length == 1) ? string.Concat("0", month2) : month2;
                    string day1 = (day2.ToString().Length == 1) ? string.Concat("0", day2.ToString()) : day2.ToString();
                    string hour1 = (tempHour.ToString().Length == 1) ? string.Concat("0", tempHour.ToString()) : tempHour.ToString();
                    string minute1 = (tempMin.ToString().Length == 1) ? string.Concat("0", tempMin.ToString()) : tempMin.ToString();
                    string second1 = (second.ToString().Length == 1) ? string.Concat("0", second.ToString()) : second.ToString();
                    dateInLong = Convert.ToInt64(string.Concat(year1, month1, day1, hour1, minute1, second1));
                }
            }
            catch
            { 
            }
            return dateInLong;
        }
        
        public static long LongDateTime(string data, int position)
        {
            int day, month, year, hour, minute, second;
            day = month = year = hour = minute = second = 0;
            if (ValueExist(data.Substring(position, 2)))
                day = Convert.ToInt32(data.Substring(position, 2));
            if (ValueExist(data.Substring(position + 2, 2)))
                month = Convert.ToInt32(data.Substring(position + 2, 2));
            if (ValueExist(data.Substring(position + 4, 2)))
                year = Convert.ToInt32(data.Substring(position + 4, 2));
            if (ValueExist(data.Substring(position + 6, 2)))
                hour = Convert.ToInt32(data.Substring(position + 6, 2));
            if (ValueExist(data.Substring(position + 8, 2)))
                minute = Convert.ToInt32(data.Substring(position + 8, 2));
            //if (ValueExist(data.Substring(position + 10, 2)))
            //    second = Convert.ToInt32(data.Substring(position + 8, 2));
            if ((day >= 1 && day <= 31) && (month >= 1 && month <= 12) && (hour <= 23 && minute <= 59))
            {
                string year1 = (year.ToString().Length == 2) ? string.Concat("20", year.ToString()) : year.ToString();
                string month1 = (month.ToString().Length == 1) ? string.Concat("0", month.ToString()) : month.ToString();
                string day1 = (day.ToString().Length == 1) ? string.Concat("0", day.ToString()) : day.ToString();
                string hour1 = (hour.ToString().Length == 1) ? string.Concat("0", hour.ToString()) : hour.ToString();
                string minute1 = (minute.ToString().Length == 1) ? string.Concat("0", minute.ToString()) : minute.ToString();
                string second1 = (second.ToString().Length == 1) ? string.Concat("0", second.ToString()) : second.ToString();
                return Convert.ToInt64(string.Concat(year1, month1, day1, hour1, minute1, second1));
            }
            else
            {
                if (day == 0 && month == 0 && year == 0)
                    return 0;
            }
            return 0;
        }
        public static int FilterData(string data, int start, int end, bool isHex)
        {
            string val = data.Substring(start, end);
            if (isHex)
                return Convert.ToInt32(val, 16);
            else
                return Convert.ToInt32(val);
        }
    }
}
