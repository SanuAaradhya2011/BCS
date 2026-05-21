/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |All rights reserved to Cabcon Technologies  |
 * | |
 * |Author : Piyush Singh. |
 * |                       |
 * |     |
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using CAB.IECChannel.ReadOut;
using System.Globalization;
using System.Windows.Forms;
using LTCTBLL;

namespace CAB.IECChannel.Formatter
{
    public class FormatterPhasor
    {
        public void GetData(string data, ref string[,] phasor)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.PHASOREXPRESSSION);
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
                phasor = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        phasor[counter, MaxLength] = tempData[MaxLength];
                    counter++;
                }
            }
            catch (Exception)
            {
            }
        }


        public void SplitData(string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            try
            {
                if (tempData == null)
                    return;
                int index = 0;
                bool OuterFlag = true;
                bool InnerFlag = true;
                IECPhasorEntity phasor = new IECPhasorEntity();
                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter == 1)
                    {
                        phasor.MeterID = tempData[counter].Substring(4).Trim();
                    }
                    else if (counter == 2)
                    {
                        phasor.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                    }
                    //else if (counter == 3)
                    //{
                    //    phasor.CMRIID = Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType = Convert.ToString(tempData[counter]).Substring(0, 1);
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        phasor.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        phasor.CMRIType = "Sands";
                    //    else
                    //        phasor.CMRIType = "BCS";
                    //}
                    else if (counter == 3)
                    {
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1)) == false)
                            OuterFlag = false;
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1)) == false)
                            InnerFlag = false;
                        if (InnerFlag || OuterFlag)
                            phasor = GetPhasor(tempData[counter], phasor);
                    }
                }
                if (!(InnerFlag == false && OuterFlag == false))
                    billingGeneralNFEntity.listPhasor.Add(phasor);
            }
            catch (Exception)
            {
              //  MessageBox.Show("Corrupted Phasor data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static IECPhasorEntity GetPhasor(string PhasorPara, IECPhasorEntity phasorEntity)
        {
            PhasorPara = "/" + PhasorPara;
            //Voltage R y  b  Phase
            phasorEntity.RPhaseVoltage = ReadoutCommon.PhasorFilterData(PhasorPara, 1, 4, 100, "0.00");
            phasorEntity.YPhaseVoltage = ReadoutCommon.PhasorFilterData(PhasorPara, 5, 4, 100, "0.00");
            phasorEntity.BPhaseVoltage = ReadoutCommon.PhasorFilterData(PhasorPara, 9, 4, 100, "0.00");
            //Current R y  b  Phase
            phasorEntity.RPhaseCurrent = ReadoutCommon.PhasorFilterData(PhasorPara, 13, 8, 1000, "0.000");
            phasorEntity.YPhaseCurrent = ReadoutCommon.PhasorFilterData(PhasorPara, 21, 8, 1000, "0.000");
            phasorEntity.BPhaseCurrent = ReadoutCommon.PhasorFilterData(PhasorPara, 29, 8, 1000, "0.000");

            phasorEntity.TotalActivePower = ReadoutCommon.PhasorFilterData(PhasorPara, 38, 8, 100000, "0.000");
            phasorEntity.TotalInductivePower = ReadoutCommon.PhasorFilterData(PhasorPara, 46, 8, 100000, "0.000");
            phasorEntity.TotalCapacitivePower = ReadoutCommon.PhasorFilterData(PhasorPara, 54, 8, 100000, "0.000");
            phasorEntity.TotalApparentPower = ReadoutCommon.PhasorFilterData(PhasorPara, 62, 8, 100000, "0.000");
            //PF R y  b  Phase
            phasorEntity.RPhasePF = ReadoutCommon.PhasorFilterData(PhasorPara, 70, 4, 10000, "0.00");
            phasorEntity.YPhasePF = ReadoutCommon.PhasorFilterData(PhasorPara, 74, 4, 10000, "0.00");
            phasorEntity.BPhasePF = ReadoutCommon.PhasorFilterData(PhasorPara, 78, 4, 10000, "0.00");
            //Net PF 
            phasorEntity.TotalInstantaneousPF = ReadoutCommon.PhasorFilterData(PhasorPara, 82, 8, 10000, "0.00");
            //Frequency   
            phasorEntity.Frequency = ReadoutCommon.PhasorFilterData(PhasorPara, 90, 4, 100, "0.00");

            phasorEntity.PhaseSequence = "Correct";

            //Total  
            phasorEntity.TotalkWDirection = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 96, 2)) == 0) ? "Import" : "Export";

            //Iport/Export R y  b  Phase
            phasorEntity.RPhasekWDirection = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 98, 2)) == 0) ? "Import" : "Export";
            phasorEntity.YPhasekWDirection = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 100, 2)) == 0) ? "Import" : "Export";
            phasorEntity.BPhasekWDirection = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 102, 2)) == 0) ? "Import" : "Export";

            //Chaneel Missing R y  b  Phase
            phasorEntity.RPhaseChannel = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 104, 2)) == 0) ? "Absent" : "Present";
            phasorEntity.YPhaseChannel = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 106, 2)) == 0) ? "Absent" : "Present";
            phasorEntity.BPhaseChannel = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 108, 2)) == 0) ? "Absent" : "Present";

            //Lag/ Lead R y  b  Phase
            phasorEntity.RPhaseLagLead = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 110, 2)) == 0) ? "Lag" : "Lead";
            phasorEntity.YPhaseLagLead = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 112, 2)) == 0) ? "Lag" : "Lead";
            phasorEntity.BPhaseLagLead = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 114, 2)) == 0) ? "Lag" : "Lead";

            //Lag/ Lead Total
            phasorEntity.Total = (Convert.ToInt16(ReadoutCommon.PhasorFilterData(PhasorPara, 116, 2)) == 0) ? "Lag" : "Lead";

            // Y B Phase Angle with respect to R Phase
            phasorEntity.YPhaseAngleWithRPhase = ReadoutCommon.PhasorFilterData(PhasorPara, 118, 2, 7.2);
            phasorEntity.BPhaseAngleWithRPhase = ReadoutCommon.PhasorFilterData(PhasorPara, 120, 2, 7.2);
            phasorEntity.AngleBWAnyPhasePresent = ReadoutCommon.PhasorFilterData(PhasorPara, 122, 2, 7.2);


            return phasorEntity;
        }
       
    }
}