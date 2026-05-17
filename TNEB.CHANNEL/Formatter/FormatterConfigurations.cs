using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using System.Windows.Forms;
using CAB.IECChannel;
using CAB.IECChannel.ReadOut;
using CAB.Contracts;
using CABEntity;
using System.Collections.ObjectModel;
using CAB.IECChannel.Programming;
using System.Text;

namespace CAB.IECChannel.Formatter
{
    public class FormatterConfigurations
    {
        BillingResetEntity billingresetentity = new BillingResetEntity();
        IECRS232LockEntity RS232Lockentity1 = new IECRS232LockEntity();
        private string GetDemandType(string str)
        {
            string demand = "";
            try
            {
                if (str == "01")
                    demand = "Block Demand";
                else if (str == "02")
                    demand = "Sliding Demand";
                return demand;
            }
            catch
            {
                return null;
            }
        }

        private string GetkvarSelection(string str)
        {
            string kvarSelection = "";
            try
            {
                if (str == "00")
                    kvarSelection = "Lag Only";
                else if (str == "01")
                    kvarSelection = "Lag and Lead";
                return kvarSelection;
            }
            catch
            {
                return null;
            }
        }

        public void SplitMDWithIPData(string configData, MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {
            MDWithIPEntity mdWithIPEntity = new MDWithIPEntity();
            string mdData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);

            if (ReadoutCommon.CalculateBcc(mdData.Substring(0, mdData.Length - 1), mdData.Length - 1, mdData.Substring(mdData.Length - 1, 1)) == false)
                meterConfigEntity.mdWithIPEntity = null;
            else
            {
                foreach (MeterConfigurationConfigSectionParameter configSectionParameter in configSection.Parameters)
                {
                    if (configSectionParameter.Name == "MD1DemandType")
                    {
                        mdWithIPEntity.KWDemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD1TimeInterval")
                    {
                        mdWithIPEntity.KWInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD1SubInterval")
                    {
                        mdWithIPEntity.KWSubInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD2DemandType")
                    {
                        mdWithIPEntity.KVADemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD2TimeInterval")
                    {
                        mdWithIPEntity.KVAInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                    if (configSectionParameter.Name == "MD2SubInterval")
                    {
                        mdWithIPEntity.KVASubInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                    }
                }
                mdWithIPEntity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.mdWithIPEntity = mdWithIPEntity;
            }
        }

        public void SplitkvarSelectionData(string configData, MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {
            IECkvarSelectionEntity kvarselectionEntity = new IECkvarSelectionEntity();
            string kvarData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);

            if (ReadoutCommon.CalculateBcc(kvarData.Substring(0, kvarData.Length - 1), kvarData.Length - 1, kvarData.Substring(kvarData.Length - 1, 1)) == false)
                meterConfigEntity.mdWithIPEntity = null;
            else
            {
                if (kvarData.Contains("FF"))
                {
                    kvarData = "(00)";
                }
                if (Convert.ToInt16(kvarData.Substring(1, 2)) == 0)
                { kvarselectionEntity.LagOnly = "1"; kvarselectionEntity.LagandLead = "0"; }
                else if (Convert.ToInt16(kvarData.Substring(1, 2)) == 1)
                { kvarselectionEntity.LagOnly = "0"; kvarselectionEntity.LagandLead = "1"; }

                kvarselectionEntity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.kvarselectionEntity = kvarselectionEntity;
            }
        }

        public void SplitDailyLogData(string configData, MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {
            IECDailyLogEntity dailylogentity = new IECDailyLogEntity();
            string DailyLogData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);
            if (ReadoutCommon.CalculateBcc(DailyLogData.Substring(0, DailyLogData.Length - 1), DailyLogData.Length - 1, DailyLogData.Substring(DailyLogData.Length - 1, 1)) == false)
            {
                meterConfigEntity.dailylogentity = null;
            }
            else
            {
                string s = DailyLogData.Substring(1, 2);
                string binary = Convert.ToString(Convert.ToInt32(s, 16), 2).PadLeft(8, '0');
                char[] c = new char[8];
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = Convert.ToChar(binary.Substring(i, 1));
                }
                dailylogentity.CumulativeKWh = Convert.ToString(c[7]);
                dailylogentity.CumulativeKVARhLag = Convert.ToString(c[6]);
                dailylogentity.CumulativeKVARhLead = Convert.ToString(c[5]);
                dailylogentity.CumulativeKVAh = Convert.ToString(c[4]);
                dailylogentity.DailyMD1 = Convert.ToString(c[3]);
                dailylogentity.DailyMD2 = Convert.ToString(c[2]);
                dailylogentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.dailylogentity = dailylogentity;

            }
        }

        // For Reset Lock out days
        public void SplitResetLockOutDays(string configData, MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {

            string ResetLockOutDaysData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);
            if (ReadoutCommon.CalculateBcc(ResetLockOutDaysData.Substring(0, ResetLockOutDaysData.Length - 1), ResetLockOutDaysData.Length - 1, ResetLockOutDaysData.Substring(ResetLockOutDaysData.Length - 1, 1)) == false)
            {
                meterConfigEntity.billingresetentity = null;
            }
            else
            {
                string s = ResetLockOutDaysData.Substring(1, 2);
                string dec = Convert.ToString(Convert.ToInt32(s, 16), 10);
                billingresetentity.ResetLockOutDays = dec;
                billingresetentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.billingresetentity = billingresetentity;

            }
        }
        // For Mode of Billing
        public void SplitModeOfBilling(string configData, MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {

            string ModeOfBilling = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);
            if (ReadoutCommon.CalculateBcc(ModeOfBilling.Substring(0, ModeOfBilling.Length - 1), ModeOfBilling.Length - 1, ModeOfBilling.Substring(ModeOfBilling.Length - 1, 1)) == false)
            {
                meterConfigEntity.billingresetentity = null;
            }
            else
            {
                string s = ModeOfBilling.Substring(1, 2);
                if (s == "00")
                {
                    billingresetentity.ModeOfBilling = IECBillingMode.EndofMonth;
                    billingresetentity.Day = "01";
                    billingresetentity.Hours = "00";
                    billingresetentity.Minutes = "00";
                    billingresetentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);



                }
                else if (s == "01")
                {
                    billingresetentity.ModeOfBilling = IECBillingMode.UserDefined;
                    s = ModeOfBilling.Substring(3, 2);
                    billingresetentity.Day = s;
                    s = ModeOfBilling.Substring(5, 2);
                    billingresetentity.Hours = s;
                    s = ModeOfBilling.Substring(7, 2);
                    billingresetentity.Minutes = s;
                    billingresetentity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);

                }
                meterConfigEntity.billingresetentity = billingresetentity;


            }

        }
        // For Billing Period
        public void SplitBillingPeriod(string configData, MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {
            string BillingPeriod = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);
            if (ReadoutCommon.CalculateBcc(BillingPeriod.Substring(0, BillingPeriod.Length - 1), BillingPeriod.Length - 1, BillingPeriod.Substring(BillingPeriod.Length - 1, 1)) == false)
            {
                meterConfigEntity.billingresetentity = null;
            }
            else
            {
                string s = BillingPeriod.Substring(1, 2);

                if (s == "00")
                    billingresetentity.BillingPeriod = BillingPeriod1.EvenMonth;
                if (s == "01")
                    billingresetentity.BillingPeriod = BillingPeriod1.OddMonth;
                if (s == "02")
                    billingresetentity.BillingPeriod = BillingPeriod1.Monthly;
                meterConfigEntity.billingresetentity = billingresetentity;

            }
        }
        // Added for RS232Lock
        public void SplitRS232Lock(string configdata, MeterConfigurationConfigSection configsection, ref IECMeterConfigurationsNFEntity meterconfigentity)
        {

            string RS232Lock = configdata.Substring(configdata.IndexOf("|") + 2, configdata.Length - configdata.IndexOf("|") - 2);
            if (ReadoutCommon.CalculateBcc(RS232Lock.Substring(0, RS232Lock.Length - 1), RS232Lock.Length - 1, RS232Lock.Substring(RS232Lock.Length - 1, 1)) == false)
            {
                meterconfigentity.RS232Entity = null;
            }
            else
            {
                string s = RS232Lock.Substring(1, 2);
                if (s == "01")
                {
                    RS232Lockentity1.LockStatus = "NotLocked";
                    RS232Lockentity1.MeterID = configdata.Substring(5, configdata.IndexOf("\r") - 5);


                }
                if (s == "00")
                {
                    RS232Lockentity1.LockStatus = "Locked";
                    RS232Lockentity1.MeterID = configdata.Substring(5, configdata.IndexOf("\r") - 5);
                }
                meterconfigentity.RS232Entity = RS232Lockentity1;


            }
        }


        public string ParseRTC(string configData)
        {
            //string tempData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);

            // bool Bccres = ReadoutCommon.CalculateBcc(tempData.Substring(1), tempData.Length - 3, tempData.Substring(tempData.Length - 1, 1));
            //if (Bccres == true)
            //{
            string tempData = configData.Substring(0, 2) + "/" + configData.Substring(2, 2) + "/" + configData.Substring(4, 2) + " " + configData.Substring(6, 2) + ":" + configData.Substring(8, 2) + ":" + configData.Substring(10, 2);
            DateTime meterRTC = new DateTime();
            if (!DateTime.TryParse(tempData, new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out meterRTC))
                return null;//this.StatusMessage = "Invalid RTC.";
            else
                return tempData;
            // }
            //else
            //  return null;

        }
        public MDWithIPEntity ParseMDWithIP(string mdData)
        {
            MDWithIPEntity mdWithIPEntity = new MDWithIPEntity();
            MeterConfigurationConfigSection configSection = XMLLoader.GetConfigSection(ConfigurationParameter.MDWithIP);
            foreach (MeterConfigurationConfigSectionParameter configSectionParameter in configSection.Parameters)
            {
                if (configSectionParameter.Name == "MD1DemandType")
                {
                    mdWithIPEntity.KWDemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                }
                if (configSectionParameter.Name == "MD1TimeInterval")
                {
                    mdWithIPEntity.KWInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                }
                if (configSectionParameter.Name == "MD1SubInterval")
                {
                    mdWithIPEntity.KWSubInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                }
                if (configSectionParameter.Name == "MD2DemandType")
                {
                    mdWithIPEntity.KVADemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                }
                if (configSectionParameter.Name == "MD2TimeInterval")
                {
                    mdWithIPEntity.KVAInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                }
                if (configSectionParameter.Name == "MD2SubInterval")
                {
                    mdWithIPEntity.KVASubInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                }
            }
            return mdWithIPEntity;
        }

        public void ParseModeOfBilling(string ModeOfBilling, BillingResetEntity billingresetentity)
        {
            string s = ModeOfBilling.Substring(1, 2);
            if (s == "00")
            {
                billingresetentity.ModeOfBilling = IECBillingMode.EndofMonth;
                billingresetentity.Day = "01";
                billingresetentity.Hours = "00";
                billingresetentity.Minutes = "00";
            }
            else if (s == "01")
            {
                billingresetentity.ModeOfBilling = IECBillingMode.UserDefined;
                s = ModeOfBilling.Substring(3, 2);
                billingresetentity.Day = s;
                s = ModeOfBilling.Substring(5, 2);
                billingresetentity.Hours = s;
                s = ModeOfBilling.Substring(7, 2);
                billingresetentity.Minutes = s;
            }
        }
        public void ParseBillingPeriod(string BillingPeriod, BillingResetEntity billingresetentity)
        {
            string s = BillingPeriod.Substring(1, 2);

            if (s == "00")
                billingresetentity.BillingPeriod = BillingPeriod1.EvenMonth;
            if (s == "01")
                billingresetentity.BillingPeriod = BillingPeriod1.OddMonth;
            if (s == "02")
                billingresetentity.BillingPeriod = BillingPeriod1.Monthly;
        }
        public void ParseLockOutDays(string lockOutDaysData, BillingResetEntity billingresetentity)
        {
            string s = lockOutDaysData.Substring(1, 2);
            string dec = Convert.ToString(Convert.ToInt32(s, 16), 10);
            billingresetentity.ResetLockOutDays = dec;
        }

        public IECkvarSelectionEntity Parsekvarselection(string kvarData)
        {
            IECkvarSelectionEntity kvarselectionEntity = new IECkvarSelectionEntity();
            if (Convert.ToInt16(kvarData.Substring(1, 2)) == 0)
            { kvarselectionEntity.LagOnly = "1"; kvarselectionEntity.LagandLead = "0"; }
            else if (Convert.ToInt16(kvarData.Substring(1, 2)) == 1)
            { kvarselectionEntity.LagOnly = "0"; kvarselectionEntity.LagandLead = "1"; }

            return kvarselectionEntity;
        }
        public string ParseTODData(string fileText)
        {
            string valueOfParamater = string.Empty;
            if (fileText != null && fileText.Contains("<TOD>"))
            {
                int lengthOfValue = fileText.IndexOf("</TOD>") - 5 - fileText.IndexOf("<TOD>");
                valueOfParamater = fileText.Substring((fileText.IndexOf("<TOD>") + 5), lengthOfValue);
            }
            return valueOfParamater;
        }
        public int[] SplitDisplayParamaterCount(string dispParamOutputString)
        {
            int[] displayParmaterCountByType = new int[3];
            displayParmaterCountByType[0] = Int32.Parse(dispParamOutputString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);// ConvertHexToAscii(dispParamOutputString.Substring(2, 2));
            displayParmaterCountByType[1] = Int32.Parse(dispParamOutputString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);// (dispParamOutputString.Substring(4, 2));
            displayParmaterCountByType[2] = Int32.Parse(dispParamOutputString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);// (dispParamOutputString.Substring(6, 2));

            return displayParmaterCountByType;
        }

        #region ParsePushModeParameters
        /// <summary>
        /// Code Region Added by Vivek on 12 August 2011 (TNEB Project).
        /// Purpose : Parsing of output of Push Paramater Read Command to 
        /// get the paramaters to be selected on grod.
        /// </summary>
        /// <param name="pushParamOutputString"></param>
        /// <param name="paramCount"></param>
        /// <returns></returns>
        public Collection<string> ParsePushModeParameters(string pushParamOutputString, int paramCount)
        {
            Collection<string> colSelectedPushModeParam = new Collection<string>();
            int upperParseLimit = paramCount * 2;
            string strParamCode = string.Empty;
            PushModeParameters pushModeParamater;
            try
            {
                for (int i = 1; i < upperParseLimit; i += 2)
                {
                    try
                    {
                        #region
                        strParamCode = pushParamOutputString.Substring(i, 2);
                        //strParamCode = (Convert.ToInt32(strParamCode)).ToString("X2");
                        pushModeParamater = (PushModeParameters)EnumUtil.GetValueByParamCode(ProgrammingCommon.GetASCIIValue(strParamCode), typeof(PushModeParameters));
                        switch (pushModeParamater)
                        {
                            case PushModeParameters.ABCEncryptedDisplay:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.ABCEncryptedDisplay));
                                break;
                            case PushModeParameters.AnomalyIndicator:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.AnomalyIndicator));
                                break;
                            case PushModeParameters.BillingDateandTimeStamp:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingDateandTimeStamp));
                                break;
                            case PushModeParameters.BillingDemandKVADateandTime:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingDemandKVADateandTime));
                                break;
                            case PushModeParameters.BillingDemandKWDateandTime:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingDemandKWDateandTime));
                                break;
                            case PushModeParameters.BillingKVAh:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingKVAh));
                                break;
                            case PushModeParameters.BillingKVARhLag:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingKVARhLag));
                                break;
                            case PushModeParameters.BillingKVARhLead:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingKVARhLead));
                                break;
                            case PushModeParameters.BillingKWh:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingKWh));
                                break;
                            case PushModeParameters.BillingLoadFactor:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingLoadFactor));
                                break;
                            case PushModeParameters.BillingPeriodAveragePowerFactor:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingPeriodAveragePowerFactor));
                                break;
                            case PushModeParameters.BillingPowerOnHours:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingPowerOnHours));
                                break;
                            case PushModeParameters.BillingResetCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingResetCounter));
                                break;
                            case PushModeParameters.BillingTOUAveragePowerFactorRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingTOUAveragePowerFactorRate1toRate8));
                                break;
                            case PushModeParameters.BillingTOUDemandKVADateandTimeRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingTOUDemandKVADateandTimeRate1toRate8));
                                break;
                            case PushModeParameters.BillingTOUDemandKWDateandTimeRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingTOUDemandKWDateandTimeRate1toRate8));
                                break;
                            case PushModeParameters.BillingTOUKVAhRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingTOUKVAhRate1toRate8));
                                break;
                            case PushModeParameters.BillingTOUKVARhLagRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingTOUKVARhLagRate1toRate8));
                                break;
                            case PushModeParameters.BillingTOUKVARhLeadRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingTOUKVARhLeadRate1toRate8));
                                break;
                            case PushModeParameters.BillingTOUKWhRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillingTOUKWhRate1toRate8));
                                break;
                            case PushModeParameters.BPhaseCurrentReversalCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BPhaseCurrentReversalCounter));
                                break;
                            case PushModeParameters.BPhaseCurrentUnbalanceCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BPhaseCurrentUnbalanceCounter));
                                break;
                            case PushModeParameters.BPhaseMissingCurrentOpenCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BPhaseMissingCurrentOpenCounter));
                                break;
                            case PushModeParameters.BPhaseMissingPotentialCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BPhaseMissingPotentialCounter));
                                break;
                            case PushModeParameters.BPhaseTHD:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BPhaseTHD));
                                break;
                            case PushModeParameters.BPhaseVoltageUnbalanceCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BPhaseVoltageUnbalanceCounter));
                                break;
                            case PushModeParameters.CumulativeDemandKVA:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeDemandKVA));
                                break;
                            case PushModeParameters.CumulativeDemandKW:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeDemandKW));
                                break;
                            case PushModeParameters.CumulativeKVAh:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeKVAh));
                                break;
                            case PushModeParameters.CumulativeKVARh_lag:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeKVARh_lag));
                                break;
                            case PushModeParameters.CumulativeKVARh_lead:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeKVARh_lead));
                                break;
                            case PushModeParameters.CumulativeKWh:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeKWh));
                                break;
                            case PushModeParameters.CumulativeKWhFundamentalonly:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeKWhFundamentalonly));
                                break;
                            case PushModeParameters.CumulativePowerOffHoursinBPhase:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativePowerOffHoursinBPhase));
                                break;
                            case PushModeParameters.CumulativePowerOffHoursinRPhase:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativePowerOffHoursinRPhase));
                                break;
                            case PushModeParameters.CumulativePowerOffHoursinYPhase:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativePowerOffHoursinYPhase));
                                break;
                            case PushModeParameters.CurrentDemandKVA:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentDemandKVA));
                                break;
                            case PushModeParameters.CurrentDemandKVADate:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentDemandKVADate));
                                break;
                            case PushModeParameters.CurrentDemandKVATime:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentDemandKVATime));
                                break;
                            case PushModeParameters.CurrentDemandKW:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentDemandKW));
                                break;
                            case PushModeParameters.CurrentDemandKWDate:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentDemandKWDate));
                                break;
                            case PushModeParameters.CurrentDemandKWTime:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentDemandKWTime));
                                break;
                            case PushModeParameters.CurrentMonthAPF:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentMonthAPF));
                                break;

                            case PushModeParameters.CurrentMonthLoadFactor:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentMonthLoadFactor));
                                break;
                            case PushModeParameters.CurrentPowerONHours:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentPowerONHours));
                                break;
                            case PushModeParameters.CurrentShort_BypassCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CurrentShort_BypassCounter));
                                break;
                            case PushModeParameters.FirmwareVersion:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.FirmwareVersion));
                                break;
                            case PushModeParameters.Frequency:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.Frequency));
                                break;
                            case PushModeParameters.FrontCoverOpenCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.FrontCoverOpenCounter));
                                break;
                            case PushModeParameters.FrontCoverTamperOccuranceTamperID:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.FrontCoverTamperOccuranceTamperID));
                                break;
                            case PushModeParameters.FrontCoverTamperOccuranceTimeStamp:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.FrontCoverTamperOccuranceTimeStamp));
                                break;
                            case PushModeParameters.InstantaneousActivePower:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousActivePower));
                                break;
                            case PushModeParameters.InstantaneousApparentPower:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousApparentPower));
                                break;
                            case PushModeParameters.InstantaneousBPhaseCurrent:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousBPhaseCurrent));
                                break;
                            case PushModeParameters.InstantaneousBPhasePowerFactor:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousBPhasePowerFactor));
                                break;
                            case PushModeParameters.InstantaneousBPhaseVoltage:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousBPhaseVoltage));
                                break;
                            case PushModeParameters.InstantaneousReactivePowerLag:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousReactivePowerLag));
                                break;
                            case PushModeParameters.InstantaneousReactivePowerLead:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousReactivePowerLead));
                                break;
                            case PushModeParameters.InstantaneousRPhaseCurrent:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousRPhaseCurrent));
                                break;
                            case PushModeParameters.InstantaneousRPhasePowerFactor:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousRPhasePowerFactor));
                                break;
                            case PushModeParameters.InstantaneousRPhaseVoltage:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousRPhaseVoltage));
                                break;
                            case PushModeParameters.InstantaneousSignedPowerinKWinBPhase:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousSignedPowerinKWinBPhase));
                                break;
                            case PushModeParameters.InstantaneousSignedPowerinKWinRPhase:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousSignedPowerinKWinRPhase));
                                break;
                            case PushModeParameters.InstantaneousSignedPowerinKWinYPhase:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousSignedPowerinKWinYPhase));
                                break;
                            case PushModeParameters.InstantaneousTotalPowerFactorwithlagandleadsign:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousTotalPowerFactorwithlagandleadsign));
                                break;
                            case PushModeParameters.InstantaneousYPhaseCurrent:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousYPhaseCurrent));
                                break;
                            case PushModeParameters.InstantaneousYPhasePowerFactor:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousYPhasePowerFactor));
                                break;
                            case PushModeParameters.InstantaneousYPhaseVoltage:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.InstantaneousYPhaseVoltage));
                                break;
                            case PushModeParameters.LatestTamperOccurrenceTamperID:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LatestTamperOccurrenceTamperID));
                                break;
                            case PushModeParameters.LatestTamperOccurrenceTimeStamp:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LatestTamperOccurrenceTimeStamp));
                                break;
                            case PushModeParameters.LatestTamperRestorationTamperID:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LatestTamperRestorationTamperID));
                                break;
                            case PushModeParameters.LatestTamperRestorationTimeStamp:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LatestTamperRestorationTimeStamp));
                                break;
                            case PushModeParameters.LBPAPFreadingatthetimeofpriortoreset:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBPAPFreadingatthetimeofpriortoreset));
                                break;
                            case PushModeParameters.LBPCumulativeKVARhreadingatthetimeofpriortoreset:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBPCumulativeKVARhreadingatthetimeofpriortoreset));
                                break;
                            case PushModeParameters.LBPCumulativeKWhreadingatthetimeofpriortoreset:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBPCumulativeKWhreadingatthetimeofpriortoreset));
                                break;
                            case PushModeParameters.LBPMaximumdemandinKWatthetimeofpriortoreset:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBPMaximumdemandinKWatthetimeofpriortoreset));
                                break;
                            case PushModeParameters.LCDTest:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LCDTest));
                                break;
                            case PushModeParameters.MagneticTamperCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MagneticTamperCounter));
                                break;
                            case PushModeParameters.MagneticTamperStatus:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MagneticTamperStatus));
                                break;
                            case PushModeParameters.MaximumDemandinKWforLastReset:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MaximumDemandinKWforLastReset));
                                break;
                            case PushModeParameters.MeterConstant:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MeterConstant));
                                break;
                            case PushModeParameters.MeterReadingCountRegister:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MeterReadingCountRegister));
                                break;
                            case PushModeParameters.MeterSerialNumber:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MeterSerialNumber));
                                break;
                            case PushModeParameters.NeutralDisturbanceCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.NeutralDisturbanceCounter));
                                break;
                            case PushModeParameters.PowerOffhoursforthelastbillingperiod:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.PowerOffhoursforthelastbillingperiod));
                                break;
                            case PushModeParameters.PowerOffhourssincelastresetbillingperiod:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.PowerOffhourssincelastresetbillingperiod));
                                break;
                            case PushModeParameters.PoweronHoursinBattery:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.PoweronHoursinBattery));
                                break;
                            case PushModeParameters.RealDate:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RealDate));
                                break;
                            case PushModeParameters.RealTime:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RealTime));
                                break;
                            case PushModeParameters.ReverseCumulativeKVAh:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.ReverseCumulativeKVAh));
                                break;
                            case PushModeParameters.ReverseCumulativeKWh:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.ReverseCumulativeKWh));
                                break;
                            case PushModeParameters.RisingKVAWithElapsedTimeInMMSS:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RisingKVAWithElapsedTimeInMMSS));
                                break;
                            case PushModeParameters.RisingKWWithElapsedTimeInMMSS:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RisingKWWithElapsedTimeInMMSS));
                                break;
                            case PushModeParameters.RPhaseCurrentReversalCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RPhaseCurrentReversalCounter));
                                break;
                            case PushModeParameters.RPhaseCurrentUnbalanceCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RPhaseCurrentUnbalanceCounter));
                                break;
                            case PushModeParameters.RPhaseMissingCurrentOpenCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RPhaseMissingCurrentOpenCounter));
                                break;
                            case PushModeParameters.RPhaseMissingPotentialCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RPhaseMissingPotentialCounter));
                                break;
                            case PushModeParameters.RPhaseTHD:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RPhaseTHD));
                                break;
                            case PushModeParameters.RPhaseVoltageUnbalanceCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.RPhaseVoltageUnbalanceCounter));
                                break;
                            case PushModeParameters.TamperCounterCumulative:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TamperCounterCumulative));
                                break;
                            case PushModeParameters.TamperStatus:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TamperStatus));
                                break;
                            case PushModeParameters.TerminalCoverOpenCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TerminalCoverOpenCounter));
                                break;
                            case PushModeParameters.TotalPowerONHours:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TotalPowerONHours));
                                break;
                            case PushModeParameters.TOUAveragePowerFactorRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TOUAveragePowerFactorRate1toRate8));
                                break;
                            case PushModeParameters.TOUDemandKVADateandTimeRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TOUDemandKVADateandTimeRate1toRate8));
                                break;
                            case PushModeParameters.TOUDemandKWDateandTimeRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TOUDemandKWDateandTimeRate1toRate8));
                                break;
                            case PushModeParameters.TOUKVAhRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TOUKVAhRate1toRate8));
                                break;
                            case PushModeParameters.TOUKVARhLagRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TOUKVARhLagRate1toRate8));
                                break;
                            case PushModeParameters.TOUKVARhLeadRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TOUKVARhLeadRate1toRate8));
                                break;
                            case PushModeParameters.TOUKWhRate1toRate8:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TOUKWhRate1toRate8));
                                break;
                            case PushModeParameters.TwoPhaseOperationCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TwoPhaseOperationCounter));
                                break;
                            case PushModeParameters.VoltagePhaseSequence:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.VoltagePhaseSequence));
                                break;
                            case PushModeParameters.VoltagePhaseSequenceReversalCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.VoltagePhaseSequenceReversalCounter));
                                break;
                            case PushModeParameters.YPhaseCurrentReversalCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.YPhaseCurrentUnbalanceCounter));
                                break;
                            case PushModeParameters.YPhaseCurrentUnbalanceCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.YPhaseCurrentUnbalanceCounter));
                                break;
                            case PushModeParameters.YPhaseMissingCurrentOpenCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.YPhaseMissingCurrentOpenCounter));
                                break;
                            case PushModeParameters.YPhaseMissingPotentialCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.YPhaseMissingPotentialCounter));
                                break;
                            case PushModeParameters.YPhaseTHD:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.YPhaseTHD));
                                break;
                            case PushModeParameters.YPhaseVoltageUnbalanceCounter:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.YPhaseVoltageUnbalanceCounter));
                                break;

                            /* GKG 21/01/2013 TANGEDCO ISSUE*/
                            case PushModeParameters.MagneticInterferenceDateTime:
                                colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MagneticInterferenceDateTime));
                                break;
                            /* GKG 21/01/2013 TANGEDCO ISSUE*/

                        }
                        #endregion
                    }
                    catch (Exception ee)
                    {
                    }
                }
            }
            catch (Exception ee)
            {
            }
            return colSelectedPushModeParam;
        }
        #endregion

        #region ParseScrollModeParameters
        /// <summary>
        /// Code Region Added by Vivek on 12 August 2011 (TNEB Project).
        /// Purpose : Parsing of output of scroll Paramater Read Command to 
        /// get the paramaters to be selected on grod.
        /// </summary>
        /// <param name="pushParamOutputString"></param>
        /// <param name="paramCount"></param>
        /// <returns></returns>
        public Collection<string> ParseScrollModeParameters(string scrollParamOutputString, int paramCount)
        {
            Collection<string> colSelectedScrollModeParam = new Collection<string>();
            int upperParseLimit =paramCount  * 2; 
            string strParamCode = string.Empty;
            ScrollModeParameters scrollModeParamater;
            for (int i = 1; i < upperParseLimit; i += 2)
            {
                try
                {
                    strParamCode = scrollParamOutputString.Substring(i, 2);
                    // strParamCode = (Convert.ToInt32(strParamCode)).ToString("X2");
                    scrollModeParamater = (ScrollModeParameters)EnumUtil.GetValueByParamCode(ProgrammingCommon.GetASCIIValue(strParamCode), typeof(ScrollModeParameters));
                    #region
                    switch (scrollModeParamater)
                    {
                        case ScrollModeParameters.ABCEncryptedDisplay:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.ABCEncryptedDisplay));
                            break;
                        case ScrollModeParameters.AnomalyIndicator:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.AnomalyIndicator));
                            break;
                        case ScrollModeParameters.BillingDateandTimeStamp:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingDateandTimeStamp));
                            break;
                        case ScrollModeParameters.BillingDemandKVADateandTime:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingDemandKVADateandTime));
                            break;
                        case ScrollModeParameters.BillingDemandKWDateandTime:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingDemandKWDateandTime));
                            break;
                        case ScrollModeParameters.BillingKVAh:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingKVAh));
                            break;
                        case ScrollModeParameters.BillingKVARhLag:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingKVARhLag));
                            break;
                        case ScrollModeParameters.BillingKVARhLead:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingKVARhLead));
                            break;
                        case ScrollModeParameters.BillingKWh:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingKWh));
                            break;
                        case ScrollModeParameters.BillingLoadFactor:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingLoadFactor));
                            break;
                        case ScrollModeParameters.BillingPeriodAveragePowerFactor:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingPeriodAveragePowerFactor));
                            break;
                        case ScrollModeParameters.BillingPowerOnHours:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingPowerOnHours));
                            break;
                        case ScrollModeParameters.BillingResetCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingResetCounter));
                            break;
                        case ScrollModeParameters.BillingTOUAveragePowerFactorRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingTOUAveragePowerFactorRate1toRate8));
                            break;
                        case ScrollModeParameters.BillingTOUDemandKVADateandTimeRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingTOUDemandKVADateandTimeRate1toRate8));
                            break;
                        case ScrollModeParameters.BillingTOUDemandKWDateandTimeRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingTOUDemandKWDateandTimeRate1toRate8));
                            break;
                        case ScrollModeParameters.BillingTOUKVAhRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingTOUKVAhRate1toRate8));
                            break;
                        case ScrollModeParameters.BillingTOUKVARhLagRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingTOUKVARhLagRate1toRate8));
                            break;
                        case ScrollModeParameters.BillingTOUKVARhLeadRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingTOUKVARhLeadRate1toRate8));
                            break;
                        case ScrollModeParameters.BillingTOUKWhRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BillingTOUKWhRate1toRate8));
                            break;
                        case ScrollModeParameters.BPhaseCurrentReversalCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BPhaseCurrentReversalCounter));
                            break;
                        case ScrollModeParameters.BPhaseCurrentUnbalanceCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BPhaseCurrentUnbalanceCounter));
                            break;
                        case ScrollModeParameters.BPhaseMissingCurrentOpenCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BPhaseMissingCurrentOpenCounter));
                            break;
                        case ScrollModeParameters.BPhaseMissingPotentialCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BPhaseMissingPotentialCounter));
                            break;
                        case ScrollModeParameters.BPhaseTHD:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BPhaseTHD));
                            break;
                        case ScrollModeParameters.BPhaseVoltageUnbalanceCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.BPhaseVoltageUnbalanceCounter));
                            break;
                        case ScrollModeParameters.CumulativeDemandKVA:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativeDemandKVA));
                            break;
                        case ScrollModeParameters.CumulativeDemandKW:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativeDemandKW));
                            break;
                        case ScrollModeParameters.CumulativeKVAh:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativeKVAh));
                            break;
                        case ScrollModeParameters.CumulativeKVARh_lag:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativeKVARh_lag));
                            break;
                        case ScrollModeParameters.CumulativeKVARh_lead:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativeKVARh_lead));
                            break;
                        case ScrollModeParameters.CumulativeKWh:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativeKWh));
                            break;
                        case ScrollModeParameters.CumulativeKWhFundamentalonly:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativeKWhFundamentalonly));
                            break;
                        case ScrollModeParameters.CumulativePowerOffHoursinBPhase:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativePowerOffHoursinBPhase));
                            break;
                        case ScrollModeParameters.CumulativePowerOffHoursinRPhase:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativePowerOffHoursinRPhase));
                            break;
                        case ScrollModeParameters.CumulativePowerOffHoursinYPhase:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CumulativePowerOffHoursinYPhase));
                            break;
                        case ScrollModeParameters.CurrentDemandKVA:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentDemandKVA));
                            break;
                        case ScrollModeParameters.CurrentDemandKVADate:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentDemandKVADate));
                            break;
                        case ScrollModeParameters.CurrentDemandKVATime:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentDemandKVATime));
                            break;
                        case ScrollModeParameters.CurrentDemandKW:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentDemandKW));
                            break;
                        case ScrollModeParameters.CurrentDemandKWDate:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentDemandKWDate));
                            break;
                        case ScrollModeParameters.CurrentDemandKWTime:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentDemandKWTime));
                            break;
                        case ScrollModeParameters.CurrentMonthAPF:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentMonthAPF));
                            break;
                        case ScrollModeParameters.CurrentMonthLoadFactor:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentMonthLoadFactor));
                            break;
                        case ScrollModeParameters.CurrentPowerONHours:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentPowerONHours));
                            break;
                        case ScrollModeParameters.CurrentShort_BypassCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.CurrentShort_BypassCounter));
                            break;
                        case ScrollModeParameters.FirmwareVersion:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.FirmwareVersion));
                            break;
                        case ScrollModeParameters.Frequency:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.Frequency));
                            break;
                        case ScrollModeParameters.FrontCoverOpenCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.FrontCoverOpenCounter));
                            break;
                        case ScrollModeParameters.FrontCoverTamperOccuranceTamperID:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.FrontCoverTamperOccuranceTamperID));
                            break;
                        case ScrollModeParameters.FrontCoverTamperOccuranceTimeStamp:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.FrontCoverTamperOccuranceTimeStamp));
                            break;
                        case ScrollModeParameters.InstantaneousActivePower:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousActivePower));
                            break;
                        case ScrollModeParameters.InstantaneousApparentPower:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousApparentPower));
                            break;
                        case ScrollModeParameters.InstantaneousBPhaseCurrent:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousBPhaseCurrent));
                            break;
                        case ScrollModeParameters.InstantaneousBPhasePowerFactor:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousBPhasePowerFactor));
                            break;
                        case ScrollModeParameters.InstantaneousBPhaseVoltage:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousBPhaseVoltage));
                            break;
                        case ScrollModeParameters.InstantaneousReactivePowerLag:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousReactivePowerLag));
                            break;
                        case ScrollModeParameters.InstantaneousReactivePowerLead:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousReactivePowerLead));
                            break;
                        case ScrollModeParameters.InstantaneousRPhaseCurrent:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousRPhaseCurrent));
                            break;
                        case ScrollModeParameters.InstantaneousRPhasePowerFactor:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousRPhasePowerFactor));
                            break;
                        case ScrollModeParameters.InstantaneousRPhaseVoltage:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousRPhaseVoltage));
                            break;
                        case ScrollModeParameters.InstantaneousSignedPowerinKWinBPhase:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPowerinKWinBPhase));
                            break;
                        case ScrollModeParameters.InstantaneousSignedPowerinKWinRPhase:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPowerinKWinRPhase));
                            break;
                        case ScrollModeParameters.InstantaneousSignedPowerinKWinYPhase:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPowerinKWinYPhase));
                            break;
                        case ScrollModeParameters.InstantaneousTotalPowerFactorwithlagandleadsign:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousTotalPowerFactorwithlagandleadsign));
                            break;
                        case ScrollModeParameters.InstantaneousYPhaseCurrent:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousYPhaseCurrent));
                            break;
                        case ScrollModeParameters.InstantaneousYPhasePowerFactor:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousYPhasePowerFactor));
                            break;
                        case ScrollModeParameters.InstantaneousYPhaseVoltage:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousYPhaseVoltage));
                            break;
                        case ScrollModeParameters.LatestTamperOccurrenceTamperID:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LatestTamperOccurrenceTamperID));
                            break;
                        case ScrollModeParameters.LatestTamperOccurrenceTimeStamp:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LatestTamperOccurrenceTimeStamp));
                            break;
                        case ScrollModeParameters.LatestTamperRestorationTamperID:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LatestTamperRestorationTamperID));
                            break;
                        case ScrollModeParameters.LatestTamperRestorationTimeStamp:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LatestTamperRestorationTimeStamp));
                            break;
                        case ScrollModeParameters.LBPAPFreadingatthetimeofpriortoreset:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LBPAPFreadingatthetimeofpriortoreset));
                            break;
                        case ScrollModeParameters.LBPCumulativeKVARhreadingatthetimeofpriortoreset:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LBPCumulativeKVARhreadingatthetimeofpriortoreset));
                            break;
                        case ScrollModeParameters.LBPCumulativeKWhreadingatthetimeofpriortoreset:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LBPCumulativeKWhreadingatthetimeofpriortoreset));
                            break;
                        case ScrollModeParameters.LBPMaximumdemandinKWatthetimeofpriortoreset:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LBPMaximumdemandinKWatthetimeofpriortoreset));
                            break;
                        case ScrollModeParameters.LCDTest:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LCDTest));
                            break;
                        case ScrollModeParameters.MagneticTamperCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MagneticTamperCounter));
                            break;
                        case ScrollModeParameters.MagneticTamperStatus:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MagneticTamperStatus));
                            break;
                        case ScrollModeParameters.MaximumDemandinKWforLastReset:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MaximumDemandinKWforLastReset));
                            break;
                        case ScrollModeParameters.MeterConstant:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MeterConstant));
                            break;
                        case ScrollModeParameters.MeterReadingCountRegister:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MeterReadingCountRegister));
                            break;
                        case ScrollModeParameters.MeterSerialNumber:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MeterSerialNumber));
                            break;

                        case ScrollModeParameters.NeutralDisturbanceCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.NeutralDisturbanceCounter));
                            break;
                        case ScrollModeParameters.PowerOffhoursforthelastbillingperiod:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.PowerOffhoursforthelastbillingperiod));
                            break;
                        case ScrollModeParameters.PowerOffhourssincelastresetbillingperiod:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.PowerOffhourssincelastresetbillingperiod));
                            break;
                        case ScrollModeParameters.PoweronHoursinBattery:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.PoweronHoursinBattery));
                            break;
                        case ScrollModeParameters.RealDate:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RealDate));
                            break;
                        case ScrollModeParameters.RealTime:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RealTime));
                            break;
                        case ScrollModeParameters.ReverseCumulativeKVAh:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.ReverseCumulativeKVAh));
                            break;
                        case ScrollModeParameters.ReverseCumulativeKWh:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.ReverseCumulativeKWh));
                            break;
                        case ScrollModeParameters.RisingKVAWithElapsedTimeInMMSS:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RisingKVAWithElapsedTimeInMMSS));
                            break;
                        case ScrollModeParameters.RisingKWWithElapsedTimeInMMSS:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RisingKWWithElapsedTimeInMMSS));
                            break;
                        case ScrollModeParameters.RPhaseCurrentReversalCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RPhaseCurrentReversalCounter));
                            break;
                        case ScrollModeParameters.RPhaseCurrentUnbalanceCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RPhaseCurrentUnbalanceCounter));
                            break;
                        case ScrollModeParameters.RPhaseMissingCurrentOpenCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RPhaseMissingCurrentOpenCounter));
                            break;
                        case ScrollModeParameters.RPhaseMissingPotentialCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RPhaseMissingPotentialCounter));
                            break;
                        case ScrollModeParameters.RPhaseTHD:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RPhaseTHD));
                            break;
                        case ScrollModeParameters.RPhaseVoltageUnbalanceCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RPhaseVoltageUnbalanceCounter));
                            break;
                        case ScrollModeParameters.TamperCounterCumulative:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TamperCounterCumulative));
                            break;
                        case ScrollModeParameters.TamperStatus:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TamperStatus));
                            break;
                        case ScrollModeParameters.TerminalCoverOpenCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TerminalCoverOpenCounter));
                            break;
                        case ScrollModeParameters.TotalPowerONHours:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TotalPowerONHours));
                            break;
                        case ScrollModeParameters.TOUAveragePowerFactorRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TOUAveragePowerFactorRate1toRate8));
                            break;
                        case ScrollModeParameters.TOUDemandKVADateandTimeRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TOUDemandKVADateandTimeRate1toRate8));
                            break;
                        case ScrollModeParameters.TOUDemandKWDateandTimeRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TOUDemandKWDateandTimeRate1toRate8));
                            break;
                        case ScrollModeParameters.TOUKVAhRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TOUKVAhRate1toRate8));
                            break;
                        case ScrollModeParameters.TOUKVARhLagRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TOUKVARhLagRate1toRate8));
                            break;
                        case ScrollModeParameters.TOUKVARhLeadRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TOUKVARhLeadRate1toRate8));
                            break;
                        case ScrollModeParameters.TOUKWhRate1toRate8:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TOUKWhRate1toRate8));
                            break;
                        case ScrollModeParameters.TwoPhaseOperationCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.TwoPhaseOperationCounter));
                            break;
                        case ScrollModeParameters.VoltagePhaseSequence:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.VoltagePhaseSequence));
                            break;
                        case ScrollModeParameters.VoltagePhaseSequenceReversalCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.VoltagePhaseSequenceReversalCounter));
                            break;
                        case ScrollModeParameters.YPhaseCurrentReversalCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.YPhaseCurrentUnbalanceCounter));
                            break;
                        case ScrollModeParameters.YPhaseCurrentUnbalanceCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.YPhaseCurrentUnbalanceCounter));
                            break;
                        case ScrollModeParameters.YPhaseMissingCurrentOpenCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.YPhaseMissingCurrentOpenCounter));
                            break;
                        case ScrollModeParameters.YPhaseMissingPotentialCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.YPhaseMissingPotentialCounter));
                            break;
                        case ScrollModeParameters.YPhaseTHD:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.YPhaseTHD));
                            break;
                        case ScrollModeParameters.YPhaseVoltageUnbalanceCounter:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.YPhaseVoltageUnbalanceCounter));
                            break;
                        /* GKG 21/01/2013 TANGEDCO ISSUE*/
                        case ScrollModeParameters.MagneticInterferenceDateTime:
                            colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MagneticInterferenceDateTime));
                            break;
                        /* GKG 21/01/2013 TANGEDCO ISSUE*/



                    }
                    #endregion
                }
                catch (Exception tt)
                {
                }
            }
            return colSelectedScrollModeParam;
        }
        #endregion

        #region Parse High Resolution Mode Paramaters
        /// <summary>
        /// Code Region Added by Vivek on 12 August 2011 (TNEB Project).
        /// Purpose : Parsing of output of High Resolution paramater Read Command to 
        /// get the paramaters to be selected on grod.
        /// </summary>
        /// <param name="pushParamOutputString"></param>
        /// <param name="paramCount"></param>
        /// <returns></returns>
        public Collection<string> ParseHighResolutionModeParameters(string hrParamOutputString, int paramCount)
        {
            Collection<string> colSelectedHRModeParam = new Collection<string>();
            int upperParseLimit =paramCount  * 2;
            string strParamCode = string.Empty;
            HighResolutionModeParameters hrModeParamater;
            for (int i = 1; i < upperParseLimit; i += 2)
            {
                strParamCode = hrParamOutputString.Substring(i, 2);
                //strParamCode = (Convert.ToInt32(strParamCode)).ToString("X2");
                hrModeParamater = (HighResolutionModeParameters)EnumUtil.GetValueByParamCode(ProgrammingCommon.GetASCIIValue(strParamCode), typeof(HighResolutionModeParameters));
                switch (hrModeParamater)
                {
                    case HighResolutionModeParameters.kVAh:
                        colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kVAh));
                        break;
                    case HighResolutionModeParameters.kVArhlag:
                        colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kVArhlag));
                        break;
                    case HighResolutionModeParameters.kVArhlead:
                        colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kVArhlead));
                        break;
                    case HighResolutionModeParameters.kWh:
                        colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kWh));
                        break;
                }
            }
            return colSelectedHRModeParam;
        }
        #endregion

        public IECDailyLogEntity ParseDailyLogData(string dailyLogData)
        {
            IECDailyLogEntity dailylogentity = new IECDailyLogEntity();
            string s = dailyLogData.Substring(1, 2);
            string binary = Convert.ToString(Convert.ToInt32(s, 16), 2).PadLeft(8, '0');
            char[] c = new char[8];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = Convert.ToChar(binary.Substring(i, 1));
            }
            dailylogentity.CumulativeKWh = Convert.ToString(c[7]);
            dailylogentity.CumulativeKVARhLag = Convert.ToString(c[6]);
            dailylogentity.CumulativeKVARhLead = Convert.ToString(c[5]);
            dailylogentity.CumulativeKVAh = Convert.ToString(c[4]);
            dailylogentity.DailyMD1 = Convert.ToString(c[3]);
            dailylogentity.DailyMD2 = Convert.ToString(c[2]);
            return dailylogentity;
        }

        public string ConvertHexToAscii(string hexvalue)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexvalue.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexvalue.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }
        // Method is added for TOD data parsing for IEC
        public string ParseTODDataSP(string fileText)
        {
            string valueOfParamater = string.Empty;
            if (fileText != null && fileText.Contains("TU"))
            {
                int lengthOfValue = fileText.IndexOf("</TOD>") - 5 - fileText.IndexOf("<TOD>");
                valueOfParamater = fileText.Substring((fileText.IndexOf("<TOD>") + 5), lengthOfValue);
            }
            return valueOfParamater;
        }
        // 
        public void GetDataForSPhase(string data, ref  string[] dtmtou)
        {
            try
            {
                int counter = 0;

                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.METERCONFIGURATIONFORSP);

                string[] dtmtouData = new string[matches.Count];

                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    dtmtouData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateDataForSPhase(dtmtouData);

                counter = 0;
                int MaxLength = 0;

                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    if (tempData.Length > MaxLength)
                        MaxLength = tempData.Length;
                    counter++;
                }
                dtmtou = new string[MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        dtmtou[MaxLength] = tempData[MaxLength];
                    counter++;
                }

            }
            catch (Exception)
            {
            }
        }

        // Mohsin
        public void SplitData(string[] listtempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity)
        {
            try
            {
                string tempData = "";

                if (listtempData == null || listtempData.Length < 5)
                    return;

                tempData = listtempData[3];

                string data = tempData.Trim();
                IECMeterConfigurationsNFEntity meterconfigentity = new IECMeterConfigurationsNFEntity();
                meterconfigentity.TODEntity = new IECTODEntity();
                string mID = listtempData[1].Substring(data.IndexOf("XXX") + 4).Trim();
                meterconfigentity.TODEntity.MeterDataID = mID.Substring(10, mID.Length - 10).Trim();
                meterconfigentity.TODEntity.ReadingDateTime = 0;
                meterconfigentity.TODEntity.TODData = tempData;
                //namePlateDetailEntity.CMRIID = data.Substring(31, 9).Trim();
                //string cmriType = Convert.ToString(namePlateDetailEntity.CMRIID.Substring(0, 1));
                //if (cmriType.Trim().ToUpper().Equals("A"))
                //    namePlateDetailEntity.CMRIType = "Analogic";
                //else if (cmriType.Trim().ToUpper().Equals("S"))
                //    namePlateDetailEntity.CMRIType = "Sands";
                //else
                //    namePlateDetailEntity.CMRIType = "BCS";
                /*string type = data.Substring(38, 2).Trim();
                if (type == "01")
                    namePlateDetailEntity.MeterType = "3PH 4W";
                else
                    namePlateDetailEntity.MeterType = "";
                string curRating = data.Substring(40, 2).Trim();
                if (curRating == "00")
                    namePlateDetailEntity.CurrentRating = "10-40";
                else if (curRating == "01")
                    namePlateDetailEntity.CurrentRating = "10-60";
                else if (curRating == "02")
                    namePlateDetailEntity.CurrentRating = "05-30";
                else if (curRating == "03")
                    namePlateDetailEntity.CurrentRating = "10-100";
                else if (curRating == "04")
                    namePlateDetailEntity.CurrentRating = "20-100";
                /* GKG added enum for TANGEDCO tender 05/06/2013 */
                /*else if (curRating == "05")
                {
                    namePlateDetailEntity.CurrentRating = "50-100";
                }
                 GKG added enum for TANGEDCO tender 05/06/2013 
                else
                    namePlateDetailEntity.CurrentRating = "";
                string meterConstant = FormatterCommon.FilterData(data, 42, 4);
                if (!string.IsNullOrEmpty(meterConstant))
                    meterConstant = meterConstant + " Imp/kWh; Imp/kvarh";
                namePlateDetailEntity.MeterConstant = meterConstant;
                string day = data.Substring(46, 2).Trim();
                namePlateDetailEntity.ManufacturingDate = FormatterCommon.FilterData(data, 48, 4);// string.Concat(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Int16.Parse(day)), " - ", FormatterCommon.FilterData(data, 48, 4));
                if (data.Length > 52)
                {
                    string voltRating = data.Substring(52, 4).Trim();
                    voltRating = Convert.ToString(Convert.ToInt32(voltRating, 16), 2);
                    while (voltRating.Length < 16) { voltRating = "0" + voltRating; }
                    if (voltRating.Substring(0, 1) == "0")
                    {
                        voltRating = String.Format("{0:X4}", Convert.ToInt32(voltRating, 2));
                        voltRating = Convert.ToString(Convert.ToInt32(voltRating, 16));
                    }
                    else
                    {
                        voltRating = String.Format("{0:X4}", Convert.ToInt32(voltRating, 2));
                        voltRating = Convert.ToString(Convert.ToInt32(voltRating, 16));
                    }
                    namePlateDetailEntity.VoltageRating = voltRating;
                }
                else
                    namePlateDetailEntity.VoltageRating = "----";*/
                billingGeneralNFEntity.meterConfigurationDetail.Add(meterconfigentity);
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted name plate detail available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
