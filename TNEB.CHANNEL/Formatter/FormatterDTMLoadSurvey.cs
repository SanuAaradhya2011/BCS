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

namespace CAB.IECChannel.Formatter
{
    public class FormatterDTMLoadSurvey
    {
        public void GetData(string data, ref  string[,] dtmLoadSurvey)
        {
            try
            {
                int counter = 0;
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.DTMLOADSURVEYEXPRESSSION);
                string[] dtmLoadSurveyData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    dtmLoadSurveyData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateDataForSPhase(dtmLoadSurveyData);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void SplitData(string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            try
            {
                if (tempData == null)
                    return;
                int index = 0;
               // billingGeneralNFEntity.DTMLoadSurveyMeterData = new MeterDataEntity();

                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter == 1)
                    {
                        //billingGeneralNFEntity.DTMLoadSurveyMeterData.MeterID = tempData[counter].Substring(4).Trim();
                        continue;
                    }
                    if (counter == 2)
                    {
                        //billingGeneralNFEntity.DTMLoadSurveyMeterData.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                        continue;
                    }
                    else
                    {
                        index = 0;
                        string bccData = tempData[counter];
                        if (string.IsNullOrEmpty(bccData))
                            return;
                        tempData[counter] = bccData.Substring(0, bccData.IndexOf("\x03") + 2);
                        bool Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        if (!Flag)
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        if (!Flag)
                            return;
                        do
                        {
                            DTMLoadSurveyEntity loadSurvey = new DTMLoadSurveyEntity();
                            loadSurvey.RPhaseKW = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.000}"); index += 4;
                            loadSurvey.YPhaseKW = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.000}"); index += 4;
                            loadSurvey.BPhaseKW = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.000}"); index += 4;
                            string tempVal = FormatterCommon.FilterData(tempData[counter], index, 4);
                            loadSurvey.RPhaseType = ((Int32.Parse(tempVal) & 0x8000) == 0) ? "Lag" : "Lead";
                            tempVal = FormatterCommon.FilterData(tempData[counter], index, 4);
                            tempVal = (Int32.Parse(tempVal) & 0x7FFF).ToString();
                            loadSurvey.RPhaseKVAr = (Convert.ToDouble(tempVal) / 1000).ToString("0.000"); index += 4;

                            tempVal = FormatterCommon.FilterData(tempData[counter], index, 4);
                            loadSurvey.YPhaseType = ((Int32.Parse(tempVal) & 0x8000) == 0) ? "Lag" : "Lead";
                            tempVal = FormatterCommon.FilterData(tempData[counter], index, 4);
                            tempVal = (Int32.Parse(tempVal) & 0x7FFF).ToString();
                            loadSurvey.YPhaseKVAr = (Convert.ToDouble(tempVal) / 1000).ToString("0.000"); index += 4;

                            tempVal = FormatterCommon.FilterData(tempData[counter], index, 4);
                            loadSurvey.BPhaseType = ((Int32.Parse(tempVal) & 0x8000) == 0) ? "Lag" : "Lead";
                            tempVal = FormatterCommon.FilterData(tempData[counter], index, 4);
                            tempVal = (Int32.Parse(tempVal) & 0x7FFF).ToString();
                            loadSurvey.BPhaseKVAr = (Convert.ToDouble(tempVal) / 1000).ToString("0.000"); index += 4;

                            loadSurvey.RPhaseVoltage = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.00}"); index += 4;
                            loadSurvey.YPhaseVoltage = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.00}"); index += 4;
                            loadSurvey.BPhaseVoltage = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.00}"); index += 4;
                            loadSurvey.PowerDownTime = FormatterCommon.FilterData(tempData[counter], index, 2); index += 2;
                            loadSurvey.KWh = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.000}"); index += 4;
                            loadSurvey.KVAh = FormatterCommon.ParseData(tempData[counter], index, 4, 1000, "{0:0.000}"); index += 4;
                            loadSurvey.DTMDateTime = FormatterCommon.DTMParseDate(tempData[counter].Substring(index, 10));
                            if (loadSurvey.DTMDateTime == 0 || loadSurvey.DTMDateTime.ToString().Length < 10)
                            {
                                index += 10;
                                continue;
                            }
                            billingGeneralNFEntity.DTMLoadSurvey.Add(loadSurvey);

                            index += 10;
                        } while (Convert.ToChar(tempData[counter].Substring(index, 1)) != Convert.ToChar(0x3));
                    }
                }
            }
            catch (Exception)
            {
               // MessageBox.Show("Corrupted DTM Loadsurvey data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}