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
    public class FormatterFraudEnergy
    {
        public void GetData(string data,ref string[,] fraudEnergy)
        {
            try
            {
                int counter = 0; 
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.FRAUDENERGYEXPRESSION);
                string[] fraudEnergyData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    fraudEnergyData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateData(fraudEnergyData);
                counter = 0;
                int MaxLength = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    if (tempData.Length > MaxLength)
                        MaxLength = tempData.Length;
                    counter++;
                }
                fraudEnergy = new string[counter,MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        fraudEnergy[counter,MaxLength] = tempData[MaxLength];
                    //SplitData(tempData, billingGeneralNFEntity);
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
                bool OuterFlag = true;
                bool InnerFlag = true;
                IECFraudEnergyEntity FraudEnergy = new IECFraudEnergyEntity();
                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter == 1)
                    {
                        FraudEnergy.MeterID = tempData[counter].Substring(4).Trim();
                    }
                    else if (counter == 2)
                    {
                        FraudEnergy.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                    }
                    //else if (counter == 3)
                    //{
                    //    FraudEnergy.CMRIID =  Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType =  Convert.ToString(tempData[counter]).Substring(0, 1); 
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        FraudEnergy.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        FraudEnergy.CMRIType = "Sands";
                    //    else
                    //        FraudEnergy.CMRIType = "BCS"; 
                    //}
                    else if (counter == 3)
                    {
                        tempData[counter] = tempData[counter].Substring(0, tempData[counter].Length - 1);
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1)) == false)
                        {
                            OuterFlag = false;
                            continue;
                        } 
                        const string regexFraud = @"(\(([\w\W]*?)\))";
                        MatchCollection matches = Regex.Matches(tempData[counter], regexFraud, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                        string[] fraudEnergy = new string[matches.Count];
                        int count = 0;
                        foreach (Match match in matches)
                        {
                            GroupCollection groups = match.Groups;
                            fraudEnergy[count++] = groups["0"].Value;
                        }
                        if (fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1).TrimStart('0') == "")
                            FraudEnergy.MagneticInfluenceKWh = "0";
                        else
                        {
                            FraudEnergy.MagneticInfluenceKWh = fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1).TrimStart('0');
                            if (FraudEnergy.MagneticInfluenceKWh.IndexOf('.') == 0) { FraudEnergy.MagneticInfluenceKWh = "0" + FraudEnergy.MagneticInfluenceKWh; }
                        }

                        if (fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1).TrimStart('0') == "")
                            FraudEnergy.MagneticInflueneceKVARhLag = "0";
                        else
                        {
                            FraudEnergy.MagneticInflueneceKVARhLag = fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1).TrimStart('0');
                            if (FraudEnergy.MagneticInflueneceKVARhLag.IndexOf('.') == 0) { FraudEnergy.MagneticInflueneceKVARhLag = "0" + FraudEnergy.MagneticInflueneceKVARhLag; }
                        }

                        if (fraudEnergy[2].Substring(1, fraudEnergy[2].IndexOf(')') - 1).TrimStart('0') == "")
                            FraudEnergy.MagneticInflueneceKVARhLead = "0";
                        else
                        {
                            FraudEnergy.MagneticInflueneceKVARhLead = fraudEnergy[2].Substring(1, fraudEnergy[2].IndexOf(')') - 1).TrimStart('0');
                            if (FraudEnergy.MagneticInflueneceKVARhLead.IndexOf('.') == 0) { FraudEnergy.MagneticInflueneceKVARhLead = "0" + FraudEnergy.MagneticInflueneceKVARhLead; }
                        }

                        if (fraudEnergy[3].Substring(1, fraudEnergy[3].IndexOf(')') - 1).TrimStart('0') == "")
                            FraudEnergy.MagneticInflueneceKVAh = "0";
                        else
                        {
                            FraudEnergy.MagneticInflueneceKVAh = fraudEnergy[3].Substring(1, fraudEnergy[3].IndexOf(')') - 1).TrimStart('0');
                            if (FraudEnergy.MagneticInflueneceKVAh.IndexOf('.') == 0) { FraudEnergy.MagneticInflueneceKVAh = "0" + FraudEnergy.MagneticInflueneceKVAh; }
                        }
                    }
                    else if (counter == 4)
                    {
                        if (ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1)) == false)
                        {
                            InnerFlag = false;
                            continue;
                        }
                        const string regexFraud = @"(\(([\w\W]*?)\))";
                        MatchCollection matches = Regex.Matches(tempData[counter], regexFraud, RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                        string[] fraudEnergy = new string[matches.Count];
                        int count = 0;
                        foreach (Match match in matches)
                        {
                            GroupCollection groups = match.Groups;
                            fraudEnergy[count++] = groups["0"].Value;
                        }

                        if (fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1).TrimStart('0') == "")
                            FraudEnergy.ReverseEnergyKWh = "0";
                        else
                        {
                            FraudEnergy.ReverseEnergyKWh = fraudEnergy[0].Substring(1, fraudEnergy[0].IndexOf(')') - 1).TrimStart('0');
                            if (FraudEnergy.ReverseEnergyKWh.IndexOf('.') == 0) { FraudEnergy.ReverseEnergyKWh = "0" + FraudEnergy.ReverseEnergyKWh; }
                        }

                        if (fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1).TrimStart('0') == "")
                            FraudEnergy.ReverseEnergyKVAh = "0";
                        else
                        {
                            FraudEnergy.ReverseEnergyKVAh = fraudEnergy[1].Substring(1, fraudEnergy[1].IndexOf(')') - 1).TrimStart('0');
                            if (FraudEnergy.ReverseEnergyKVAh.IndexOf('.') == 0) { FraudEnergy.ReverseEnergyKVAh = "0" + FraudEnergy.ReverseEnergyKVAh; }
                        }
                    }
                }
                if (!(InnerFlag == false && OuterFlag == false))
                    billingGeneralNFEntity.listFraudEnergy.Add(FraudEnergy);
            }
            catch (Exception)
            {
                //MessageBox.Show("Corrupted Fraud Energy data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
