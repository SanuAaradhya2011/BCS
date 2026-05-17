using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace CAB.Framework
{
    public static class Extensions
    {
        static bool hasRows = false;
        public static string[] CommaSplit(this string str)
        {
            return str.Split(',');
        }
        public static string FormatToTwoDigit(this int num)
        {
            return string.Format("{0:00}", num);
        }
        public static string FormatToTwoDigit(this decimal num)
        {
            return string.Format("{0:00}", num);
        }
        public static string ToShortDateTimeCABFormat(this DateTime dateTime)
        {
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";
            return dateTime.ToString("dd/MM/yyyy", dateInfo);
        }
        public static bool FirstTableHasRows(this DataSet dataSet)
        {
            hasRows = false;
            if (dataSet != null)
            {
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        hasRows = true;
                    }
                }
            }
            return hasRows;
        }
        public static string TruncateToPrecision(this decimal targetValue, int precision)
        {
            string value = string.Empty;
            try
            {
                value = targetValue.ToString();
                decimal step = (decimal)Math.Pow(10, precision);
                int tmp = (int)Math.Truncate(step * targetValue);
                targetValue = tmp / step;
                value = string.Format("{0:F" + precision.ToString() + "}", targetValue);
            }
            catch (Exception ex)
            {
                return value;
            }
            return value;
        }
    }
}
