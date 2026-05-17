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
using CAB.IECChannel.ReadOut;
using System.Windows.Forms;
using LTCTBLL;

namespace CAB.IECChannel.Formatter
{
    public class FormatterDTMDailyProfile
    {
        private bool isCumulative_kWh = false;
        private bool isCumulative_kVArh_lag = false;
        private bool isCumulative_kVArh_lead = false;
        private bool isCumulative_kVAh = false;
        private bool isMD1 = false;
        private bool isMD1TimeStamp = false;
        private bool isMD2 = false;
        private bool isMD2TimeStamp = false;
        private bool isPowerOnHours = false;
        private string AvailableDays = "";
        private string MaximumDays = "";
        private string parameters = "";
        public void GetData(string data, ref  string[,] dtmLoadSurvey)
        {
            try
            {
                int counter = 0;
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.DTMDAILYPROFILEEXPRESSSION);
                string[] dtmLoadSurveyData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    dtmLoadSurveyData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateData(dtmLoadSurveyData);
                counter = 0;
                int MaxLength = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    if (tempData.Length > MaxLength)
                        MaxLength = tempData.Length;
                    counter++;
                }
                dtmLoadSurvey = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        dtmLoadSurvey[counter, MaxLength] = tempData[MaxLength];
                    counter++;
                }

            }
            catch (Exception)
            {

            }
        }
        public void GetDataForSPhase(string data, ref  string[,] dtmLoadSurvey)
        {
            try
            {
                int counter = 0;
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.DTMDAILYPROFILEEXPRESSSIONFORSPHASE);
                string[] dtmLoadSurveyData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    dtmLoadSurveyData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateDataForSPhase(dtmLoadSurveyData);
                counter = 0;
                int MaxLength = 0;

                //if(availableData.Length>0)
                //{
                //    string[] tempData1 = availableData[0].Split('/');
                //}
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    if (tempData.Length > MaxLength)
                        MaxLength = tempData.Length;
                    counter++;
                }
                dtmLoadSurvey = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        dtmLoadSurvey[counter, MaxLength] = tempData[MaxLength];
                    counter++;
                }

            }
            catch (Exception)
            {

            }
        }
        private void CheckParameter(string data, int index)
        {
            try
            {
                isCumulative_kWh = false;
                isCumulative_kVArh_lag = false;
                isCumulative_kVArh_lead = false;
                isCumulative_kVAh = false;
                isMD1 = false;
                isMD1TimeStamp = false;
                isMD2 = false;
                isMD2TimeStamp = false;
                isPowerOnHours = false;
                parameters = string.Empty;
                string binParam = "";
                binParam = Convert.ToString(Convert.ToInt32(data.Substring(index, 2), 16), 2);
                while (binParam.Length < 6) { binParam = "0" + binParam; }
                // int parameter1 = Int32.Parse(FormatterCommon.FilterData(data, index, 2)); index += 2;                 
                /* GKG 21/01/2013 TANGEDCO ISSUE*/
                // if (UtilityDetails.ShowPowerOnHours) { isPowerOnHours = true; parameters = string.Concat(parameters, "PowerOnHours as 'Power On Hours (HH)',"); }
                //if (binParam.Substring(6, 1) == "1") { isCumulative_kWh = true; parameters = string.Concat(parameters, "CumulativekWh as 'Cumulative kWh',"); }
                //if (binParam.Substring(5, 1) == "1") { isCumulative_kVArh_lag = true; parameters = string.Concat(parameters, "CumulativekVArh_lag as 'Cumulative kVArh (Lag)',"); }
                //if (binParam.Substring(4, 1) == "1") { isCumulative_kVArh_lead = true; parameters = string.Concat(parameters, "CumulativekVArh_lead as 'Cumulative kVArh (Lead)',"); }
                //if (binParam.Substring(3, 1) == "1") { isCumulative_kVAh = true; parameters = string.Concat(parameters, "CumulativekVAh as 'Cumulative kVAh',"); }
                //if (binParam.Substring(2, 1) == "1") { isMD1TimeStamp = isMD1 = true; parameters = string.Concat(parameters, "DailyMD1 as 'Daily MD1',", "MD1TimeStamp as 'MD1 Time Stamp',"); }
                //if (binParam.Substring(1, 1) == "1") { isMD2TimeStamp = isMD2 = true; parameters = string.Concat(parameters, "DailyMD2 as 'Daily MD2',", "MD2TimeStamp as 'MD2 Time Stamp',"); }



                //if (UtilityDetails.ShowPowerOnHours)
                //{
                //    if (UtilityDetails.ShowPowerOnHours) { isPowerOnHours = true; parameters = string.Concat(parameters, "PowerOnHours as 'Power On Hours (HH)',"); }
                //    if (binParam.Substring(6, 1) == "1") { isCumulative_kWh = true; parameters = string.Concat(parameters, "CumulativekWh as 'Cumulative kWh',"); }
                //    if (binParam.Substring(5, 1) == "1") { isCumulative_kVArh_lag = true; parameters = string.Concat(parameters, "CumulativekVArh_lag as 'Cumulative kVArh (Lag)',"); }
                //    if (binParam.Substring(4, 1) == "1") { isCumulative_kVArh_lead = true; parameters = string.Concat(parameters, "CumulativekVArh_lead as 'Cumulative kVArh (Lead)',"); }
                //    if (binParam.Substring(3, 1) == "1") { isCumulative_kVAh = true; parameters = string.Concat(parameters, "CumulativekVAh as 'Cumulative kVAh',"); }
                //    if (binParam.Substring(2, 1) == "1") { isMD1TimeStamp = isMD1 = true; parameters = string.Concat(parameters, "DailyMD1 as 'Daily MD1',", "MD1TimeStamp as 'MD1 Time Stamp',"); }
                //    if (binParam.Substring(1, 1) == "1") { isMD2TimeStamp = isMD2 = true; parameters = string.Concat(parameters, "DailyMD2 as 'Daily MD2',", "MD2TimeStamp as 'MD2 Time Stamp',"); }
                //}
                //else
                //{

                if (binParam.Substring(5, 1) == "1") { isCumulative_kWh = true; parameters = string.Concat(parameters, "CumulativekWh as 'Cumulative kWh',"); }
                if (binParam.Substring(4, 1) == "1") { isCumulative_kVArh_lag = true; parameters = string.Concat(parameters, "CumulativekVArh_lag as 'Cumulative kVArh (Lag)',"); }
                if (binParam.Substring(3, 1) == "1") { isCumulative_kVArh_lead = true; parameters = string.Concat(parameters, "CumulativekVArh_lead as 'Cumulative kVArh (Lead)',"); }
                if (binParam.Substring(2, 1) == "1") { isCumulative_kVAh = true; parameters = string.Concat(parameters, "CumulativekVAh as 'Cumulative kVAh',"); }
                if (binParam.Substring(1, 1) == "1") { isMD1TimeStamp = isMD1 = true; parameters = string.Concat(parameters, "DailyMD1 as 'Daily MD1',", "MD1TimeStamp as 'MD1 Time Stamp',"); }
                if (binParam.Substring(0, 1) == "1") { isMD2TimeStamp = isMD2 = true; parameters = string.Concat(parameters, "DailyMD2 as 'Daily MD2',", "MD2TimeStamp as 'MD2 Time Stamp',"); }

                //}
                /* GKG 21/01/2013 TANGEDCO ISSUE*/



                index += 4;

                MaximumDays = FormatterCommon.FilterData(data, index, 4); index += 4;
                AvailableDays = FormatterCommon.FilterData(data, index, 4); index += 4;
                if (!String.IsNullOrEmpty(parameters))
                    parameters = parameters.Substring(0, parameters.Length - 1);
            }
            catch (Exception)
            {
            }
        }

        public void SplitData(string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            int index = 0;
            try
            {
                if (tempData == null)
                    return;
                bool Flag = false;
                DTMDailyProfileData DTMDailyProfileData = new DTMDailyProfileData();
                DTMDailyProfileData.DTMDailyProfileMeterData = new IECMeterDataEntity();
                DTMDailyProfileData.DTMDailyProfile = new List<DTMDailyProfileEntity>();
                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter == 1)
                    {
                        DTMDailyProfileData.DTMDailyProfileMeterData.MeterID = tempData[counter].Substring(4).Trim();
                        continue;
                    }
                    else if (counter == 2)
                    {
                        DTMDailyProfileData.DTMDailyProfileMeterData.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                        continue;
                    }
                    //else if (counter == 3)
                    //{
                    //    billingGeneralNFEntity.DTMDailyProfileMeterData.CMRIID = Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType = Convert.ToString(tempData[counter]).Substring(0, 1);
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        billingGeneralNFEntity.DTMDailyProfileMeterData.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        billingGeneralNFEntity.DTMDailyProfileMeterData.CMRIType = "Sands";
                    //    else
                    //        billingGeneralNFEntity.DTMDailyProfileMeterData.CMRIType = "BCS";
                    //}
                    else if (counter == 3)
                    {
                        index = 0;
                        Flag = false;
                        string bccData = tempData[counter];
                        if (string.IsNullOrEmpty(bccData)) return;
                        tempData[counter] = bccData.Substring(0, bccData.IndexOf("\x03") + 2);
                        Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        if (!Flag) Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        if (!Flag) return;
                        index += 12;
                        CheckParameter(tempData[counter], index);
                    }
                    else
                    {
                        index = 0;
                        string bccData = tempData[counter];
                        if (string.IsNullOrEmpty(bccData))
                            return;
                        tempData[counter] = bccData.Substring(0, bccData.IndexOf("\x03") + 2);
                        Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        if (!Flag)
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        if (!Flag)
                            return;

                        /* GKG 28/01/2013 TANGEDCO ISSUE*/
                        //do
                        //{
                        //    DTMDailyProfileEntity dailyProfile = new DTMDailyProfileEntity();
                        //    if (isCumulative_kWh) { dailyProfile.CumulativekWh = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                        //    if (isCumulative_kVArh_lag) { dailyProfile.CumulativekVArh_lag = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                        //    if (isCumulative_kVArh_lead == true) { dailyProfile.CumulativekVArh_lead = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                        //    if (isCumulative_kVAh == true) { dailyProfile.CumulativekVAh = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                        //    if (isMD1) { dailyProfile.DailyMD1 = FormatterCommon.FilterData(tempData[counter], index, 8, 1000000, "0.0000"); index += 8; dailyProfile.MD1TimeStamp = FormatterCommon.DTMDailyProfileDate(tempData[counter], index, false); index += 10; }
                        //    if (isMD2) { dailyProfile.DailyMD2 = FormatterCommon.FilterData(tempData[counter], index, 8, 1000000, "0.0000"); index += 8; dailyProfile.MD2TimeStamp = FormatterCommon.DTMDailyProfileDate(tempData[counter], index, false); index += 10; }
                        //    if (isPowerOnHours){dailyProfile.PowerOnHours = FormatterCommon.FilterData(tempData[counter], index, 8); index += 8;}
                        //    dailyProfile.DailyProfileDate = FormatterCommon.DTMDailyProfileDate(tempData[counter], index, true); index += 6;                       
                        //    dailyProfile.AvailableDays = AvailableDays;
                        //    dailyProfile.MaximumDays = MaximumDays;
                        //    dailyProfile.Parameters = parameters;
                        //    DTMDailyProfileData.DTMDailyProfile.Add(dailyProfile);
                        //} while (Convert.ToChar(tempData[counter].Substring(index, 1)) != Convert.ToChar(0x3));
                        int dataLength = tempData[counter].Length;
                        int recordLength = 0;

                        if (isCumulative_kWh) { recordLength += 12; }
                        if (isCumulative_kVArh_lag) { recordLength += 12; }
                        if (isCumulative_kVArh_lead == true) { recordLength += 12; }
                        if (isCumulative_kVAh == true) { recordLength += 12; }
                        if (isMD1) { recordLength += 8; recordLength += 10; }
                        if (isMD2) { recordLength += 8; recordLength += 10; }
                        if (isPowerOnHours) { recordLength += 8; }
                        recordLength += 6;

                        while ((index + recordLength) <= dataLength)
                        {
                            DTMDailyProfileEntity dailyProfile = new DTMDailyProfileEntity();
                            if (isCumulative_kWh) { dailyProfile.CumulativekWh = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                            if (isCumulative_kVArh_lag) { dailyProfile.CumulativekVArh_lag = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                            if (isCumulative_kVArh_lead == true) { dailyProfile.CumulativekVArh_lead = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                            if (isCumulative_kVAh == true) { dailyProfile.CumulativekVAh = FormatterCommon.FilterData(tempData[counter], index, 12, 1000000, "0.0000"); index += 12; }
                            if (isMD1) { dailyProfile.DailyMD1 = FormatterCommon.FilterData(tempData[counter], index, 8, 1000000, "0.0000"); index += 8; dailyProfile.MD1TimeStamp = FormatterCommon.DTMDailyProfileDate(tempData[counter], index, false); index += 10; }
                            if (isMD2) { dailyProfile.DailyMD2 = FormatterCommon.FilterData(tempData[counter], index, 8, 1000000, "0.0000"); index += 8; dailyProfile.MD2TimeStamp = FormatterCommon.DTMDailyProfileDate(tempData[counter], index, false); index += 10; }
                            if (isPowerOnHours) { dailyProfile.PowerOnHours = FormatterCommon.FilterData(tempData[counter], index, 8); index += 8; }
                            dailyProfile.DailyProfileDate = FormatterCommon.DTMDailyProfileDate(tempData[counter], index, true); index += 6;
                            dailyProfile.AvailableDays = AvailableDays;
                            dailyProfile.MaximumDays = MaximumDays;
                            dailyProfile.Parameters = parameters;
                            if (dailyProfile.DailyProfileDate == 0)
                            {
                                // In case of date and time is 0
                            }
                            else
                            {
                                DTMDailyProfileData.DTMDailyProfile.Add(dailyProfile);
                            }
                        };

                        /* GKG 28/01/2013 TANGEDCO ISSUE*/

                    }
                }
                billingGeneralNFEntity.listDTMDailyProfileData.Add(DTMDailyProfileData);
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted Daily Profile data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SplitDataForSPhase(string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {            
            try
            {
                if (tempData == null)
                    return;               
                DTMDailyProfileData DTMDailyProfileData = new DTMDailyProfileData();
                DTMDailyProfileData.DTMDailyProfileMeterData = new IECMeterDataEntity();
                DTMDailyProfileData.DTMDailyProfile = new List<DTMDailyProfileEntity>();

                parameters = string.Empty;
                isCumulative_kWh = true; parameters = string.Concat(parameters, "CumulativekWh as 'Cumulative kWh',");
                isCumulative_kVAh = true; parameters = string.Concat(parameters, "CumulativekVAh as 'Cumulative kVAh',");
                if (!String.IsNullOrEmpty(parameters))
                    parameters = parameters.Substring(0, parameters.Length - 1);

                for (int counter = 1; counter < tempData.Length - 1; counter++)//fix limit to 4, due to duplicate rows occurse
                {
                    if (counter == 1)
                    {
                        DTMDailyProfileData.DTMDailyProfileMeterData.MeterID = tempData[counter].Substring(13,16).Trim();
                        continue;
                    }
                    else if (counter == 2)
                    {
                        DTMDailyProfileData.DTMDailyProfileMeterData.ReadingDateTime = 0;// Convert.ToInt64(tempData[counter]);
                        continue;
                    }                    
                    else if (counter == 3)
                    { 
                        string[] dailyLoadData = tempData[counter].Split('(', ')');
                        for (int count = 0; count < dailyLoadData.Length - 1; count++) // Loop was breaking for last record
                        {
                            count++;
                            DTMDailyProfileEntity dailyProfile = new DTMDailyProfileEntity();
                            string kwh = dailyLoadData[count].Substring(6, 2) + dailyLoadData[count].Substring(4, 2) + dailyLoadData[count].Substring(2, 2) + dailyLoadData[count].Substring(0, 2); ;
                            string kVAh = dailyLoadData[count].Substring(14, 2) + dailyLoadData[count].Substring(12, 2) + dailyLoadData[count].Substring(10, 2) + dailyLoadData[count].Substring(8, 2); ;

                            if (isCumulative_kWh)
                                dailyProfile.CumulativekWh = FormatterCommon.FilterData(kwh, 0, 8, 1000, "0.000");
                            if (isCumulative_kVAh)
                                dailyProfile.CumulativekVAh = FormatterCommon.FilterData(kVAh, 0, 8, 1000, "0.000");
                            dailyProfile.DailyProfileDate = FormatterCommon.DTMDailyProfileDateSP(dailyLoadData[count].Substring(16, 8));
                            dailyProfile.AvailableDays = AvailableDays;
                            dailyProfile.MaximumDays = "90";
                            dailyProfile.Parameters = parameters;
                            if (dailyProfile.DailyProfileDate == 0)
                            {
                                break;
                            }
                            else
                            {
                                DTMDailyProfileData.DTMDailyProfile.Add(dailyProfile);
                            }
                        }
                        billingGeneralNFEntity.listDTMDailyProfileData.Add(DTMDailyProfileData);
                    }
                }
                billingGeneralNFEntity.listDTMDailyProfileData.Add(DTMDailyProfileData);
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted Daily Profile data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SplitDataSPhaseConfig(string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity, int dailySurveyDaysCnt)
        {
            try
            {
                if (tempData == null)
                    return;

                int interval = 0;
                DTMDailyProfileData DailySurveyData = new DTMDailyProfileData();
                DailySurveyData.DTMDailyProfile = new List<DTMDailyProfileEntity>();
                DailySurveyData.DTMDailyProfileMeterData = new IECMeterDataEntity();
                int Days = 90;

                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter > dailySurveyDaysCnt - 1)
                        continue;

                    if (counter == 1)
                    {
                        DailySurveyData.DTMDailyProfileMeterData.MeterID = tempData[counter].Substring(13, 16).Trim();
                        continue;
                    }
                    else if (counter == 2)
                    {
                        DailySurveyData.DTMDailyProfileMeterData.ReadingDateTime = 0;// Convert.ToInt64(tempData[counter]);
                        continue;
                    }
                    else if (counter == 3)
                    {
                    //    string[] dailyLoadData = tempData[counter].Split('(', ')');
                    //    string paarameters = dailyLoadData[1];

                    //    if (paarameters.Length >= 12)
                    //    {
                    //        Days = Convert.ToByte(paarameters.Substring(8, 2), 16);
                    //    }
                    //    else if (paarameters.ToUpper().Contains("ER"))
                    //    {
                    //        Days = 90;
                    //    }

                        string[] dailyprofileData = tempData[counter].Split('(', ')');

                        string paarameter = dailyprofileData[1];
                      
                        if ((paarameter != "") && (!paarameter.Contains("ER30")))
                        {
                            Days = Convert.ToByte(paarameter.Substring(6, 2), 16);
                            string byte1 = Convert.ToString(Convert.ToInt32(paarameter.Substring(0, 2), 16), 2);
                            string byte2 = Convert.ToString(Convert.ToInt32(paarameter.Substring(2, 2), 16), 2);
                            while (byte1.Length < 8) { byte1 = "0" + byte1; }
                            while (byte2.Length < 8) { byte2 = "0" + byte2; }
                            byte1 = ReverseMyString(byte1);
                            byte2 = ReverseMyString(byte2);
                            paarameter = byte1 + byte2;
                        }
                        else
                        {
                            paarameter = "1110000000000000";
                        }                         
                           


                        bool isEnergy = paarameter.Substring(0, 1) == "1" ? true : false;
                        bool isEnergykvah = paarameter.Substring(1, 1) == "1" ? true : false;
                        bool isLSTimeStamp = paarameter.Substring(2, 1) == "1" ? true : false;
                        bool isPowerOnHours = paarameter.Substring(3, 1) == "1" ? true : false;
                        bool iskVArhLead = paarameter.Substring(4, 1) == "1" ? true : false;
                        bool isPhaseVolatgeMin = paarameter.Substring(5, 1) == "1" ? true : false;
                        bool isPhaseVolatgeMax = paarameter.Substring(6, 1) == "1" ? true : false;
                        bool isPhaseVolatgeAvg = paarameter.Substring(7, 1) == "1" ? true : false;
                        bool isAveragePhaseCurrent = paarameter.Substring(8, 1) == "1" ? true : false;
                        bool isAverageNeutralCurrent = paarameter.Substring(9, 1) == "1" ? true : false;
                        bool isAveragePF = paarameter.Substring(10, 1) == "1" ? true : false;
                        bool isAverageCurrent = paarameter.Substring(11, 1) == "1" ? true : false;


                        for (int count = 2; count < dailyprofileData.Length - 1; count++)
                        {
                            count++;
                            parameters = string.Empty;
                            string data = dailyprofileData[count];
                            int startIndex = 0;                           
                            while (startIndex < data.Length)
                            {
                                DTMDailyProfileEntity dailySurvey = new DTMDailyProfileEntity();
                                if (isEnergy)
                                {
                                    
                                    dailySurvey.CumulativekWh = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 8)), 0, 8, 1000, "0.000");
                                    startIndex = startIndex + 8;
                                    parameters =  string.Concat(parameters, "CumulativekWh as 'Cumulative kWh',");
                                }
                                if (isEnergykvah)
                                {
                                    dailySurvey.CumulativekVAh = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 8)), 0, 8, 1000, "0.000");
                                    startIndex = startIndex + 8;
                                    parameters = string.Concat(parameters, "CumulativekVAh as 'Cumulative kVAh',");
                                }
                                if (isLSTimeStamp)
                                {
                                    dailySurvey.DailyProfileDate = Convert.ToInt64(FormatterCommon.DTMDailyProfileDateSP(data.Substring(startIndex, 8)));
                                    startIndex = startIndex + 8;
                                }
                                if (isPowerOnHours)
                                {
                                    // Two columns
                                    dailySurvey.PowerOnHours = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 8)), 0, 8, 1, "0000");
                                    startIndex = startIndex + 8;
                                    parameters = string.Concat(parameters, "PowerOnHours as 'Power On Duration',");
                                }

                                if (iskVArhLead)
                                {
                                    dailySurvey.CumulativekVArh_lead = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 8)), 0, 8, 1000, "0.000");
                                    //if (dailySurvey.DemandKVARLead.ToString() == "65.278") // when meter is power off then default value is 65.278, it should be 0.000
                                    //    dailySurvey.DemandKVARLead = "0.000";
                                    startIndex = startIndex + 4;
                                    parameters = parameters + "DemandKVARLead as 'Demand kvar (lead)'" + ",";
                                }

                                if (isPhaseVolatgeMin)
                                {
                                    dailySurvey.MinAvgVoltage = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 4)), 0, 4, 1000, "0.000");
                                    startIndex = startIndex + 4;
                                    if (!parameters.Contains("AvgVoltage as 'Average Voltage'"))
                                        parameters = parameters + "AvgVoltage as 'Average Voltage'" + ",";
                                }
                                if (isPhaseVolatgeMax)
                                {
                                    dailySurvey.MaxAvgVoltage = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 4)), 0, 4, 1000, "0.000");
                                    startIndex = startIndex + 4;
                                    if (!paarameter.Contains("AvgVoltage as 'Average Voltage'"))
                                        parameters = parameters + "AvgVoltage as 'Average Voltage'" + ",";
                                }
                                if (isPhaseVolatgeAvg)
                                {
                                    dailySurvey.AVERAGeVoltage = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 4)), 0, 4, 100, "0.000");
                                    startIndex = startIndex + 4;
                                    if (!parameters.Contains("AvgVoltage as 'Average Voltage'"))
                                        parameters = parameters + "AvgVoltage as 'Average Voltage'" + ",";
                                }                                 
                                if (isAveragePhaseCurrent)
                                {
                                    dailySurvey.AVERAGECURRENT = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 4)), 0, 4, 1000, "0.000");
                                    startIndex = startIndex + 4;
                                    if (!parameters.Contains("AvgCurrent as 'Average Current'"))
                                        parameters = parameters + "AvgCurrent as 'Average Current'" + ",";
                                }
                                if (isAverageNeutralCurrent)
                                {
                                    dailySurvey.AVERAGEneautralCURRENT = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 4)), 0, 4, 1000, "0.000");
                                    startIndex = startIndex + 2;
                                    if (!parameters.Contains("AvgCurrent as 'Average Current'"))
                                        parameters = parameters + "AvgCurrent as 'Average Current'" + ",";
                                }
                                 if (isAveragePF)
                                {
                                    dailySurvey.POWERFACTOR = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 4)), 0, 4, 1000, "0.000");
                                    startIndex = startIndex + 2;
                                    parameters = parameters + "PowerFactor" + ",";
                                }                             

                                if (isAverageCurrent)
                                {
                                    dailySurvey.MaxAvgCurrent = FormatterCommon.FilterData(ReverseString(data.Substring(startIndex, 4)), 0, 4, 100, "0.000");
                                    startIndex = startIndex + 2;
                                    if (!parameters.Contains("AvgCurrent as 'Average Current'"))
                                        parameters = parameters + "AvgCurrent as 'Average Current'" + ",";
                                } 
                                                                                  
                                if (!String.IsNullOrEmpty(parameters))
                                    parameters = parameters.Substring(0, parameters.Length - 1);
                                Days = Days + 1;
                                dailySurvey.AvailableDays = Days.ToString();
                                dailySurvey.Parameters = parameters;

                                if (dailySurvey.DailyProfileDate == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    DailySurveyData.DTMDailyProfile.Add(dailySurvey);
                                }                                
                            }
                        }
                    }
                    billingGeneralNFEntity.listDTMDailyProfileData.Add(DailySurveyData);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Corrupted Load Survey data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string ReverseMyString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        private string ReverseString(string str)
        {
            int count = str.Length - 2;
            string revString = "";
            for (count = str.Length - 2; count >= 0; count -= 2)
            {
                revString += str.Substring(count, 2);
            }
            return revString;
        }
    }
}
