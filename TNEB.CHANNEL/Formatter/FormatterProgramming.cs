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
    public class FormatterProgramming
    {
        public void GetData(string data, ref string[,] programming)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.PROGRAMMINGEXPRESSSION);
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
                programming = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        programming[counter, MaxLength] = tempData[MaxLength];
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
                TransactionData transactionData = new TransactionData();
                ProgrammingEntity programmingEntity = new ProgrammingEntity();
                transactionData.programmingData = new List<ProgrammingEntity>();
                IECMeterDataEntity meterData = new IECMeterDataEntity();
                for (int counter = 1; counter < tempData.Length; 
                    counter++)
                {
                    if (counter == 1)
                    {
                        programmingEntity.MeterID = tempData[counter].Substring(4).Trim();
                        meterData.MeterID = programmingEntity.MeterID;
                    }
                    else if (counter == 2)
                    {
                        programmingEntity.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                        meterData.ReadingDateTime=programmingEntity.ReadingDateTime;
                    }
                    //else if (counter == 3)
                    //{
                    //    programmingEntity.CMRIID = Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType = Convert.ToString(tempData[counter]).Substring(0, 1);
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        programmingEntity.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        programmingEntity.CMRIType = "Sands";
                    //    else
                    //        programmingEntity.CMRIType = "BCS";
                    //}
                    else if (counter == 3)
                    {
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1)) == false)
                            OuterFlag = false;
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1)) == false)
                            InnerFlag = false;
                        if (InnerFlag || OuterFlag)
                        {
                            int totalProgrammingUpdates = Convert.ToInt32(tempData[counter].Substring(index, 2), 16);
                            //if (totalProgrammingUpdates > 10) totalProgrammingUpdates = 10;
                            index += 2;
                            int timeStampCount = 9;
                            for (int loop = 0; loop < 10; loop++)
                            {
                                programmingEntity.TotalProgrammingUpdates = Convert.ToString( totalProgrammingUpdates);
                                programmingEntity.LastTimestamp = FormatterCommon.GetTimeStamp(tempData[counter], index);
                                index += 10;
                                programmingEntity.UpdateSequence = FormatterCommon.ColText(timeStampCount--);
                                FormatterCommon.SetDescription(tempData[counter].Substring(index, 6).ToString(), programmingEntity);
                                transactionData.programmingData.Add(programmingEntity);
                                //billingGeneralNFEntity.Programming.Add(programmingEntity);
                                index += 6;
                                programmingEntity = new ProgrammingEntity();
                            }
                        }
                    }
                }
                transactionData.meterDataEntity = meterData;
                if(programmingEntity.TotalProgrammingUpdates !=null)
                transactionData.programmingData.Add(programmingEntity);
                billingGeneralNFEntity.listTransactionData.Add(transactionData);
            }
            catch (Exception)
            {
               // MessageBox.Show("Corrupted Programming data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void SplitDataForSPhase(Dictionary<string, string> dicOBISandData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            try
            {
                IECMeterDataEntity meterData = new IECMeterDataEntity();
                TransactionData transactionData = new TransactionData();
                ProgrammingEntity programmingEntity = new ProgrammingEntity();
                transactionData.programmingData = new List<ProgrammingEntity>();

                programmingEntity.MeterID = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.1");
                programmingEntity.ReadingDateTime = 0;
                meterData.MeterID = programmingEntity.MeterID;
                meterData.ReadingDateTime = programmingEntity.ReadingDateTime;
                programmingEntity.TotalProgrammingUpdates = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.99");

                for (int i = 1; i <= 10; i++)
                {
                    programmingEntity.LastTimestamp = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.11." + i.ToString("00")));
                    programmingEntity.UpdateSequence = FormatterCommon.ColText(i - 1);
                    programmingEntity.Description1 = "Tamper Reset";
                    transactionData.programmingData.Add(programmingEntity);
                    programmingEntity = new ProgrammingEntity();
                }
                for (int i = 1; i <= 10; i++)
                {
                    programmingEntity.LastTimestamp = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.12." + i.ToString("00")));
                    programmingEntity.UpdateSequence = FormatterCommon.ColText(i - 1);
                    programmingEntity.Description1 = "Future TOU";
                    transactionData.programmingData.Add(programmingEntity);
                    programmingEntity = new ProgrammingEntity();
                }

                for (int i = 1; i <= 10; i++)
                {
                    programmingEntity.LastTimestamp = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.13." + i.ToString("00")));
                    programmingEntity.UpdateSequence = FormatterCommon.ColText(i - 1);
                    programmingEntity.Description1 = "Maximum Demand";
                    transactionData.programmingData.Add(programmingEntity);
                    programmingEntity = new ProgrammingEntity();
                }
                for (int i = 1; i <= 10; i++)
                {
                    programmingEntity.LastTimestamp = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.14." + i.ToString("00")));
                    programmingEntity.UpdateSequence = FormatterCommon.ColText(i - 1);
                    programmingEntity.Description1 = "Billing Date & Time";
                    transactionData.programmingData.Add(programmingEntity);
                    programmingEntity = new ProgrammingEntity();
                }
                for (int i = 1; i <= 10; i++)
                {
                    programmingEntity.LastTimestamp = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.15." + i.ToString("00")));
                    programmingEntity.UpdateSequence = FormatterCommon.ColText(i - 1);
                    programmingEntity.Description1 = "MD Reset";
                    transactionData.programmingData.Add(programmingEntity);
                    programmingEntity = new ProgrammingEntity();
                }
                for (int i = 1; i <= 10; i++)
                {
                    programmingEntity.LastTimestamp = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.16." + i.ToString("00")));
                    programmingEntity.UpdateSequence = FormatterCommon.ColText(i - 1);
                    programmingEntity.Description1 = "Display Parameter - Push";
                    transactionData.programmingData.Add(programmingEntity);
                    programmingEntity = new ProgrammingEntity();
                }
                for (int i = 1; i <= 10; i++)
                {
                    programmingEntity.LastTimestamp = DateUtility.GetFormatedDateTme1(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.17." + i.ToString("00")));
                    programmingEntity.UpdateSequence = FormatterCommon.ColText(i - 1);
                    programmingEntity.Description1 = "Display Parameter - Scroll";
                    transactionData.programmingData.Add(programmingEntity);
                    programmingEntity = new ProgrammingEntity();
                }
               
                transactionData.meterDataEntity = meterData;

                billingGeneralNFEntity.listTransactionData.Add(transactionData);
            }
            catch (Exception)
            {
                //MessageBox.Show("Corrupted RTC data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
