using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using System.Windows.Forms;
using CAB.IECChannel;
using CAB.IECChannel.ReadOut;
using CAB.Contracts;

namespace CAB.IECChannel.Formatter
{
    public class FormatterMDWithIP
    {
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

        public void SplitMDWithIPData(string configData, ref MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {
            MDWithIPEntity mdWithIPEntity = new MDWithIPEntity();
            string mdData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|")-2);

            if (ReadoutCommon.CalculateBcc(mdData.Substring(0, mdData.Length - 1), mdData.Length - 1, mdData.Substring(mdData.Length - 1, 1)) == false)
                meterConfigEntity.mdWithIPEntity = null;
            else
            {
                //foreach (MeterConfigurationConfigSectionParametersParameter configSectionParameter in configSection.Parameters)//meterConfiguration.ConfigSection
                //{
                //    if (configSectionParameter.Name == "MD1DemandType")
                //        mdWithIPEntity.KWDemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //    if (configSectionParameter.Name == "MD1TimeInterval")
                //        mdWithIPEntity.KWInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //    if (configSectionParameter.Name == "MD1SubInterval")
                //        mdWithIPEntity.KWSubInterval =Convert.ToInt16( mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //    if (configSectionParameter.Name == "MD2DemandType")
                //        mdWithIPEntity.KVADemandType = GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //    if (configSectionParameter.Name == "MD2TimeInterval")
                //        mdWithIPEntity.KVAInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //    if (configSectionParameter.Name == "MD2SubInterval")
                //        mdWithIPEntity.KVASubInterval = Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //}
                mdWithIPEntity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.mdWithIPEntity = mdWithIPEntity;
            }
        }

        public void SplitkvarSelectionData(string configData, ref MeterConfigurationConfigSection configSection, ref IECMeterConfigurationsNFEntity meterConfigEntity)
        {
            IECkvarSelectionEntity kvarselectionEntity = new IECkvarSelectionEntity();
            string mdData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);

            if (ReadoutCommon.CalculateBcc(mdData.Substring(0, mdData.Length - 1), mdData.Length - 1, mdData.Substring(mdData.Length - 1, 1)) == false)
                meterConfigEntity.mdWithIPEntity = null;
            else
            {
                //foreach (MeterConfigurationConfigSectionParametersParameter configSectionParameter in configSection.Parameters)//meterConfiguration.ConfigSection
                //{
                //    if (configSectionParameter.Name == "lagOnly")
                //        kvarselectionEntity.LagOnly  = "";//GetDemandType(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //    if (configSectionParameter.Name == "lagandLead")
                //        kvarselectionEntity.LagandLead = "";//Convert.ToInt16(mdData.Substring(Convert.ToInt16(configSectionParameter.StartIndex), Convert.ToInt16(configSectionParameter.NumberOfBytes) * 2));
                //}
                kvarselectionEntity.MeterID = configData.Substring(5, configData.IndexOf("\r") - 5);
                meterConfigEntity.kvarselectionEntity = kvarselectionEntity;
            }
        }
    }
}
