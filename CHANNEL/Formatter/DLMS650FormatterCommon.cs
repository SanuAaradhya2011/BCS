/* 
 * |---------------------------------------------------------------------------------------------------------------|
 * |All rights reserved to Cabcon Technologies                                                                             |
 * |                                                                                                               |
 * |Author : Piyush Singh.                                                                               |
 * |                                                                                                               |
 * |                                                                                                               |
 * |---------------------------------------------------------------------------------------------------------------| */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CAB.Entity;
using CAB.BLL;
using CAB.Framework.Utility;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;
using CAB.Framework;
namespace CAB.Channel.Formatter
{

    public enum DLMSParameter
    {
        Current,
        Instant,
        Billing,
        LoadSurvey,
        Tamper,
        General,
        //added for MVVNL
        MidnightData,
        SelfDiagnosis,
        //added for MVVNL
        /* GKG JVVNL Current TOU Read */
        TOU,
        /* GKG JVVNL Current TOU Read */
        Phasor,
        //BhardwajG For NDPL, demand integration tag addition in file
        DemandIntegrationPeriod,
        //Normal DLMS phasor for CESC Ruby
        NormalPhasor,
        None
    }

    public enum DefaultValues
    {
        [DescriptionAttribute("FFFF")]
        Year,
        [DescriptionAttribute("FF")]
        Month,
        [DescriptionAttribute("FF")]
        Day,
        [DescriptionAttribute("9999")]
        DefaultYear,
        [DescriptionAttribute("99")]
        Default,
        [DescriptionAttribute("FF")]
        Hour,
        [DescriptionAttribute("FF")]
        Minutes,
        [DescriptionAttribute("FF")]
        Seconds

    }

    public class DLMS650FormatterCommon
    {
        public Dictionary<string, string> OBISColumns = new Dictionary<string, string>();
        public Dictionary<string, string> OBISLoadSurveyColumns = new Dictionary<string, string>();
        private static int energyResolution = 0;
        private static int demandResolution = 0;
        public static int EnergyResolution
        {
            get
            {
                return energyResolution;
            }
            set
            {
                energyResolution = value;
            }
        }
        public string NewLine = "\n";
        public static bool CheckBCC(string data)
        {
            File.Delete("output.txt");
            string[] lines = data.Split('\n');
            // string[] lines = File.ReadAllLines(strFileName);
            StringBuilder sb = new StringBuilder();
            int count = lines.Length - 2; // except last line 
            int i;
            for (i = 0; i < count; i++)
            {
                sb.AppendLine(lines[i].Replace("\r", ""));
            }
            File.WriteAllText("output.txt", sb.ToString());
            String temp = lines[i];
            temp = temp.Replace("\r", "");
            string val = GetMD5ChecksumForFile("output.txt");
            if (temp == val)
                return true;
            else
                return false;
        }

        private static string GetMD5ChecksumForFile(string filename)
        {
            if (filename == null)
                return string.Empty;
            if (!File.Exists(filename))
                return string.Empty;
            FileStream fstream = new FileStream(filename, FileMode.Open);
            fstream.Close();
            using (fstream = new FileStream(filename, FileMode.Open))
            {

                byte[] hash = new MD5CryptoServiceProvider().ComputeHash(fstream);
                StringBuilder sb = new StringBuilder(32);
                foreach (byte hex in hash)
                    sb.Append(hex.ToString("X2"));
                fstream.Close();
                return sb.ToString().ToUpper();
            }
        }

        public int TotByte(string val)
        {
            int bytes = ConvertHexToDecimal(val);
            if (bytes > 128)
                return 2;
            else
                return 1;
        }
        public int ConvertHexToDecimal(string val)
        {
            return int.Parse(val, System.Globalization.NumberStyles.HexNumber);
        }
        /// <summary>
        /// This is used to calculate power off duration.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetCumPowerOffDuration(string data)
        {
            string tempData = string.Empty;
            string tempdata2 = string.Empty;
            int index = 0;
            string day = string.Empty;
            while (index < 14)
            {
                if (index == 8) { index += 2; continue; }
                if (!string.IsNullOrEmpty(data))
                {
                    tempdata2 = Convert.ToString(Convert.ToInt32(data.Substring(index, 2), 16));
                    // Added to make day in DDD format.
                    if (index == 6)
                    {
                        day = tempdata2;
                        if (day.Length < 3)
                            day = day.PadLeft(3, '0');
                    }
                    index += 2;
                    while (tempdata2.Length < 2)
                    {
                        tempdata2 = "0" + tempdata2;
                    }
                    tempData += (tempdata2);
                }
            }
            return tempData.Substring(2, 2) + ":" + tempData.Substring(4, 2) + ":" + day + ":" + tempData.Substring(8, 2) + ":" + tempData.Substring(10, 2);
        }
        public string ConvertHexToString(string val)
        {
            int num = 0;
            string obis = "";
            string val1 = "";
            while (true)
            {
                val1 = val.Substring(num, 2);
                num += 2;
                obis = obis + (char)Convert.ToInt32(val1, 16);
                if (num >= val.Length)
                    break;
            }
            return obis;
        }
        /// <summary>
        /// This method converts the Date time values(Hexadecimal format) for a parameter into proper date time string
        /// </summary>
        /// <param name="DateTimeValue"></param>
        /// <returns></returns>
        public string GetDateTimeString(string DateTimeValue)
        {
            int num = 0;
            string Year = string.Empty;
            string Month = string.Empty;
            string Day = string.Empty;
            string Hour = string.Empty;
            string Minute = string.Empty;
            string Seconds = string.Empty;
            string CurrentYear = DateTime.Now.Year.ToString().Substring(0, 2);
            try
            {
                // Extracting the year value
                Year = DateTimeValue.Substring(num, 4);
                if (Year.Trim().ToUpper().Equals(EnumUtil.stringValueOf(DefaultValues.Year)))
                    Year = EnumUtil.stringValueOf(DefaultValues.DefaultYear);
                else
                    Year = ConvertHexToDecimal(Year).ToString();
                num += 4;
                // Extracting the month value
                Month = DateTimeValue.Substring(num, 2);
                if (Month.Trim().ToUpper().Equals(EnumUtil.stringValueOf(DefaultValues.Month)))
                    Month = EnumUtil.stringValueOf(DefaultValues.Default);
                else
                    Month = ConvertHexToDecimal(Month).ToString();
                num += 2;
                // Extracting the Day value
                Day = DateTimeValue.Substring(num, 2);
                if (Day.Trim().ToUpper().Equals(EnumUtil.stringValueOf(DefaultValues.Day)))
                    Day = EnumUtil.stringValueOf(DefaultValues.Default);
                else
                    Day = ConvertHexToDecimal(Day).ToString();
                num += 4;

                // Extracting the Hour value
                Hour = DateTimeValue.Substring(num, 2);
                if (Hour.Trim().ToUpper().Equals(EnumUtil.stringValueOf(DefaultValues.Hour)))
                    Hour = EnumUtil.stringValueOf(DefaultValues.Default);
                else
                    Hour = ConvertHexToDecimal(Hour).ToString();
                num += 2;
                // Extracting the Minutes value
                Minute = DateTimeValue.Substring(num, 2);
                if (Minute.Trim().ToUpper().Equals(EnumUtil.stringValueOf(DefaultValues.Minutes)))
                    Minute = EnumUtil.stringValueOf(DefaultValues.Default);
                else
                    Minute = ConvertHexToDecimal(Minute).ToString();
                num += 2;
                // Extracting the Seconds value
                Seconds = DateTimeValue.Substring(num, 2);
                if (Seconds.Trim().ToUpper().Equals(EnumUtil.stringValueOf(DefaultValues.Seconds)))
                    Seconds = EnumUtil.stringValueOf(DefaultValues.Default);
                else
                    Seconds = ConvertHexToDecimal(Seconds).ToString();
                num += 2;

                Year = (Year.Length == 2) ? string.Concat(CurrentYear, Year) : Year;
                Month = (Month.Length == 1) ? string.Concat("0", Month) : Month;
                Day = (Day.Length == 1) ? string.Concat("0", Day) : Day;
                Hour = (Hour.Length == 1) ? string.Concat("0", Hour) : Hour;
                Minute = (Minute.Length == 1) ? string.Concat("0", Minute) : Minute;
                Seconds = (Seconds.Length == 1) ? string.Concat("0", Seconds) : Seconds;
                return string.Concat(Year, Month, Day, Hour, Minute, Seconds);
            }
            catch (Exception ex)
            {
                // To solve bug 94904.
                throw ex;
                //MessageBox.Show("File Corrupt", BCSConstants.BCS);

            }

            return string.Concat(Year, Month, Day, Hour, Minute, Seconds);
        }
        public string GetOBISCode(string val)
        {
            int num = 0;
            string obis = "";
            string val1 = "";
            while (true)
            {
                val1 = val.Substring(num, 2);
                num += 2;
                obis = obis + ConvertHexToDecimal(val1);
                if (num >= val.Length)
                    break;
                else
                    obis = obis + ".";
            }
            return obis;
        }
        public DLMSParameter GetParameterType(string val)
        {
            try
            {
                //BhardwajG parse it as hex
                int param = ConvertHexToDecimal(val);
                switch (param)
                {
                    case 0:
                        return DLMSParameter.Current;
                    case 1:
                        return DLMSParameter.Instant;
                    case 2:
                        return DLMSParameter.Billing;
                    case 3:
                        return DLMSParameter.LoadSurvey;
                    case 4:
                        return DLMSParameter.Tamper;
                    case 5:
                        return DLMSParameter.General;
                    //added for MVVNL
                    case 6:
                        return DLMSParameter.MidnightData;
                    case 7:
                        return DLMSParameter.SelfDiagnosis;
                    /* GKG JVVNL Current TOU Read */
                    case 8:
                        return DLMSParameter.TOU;
                    /* GKG JVVNL Current TOU Read */
                    case 9:
                        return DLMSParameter.Phasor;
                    //added for MVVNL
                    //BhardwajG : For demand integration period.
                    case 10 :
                        return DLMSParameter.DemandIntegrationPeriod;
                    //CESC : Normal phasor addition
                    case 11:
                        return DLMSParameter.NormalPhasor;
                    default:
                        return DLMSParameter.None;
                }
            }
            catch (Exception)
            {
                return DLMSParameter.None;
            }
        }
        public void LoadMeterData(string data, BillingGeneralNFDLMSEntity master)
        {
            MeterDataEntity meterData = new MeterDataEntity();
            int index;
            //VBM - 1 is prefixed in meter srial number so need to increase length by 1 .
            if (UtilityDetails.PrimaryUtlityName == UtilityEntity.SHYAMINDUS.ToString())
            {
                index = Convert.ToInt16(data.Substring(0, 2)) + 1;
            }
            else
            {
                index = Convert.ToInt16(data.Substring(0, 2));
            }
            meterData.MeterID = data.Substring(2, index).Trim();
            meterData.ReadingDateTime = Int64.Parse(data.Substring((index + 2), 14));
            meterData.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
            master.MeterData = meterData;
        }
        public string GetUnit(string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
                return string.Empty;
            else if (unitName.Trim().ToUpper().Equals("W"))
                return "kW";
            else if (unitName.Trim().ToUpper().Equals("VA"))
                return "kVA";
            else if (unitName.Trim().ToUpper().Equals("VAR"))
                return "kvar";
            else if (unitName.Trim().ToUpper().Equals("WH"))
                return "kWh";
            else if (unitName.Trim().ToUpper().Equals("VAH"))
                return "kVAh";
            else if (unitName.Trim().ToUpper().Equals("VARH"))
                return "kvarh";
            else
                return unitName;
        }
        public string GetUnit(string unitName, bool isHTCT)
        {
            string preUnit = "k";
            if (isHTCT)
            {
                preUnit = "M";
            }
            if (string.IsNullOrEmpty(unitName))
                return string.Empty;
            else if (unitName.Trim().ToUpper().Equals("W"))
                return preUnit + "W";
            else if (unitName.Trim().ToUpper().Equals("VA"))
                return preUnit + "VA";
            else if (unitName.Trim().ToUpper().Equals("VAR"))
                return preUnit + "var";
            else if (unitName.Trim().ToUpper().Equals("WH"))
                return preUnit + "Wh";
            else if (unitName.Trim().ToUpper().Equals("VAH"))
                return preUnit + "VAh";
            else if (unitName.Trim().ToUpper().Equals("VARH"))
                return preUnit + "varh";
            else
                return unitName;
        }
        public int GetUnitScale(string unitName, int scaleDiff, string sign)
        {
            if (string.IsNullOrEmpty(unitName))
                return scaleDiff;
            else if (unitName.Trim().ToUpper().Equals("W"))
                if (sign == "+")
                    return (scaleDiff + -3);
                else
                    return (scaleDiff + 3);
            else if (unitName.Trim().ToUpper().Equals("VA"))
                if (sign == "+")
                    return (scaleDiff + -3);
                else
                    return (scaleDiff + 3);
            else if (unitName.Trim().ToUpper().Equals("VAR"))
                if (sign == "+")
                    return (scaleDiff + -3);
                else
                    return (scaleDiff + 3);
            else if (unitName.Trim().ToUpper().Equals("WH"))
                if (sign == "+")
                    return (scaleDiff + -3);
                else
                    return (scaleDiff + 3);
            else if (unitName.Trim().ToUpper().Equals("VAH"))
                if (sign == "+")
                    return (scaleDiff + -3);
                else
                    return (scaleDiff + 3);
            else if (unitName.Trim().ToUpper().Equals("VARH"))
                if (sign == "+")
                    return (scaleDiff + -3);
                else
                    return (scaleDiff + 3);
            else
                return scaleDiff;
        }
        public DLMSObjectType GetDLMSObjectType(string columnName)
        {
            if (columnName.ToLower().Contains("date") || columnName.ToLower().Contains("time") || columnName.ToLower().Contains("failure"))
            {
                return DLMSObjectType.None;
            }
            else if (columnName.ToLower().Contains("power factor"))
            {
                return DLMSObjectType.None;
            }
            else if (columnName.ToLower().Contains("energy"))
            {
                return DLMSObjectType.Energy;
            }
            else if (columnName.ToLower().Contains("demand"))
            {
                return DLMSObjectType.Demand;
            }
            else if (columnName.ToLower().Contains("power"))
            {
                return DLMSObjectType.Power;
            }
            else
            {
                return DLMSObjectType.None;
            }
        }
        public int GetUnitScale(string unitName, int scaleDiff, string sign, DLMSObjectType dlmsObjectType, out bool isHTCT, bool isLoadSurvey)
        {
            //LTCT multiplier
            int internalScalar = 3;
            isHTCT = false;
            if (!string.IsNullOrEmpty(unitName))
            {
                // only if it is not load survey
                if (!isLoadSurvey)
                {
                    if (unitName.Trim().ToUpper().Equals("W") || unitName.Trim().ToUpper().Equals("VA") || unitName.Trim().ToUpper().Equals("VAR"))
                    {
                        // power scalar is hardcoded = FD for HTCT
                        //Demand values for scalar = 4,5,6
                        if ((dlmsObjectType == DLMSObjectType.Power && scaleDiff == 0) || (dlmsObjectType == DLMSObjectType.Demand) && scaleDiff > 2)
                        {
                            internalScalar = 6;
                            isHTCT = true;

                        }
                        scaleDiff = GetScaleDiff(scaleDiff, internalScalar, sign);

                    }
                    else if (unitName.Trim().ToUpper().Equals("WH") || unitName.Trim().ToUpper().Equals("VAH") || unitName.Trim().ToUpper().Equals("VARH"))
                    {
                        //for energy, scalediff >3 = HTCT
                        if (dlmsObjectType == DLMSObjectType.Energy && scaleDiff > 3)
                        {
                            internalScalar = 6;
                            isHTCT = true;
                            if (scaleDiff == 4)
                            {
                                energyResolution = 6;
                            }
                            else if (scaleDiff == 5)
                            {
                                energyResolution = 5;
                            }
                            else if (scaleDiff == 6)
                            {
                                energyResolution = 4;
                            }

                        }
                        else if (dlmsObjectType == DLMSObjectType.Energy && scaleDiff <= 3)
                        {
                            if (scaleDiff == 0)
                            {
                                energyResolution = 3;
                            }
                            else if (scaleDiff == 1)
                            {
                                energyResolution = 2;
                            }
                            else if (scaleDiff == 2)
                            {
                                energyResolution = 1;
                            }
                            else if (scaleDiff == 3)
                            {
                                energyResolution = 0;
                            }
                        }
                        scaleDiff = GetScaleDiff(scaleDiff, internalScalar, sign);

                    }

                }
                else
                {
                    if (unitName.Trim().ToUpper().Equals("WH") || unitName.Trim().ToUpper().Equals("VAH") || unitName.Trim().ToUpper().Equals("VARH") || unitName.Trim().ToUpper().Equals("WH") || unitName.Trim().ToUpper().Equals("VAH") || unitName.Trim().ToUpper().Equals("VARH"))
                    {
                        //In loadsurvey scalar =3 is fixed for mega values
                        if (scaleDiff == 3)
                        {
                            internalScalar = 6;
                            isHTCT = true;

                        }
                        scaleDiff = GetScaleDiff(scaleDiff, internalScalar, sign);
                    }

                }

            }

            return scaleDiff;
        }
        private int GetScaleDiff(int scaleDiff, int internalScalar, string sign)
        {
            if (sign == "+")
            {
                scaleDiff = (scaleDiff + -internalScalar);
            }
            else
            {
                scaleDiff = (scaleDiff + internalScalar);
            }
            return scaleDiff;
        }
        public string GetSignValue(int sataValue, int scaleDiff, string sign, string unitName)
        {
            string signV = "";
            if (sataValue < 0)
            {
                signV = "-";
                string valx = Convert.ToString(sataValue);
                valx = valx.Replace("-", "");
                sataValue = Int32.Parse(valx);
            }
            string retVal = string.Empty;
            try
            {
                scaleDiff = GetUnitScale(unitName, scaleDiff, sign);
                string val = "0";
                int diff = scaleDiff;
                if (diff < 0)
                {
                    diff = Int32.Parse(diff.ToString().Substring(diff.ToString().IndexOf('-') + 1, diff.ToString().Length - 1));
                    sign = "-";
                }
                for (int counter = 1; counter < diff; counter++)
                    val = val + "0";
                if (sataValue == 0 && sign == "-")
                    retVal = string.Concat("0.", val);
                else if (sataValue == 0 && sign == "+")
                    retVal = string.Concat("0", val);
                else
                {
                    if (sign == "+")
                    {   
                        //BhardwajG : If the sign is positive and the scale difference is zero, no need to append a zero.
                        if (diff == 0)
                        {
                            val = "";
                        }
                        retVal = string.Concat(sataValue.ToString(), val);
                    }
                    else
                    {
                        val = sataValue.ToString();
                        if (val.Length < diff)
                        {
                            for (int i = 1; i < diff; i++)
                                val = "0" + val;
                        }
                        string rightNum = val.Substring(val.Length - diff, diff);
                        string leftNum = val.Substring(0, val.Length - diff);
                        retVal = string.Concat(leftNum, ".", rightNum);
                    }
                }
            }
            catch (Exception)
            {
            }
            string nums = CheckZero(retVal);
            nums = string.Concat(signV, nums);
            return nums;
        }
        public string GetSignValue(int sataValue, int scaleDiff, string sign, string unitName, DLMSObjectType dlmsObjectType, out bool isHTCT, bool isLoadSurvey)
        {
            string signV = "";
            isHTCT = false;
            if (sataValue < 0)
            {
                signV = "-";
                string valx = Convert.ToString(sataValue);
                valx = valx.Replace("-", "");
                sataValue = Int32.Parse(valx);
            }
            string retVal = string.Empty;
            try
            {
                scaleDiff = GetUnitScale(unitName, scaleDiff, sign, dlmsObjectType, out isHTCT, isLoadSurvey);
                string val = "0";
                int diff = scaleDiff;
                if (diff < 0)
                {
                    diff = Int32.Parse(diff.ToString().Substring(diff.ToString().IndexOf('-') + 1, diff.ToString().Length - 1));
                    sign = "-";
                }
                for (int counter = 1; counter < diff; counter++)
                    val = val + "0";
                if (sataValue == 0 && sign == "-")
                    retVal = string.Concat("0.", val);
                else if (sataValue == 0 && sign == "+")
                    retVal = string.Concat("0", val);
                else
                {
                    if (sign == "+")
                    {
                        if (diff == 0)
                        {
                            val = "";
                        }
                        retVal = string.Concat(sataValue.ToString(), val);

                    }
                    else
                    {
                        val = sataValue.ToString();
                        if (val.Length < diff)
                        {
                            for (int i = 1; i < diff; i++)
                                val = "0" + val;
                        }
                        string rightNum = val.Substring(val.Length - diff, diff);
                        string leftNum = val.Substring(0, val.Length - diff);
                        retVal = string.Concat(leftNum, ".", rightNum);
                    }
                }
            }
            catch (Exception)
            {
            }
            string nums = CheckZero(retVal);
            nums = string.Concat(signV, nums);
            return nums;
        }
        private string CheckZero(string val)
        {
            string[] unit = new string[2];
            unit[0] = unit[1] = string.Empty;
            string[] data = val.Split('.');
            unit[0] = data[0];
            if (data.Length == 2)
                unit[1] = data[1];
            if (string.IsNullOrEmpty(unit[0]))
                unit[0] = "0";
            if (unit[0] == "-")
                unit[0] = "-0";
            string valx = unit[0];
            if (valx == "0000")
                valx = "000";
            if (valx == "000")
                valx = "00";
            if (string.IsNullOrEmpty(unit[1]))
            {
                return valx;
            }
            else
                return string.Concat(valx, ".", unit[1]);
        }

        public Dictionary<string, string> GetOBISCodeValue(bool isPUMA)
        {
            OBISColumns.Add("0.0.1.0.0.255", "0");
            OBISColumns.Add("1.0.31.27.0.255", "1");
            OBISColumns.Add("1.0.51.27.0.255", "2");
            OBISColumns.Add("1.0.71.27.0.255", "3");
            OBISColumns.Add("1.0.32.27.0.255", "4");
            OBISColumns.Add("1.0.52.27.0.255", "5");
            OBISColumns.Add("1.0.72.27.0.255", "6");
            OBISColumns.Add("1.0.1.29.0.255", "7");
            OBISColumns.Add("1.0.5.29.0.255", "8");
            OBISColumns.Add("1.0.8.29.0.255", "9");
            OBISColumns.Add("1.0.9.29.0.255", "10");
            //BhardwajG:Remove database hit
            if (isPUMA)
            {
                OBISColumns.Add("1.0.14.27.0.255", "11");
                OBISColumns.Add("0.0.96.1.152.255", "12");
            }
            return OBISColumns;
        }

        public Dictionary<string, string> GetOBISCodeColumnNames(bool isPUMA)
        {
            OBISLoadSurveyColumns.Add("0.0.1.0.0.255", "realTimeClockDateandTime");
            OBISLoadSurveyColumns.Add("1.0.31.27.0.255", "rPhaseCurrent");
            OBISLoadSurveyColumns.Add("1.0.51.27.0.255", "yPhaseCurrent");
            OBISLoadSurveyColumns.Add("1.0.71.27.0.255", "bPhaseCurrent");
            OBISLoadSurveyColumns.Add("1.0.32.27.0.255", "rPhaseVoltage");
            OBISLoadSurveyColumns.Add("1.0.52.27.0.255", "yPhaseVoltage");
            OBISLoadSurveyColumns.Add("1.0.72.27.0.255", "bPhaseVoltage");
            OBISLoadSurveyColumns.Add("1.0.1.29.0.255", "blockEnergykWh");
            OBISLoadSurveyColumns.Add("1.0.5.29.0.255", "blockEnergykvarhlag");
            OBISLoadSurveyColumns.Add("1.0.8.29.0.255", "blockEnergykvarhlead");
            OBISLoadSurveyColumns.Add("1.0.9.29.0.255", "blockEnergykVAh");
            //BhardwajG : Remove database hit.
            if (isPUMA)
            {
                OBISLoadSurveyColumns.Add("1.0.14.27.0.255", "frequency");
                OBISLoadSurveyColumns.Add("0.0.96.1.152.255", "tamperStatus");
            }

            return OBISLoadSurveyColumns;
        }
    }
}
