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
using System.Globalization;
using System.Windows.Forms;
using CAB.IECFramework.Utility;

namespace CAB.IECChannel.Formatter
{
    public class FormatterTamper
    {

        public void GetData(string data, ref  string[,] tamper)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.TAMPEREXPRESSSION);
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
                tamper = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        tamper[counter, MaxLength] = tempData[MaxLength];
                    counter++;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetDataForSPhase(string data, ref  string[,] tamper)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.TAMPEREXPRESSSIONFORSPHASE);
                string[] programmingData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    programmingData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateDataForSPhase(programmingData);
                counter = 0;
                int MaxLength = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    if (tempData.Length > MaxLength)
                        MaxLength = tempData.Length;
                    counter++;
                }
                tamper = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        tamper[counter, MaxLength] = tempData[MaxLength];
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
            if (tempData == null)
                return;
            bool Flag = true;
            //billingGeneralNFEntity.Tamper.General = new TamperCounterGeneralEntity();
            //billingGeneralNFEntity.Tamper.Counter = new TamperCounterEntity();
            //billingGeneralNFEntity.Tamper.Snapshot = new List<TamperSnapshotEntity>();

            TamperData tamperData = new TamperData();
            tamperData.General = new TamperCounterGeneralEntity();
            tamperData.Counter = new TamperCounterEntity();
            tamperData.Snapshot = new List<TamperSnapshotEntity>();

            TamperCounterGeneralEntity generalEntity = new TamperCounterGeneralEntity();
            TamperCounterEntity counterEntity = new TamperCounterEntity();
            List<TamperSnapshotEntity> SnapshotEntity = new List<TamperSnapshotEntity>();
            int index = 0;
            try
            {
                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter == 1)
                    {
                        generalEntity.MeterID = tempData[counter].Substring(tempData[counter].IndexOf("5") + 1); ;
                        continue;
                    }
                    if (counter == 2)
                    {
                        generalEntity.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                        continue;
                    }
                    // if (counter == 3)
                    //{
                    //    generalEntity.CMRIID = Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType = Convert.ToString(tempData[counter]).Substring(0, 1);
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        generalEntity.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        generalEntity.CMRIType = "Sands";
                    //    else
                    //        generalEntity.CMRIType = "BCS";
                    //    continue;
                    //}
                    if (counter == 3)
                    {
                        if (string.IsNullOrEmpty(tempData[counter]))
                            break;
                        if (counter < tempData.GetUpperBound(0))
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        if (!Flag)
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        if (!Flag)
                            break;
                        index = 0;
                        generalEntity.VoltageImbalanceRPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.VoltageImbalanceYPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.VoltageImbalanceBPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.MissingPotentialRPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.MissingPotentialYPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.MissingPotentialBPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CTShortTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CTOpenRPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CTOpenYPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CTOpenBPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.OnePhaseNeutralAbsentTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.VoltagePhaseReversalTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CurrentImbalanceRPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CurrentImbalanceYPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CurrentImbalanceBPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CurrentReversalRPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CurrentReversalYPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.CurrentReversalBPhaseTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.MagneticInfluenceTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.NeutralDisturbanceTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.FrontCoverOpeningTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.TerminalCoverOpeningTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        counterEntity.TotalTamperCounter = FormatterCommon.FilterData(tempData[counter], index, 8, true); index += 8;
                        counterEntity.PowerOnOffCounter = FormatterCommon.FilterData(tempData[counter], index, 4, true); index += 4;
                        generalEntity.RelatedTo = "T";
                        //billingGeneralNFEntity.Tamper.General = generalEntity;
                        //billingGeneralNFEntity.Tamper.Counter = counterEntity;

                        tamperData.General = generalEntity;
                        tamperData.Counter = counterEntity;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(tempData[counter]))
                            break;
                        if (counter < tempData.GetUpperBound(0))
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        if (!Flag)
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        if (!Flag)
                            break;
                        index = 0;
                        do
                        {
                            TamperSnapshotEntity tamperSnapshotEntity = new TamperSnapshotEntity();
                            int tamperCode = 0;
                            // For LPR Meter West Bengal Tender
                            tamperCode = TamperCode(tempData[counter], tamperCode, index);

                            index += 2;
                            if (tamperCode == 0)
                                continue;
                            // MessageBox.Show(tamperCode.ToString());
                            tamperSnapshotEntity.TamperCode = tamperCode;
                            tamperSnapshotEntity.TamperOccurredTime = FormatterCommon.LongDateTime(tempData[counter].Substring(index, 10), 0);
                            if (tamperSnapshotEntity.TamperOccurredTime == 0)
                                tamperSnapshotEntity.TamperOccurredTime = 19000101000000;
                            index += 10;
                            tamperSnapshotEntity.TamperRestoredTime = FormatterCommon.LongDateTime(tempData[counter], index);
                            if (tamperSnapshotEntity.TamperRestoredTime == 0)
                                tamperSnapshotEntity.TamperRestoredTime = 19000101000000;
                            index += 10;
                            tamperSnapshotEntity.RVoltageRestored = FormatterCommon.FilterData(tempData[counter], index, 4, 100, "0.00"); index += 4;
                            tamperSnapshotEntity.YVoltageRestored = FormatterCommon.FilterData(tempData[counter], index, 4, 100, "0.00"); index += 4;
                            tamperSnapshotEntity.BVoltageRestored = FormatterCommon.FilterData(tempData[counter], index, 4, 100, "0.00"); index += 4;
                            int factor = Int32.Parse(FormatterCommon.FilterData(tempData[counter], index, 2)); index += 2;
                            if (factor == 1)
                                factor = -1;
                            else
                                factor = 1;
                            tamperSnapshotEntity.RCurrentRestored = (Convert.ToSingle(FormatterCommon.FilterData(tempData[counter], index, 8, factor)) / 100).ToString("0.000");
                            factor = 1;
                            index += 8;
                            if (Int32.Parse(FormatterCommon.FilterData(tempData[counter], index, 2)) == 1)
                                factor = -1;
                            index += 2;
                            tamperSnapshotEntity.YCurrentRestored = (Convert.ToSingle(FormatterCommon.FilterData(tempData[counter], index, 8, factor)) / 100).ToString("0.000");
                            factor = 1;
                            index += 8;
                            if (Int32.Parse(FormatterCommon.FilterData(tempData[counter], index, 2)) == 1)
                                factor = -1;
                            index += 2;

                            tamperSnapshotEntity.BCurrentRestored = (Convert.ToSingle(FormatterCommon.FilterData(tempData[counter], index, 8, factor)) / 100).ToString("0.000");
                            factor = 1;
                            index += 8;
                            tamperSnapshotEntity.RPFRestored = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.YPFRestored = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.BPFRestored = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.TotalPFRestored = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.KWhRestored = FormatterCommon.FilterData(tempData[counter], index, 14, 1000000, "0.000"); index += 14;
                            tamperSnapshotEntity.KVAhRestored = FormatterCommon.FilterData(tempData[counter], index, 14, 1000000, "0.000"); index += 14;
                            tamperSnapshotEntity.RVoltageOccurred = FormatterCommon.FilterData(tempData[counter], index, 4, 100, "0.00"); index += 4;

                            tamperSnapshotEntity.YVoltageOccurred = FormatterCommon.FilterData(tempData[counter], index, 4, 100, "0.00"); index += 4;
                            tamperSnapshotEntity.BVoltageOccurred = FormatterCommon.FilterData(tempData[counter], index, 4, 100, "0.00"); index += 4;

                            factor = 1;
                            if (Int32.Parse(FormatterCommon.FilterData(tempData[counter], index, 2)) == 1)
                                factor = -1;
                            index += 2;
                            tamperSnapshotEntity.RCurrentOccurred = (Convert.ToSingle(FormatterCommon.FilterData(tempData[counter], index, 8, factor)) / 100).ToString("0.000"); index += 8;

                            factor = 1;
                            if (Int32.Parse(FormatterCommon.FilterData(tempData[counter], index, 2)) == 1)
                                factor = -1;
                            index += 2;
                            tamperSnapshotEntity.YCurrentOccurred = (Convert.ToSingle(FormatterCommon.FilterData(tempData[counter], index, 8, factor)) / 100).ToString("0.000"); index += 8;

                            factor = 1;
                            if (Int32.Parse(FormatterCommon.FilterData(tempData[counter], index, 2)) == 1)
                                factor = -1;
                            index += 2;
                            tamperSnapshotEntity.BCurrentOccurred = (Convert.ToSingle(FormatterCommon.FilterData(tempData[counter], index, 8, factor)) / 100).ToString("0.000"); index += 8;
                            tamperSnapshotEntity.RPFOccurred = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.YPFOccurred = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.BPFOccurred = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.TotalPFOccurred = FormatterCommon.FilterData(tempData[counter], index, 2, 100, "0.00"); index += 2;
                            tamperSnapshotEntity.KWhOccurred = FormatterCommon.FilterData(tempData[counter], index, 14, 1000000, "0.000"); index += 14;
                            tamperSnapshotEntity.KVAhOccurred = FormatterCommon.FilterData(tempData[counter], index, 14, 1000000, "0.000"); index += 14;

                            //billingGeneralNFEntity.Tamper.Snapshot.Add(tamperSnapshotEntity);
                            tamperData.Snapshot.Add(tamperSnapshotEntity);

                        } while (CharData(tempData[counter], index) != Convert.ToChar(0x3));
                    }
                }
                billingGeneralNFEntity.listTamper.Add(tamperData);
            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Corrupted Tamper data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);  
            }
        }
        public void SplitDataSPhase(Dictionary<string, string> dicOBISandData,string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            if (tempData == null)
                return;
          
            //billingGeneralNFEntity.Tamper.General = new TamperCounterGeneralEntity();
            //billingGeneralNFEntity.Tamper.Counter = new TamperCounterEntity();
            //billingGeneralNFEntity.Tamper.Snapshot = new List<TamperSnapshotEntity>();

            TamperData tamperData = new TamperData();
            tamperData.General = new TamperCounterGeneralEntity();
            tamperData.Counter = new TamperCounterEntity();
            tamperData.Snapshot = new List<TamperSnapshotEntity>();

            TamperCounterGeneralEntity generalEntity = new TamperCounterGeneralEntity();
            TamperCounterEntity counterEntity = new TamperCounterEntity();
            List<TamperSnapshotEntity> SnapshotEntity = new List<TamperSnapshotEntity>();
            int index = 0;
            try
            {
                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter == 1)
                    {
                        generalEntity.MeterID = tempData[counter].Substring(13, 16).Trim();
                        continue;
                    }
                    if (counter == 2)
                    {
                        generalEntity.ReadingDateTime = 0;// Convert.ToInt64(tempData[counter]);
                        continue;
                    }
                    // if (counter == 3)
                    //{
                    //    generalEntity.CMRIID = Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType = Convert.ToString(tempData[counter]).Substring(0, 1);
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        generalEntity.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        generalEntity.CMRIType = "Sands";
                    //    else
                    //        generalEntity.CMRIType = "BCS";
                    //    continue;
                    //}
                    if (counter == 3)
                    {
                        //if (string.IsNullOrEmpty(tempData[counter]))
                        //    break;
                        //if (counter < tempData.GetUpperBound(0))
                        //    Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        //if (!Flag)
                        //    Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        //if (!Flag)
                        //    break;
                        //index = 0;
                        generalEntity.VoltageImbalanceRPhaseTamperCounter = 0;
                        generalEntity.VoltageImbalanceYPhaseTamperCounter = 0;
                        generalEntity.VoltageImbalanceBPhaseTamperCounter = 0;
                        generalEntity.MissingPotentialRPhaseTamperCounter = 0;
                        generalEntity.MissingPotentialYPhaseTamperCounter = 0;
                        generalEntity.MissingPotentialBPhaseTamperCounter = 0;
                        generalEntity.CTShortTamperCounter = 0;
                        generalEntity.CTOpenRPhaseTamperCounter = 0;
                        generalEntity.CTOpenYPhaseTamperCounter = 0;
                        generalEntity.CTOpenBPhaseTamperCounter = 0;
                        generalEntity.OnePhaseNeutralAbsentTamperCounter = 0;
                        generalEntity.VoltagePhaseReversalTamperCounter = 0;
                        generalEntity.CurrentImbalanceRPhaseTamperCounter = 0;
                        generalEntity.CurrentImbalanceYPhaseTamperCounter = 0;
                        generalEntity.CurrentImbalanceBPhaseTamperCounter = 0;
                        generalEntity.CurrentReversalRPhaseTamperCounter = 0;
                        generalEntity.CurrentReversalYPhaseTamperCounter = 0;
                        generalEntity.CurrentReversalBPhaseTamperCounter = 0;
                        generalEntity.MagneticInfluenceTamperCounter = 0;
                        generalEntity.NeutralDisturbanceTamperCounter = 0;
                        generalEntity.FrontCoverOpeningTamperCounter = 0;
                        generalEntity.TerminalCoverOpeningTamperCounter = 0;
                        counterEntity.TotalTamperCounter = 0;
                        counterEntity.PowerOnOffCounter = 0;
                        generalEntity.RelatedTo = "T";
                        //billingGeneralNFEntity.Tamper.General = generalEntity;
                        //billingGeneralNFEntity.Tamper.Counter = counterEntity;

                        tamperData.General = generalEntity;
                        tamperData.Counter = counterEntity;
                        //}
                        //else
                        //{
                        //if (string.IsNullOrEmpty(tempData[counter]))
                        //    break;
                        //if (counter < tempData.GetUpperBound(0))
                        //    Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        //if (!Flag)
                        //    Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        //if (!Flag)
                        //    break;
                        //index = 0;

                        string[] tamperData1 = tempData[counter].Split('(', ')');

                        //do
                        //{
                        String parameterValue = string.Empty;
                        int tamperCode = 0;

                        for (int count = 0; count < tamperData1.Length - 2; count++)
                        {
                            index = 0;

                            count++;
                            TamperSnapshotEntity tamperSnapshotEntity = new TamperSnapshotEntity();

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 2)); index = index + 2;
                            tamperCode = Convert.ToInt32(parameterValue, 16);

                            if (tamperCode == 0)
                                continue;
                            tamperSnapshotEntity.TamperCode = tamperCode;
                            parameterValue = tamperData1[count].Substring(index, 8); index = index + 8;

                            tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(FormatterCommon.DTMDailyProfileDateSP(parameterValue));
                            if (tamperSnapshotEntity.TamperOccurredTime == 0)
                                tamperSnapshotEntity.TamperOccurredTime = 19000101000000;

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 8)); index = index + 8;
                            tamperSnapshotEntity.KWhOccurred = FormatterCommon.FilterDataNR(parameterValue, 0, 8, 1000, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 8)); index = index + 8;
                            tamperSnapshotEntity.KVAhOccurred = FormatterCommon.FilterDataNR(parameterValue, 0, 8, 1000, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 4)); index = index + 4;
                            tamperSnapshotEntity.PhaseVoltageOccured = FormatterCommon.FilterDataNR(parameterValue, 0, 4, 100, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 4)); index = index + 4;
                            tamperSnapshotEntity.PhaseCurrentOccured = FormatterCommon.FilterDataNR(parameterValue, 0, 4, 100, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 4)); index = index + 4;
                            tamperSnapshotEntity.OccuredNeutralCurrent = FormatterCommon.FilterDataNR(parameterValue, 0, 4, 100, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 2)); index = index + 2;
                            tamperSnapshotEntity.TotalPFOccurred = FormatterCommon.FilterDataNR(parameterValue, 0, 2, 100, "{00:0.00}");


                            parameterValue = tamperData1[count].Substring(index, 8); index = index + 8;
                            tamperSnapshotEntity.TamperRestoredTime = Convert.ToInt64(FormatterCommon.DTMDailyProfileDateSP(parameterValue));
                            if (tamperSnapshotEntity.TamperRestoredTime == 0)
                                tamperSnapshotEntity.TamperRestoredTime = 19000101000000;

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 8)); index = index + 8;
                            tamperSnapshotEntity.KWhRestored = FormatterCommon.FilterDataNR(parameterValue, 0, 8, 1000, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 8)); index = index + 8;
                            tamperSnapshotEntity.KVAhRestored = FormatterCommon.FilterDataNR(parameterValue, 0, 8, 1000, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 4)); index = index + 4;
                            tamperSnapshotEntity.PhaseVoltageRestore = FormatterCommon.FilterDataNR(parameterValue, 0, 4, 100, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 4)); index = index + 4;
                            tamperSnapshotEntity.PhaseCurrentRestore = FormatterCommon.FilterDataNR(parameterValue, 0, 4, 100, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 4)); index = index + 4;
                            tamperSnapshotEntity.RestoreNeutralCurrent = FormatterCommon.FilterDataNR(parameterValue, 0, 4, 100, "{00:0.00}");

                            parameterValue = ReverseString(tamperData1[count].Substring(index, 2)); index = index + 2;
                            tamperSnapshotEntity.TotalPFRestored = FormatterCommon.FilterDataNR(parameterValue, 0, 2, 100, "{00:0.00}");
                            //billingGeneralNFEntity.Tamper.Snapshot.Add(tamperSnapshotEntity);
                            tamperSnapshotEntity.TempratureOccured = "----";
                            tamperData.Snapshot.Add(tamperSnapshotEntity);

                        }//while (CharData(tempData[counter], index) != Convert.ToChar(0x3));
                    }
                }

                /* Check is implemented for Meter Cover open tamper snapshot is available then skip the below code */
                if (tempData != null && tempData.Length > 3 && !tempData[3].Contains("EE"))
                {
                    // Tamper Cover Open
                    string cvrOpenCount = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.4");
                    if (Convert.ToInt32(cvrOpenCount) > 0)
                    {
                        String parameterValue = string.Empty;
                        int tamperCode = 0;
                        TamperSnapshotEntity tamperSnapshotEntity = new TamperSnapshotEntity();

                        tamperCode = 211;
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.4.01")));

                        if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.4.02")!=string.Empty)
                            tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.4.02")));

                        if (tamperSnapshotEntity.TamperOccurredTime == 0)
                            tamperSnapshotEntity.TamperOccurredTime = 19000101000000;

                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }
                }
                /* Check is implemented for Meter ESD tamper snapshot is available then skip the below code */
                if (tempData != null) //&& tempData.Length > 3 && !tempData[3].Contains("E8"))
                {
                    // Tamper Cover Open
                    string ESDCount = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.3");
                    if (Int32.TryParse(ESDCount, out int val) && val > 0)
                    {
                        String parameterValue = string.Empty;
                        int tamperCode = 0;
                        TamperSnapshotEntity tamperSnapshotEntity = new TamperSnapshotEntity();

                        tamperCode = 232;
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.2.01")));

                        //if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.2.01") != string.Empty)
                        //    tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.50.4.02")));

                        if (tamperSnapshotEntity.TamperOccurredTime == 0)
                            tamperSnapshotEntity.TamperOccurredTime = 19000101000000;

                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }
                }
				//********************Add for Temperature 
                
                if (tempData != null) //&& tempData.Length > 3 && !tempData[3].Contains("EE"))
                {
                    // Tamper Temperature
                    String parameterValue = string.Empty;
                    int tamperCode = 0;                   
                    TamperSnapshotEntity tamperSnapshotEntity = null;
                    tamperCode = 239;
                  

                    if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.01") != string.Empty)
                    {
                        tamperSnapshotEntity = new TamperSnapshotEntity();
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        tamperSnapshotEntity.TempratureOccured = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.01").Substring(0,3);
                        if (tamperSnapshotEntity.TempratureOccured == "000")
                        {
                            tamperSnapshotEntity.TempratureOccured = "----";
                        }
                        tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.01").Substring(6, 14)));
                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }
                    if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.02") != string.Empty)
                    {
                        tamperSnapshotEntity = new TamperSnapshotEntity();
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        tamperSnapshotEntity.TempratureOccured = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.02").Substring(0, 3);
                        if (tamperSnapshotEntity.TempratureOccured == "000")
                        {
                            tamperSnapshotEntity.TempratureOccured = "----";
                        }
                        tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.02").Substring(6, 14)));
                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }
                    if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.03") != string.Empty)
                    {
                        tamperSnapshotEntity = new TamperSnapshotEntity();
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        tamperSnapshotEntity.TempratureOccured = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.03").Substring(0, 3);
                        tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.03").Substring(6, 14)));
                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }
                    if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.04") != string.Empty)
                    {
                        tamperSnapshotEntity = new TamperSnapshotEntity();
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        tamperSnapshotEntity.TempratureOccured = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.04").Substring(0, 3);
                        if (tamperSnapshotEntity.TempratureOccured == "000")
                        {
                            tamperSnapshotEntity.TempratureOccured = "----";
                        }
                        tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.04").Substring(6, 14)));
                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }
                    if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.05") != string.Empty)
                    {
                        tamperSnapshotEntity = new TamperSnapshotEntity();
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        tamperSnapshotEntity.TempratureOccured = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.05").Substring(0, 3);
                        if (tamperSnapshotEntity.TempratureOccured == "000")
                        {
                            tamperSnapshotEntity.TempratureOccured = "----";
                        }                        
                        tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.7.05").Substring(6, 14)));
                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }  
                        
                   
                }

                //***************************************

                // Power failure Tamper(Power OFF-ON Event)
                if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.51.1.01") != string.Empty)
                {
                    String parameterValue = string.Empty;
                    int tamperCode = 225;
                    for (int i = 1; i <= 100; i++)
                    {
                        TamperSnapshotEntity tamperSnapshotEntity = new TamperSnapshotEntity();
                        tamperSnapshotEntity.TamperCode = tamperCode;
                        string Obis = "C.51.1." + i.ToString("D2");
                        if (FormatterCommon.ParseDataFor1Phase(dicOBISandData, Obis) != string.Empty)
                        {
                            tamperSnapshotEntity.TamperOccurredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(FormatterCommon.ParseDataFor1Phase(dicOBISandData, Obis)));
                            // Exit from the loop when Power failure Tamper(Power OFF-ON Event) Occurence time is coming "00:00;00-00-00" from the meter
                            if (tamperSnapshotEntity.TamperOccurredTime == 20000000000000)
                            {
                                break;
                            }                           
                            tamperSnapshotEntity.TamperRestoredTime = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhaseRestoration(FormatterCommon.ParseDataFor1Phase(dicOBISandData, Obis)));                            
                        }
                        tamperData.Snapshot.Add(tamperSnapshotEntity);
                    }                    
                }
                billingGeneralNFEntity.listTamper.Add(tamperData);
            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Corrupted Tamper data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);  
            }
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
        private int TamperCode(string tempData, int tamperCode, int index)
        {
            tamperCode = 0;
            try
            {

                if (int.TryParse(Convert.ToInt32(tempData.Substring(index, 2), 16).ToString(), out tamperCode))
                {

                    return tamperCode;
                }
                return tamperCode;
            }
            catch (FormatException ex)
            {
                return tamperCode;
            }
            catch (Exception ex)
            {
                return tamperCode;
            }

        }
        private char CharData(string charString, int index)
        {
            char lastChar = Convert.ToChar(0x03);
            try
            {
                if (!string.IsNullOrEmpty(charString))
                {
                    lastChar = Convert.ToChar(charString.Substring(index, 1));
                }
                return lastChar;
            }
            catch (FormatException ex)
            {
                return lastChar;
            }
            catch (Exception ex)
            {
                return lastChar;
            }
        }
    }
}