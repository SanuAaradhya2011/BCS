#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CAB.Entity;
using CAB.Parser.Entity;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Maps parsed billing data to billing entity
    /// </summary>
    public class Billing
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to get billing data and tarrif data 
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="isTarrif"></param>
        /// <returns></returns>
        public List<Object> GetData(List<ProfileData> inputData, List<ProfileData> instantData,List<ProfileData> fraudEnergyData, byte flag)
        {
            List<Object> resultData = new List<Object>();

            if (inputData != null && inputData.Count > 0 && inputData[0].ListMeterDataPacket.Count > 0)
            {

                resultData = flag == 2 ? GetTarrifEntity(inputData[0], fraudEnergyData[0]).Cast<Object>().ToList() :
                  flag == 1 ? GetBillingEntity(inputData[0], instantData[0], fraudEnergyData[0]).Cast<Object>().ToList() : 
                  GetTamperCounterEntity(inputData[0], instantData[0], fraudEnergyData[0]).Cast<Object>().ToList();
               
            }
           
            return resultData;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        /// Gets Filled billing Entity
        /// </summary>
        /// <param name="inputData"></param>
        private List<IECBillingEntity> GetBillingEntity(ProfileData inputData, ProfileData instantData, ProfileData fraudEnergyData)
        {
            List<DataElement> inputRecords;
            IECBillingEntity billingEntity;
            DataElement dataElement = null;
            List<IECBillingEntity> resultBillingData = new List<IECBillingEntity>();
            string defaultValue = "----";

            //Current
            List<DataElement> instantRecords = instantData.ListMeterDataPacket[0].ListDataElementValue;
            billingEntity = new IECBillingEntity();
            dataElement = Common.GetDataElementByDataDefId(instantRecords, 34);
            billingEntity.CumulativeEnergyKVAH = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 31);
            billingEntity.CumulativeEnergyKWH = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 32);
            billingEntity.CumulativeEnergyKVARHLag = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 33);
            billingEntity.CumulativeEnergyKVARHLead = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 35);
            billingEntity.CumulativeMD1 = Common.FormatCurrentMonthMD(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 36);
            billingEntity.CumulativeMD1TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value).ToString();

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 37);
            billingEntity.CumulativeMD2 = Common.FormatCurrentMonthMD(dataElement);

            dataElement = Common.GetDataElementByDataDefId(instantRecords, 38);
            billingEntity.CumulativeMD2TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value).ToString();
          

            billingEntity.History_ID = 0;
            dataElement = Common.GetDataElementByDataDefId(instantRecords, 19);
            billingEntity.AveragePowerFactor = Convert.ToDecimal(dataElement.Value).ToString("00.00") + "*pf";
            billingEntity.BillingResetType = defaultValue;
            billingEntity.PowerOnHours = defaultValue;           

            resultBillingData.Add(billingEntity);

            //history
            foreach (MeterDataPacket meterDatapacket in inputData.ListMeterDataPacket)
            {
                    inputRecords = meterDatapacket.ListDataElementValue;
                    billingEntity = new IECBillingEntity();
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 31);
                    billingEntity.CumulativeEnergyKWH = Common.FormatData(dataElement);
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 32);
                    billingEntity.CumulativeEnergyKVARHLag = Common.FormatData(dataElement);
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 33);
                    billingEntity.CumulativeEnergyKVARHLead = Common.FormatData(dataElement);
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 34);
                    billingEntity.CumulativeEnergyKVAH = Common.FormatData(dataElement);


                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 302);
                    billingEntity.AveragePowerFactor = Convert.ToDecimal(dataElement.Value).ToString("00.00")+"*pf";
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 360);
                    int billingType = Convert.ToInt32(dataElement.Value);
                    billingEntity.BillingResetType = billingType == 64 ? "S AUTO" : billingType == 35 ? "S MANUAL" : billingType == 0 ? "NO BILLING" : defaultValue;                  
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 35);
                    billingEntity.CumulativeMD1 = Common.FormatCurrentMonthMD(dataElement);
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 36);
                    billingEntity.CumulativeMD1TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value).ToString();

                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 37);
                    billingEntity.CumulativeMD2 = Common.FormatCurrentMonthMD(dataElement);
                    dataElement = Common.GetDataElementByDataDefId(inputRecords, 38);
                    billingEntity.CumulativeMD2TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value).ToString();

                    billingEntity.History_ID = inputData.ListMeterDataPacket.IndexOf(meterDatapacket) + 1;
                    billingEntity.PowerOnHours = defaultValue;
                    billingEntity.LoadFactor = "0000*%";
                    

                    resultBillingData.Add(billingEntity);                
            }

            return resultBillingData;
        }

        /// <summary>
        /// Gets Filled tamper entity for billing
        /// </summary>
        /// <param name="inputData"></param>
        private List<TamperCounterGeneralEntity> GetTamperCounterEntity(ProfileData inputData, ProfileData instantData, ProfileData fraudEnergyData)
        {
            List<TamperCounterGeneralEntity> listTamperCounterEntity = new List<TamperCounterGeneralEntity>();
            TamperCounterGeneralEntity tamperCounterEntity = null;
            List<DataElement> inputRecords;
            //Current
            tamperCounterEntity = new TamperCounterGeneralEntity();
            tamperCounterEntity.BillingCounter = 0;
            tamperCounterEntity.History_ID = 0;
            tamperCounterEntity.RelatedTo = "G";
            tamperCounterEntity.CTOpenBPhaseTamperCounter = 0;
            tamperCounterEntity.CTOpenRPhaseTamperCounter = 0;
            tamperCounterEntity.CTOpenYPhaseTamperCounter = 0;
            tamperCounterEntity.CTShortTamperCounter = 0;
            tamperCounterEntity.CurrentImbalanceBPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentImbalanceRPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentImbalanceYPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentPhaseReversalTamperCounter = 0;
            tamperCounterEntity.CurrentReversalBPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentReversalRPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentReversalYPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentWithoutVoltageBPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentWithoutVoltageRPhaseTamperCounter = 0;
            tamperCounterEntity.CurrentWithoutVoltageYPhaseTamperCounter = 0;
            tamperCounterEntity.FrontCoverOpeningTamperCounter = 0;
            tamperCounterEntity.HighOverVoltageBPhaseTamperCounter = 0;
            tamperCounterEntity.HighOverVoltageRPhaseTamperCounter = 0;
            tamperCounterEntity.HighOverVoltageYPhaseTamperCounter = 0;
            tamperCounterEntity.LowPowerFactorBPhaseTamperCounter = 0;
            tamperCounterEntity.LowPowerFactorRPhaseTamperCounter = 0;
            tamperCounterEntity.LowPowerFactorYPhaseTamperCounter = 0;
            tamperCounterEntity.LowUnderVoltageBPhaseTamperCounter = 0;
            tamperCounterEntity.LowUnderVoltageRPhaseTamperCounter = 0;
            tamperCounterEntity.LowUnderVoltageYPhaseTamperCounter = 0;
            tamperCounterEntity.MagneticInfluenceTamperCounter = 0;
            tamperCounterEntity.MissingPotentialBPhaseTamperCounter = 0;
            tamperCounterEntity.MissingPotentialRPhaseTamperCounter = 0;
            tamperCounterEntity.MissingPotentialYPhaseTamperCounter = 0;
            tamperCounterEntity.NeutralDisturbanceTamperCounter = 0;
            tamperCounterEntity.OnePhaseNeutralAbsentTamperCounter = 0;
            tamperCounterEntity.ReadingDateTime = 0;
            tamperCounterEntity.TerminalCoverOpeningTamperCounter = 0;
            tamperCounterEntity.VoltagePhaseReversalTamperCounter = 0;
            tamperCounterEntity.VoltageImbalanceBPhaseTamperCounter = 0;
            tamperCounterEntity.VoltageImbalanceRPhaseTamperCounter = 0;
            tamperCounterEntity.VoltageImbalanceYPhaseTamperCounter = 0;
            tamperCounterEntity.BillingTimeStamp = 0;

            listTamperCounterEntity.Add(tamperCounterEntity);

            //history
            foreach (MeterDataPacket meterDatapacket in inputData.ListMeterDataPacket)
            {
                inputRecords = meterDatapacket.ListDataElementValue;

                tamperCounterEntity = new TamperCounterGeneralEntity();
                tamperCounterEntity.BillingCounter = 0;
                tamperCounterEntity.History_ID = inputData.ListMeterDataPacket.IndexOf(meterDatapacket)+1;
                tamperCounterEntity.RelatedTo = "B";
                tamperCounterEntity.CTOpenBPhaseTamperCounter = 0;
                tamperCounterEntity.CTOpenRPhaseTamperCounter = 0;
                tamperCounterEntity.CTOpenYPhaseTamperCounter = 0;
                tamperCounterEntity.CTShortTamperCounter = 0;
                tamperCounterEntity.CurrentImbalanceBPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentImbalanceRPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentImbalanceYPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentPhaseReversalTamperCounter = 0;
                tamperCounterEntity.CurrentReversalBPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentReversalRPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentReversalYPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentWithoutVoltageBPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentWithoutVoltageRPhaseTamperCounter = 0;
                tamperCounterEntity.CurrentWithoutVoltageYPhaseTamperCounter = 0;
                tamperCounterEntity.FrontCoverOpeningTamperCounter = 0;
                tamperCounterEntity.HighOverVoltageBPhaseTamperCounter = 0;
                tamperCounterEntity.HighOverVoltageRPhaseTamperCounter = 0;
                tamperCounterEntity.HighOverVoltageYPhaseTamperCounter = 0;
                tamperCounterEntity.LowPowerFactorBPhaseTamperCounter = 0;
                tamperCounterEntity.LowPowerFactorRPhaseTamperCounter = 0;
                tamperCounterEntity.LowPowerFactorYPhaseTamperCounter = 0;
                tamperCounterEntity.LowUnderVoltageBPhaseTamperCounter = 0;
                tamperCounterEntity.LowUnderVoltageRPhaseTamperCounter = 0;
                tamperCounterEntity.LowUnderVoltageYPhaseTamperCounter = 0;
                tamperCounterEntity.MagneticInfluenceTamperCounter = 0;
                tamperCounterEntity.MissingPotentialBPhaseTamperCounter = 0;
                tamperCounterEntity.MissingPotentialRPhaseTamperCounter = 0;
                tamperCounterEntity.MissingPotentialYPhaseTamperCounter = 0;
                tamperCounterEntity.NeutralDisturbanceTamperCounter = 0;
                tamperCounterEntity.OnePhaseNeutralAbsentTamperCounter = 0;
                tamperCounterEntity.ReadingDateTime = 0;
                tamperCounterEntity.TerminalCoverOpeningTamperCounter = 0;
                tamperCounterEntity.VoltagePhaseReversalTamperCounter = 0;
                tamperCounterEntity.VoltageImbalanceBPhaseTamperCounter = 0;
                tamperCounterEntity.VoltageImbalanceRPhaseTamperCounter = 0;
                tamperCounterEntity.VoltageImbalanceYPhaseTamperCounter = 0;

                 DataElement dataElement = Common.GetDataElementByDataDefId(inputRecords, 30);
                 tamperCounterEntity.BillingTimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);

                listTamperCounterEntity.Add(tamperCounterEntity);
            }

            return listTamperCounterEntity;
        }


        /// <summary>
        /// Gets Filled Tarrif Entity
        /// </summary>
        /// <param name="inputData"></param>
        private List<TariffEntity> GetTarrifEntity(ProfileData inputData, ProfileData fraudEnergyData)
        {
            List<DataElement> inputRecords;
            TariffEntity tarrifEntity;
            DataElement dataElement = null;
            string defaultPowerFactor = "00.00*Pf";
            List<TariffEntity> resultTarrifData = new List<TariffEntity>();

            List<DataElement> fraudEnergyRecords = fraudEnergyData.ListMeterDataPacket[0].ListDataElementValue;
            tarrifEntity = new TariffEntity();
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 91);
            tarrifEntity.Tariff1_kWh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 92);
            tarrifEntity.Tariff2_kWh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 93);
            tarrifEntity.Tariff3_kWh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 94);
            tarrifEntity.Tariff4_kWh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 95);
            tarrifEntity.Tariff5_kWh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 96);
            tarrifEntity.Tariff6_kWh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 97);
            tarrifEntity.Tariff7_kWh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 98);
            tarrifEntity.Tariff8_kWh = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 115);
            tarrifEntity.Tariff1_kVAh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 116);
            tarrifEntity.Tariff2_kVAh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 117);
            tarrifEntity.Tariff3_kVAh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 118);
            tarrifEntity.Tariff4_kVAh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 119);
            tarrifEntity.Tariff5_kVAh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 120);
            tarrifEntity.Tariff6_kVAh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 121);
            tarrifEntity.Tariff7_kVAh = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 122);
            tarrifEntity.Tariff8_kVAh = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 125);
            tarrifEntity.Tariff1_MD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 126);
            tarrifEntity.Tariff2_MD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 127);
            tarrifEntity.Tariff3_MD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 128);
            tarrifEntity.Tariff4_MD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 129);
            tarrifEntity.Tariff5_MD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 130);
            tarrifEntity.Tariff6_MD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 131);
            tarrifEntity.Tariff7_MD1 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 132);
            tarrifEntity.Tariff8_MD1 = Common.FormatCurrentMonthMD(dataElement);

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 133);
            tarrifEntity.Tariff1_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 134);
            tarrifEntity.Tariff2_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 135);
            tarrifEntity.Tariff3_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 136);
            tarrifEntity.Tariff4_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 137);
            tarrifEntity.Tariff5_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 138);
            tarrifEntity.Tariff6_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 139);
            tarrifEntity.Tariff7_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 140);
            tarrifEntity.Tariff8_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);


            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 143);
            tarrifEntity.Tariff1_MD2 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 144);
            tarrifEntity.Tariff2_MD2 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 145);
            tarrifEntity.Tariff3_MD2 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 146);
            tarrifEntity.Tariff4_MD2 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 147);
            tarrifEntity.Tariff5_MD2 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 148);
            tarrifEntity.Tariff6_MD2 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 149);
            tarrifEntity.Tariff7_MD2 = Common.FormatCurrentMonthMD(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 150);
            tarrifEntity.Tariff8_MD2 = Common.FormatCurrentMonthMD(dataElement);

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 151);
            tarrifEntity.Tariff1_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 152);
            tarrifEntity.Tariff2_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 153);
            tarrifEntity.Tariff3_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 154);
            tarrifEntity.Tariff4_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 155);
            tarrifEntity.Tariff5_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 156);
            tarrifEntity.Tariff6_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 157);
            tarrifEntity.Tariff7_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 158);
            tarrifEntity.Tariff8_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 99);
            tarrifEntity.Tariff1_kVARh_lag = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 100);
            tarrifEntity.Tariff2_kVARh_lag = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 101);
            tarrifEntity.Tariff3_kVARh_lag = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 102);
            tarrifEntity.Tariff4_kVARh_lag = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 103);
            tarrifEntity.Tariff5_kVARh_lag = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 104);
            tarrifEntity.Tariff6_kVARh_lag = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 105);
            tarrifEntity.Tariff7_kVARh_lag = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 106);
            tarrifEntity.Tariff8_kVARh_lag = Common.FormatData(dataElement);

            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 107);
            tarrifEntity.Tariff1_kVARh_lead = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 108);
            tarrifEntity.Tariff2_kVARh_lead = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 109);
            tarrifEntity.Tariff3_kVARh_lead = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 110);
            tarrifEntity.Tariff4_kVARh_lead = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 374);
            tarrifEntity.Tariff5_kVARh_lead = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 111);
            tarrifEntity.Tariff6_kVARh_lead = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 112);
            tarrifEntity.Tariff7_kVARh_lead = Common.FormatData(dataElement);
            dataElement = Common.GetDataElementByDataDefId(fraudEnergyRecords, 113);
            tarrifEntity.Tariff8_kVARh_lead = Common.FormatData(dataElement);

            tarrifEntity.Tariff1_Aver_PF = defaultPowerFactor;
            tarrifEntity.Tariff2_Aver_PF = defaultPowerFactor;
            tarrifEntity.Tariff3_Aver_PF = defaultPowerFactor;
            tarrifEntity.Tariff4_Aver_PF = defaultPowerFactor;
            tarrifEntity.Tariff5_Aver_PF = defaultPowerFactor;
            tarrifEntity.Tariff6_Aver_PF = defaultPowerFactor;
            tarrifEntity.Tariff7_Aver_PF = defaultPowerFactor;
            tarrifEntity.Tariff8_Aver_PF = defaultPowerFactor;

            tarrifEntity.HistoryID = 0;

            resultTarrifData.Add(tarrifEntity);


            foreach (MeterDataPacket meterDatapacket in inputData.ListMeterDataPacket)
            {
                inputRecords = meterDatapacket.ListDataElementValue;
                tarrifEntity = new TariffEntity();

                dataElement = Common.GetDataElementByDataDefId(inputRecords, 304);
                tarrifEntity.Tariff1_kWh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 305);
                tarrifEntity.Tariff2_kWh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 306);
                tarrifEntity.Tariff3_kWh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 307);
                tarrifEntity.Tariff4_kWh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 308);
                tarrifEntity.Tariff5_kWh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 309);
                tarrifEntity.Tariff6_kWh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 310);
                tarrifEntity.Tariff7_kWh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 311);
                tarrifEntity.Tariff8_kWh = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(inputRecords, 315);
                tarrifEntity.Tariff1_kVAh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 316);
                tarrifEntity.Tariff2_kVAh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 317);
                tarrifEntity.Tariff3_kVAh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 318);
                tarrifEntity.Tariff4_kVAh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 319);
                tarrifEntity.Tariff5_kVAh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 320);
                tarrifEntity.Tariff6_kVAh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 321);
                tarrifEntity.Tariff7_kVAh = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 322);
                tarrifEntity.Tariff8_kVAh = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(inputRecords, 325);
                tarrifEntity.Tariff1_MD1 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 326);
                tarrifEntity.Tariff2_MD1 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 327);
                tarrifEntity.Tariff3_MD1 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 328);
                tarrifEntity.Tariff4_MD1 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 329);
                tarrifEntity.Tariff5_MD1 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 330);
                tarrifEntity.Tariff6_MD1 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 331);
                tarrifEntity.Tariff7_MD1 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 332);
                tarrifEntity.Tariff8_MD1 = Common.FormatCurrentMonthMD(dataElement);

                dataElement = Common.GetDataElementByDataDefId(inputRecords, 333);
                tarrifEntity.Tariff1_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 334);
                tarrifEntity.Tariff2_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 335);
                tarrifEntity.Tariff3_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 336);
                tarrifEntity.Tariff4_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 337);
                tarrifEntity.Tariff5_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 338);
                tarrifEntity.Tariff6_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 339);
                tarrifEntity.Tariff7_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 340);
                tarrifEntity.Tariff8_MD1_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);


                dataElement = Common.GetDataElementByDataDefId(inputRecords, 343);
                tarrifEntity.Tariff1_MD2 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 344);
                tarrifEntity.Tariff2_MD2 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 345);
                tarrifEntity.Tariff3_MD2 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 346);
                tarrifEntity.Tariff4_MD2 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 347);
                tarrifEntity.Tariff5_MD2 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 348);
                tarrifEntity.Tariff6_MD2 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 349);
                tarrifEntity.Tariff7_MD2 = Common.FormatCurrentMonthMD(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 350);
                tarrifEntity.Tariff8_MD2 = Common.FormatCurrentMonthMD(dataElement);

                dataElement = Common.GetDataElementByDataDefId(inputRecords, 351);
                tarrifEntity.Tariff1_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 352);
                tarrifEntity.Tariff2_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 353);
                tarrifEntity.Tariff3_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 354);
                tarrifEntity.Tariff4_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 355);
                tarrifEntity.Tariff5_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 356);
                tarrifEntity.Tariff6_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 357);
                tarrifEntity.Tariff7_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 358);
                tarrifEntity.Tariff8_MD2_TimeStamp = Common.StringToLongDateTimeFormat(dataElement.Value);

                dataElement = Common.GetDataElementByDataDefId(inputRecords, 362);
                tarrifEntity.Tariff1_kVARh_lag = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 363);
                tarrifEntity.Tariff2_kVARh_lag = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 364);
                tarrifEntity.Tariff3_kVARh_lag = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 365);
                tarrifEntity.Tariff4_kVARh_lag = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 366);
                tarrifEntity.Tariff5_kVARh_lag = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 367);
                tarrifEntity.Tariff6_kVARh_lag = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 368);
                tarrifEntity.Tariff7_kVARh_lag = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 369);
                tarrifEntity.Tariff8_kVARh_lag = Common.FormatData(dataElement);

                dataElement = Common.GetDataElementByDataDefId(inputRecords, 370);
                tarrifEntity.Tariff1_kVARh_lead= Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 371);
                tarrifEntity.Tariff2_kVARh_lead = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 372);
                tarrifEntity.Tariff3_kVARh_lead = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 373);
                tarrifEntity.Tariff4_kVARh_lead = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 374);
                tarrifEntity.Tariff5_kVARh_lead = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 375);
                tarrifEntity.Tariff6_kVARh_lead = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 376);
                tarrifEntity.Tariff7_kVARh_lead = Common.FormatData(dataElement);
                dataElement = Common.GetDataElementByDataDefId(inputRecords, 377);
                tarrifEntity.Tariff8_kVARh_lead = Common.FormatData(dataElement);

                tarrifEntity.Tariff1_Aver_PF = defaultPowerFactor;
                tarrifEntity.Tariff2_Aver_PF = defaultPowerFactor;
                tarrifEntity.Tariff3_Aver_PF = defaultPowerFactor;
                tarrifEntity.Tariff4_Aver_PF = defaultPowerFactor;
                tarrifEntity.Tariff5_Aver_PF = defaultPowerFactor;
                tarrifEntity.Tariff6_Aver_PF = defaultPowerFactor;
                tarrifEntity.Tariff7_Aver_PF = defaultPowerFactor;
                tarrifEntity.Tariff8_Aver_PF = defaultPowerFactor;

                tarrifEntity.HistoryID = inputData.ListMeterDataPacket.IndexOf(meterDatapacket) + 1;

                resultTarrifData.Add(tarrifEntity);
            }

            return resultTarrifData;
        }
        #endregion
    }
}
