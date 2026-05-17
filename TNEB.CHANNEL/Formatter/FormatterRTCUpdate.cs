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
using CAB.IECFramework.Utility;

namespace CAB.IECChannel.Formatter
{
    public class FormatterRTCUpdate
    {
        public void GetData(string data, ref string[,] rtcUpdate)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.RTCUPDATEEXPRESSSION);
                string[] programmingData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    programmingData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateData(programmingData);
                counter = 0;
                int MaxLength = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    if (tempData.Length > MaxLength)
                        MaxLength = tempData.Length;
                    counter++;
                }
                rtcUpdate = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        rtcUpdate[counter, MaxLength] = tempData[MaxLength];
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
                int index = 0;
                bool OuterFlag = true;
                bool InnerFlag = true;
                RTCUpdateEntity rTCUpdateEntity = new RTCUpdateEntity();
                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter == 1)
                    {
                        rTCUpdateEntity.MeterID = tempData[counter].Substring(4).Trim();
                    }
                    else if (counter == 2)
                    {
                        rTCUpdateEntity.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                    }
                    //else if (counter == 3)
                    //{
                    //    rTCUpdateEntity.CMRIID = Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType = Convert.ToString(tempData[counter]).Substring(0, 1);
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        rTCUpdateEntity.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        rTCUpdateEntity.CMRIType = "Sands";
                    //    else
                    //        rTCUpdateEntity.CMRIType = "BCS";
                    //}
                    else if (counter == 3)
                    {
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1)) == false)
                            OuterFlag = false;
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1)) == false)
                            InnerFlag = false;
                        if (InnerFlag || OuterFlag)
                        {
                            rTCUpdateEntity.TotalRTCUpdates = Convert.ToInt32(tempData[counter].Substring(index, 2), 16).ToString();
                            index += 2;
                            rTCUpdateEntity.PreviousRTC1 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC1 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC2 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC2 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC3 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC3 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC4 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC4 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC5 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC5 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC6 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC6 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC7 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC7 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC8 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC8 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC9 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC9 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.PreviousRTC10 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                            rTCUpdateEntity.CurrentRTC10 = FormatterCommon.GetTimeStamp(tempData[counter], index);
                            index += 10;
                        }
                    }
                }
                if (!(InnerFlag == false && OuterFlag == false))
                    billingGeneralNFEntity.listRTCUpdate.Add(rTCUpdateEntity);
            }
            catch (Exception)
            {
                //MessageBox.Show("Corrupted RTC data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SplitDataForSPhase(Dictionary<string, string> dicOBISandData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            try
            {             
                RTCUpdateEntity rTCUpdateEntity = new RTCUpdateEntity();               
                rTCUpdateEntity.MeterID = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.1");               
                rTCUpdateEntity.ReadingDateTime = 0;                                     
                rTCUpdateEntity.TotalRTCUpdates = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.99");
                rTCUpdateEntity.CurrentRTC1 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.01"));
                rTCUpdateEntity.CurrentRTC2 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.02"));
                rTCUpdateEntity.CurrentRTC3 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.03"));
                rTCUpdateEntity.CurrentRTC4 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.04"));
                rTCUpdateEntity.CurrentRTC5 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.05"));
                rTCUpdateEntity.CurrentRTC6 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.06"));
                rTCUpdateEntity.CurrentRTC7 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.07"));
                rTCUpdateEntity.CurrentRTC8 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.08"));
                rTCUpdateEntity.CurrentRTC9 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.09")); 
                rTCUpdateEntity.CurrentRTC10 = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.10.10"));
                billingGeneralNFEntity.listRTCUpdate.Add(rTCUpdateEntity);
            }
            catch (Exception)
            {
                //MessageBox.Show("Corrupted RTC data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

