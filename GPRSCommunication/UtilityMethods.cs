using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSCommunication
{
    class UtilityMethods
    {

        /// <summary>
        /// Method convers Byte Array to Hex string
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string ConvertByteArrayToHex(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConvertToHex(byteArray));
            return StuffPatternInString(sb.ToString(), " ", 2);
        }
        
        /// <summary>
        /// Method byte to hex string. 
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string ConvertToHex(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Convert Hex string to Byte Array
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns></returns>
        public static byte[] ConvertHexToByteArray(string hexValue)
        {
            if (hexValue.Trim().Length > 2)
            {
                if (hexValue.Trim().Substring(2) != " ")
                {
                    hexValue = hexValue.Replace(" ", "");
                    hexValue = StuffPatternInString(hexValue, " ", 2);
                }
            }
            List<byte> list = new List<byte>();
            string[] hexArray = hexValue.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in hexArray)
            {
                list.Add(Convert.ToByte(int.Parse(s, System.Globalization.NumberStyles.HexNumber)));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Return string containing stuffed pattern after specified interval
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static String StuffPatternInString(String source, String pattern, Int32 interval)
        {
            StringBuilder stuffedValue = new StringBuilder(source.Length);
            if (source.Length <= interval)
            {
                stuffedValue.Append(source);
            }
            else
            {
                Int32 offset = 0;
                while (offset < source.Length)
                {
                    if ((offset + interval) <= source.Length)
                    {
                        stuffedValue.Append(source.Substring(offset, interval) + pattern);
                    }
                    offset += interval;
                }
            }

            return stuffedValue.ToString().Trim();
        }

    }
}
