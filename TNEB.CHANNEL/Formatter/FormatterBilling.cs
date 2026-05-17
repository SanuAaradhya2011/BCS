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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using LTCTBLL;
using CABEntity;
using CAB.IECFramework.Utility;
using CAB.IECChannel.ReadOut;

namespace CAB.IECChannel.Formatter
{
    public class FormatterBilling
    {
        //UtilityDetails UtilityDetails = new UtilityDetails();

        public string Parameters = "";
        public string firmwareVersion = "";
        public string billingType = "";
        FormatterHeaderInfo formatterHeaderInfo = new FormatterHeaderInfo();
        IECBillingGeneralNFEntity billingGeneralNFEntity = new IECBillingGeneralNFEntity();
        FormatterFraudEnergy formatterFraudEnergy = new FormatterFraudEnergy();
        FormatterProgramming formatterProgramming = new FormatterProgramming();
        FormatterRTCUpdate formatterRTCUpdate = new FormatterRTCUpdate();
        FormatterPhasor formatterPhasor = new FormatterPhasor();
        FormatterLoadSurvey formatterLoadSurvey = new FormatterLoadSurvey();
        FormatterTamper formatterTamper = new FormatterTamper();
        FormatterDTMLoadSurvey formatterDTMLoadSurvey = new FormatterDTMLoadSurvey();
        FormatterDTMDailyProfile formatterDTMDailyProfile = new FormatterDTMDailyProfile();
        FormatterNamePlateDetail formatterNamePlateDetail = new FormatterNamePlateDetail();
        FormatterConfigurations formaterconfiguration = new FormatterConfigurations();

        private string UtilityName;
        bool isWBExportVCL = false;
        private const string IMPORT = "";
        private const string EXPORT = " (E)";
        public FormatterBilling()
        {
            //if (UtilityDetails.UtilityName == UtilityEntity.WBEXPORTVCL)
            //{
            //    isWBExportVCL = true;
            //}
        }
        public string FirmWareVersion { get; set; }
        public void GetData(string data, List<IECBillingGeneralNFEntity> master, Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>> readOuts)
        {
            int counter = 0;
            string Original = data;
            data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
            MatchCollection matches = FormatterCommon.ValidateData(data);
            string[] generalData = new string[matches.Count];
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                generalData[counter++] = groups[0].Value;
            }
            string[] GeneralData = FormatterCommon.RemoveDuplicateData(generalData);
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0)) counter++;
            string[] general = new string[counter];

            string[] headerInfo = new string[counter];
            string[,] fraudEnergy = new string[counter, 1];
            string[,] programming = new string[counter, 1];
            string[,] rtcUpdate = new string[counter, 1];
            string[,] phasor = new string[counter, 1];
            string[,] loadSurvey = new string[counter, 1];
            string[,] tamper = new string[counter, 1];
            string[,] dtmLoadSurvey = new string[counter, 1];
            string[,] dtmDailyProfile = new string[counter, 1];
            string[] namePlateDetail = new string[counter];


            if (readOuts.ContainsKey(ReadOutItem.HeaderDetails))
                headerInfo = new string[readOuts[ReadOutItem.HeaderDetails].Count];
            if (readOuts.ContainsKey(ReadOutItem.FraudEnergy))
                fraudEnergy = new string[readOuts[ReadOutItem.FraudEnergy].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.Transaction))
                programming = new string[readOuts[ReadOutItem.Transaction].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.RTCUpdate))
                rtcUpdate = new string[readOuts[ReadOutItem.RTCUpdate].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.Phasor))
                phasor = new string[readOuts[ReadOutItem.Phasor].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.LoadSurvey))
                loadSurvey = new string[readOuts[ReadOutItem.LoadSurvey].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.Tamper))
                tamper = new string[readOuts[ReadOutItem.Tamper].Count, 1];

            dtmLoadSurvey = new string[counter, 1];

            if (readOuts.ContainsKey(ReadOutItem.DailyProfile))
                dtmDailyProfile = new string[readOuts[ReadOutItem.DailyProfile].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.HeaderDetails))
                namePlateDetail = new string[readOuts[ReadOutItem.HeaderDetails].Count];


            if (headerInfo == null || headerInfo.Length == 0) headerInfo = new string[1];
            if (namePlateDetail == null || namePlateDetail.Length == 0) namePlateDetail = new string[1];
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0))
            {
                general[counter] = GeneralData[counter];
                counter++;
            }
            formatterHeaderInfo.GetData(Original, ref headerInfo);
            formatterFraudEnergy.GetData(Original, ref fraudEnergy);
            formatterProgramming.GetData(Original, ref programming);
            formatterRTCUpdate.GetData(Original, ref rtcUpdate);
            formatterPhasor.GetData(Original, ref phasor);
            int[] loadSurveyDaysCnt = formatterLoadSurvey.GetData(Original, ref loadSurvey);
            formatterTamper.GetData(Original, ref tamper);
            formatterDTMLoadSurvey.GetData(Original, ref dtmLoadSurvey);
            formatterDTMDailyProfile.GetData(Original, ref dtmDailyProfile);
            formatterNamePlateDetail.GetData(Original, ref namePlateDetail);

            //int MaxLength = GetMaxLength(headerInfo, general, fraudEnergy, programming, rtcUpdate, phasor, loadSurvey, tamper, dtmLoadSurvey, dtmDailyProfile, namePlateDetail);
            //for (counter = 0; counter < MaxLength; counter++)
            //{
            IECBillingGeneralNFEntity billingGeneralNFEntity = new IECBillingGeneralNFEntity();

            billingGeneralNFEntity.listGeneralData = new List<GeneralData>();
            for (counter = 0; counter < general.GetLength(0); counter++)
            {
                GeneralData generalDataItem = new GeneralData();
                generalDataItem.MeterData = new IECMeterDataEntity();
                if (general.GetLength(0) != 0)
                {
                    generalDataItem.CurrentInstant = new InstantPowerEntity();
                    generalDataItem.CurrentGeneral = new GeneralEntity();
                    generalDataItem.CurrentBilling = new IECBillingEntity();
                    generalDataItem.CurrentTariff = new TariffEntity();
                    generalDataItem.CurrentTamper = new TamperCounterGeneralEntity();
                    generalDataItem.listHistoryBilling = new List<IECBillingEntity>();
                    generalDataItem.listHistoryTariff = new List<TariffEntity>();
                    generalDataItem.listHistoryTamper = new List<TamperCounterGeneralEntity>();
                    this.SplitData(general[counter], ref generalDataItem);

                }
                billingGeneralNFEntity.listGeneralData.Add(generalDataItem);
            }

            billingGeneralNFEntity.listFraudEnergy = new List<IECFraudEnergyEntity>();
            for (counter = 0; counter < fraudEnergy.GetLength(0); counter++)
            {
                if (fraudEnergy.GetLength(0) != 0)
                {
                    formatterFraudEnergy.SplitData(getData(fraudEnergy, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listTransactionData = new List<TransactionData>();
            for (counter = 0; counter < programming.GetLength(0); counter++)
            {
                if (programming.GetLength(0) != 0)
                {
                    formatterProgramming.SplitData(getData(programming, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listRTCUpdate = new List<RTCUpdateEntity>();
            for (counter = 0; counter < rtcUpdate.GetLength(0); counter++)
            {
                if (rtcUpdate.GetLength(0) != 0)
                {
                    formatterRTCUpdate.SplitData(getData(rtcUpdate, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listPhasor = new List<IECPhasorEntity>();
            for (counter = 0; counter < phasor.GetLength(0); counter++)
            {
                if (phasor.GetLength(0) != 0)
                {
                    formatterPhasor.SplitData(getData(phasor, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listLoadSurveyData = new List<LoadSurveyData>();
            for (counter = 0; counter < loadSurvey.GetLength(0); counter++)
            {
                if (loadSurvey.GetLength(0) != 0)
                {
                    //billingGeneralNFEntity.LoadSurvey = new List<LoadSurveyEntity>();

                    //for (int x = 0; x < loadSurvey.GetLength(0); x++)
                    //{

                    formatterLoadSurvey.SplitData(getData(loadSurvey, counter), ref billingGeneralNFEntity, loadSurveyDaysCnt[counter]);
                    //}
                }
            }
            billingGeneralNFEntity.listTamper = new List<TamperData>();
            for (counter = 0; counter < tamper.GetLength(0); counter++)
            {
                if (tamper.GetLength(0) != 0)
                {
                    //  billingGeneralNFEntity.Tamper = new TamperData();
                    formatterTamper.SplitData(getData(tamper, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listDTMDailyProfileData = new List<DTMDailyProfileData>();
            for (counter = 0; counter < dtmDailyProfile.GetLength(0); counter++)
            {
                if (dtmDailyProfile.GetLength(0) != 0)
                {
                    //billingGeneralNFEntity.DTMDailyProfile = new List<DTMDailyProfileEntity>();
                    formatterDTMDailyProfile.SplitData(getData(dtmDailyProfile, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listHeaderInfo = new List<MeterDataHeaderInfoEntity>();
            for (counter = 0; counter < headerInfo.GetLength(0); counter++)
            {
                if (headerInfo.GetLength(0) != 0)
                {
                    formatterHeaderInfo.SplitData(headerInfo[counter], ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listNamePlateDetail = new List<NamePlateDetailEntity>();
            for (counter = 0; counter < namePlateDetail.GetLength(0); counter++)
            {
                if (namePlateDetail.GetLength(0) != 0)
                {
                    formatterNamePlateDetail.SplitData(namePlateDetail[counter], ref billingGeneralNFEntity);
                }
            }


            master.Add(billingGeneralNFEntity);
            //}
        }

        public void GetDataForSPhase(string data, List<IECBillingGeneralNFEntity> master, Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>> readOuts, Dictionary<string, string> dicOBISandData)
        {
            int counter = 0;
            string Original = data;
            data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
            MatchCollection matches = FormatterCommon.ValidateData(data);//To Be Changed
            string[] generalData = new string[matches.Count];
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                generalData[counter++] = groups[0].Value;
            }
            string[] GeneralData = FormatterCommon.RemoveDuplicateDataForSPhase(generalData);
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0)) counter++;
            string[] general = new string[counter];

            string[] headerInfo = new string[counter];
            string[,] fraudEnergy = new string[counter, 1];
            string[,] programming = new string[counter, 1];
            string[,] rtcUpdate = new string[counter, 1];
            string[,] phasor = new string[counter, 1];
            string[,] loadSurvey = new string[counter, 1];
            string[,] tamper = new string[counter, 1];
            string[,] dtmLoadSurvey = new string[counter, 1];
            string[,] dtmDailyProfile = new string[counter, 1];
            string[] meterconfiguration = new string[counter];
            string[] namePlateDetail = new string[counter];


            if (readOuts.ContainsKey(ReadOutItem.HeaderDetails))
                headerInfo = new string[readOuts[ReadOutItem.HeaderDetails].Count];
            if (readOuts.ContainsKey(ReadOutItem.FraudEnergy))
                fraudEnergy = new string[readOuts[ReadOutItem.FraudEnergy].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.Transaction))
                programming = new string[readOuts[ReadOutItem.Transaction].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.RTCUpdate))
                rtcUpdate = new string[readOuts[ReadOutItem.RTCUpdate].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.Phasor))
                phasor = new string[readOuts[ReadOutItem.Phasor].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.LoadSurvey))
                loadSurvey = new string[readOuts[ReadOutItem.LoadSurvey].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.Tamper))
                tamper = new string[readOuts[ReadOutItem.Tamper].Count, 1];

            dtmLoadSurvey = new string[counter, 1];

            if (readOuts.ContainsKey(ReadOutItem.DailyProfile))
                dtmDailyProfile = new string[readOuts[ReadOutItem.DailyProfile].Count, 1];
            if (readOuts.ContainsKey(ReadOutItem.HeaderDetails))
                namePlateDetail = new string[readOuts[ReadOutItem.HeaderDetails].Count];


            if (headerInfo == null || headerInfo.Length == 0) headerInfo = new string[1];
            if (namePlateDetail == null || namePlateDetail.Length == 0) namePlateDetail = new string[1];
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0))
            {
                general[counter] = GeneralData[counter];
                counter++;
            }
            formatterHeaderInfo.GetData(Original, ref headerInfo);
            formatterFraudEnergy.GetData(Original, ref fraudEnergy);
            formatterProgramming.GetData(Original, ref programming);
            //formatterRTCUpdate.GetData(Original, ref rtcUpdate);
            formatterPhasor.GetData(Original, ref phasor);
            //int[] loadSurveyDaysCnt = formatterLoadSurvey.GetData(Original, ref loadSurvey);
            formatterLoadSurvey.GetDataForSPhase(Original, ref loadSurvey);
            formatterTamper.GetDataForSPhase(Original, ref tamper);
            formatterDTMLoadSurvey.GetData(Original, ref dtmLoadSurvey);
            formatterDTMDailyProfile.GetDataForSPhase(Original, ref dtmDailyProfile);
            formatterNamePlateDetail.GetData(Original, ref namePlateDetail);
            formaterconfiguration.GetDataForSPhase(Original, ref meterconfiguration);
            //int MaxLength = GetMaxLength(headerInfo, general, fraudEnergy, programming, rtcUpdate, phasor, loadSurvey, tamper, dtmLoadSurvey, dtmDailyProfile, namePlateDetail);
            //for (counter = 0; counter < MaxLength; counter++)
            //{
            IECBillingGeneralNFEntity billingGeneralNFEntity = new IECBillingGeneralNFEntity();

            billingGeneralNFEntity.listGeneralData = new List<GeneralData>();
            for (counter = 0; counter < general.GetLength(0); counter++)
            {
                GeneralData generalDataItem = new GeneralData();
                generalDataItem.MeterData = new IECMeterDataEntity();
                if (general.GetLength(0) != 0)
                {
                    generalDataItem.CurrentInstant = new InstantPowerEntity();
                    generalDataItem.CurrentGeneral = new GeneralEntity();
                    generalDataItem.CurrentBilling = new IECBillingEntity();
                    generalDataItem.CurrentTariff = new TariffEntity();
                    generalDataItem.CurrentTamper = new TamperCounterGeneralEntity();
                    generalDataItem.listHistoryBilling = new List<IECBillingEntity>();
                    generalDataItem.listHistoryTariff = new List<TariffEntity>();
                    generalDataItem.listHistoryTamper = new List<TamperCounterGeneralEntity>();
                    this.SplitDataFor1Phase(general[counter], ref generalDataItem, dicOBISandData);
                }
                billingGeneralNFEntity.listGeneralData.Add(generalDataItem);
            }

            billingGeneralNFEntity.listFraudEnergy = new List<IECFraudEnergyEntity>();
            for (counter = 0; counter < fraudEnergy.GetLength(0); counter++)
            {
                if (fraudEnergy.GetLength(0) != 0)
                {
                    formatterFraudEnergy.SplitData(getData(fraudEnergy, counter), ref billingGeneralNFEntity);
                }
            }

            //billingGeneralNFEntity.listTransactionData = new List<TransactionData>();
            //for (counter = 0; counter < programming.GetLength(0); counter++)
            //{
            //    if (programming.GetLength(0) != 0)
            //    {
            //        formatterProgramming.SplitData(getData(programming, counter), ref billingGeneralNFEntity);
            //    }
            //}

            billingGeneralNFEntity.listRTCUpdate = new List<RTCUpdateEntity>();
            billingGeneralNFEntity.listTransactionData = new List<TransactionData>();
            //for (counter = 0; counter < rtcUpdate.GetLength(0); counter++)
            //{
            if (general.GetLength(0) != 0 && tamper.GetLength(0) != 0)
            {
                formatterRTCUpdate.SplitDataForSPhase(dicOBISandData, ref billingGeneralNFEntity);
                formatterProgramming.SplitDataForSPhase(dicOBISandData, ref billingGeneralNFEntity);
            }
            //}

            billingGeneralNFEntity.listPhasor = new List<IECPhasorEntity>();
            for (counter = 0; counter < phasor.GetLength(0); counter++)
            {
                if (phasor.GetLength(0) != 0)
                {
                    formatterPhasor.SplitData(getData(phasor, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listLoadSurveyData = new List<LoadSurveyData>();
            for (counter = 0; counter < loadSurvey.GetLength(0); counter++)
            {
                if (loadSurvey.GetLength(0) != 0)
                {
                    //billingGeneralNFEntity.LoadSurvey = new List<LoadSurveyEntity>();

                    //for (int x = 0; x < loadSurvey.GetLength(0); x++)
                    //{

                    //formatterLoadSurvey.SplitDataSPhase(getData(loadSurvey, counter), ref billingGeneralNFEntity, loadSurveyDaysCnt[counter]);
                    formatterLoadSurvey.SplitDataSPhase(getData(loadSurvey, counter), ref billingGeneralNFEntity, 90);
                    //}
                }
            }
            billingGeneralNFEntity.listTamper = new List<TamperData>();
            for (counter = 0; counter < tamper.GetLength(0); counter++)
            {
                if (tamper.GetLength(0) != 0)
                {
                    //  billingGeneralNFEntity.Tamper = new TamperData();
                    formatterTamper.SplitDataSPhase(dicOBISandData, getData(tamper, counter), ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listDTMDailyProfileData = new List<DTMDailyProfileData>();
            for (counter = 0; counter < dtmDailyProfile.GetLength(0); counter++)
            {
                if (dtmDailyProfile.GetLength(0) != 0)
                {
                    //billingGeneralNFEntity.DTMDailyProfile = new List<DTMDailyProfileEntity>();
                    formatterDTMDailyProfile.SplitDataSPhaseConfig(getData(dtmDailyProfile, counter), ref billingGeneralNFEntity,90);
                }
            }

            billingGeneralNFEntity.listHeaderInfo = new List<MeterDataHeaderInfoEntity>();
            for (counter = 0; counter < headerInfo.GetLength(0); counter++)
            {
                if (headerInfo.GetLength(0) != 0)
                {
                    formatterHeaderInfo.SplitData(headerInfo[counter], ref billingGeneralNFEntity);
                }
            }

            billingGeneralNFEntity.listNamePlateDetail = new List<NamePlateDetailEntity>();
            for (counter = 0; counter < namePlateDetail.GetLength(0); counter++)
            {
                if (namePlateDetail.GetLength(0) != 0)
                {
                    formatterNamePlateDetail.SplitData(namePlateDetail[counter], ref billingGeneralNFEntity);
                }
            }
            //Get TOD data in Meter Configuration for 1P IEC meters
            billingGeneralNFEntity.meterConfigurationDetail = new List<IECMeterConfigurationsNFEntity>();

            if (meterconfiguration.Length > 4)
            {
                formaterconfiguration.SplitData(meterconfiguration, ref billingGeneralNFEntity);
            }


            master.Add(billingGeneralNFEntity);
            //}
        }

        private int GetMaxLength(string[] headerInfo, string[] general, string[,] fraudEnergy, string[,] programming, string[,] rtcUpdate, string[,] phasor, string[,] loadSurvey, string[,] tamper, string[,] dtmLoadSurvey, string[,] dtmDailyProfile, string[] namePlateDetail)
        {
            if (general.GetLength(0) != 0)
                return general.GetLength(0);
            else if (headerInfo.GetLength(0) != 0)
                return headerInfo.GetLength(0);
            else if (fraudEnergy.GetLength(0) != 0)
                return fraudEnergy.GetLength(0);
            else if (programming.GetLength(0) != 0)
                return programming.GetLength(0);
            else if (rtcUpdate.GetLength(0) != 0)
                return rtcUpdate.GetLength(0);
            else if (phasor.GetLength(0) != 0)
                return phasor.GetLength(0);
            else if (loadSurvey.GetLength(0) != 0)
                return loadSurvey.GetLength(0);
            else if (tamper.GetLength(0) != 0)
                return tamper.GetLength(0);
            else if (dtmLoadSurvey.GetLength(0) != 0)
                return dtmLoadSurvey.GetLength(0);
            else if (dtmDailyProfile.GetLength(0) != 0)
                return dtmDailyProfile.GetLength(0);
            else if (namePlateDetail.GetLength(0) != 0)
                return namePlateDetail.GetLength(0);
            else
                return 0;
        }

        private string[] getData(string[,] data, int counter)
        {
            try
            {
                string[] tempData = new string[data.GetLength(1)];
                for (int i = 0; i < tempData.Length; i++)
                    tempData[i] = data[counter, i];
                return tempData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private void SplitData(string data, ref GeneralData generalDataItem)
        {
            FirmWareVersion = "";
            billingType = "";
            int counter = 0;
            data = data.Replace(FormatterConstant.NEWLINE, string.Empty);
            MatchCollection allMatches = FormatterCommon.ValidateData(data, FormatterConstant.ALLPARAMETER);
            string[] commnonData = new string[allMatches.Count];
            foreach (Match match in allMatches)
            {
                GroupCollection groups = match.Groups;
                commnonData[counter++] = groups[0].Value;
            }

            string[] readingDate = data.Split('/');
            MatchCollection matcheMeterID = FormatterCommon.ValidateData(data, FormatterConstant.MeterIDEXPRESSION);
            if (matcheMeterID.Count > 0)
            {
                string[] meterId = new string[matcheMeterID.Count];
                int index = 0;
                foreach (Match match in matcheMeterID)
                {
                    GroupCollection groups = match.Groups;
                    meterId[index++] = groups["0"].Value;
                }
                commnonData[0] = meterId[0].Substring(5, meterId[0].Length - 6);
            }
            FirmWareVersion = getFirmwareVersion(commnonData);
            //if (FirmWareVersion != "02.34" || FirmWareVersion != "01.34")
            //{
            //    if (!string.IsNullOrEmpty(FirmWareVersion))
            //        return;
            //}

            counter = 0;
            IECMeterDataEntity meterData = new IECMeterDataEntity();
            meterData.MeterID = commnonData[0];
            meterData.ReadingDateTime = Convert.ToInt64(readingDate[2]);
            //meterData.CMRIID = readingDate[3].Substring(1, 8);
            //string cmriType = readingDate[3].Substring(0, 1);
            //if (cmriType.Trim().ToUpper().Equals("A"))
            //    meterData.CMRIType = "Analogic";
            //else if (cmriType.Trim().ToUpper().Equals("S"))
            //    meterData.CMRIType = "Sands";
            //else
            //    meterData.CMRIType = "BCS";
            generalDataItem.MeterData = meterData;
            generalDataItem.CurrentInstant = LoadInstantPower(commnonData);
            generalDataItem.CurrentGeneral = LoadGeneralData(commnonData);
            firmwareVersion = generalDataItem.CurrentGeneral.FirmwareVersion;
            generalDataItem.CurrentBilling = LoadCurrentHistory(commnonData, counter);
            //Added for backward compatibility.
            //if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
            //{
            //    generalDataItem.CurrentTariff = LoadTariff(commnonData, counter, 50);
            //    generalDataItem.CurrentTamper = LoadTamper(commnonData, counter, 122, "G");
            //}
            //else if (isWBExportVCL)
            //{
            //    generalDataItem.CurrentTariff = LoadTariff(commnonData, counter, 51);
            //    generalDataItem.CurrentTamper = LoadTamper(commnonData, counter, 123, "G");
            //}
            //else if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
            //{

            generalDataItem.CurrentTariff = LoadTariff(commnonData, counter, 49);
            generalDataItem.CurrentTamper = LoadTamper(commnonData, counter, 121, "G");

            //}
            //else
            //{
            //    generalDataItem.CurrentTariff = LoadTariff(commnonData, counter, 49);
            //    generalDataItem.CurrentTamper = LoadTamper(commnonData, counter, 121, "G");
            //}

            //Loading Billing Data
            List<IECBillingEntity> historyBilling = new List<IECBillingEntity>();
            List<TariffEntity> historyTariff = new List<TariffEntity>();
            List<TamperCounterGeneralEntity> historyTamper = new List<TamperCounterGeneralEntity>();
            counter = 1;
            // Added for backward compatibility
            int historyBillingIndex = 152;
            //if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || isWBExportVCL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
            //{
            historyBillingIndex = 142;
            //}
            //if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
            //{
            //    historyBillingIndex = 152;
            //}
            //if (UtilityDetails.UtilityName == UtilityEntity.TNEB1)
            //{
            //    historyBillingIndex = 143;
            //}
            //if (UtilityDetails.UtilityName == UtilityEntity.WBEXPORTVCL && !isWBExportVCL)
            //{
            //    historyBillingIndex = 142;
            //}
            int pos = 142;
            int totalHistory = (commnonData.Length - historyBillingIndex) / 108;
            pos = 0;
            while (counter <= totalHistory)
            {
                //if (isWBExportVCL)
                //{
                //    historyBilling.Add(LoadBillingHistory(commnonData, counter, (historyBillingIndex + (2 * counter) + pos)));
                //}
                //else
                //{
                historyBilling.Add(LoadBillingHistory(commnonData, counter, (historyBillingIndex + pos)));
                //}
                billingType = historyBilling[counter - 1].BillingResetType;//change by Abhay
                //if (isWBExportVCL)
                //{
                //    historyTariff.Add(LoadTariff(commnonData, counter, (historyBillingIndex + 15 + (2*counter) + pos)));
                //}
                //else
                //{
                historyTariff.Add(LoadTariff(commnonData, counter, (historyBillingIndex + 13 + pos)));
                //}
                //if (isWBExportVCL)
                //{
                //    historyTamper.Add(LoadTamper(commnonData, counter, (historyBillingIndex + 87 + (2*counter) + pos), "B"));
                //}
                //else
                //{
                historyTamper.Add(LoadTamper(commnonData, counter, (historyBillingIndex + 85 + pos), "B"));
                //}
                pos = 108 * counter;
                counter++;
            }

            //int pos = 143;// now this pos will be 152
            //int totalHistory = (commnonData.Length - 152) / 108;
            //pos = 0;
            //while (counter <= totalHistory)
            //{
            //    historyBilling.Add(LoadBillingHistory(commnonData, counter, (144 + 9 + pos)));
            //    historyTariff.Add(LoadTariff(commnonData, counter, (165 + pos)));
            //    historyTamper.Add(LoadTamper(commnonData, counter, (237 + pos), "B"));
            //    pos = 108 * counter;
            //    counter++;
            //}
            generalDataItem.listHistoryBilling = historyBilling;
            generalDataItem.listHistoryTariff = historyTariff;
            generalDataItem.listHistoryTamper = historyTamper;
        }
        private void SplitDataFor1Phase(string data, ref GeneralData generalDataItem, Dictionary<string, string> dicOBISandData)
        {
            FirmWareVersion = "";
            billingType = "";
            int counter = 0;
            data = data.Replace(FormatterConstant.NEWLINE, string.Empty);
            MatchCollection allMatches = FormatterCommon.ValidateData(data, FormatterConstant.ALLPARAMETER);
            string[] commnonData = new string[allMatches.Count];
            foreach (Match match in allMatches)
            {
                GroupCollection groups = match.Groups;
                commnonData[counter++] = groups[0].Value;
            }
            MatchCollection matcheMeterID = FormatterCommon.ValidateData(data, FormatterConstant.MeterIDEXPRESSIONFOR1PHASE);
            if (matcheMeterID.Count > 0)
            {
                string[] meterId = new string[matcheMeterID.Count];
                int index = 0;
                foreach (Match match in matcheMeterID)
                {
                    GroupCollection groups = match.Groups;
                    meterId[index++] = groups["0"].Value;
                }
                commnonData[0] = meterId[0].Substring(5, meterId[0].Length - 6);
            }
            FirmWareVersion = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "0.2.0"); //getFirmwareVersion(commnonData);  
            counter = 0;
            IECMeterDataEntity meterData = new IECMeterDataEntity();

            meterData.MeterID = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "C.1");
            //meterData.ReadingDateTime = 0;// FormatterCommon.ParseDataFor1Phase(dicOBISandData, "0.9.1");
            generalDataItem.MeterData = meterData;
            generalDataItem.CurrentInstant = LoadInstantPowerFor1Phase(dicOBISandData);
            generalDataItem.CurrentGeneral = LoadGeneralDataFor1Phase(dicOBISandData);
            firmwareVersion = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "0.2.0");
            generalDataItem.CurrentBilling = LoadCurrentHistoryForSPhase(dicOBISandData, counter);
            generalDataItem.CurrentTariff = LoadTariffOfCurrentForSPhase(dicOBISandData, counter);
            //generalDataItem.CurrentTamper = LoadTamperForSPhase(dicOBISandData, counter, 121, "G");
            //Loading Billing Data
            List<IECBillingEntity> historyBilling = new List<IECBillingEntity>();
            List<TariffEntity> historyTariff = new List<TariffEntity>();
            List<TamperCounterGeneralEntity> historyTamper = new List<TamperCounterGeneralEntity>();
            // Story - 365971 - 13 billing for Power ON Hours - Single Phase Non DLMS integration
            counter = 1;
            // Story - 581355 - To Support 60 months billing for Nepal 1P VIM Tender requirement
            while (counter <= 61)
            {
                string value = FormatterCommon.ParseDataFor1Phase(dicOBISandData, string.Concat("1.8.0.", (counter.ToString("00"))));
                if (value != string.Empty)
                {
                    if (value.Contains("00:00;00-00-00"))
                        break;
                }
                historyBilling.Add(LoadBillingHistoryForSPhase(dicOBISandData, counter));
                //billingType = historyBilling[counter - 1].BillingResetType;//change by Abhay                
                historyTariff.Add(LoadTariffForSPhase(dicOBISandData, counter));
                //historyTamper.Add(LoadTamper(commnonData, counter, (historyBillingIndex + 85 + pos), "B"));               
                counter++;
            }
            generalDataItem.listHistoryBilling = historyBilling;
            generalDataItem.listHistoryTariff = historyTariff;
            //generalDataItem.listHistoryTamper = historyTamper;
            //Anomaly
            string anomalyData = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "F.F");
            string anomalyData_err2 = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "F.E");

            generalDataItem.Anomaly = LoadAnomalyFor1Phase(anomalyData);
            generalDataItem.Anomaly = LoadAnomalyFor1Phase_err2(anomalyData_err2,generalDataItem.Anomaly);
        }
        /// <summary>
        /// Prepare anomaly Entity
        /// struct{
        //uint8_t Nv_Error                      :1;/// for nv memory
        //uint8_t Power_Save_Error              :1;                                 
        //uint8_t Calibration_Error             :1;
        //uint8_t RTC_Error                     :1;    /// for rtc status
        //uint8_t Power_Save_Fatal_Error        :1;    //Error Code E005                           
        //uint8_t Main_Battery_Error            :1;
        //uint8_t Rtc_Battery_Error             :1;
        //uint8_t Nv_Error_RW                   :1;
        //}Detail; 

        /// </summary>
        /// <param name="anomalyData"></param>
        /// <returns></returns>
        private AnomalyEntityForSP LoadAnomalyFor1Phase(string anomalyData)
        {
            AnomalyEntityForSP anomalyEntityForSP = new AnomalyEntityForSP();
            int onjint;
            //************* If anamoly data not configured in meter then RTC battery and Eprom is -1 not shown in selfdiagnostic tab instant **************
            if (anomalyData == "" || anomalyData.Length == 0)
            {
                anomalyEntityForSP.RTCBattery = -1;
                anomalyEntityForSP.EeProm = -1;
            }

            if (int.TryParse(anomalyData, out onjint))
            {
                int decValue = int.Parse(anomalyData, System.Globalization.NumberStyles.HexNumber);
                string anamoly = decValue.ToString();
                string error_messages = Convert.ToString(Convert.ToInt32(anamoly, 10), 2);
                error_messages = error_messages.PadLeft(8, '0');
                error_messages= ReverseString(error_messages); //Reverse string of binary data
                anomalyEntityForSP.RTCBattery = 0;
                anomalyEntityForSP.EeProm = 1;
                if (error_messages[3] == '0')
                    anomalyEntityForSP.RTCBattery = 1;
                if (error_messages[4] == '0')
                    anomalyEntityForSP.Flash = 1;//---Flash is used for this (1P-IEC) Error code "E005" Code display
            }
            return anomalyEntityForSP;
        }

        private AnomalyEntityForSP LoadAnomalyFor1Phase_err2(string anomalyData, AnomalyEntityForSP anomalyEntityForSP)
        {

            int onjint;
            //************* If anamoly data not configured in meter then RTC battery and Eprom is -1 not shown in selfdiagnostic tab instant **************

            if (int.TryParse(anomalyData, out onjint))
            {
                int decValue = int.Parse(anomalyData, System.Globalization.NumberStyles.HexNumber);
                string anamoly = decValue.ToString();
                string error_messages = Convert.ToString(Convert.ToInt32(anamoly, 10), 2);
                error_messages = error_messages.PadLeft(8, '0');
                // error_messages = ReverseString(error_messages); //Reverse string of binary data
                anomalyEntityForSP.MainBattery = -2;

                if (error_messages[7] == '0')
                    anomalyEntityForSP.RTCBattery  = 1;

            }
            return anomalyEntityForSP;
        }


        public string ReverseString(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
        private InstantPowerEntity LoadInstantPower(string[] data)
        {
            InstantPowerEntity instantEntity = new InstantPowerEntity();

            try
            {
                instantEntity.MeterID = FormatterCommon.ParseData(data[0]);
                instantEntity.MeterDateTime = FormatterCommon.ParseDate(data[1]);
                instantEntity.VoltageRPhase = FormatterCommon.ParseData(data[7]);
                instantEntity.VoltageYPhase = FormatterCommon.ParseData(data[8]);
                instantEntity.VoltageBPhase = FormatterCommon.ParseData(data[9]);
                instantEntity.CurrentRPhase = FormatterCommon.ParseData(data[10]);
                instantEntity.CurrentYPhase = FormatterCommon.ParseData(data[11]);
                instantEntity.CurrentBPhase = FormatterCommon.ParseData(data[12]);
                instantEntity.InstantActivepower = FormatterCommon.ParseData(data[13]);
                instantEntity.InstantReactiveLagPower = FormatterCommon.ParseData(data[14]);
                instantEntity.InstantReactiveLeadPower = FormatterCommon.ParseData(data[15]);
                instantEntity.InstantApparentPower = FormatterCommon.ParseData(data[16]);
                instantEntity.TotalPowerFactor = FormatterCommon.ParseData(data[17]);
                instantEntity.PowerFactorRPhase = FormatterCommon.ParseData(data[18]);
                instantEntity.PowerFactorYPhase = FormatterCommon.ParseData(data[19]);
                instantEntity.PowerFactorBPhase = FormatterCommon.ParseData(data[20]);
                instantEntity.Frequency = FormatterCommon.ParseData(data[21]);
                instantEntity.TotalFundamentalActiveEnergy = FormatterCommon.ParseData(data[22]);
                return instantEntity;

            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted Instant Power data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                instantEntity = null;
            }
            return instantEntity;
        }
        private InstantPowerEntity LoadInstantPowerFor1Phase(Dictionary<string, string> instantDic)
        {
            InstantPowerEntity instantEntity = new InstantPowerEntity();
            string totalPowerOnHours = string.Empty;
            instantEntity.TotalPowerOffMinutes = string.Empty;

            try
            {
                instantEntity.MeterID = FormatterCommon.ParseDataFor1Phase(instantDic, "C.1");//FormatterCommon.ParseData(data[0]);
                // Story - 349654 - Meter Time is coming with 0.9.1 OBIS code
                //string data = FormatterCommon.ParseDataFor1Phase(instantDic, "0.9.3");
                string data = FormatterCommon.ParseDataFor1Phase(instantDic, "0.9.1");
                string[] value = data.Split(';');
                //string dateTime = DateUtility.GetFormatedDateTme(string.Concat(value[4], ";", value[5]));
                string dateTime = DateUtility.GetFormatedDateTme(string.Concat(value[0], ";", value[1]));

                // User Story 464096
                string dataProgrammedBilldayTimeValue = FormatterCommon.ParseDataFor1Phase(instantDic, "0.9.2");
                instantEntity.ProgrammedBillDayTime = DateUtility.GetFormatedDaysHrsMinutes(dataProgrammedBilldayTimeValue);


                instantEntity.MeterDateAndTime = dateTime;//Convert.ToInt64(DateUtility.GetDateFromOBIS(FormatterCommon.ParseDataFor1Phase(instantDic, "0.9.1")));// Convert.ToInt64(FormatterCommon.ParseDataFor1Phase(instantDic, "0.9.1"));
                instantEntity.TamperResetCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51");

                instantEntity.PhaseVoltage = FormatterCommon.ParseDataFor1Phase(instantDic, "12.7");
                instantEntity.PhaseCurrent = FormatterCommon.ParseDataFor1Phase(instantDic, "11.7.1");
                instantEntity.NeutralCurrent = FormatterCommon.ParseDataFor1Phase(instantDic, "11.7.2");
                instantEntity.ActivePowerKW = FormatterCommon.ParseDataFor1Phase(instantDic, "1.7.0");
                instantEntity.NeutralPowerKW = FormatterCommon.ParseDataFor1Phase(instantDic, "1.7.1");
                instantEntity.TotalPowerFactor = FormatterCommon.ParseDataFor1Phase(instantDic, "13.7");
                instantEntity.PowerFactor = FormatterCommon.ParseDataFor1Phase(instantDic, "13.8");
                instantEntity.PresentMonthAveragePF = FormatterCommon.ParseDataFor1Phase(instantDic, "13.9");
                instantEntity.CumulativeEnergyKVAh = FormatterCommon.ParseDataFor1Phase(instantDic, "1.8.7");
                instantEntity.CumulativeEnergyKWh = FormatterCommon.ParseDataFor1Phase(instantDic, "1.8.0");
                instantEntity.CumulativeEnergyKVArh = FormatterCommon.ParseDataFor1Phase(instantDic, "1.8.8");
                instantEntity.InstantApparentPower = FormatterCommon.ParseDataFor1Phase(instantDic, "1.7.2");
                instantEntity.TotalPowerOnMinutes = FormatterCommon.ParseDataFor1Phase(instantDic, "C.8.0.1");
                //Power on hours obis code added for Starlight IEC meter 
                if (instantEntity.TotalPowerOnMinutes == string.Empty)
                {
                    totalPowerOnHours = FormatterCommon.ParseDataFor1Phase(instantDic, "C.8.0");
                    if (totalPowerOnHours != string.Empty)
                    {
                        totalPowerOnHours = totalPowerOnHours.Replace("h", "");
                        int totalPowerHrs = 0;
                        int.TryParse(totalPowerOnHours, out totalPowerHrs);
                        totalPowerHrs = totalPowerHrs * 60;
                        totalPowerOnHours = totalPowerHrs.ToString() + "M";
                    }
                    instantEntity.TotalPowerOnMinutes = totalPowerOnHours;
                }

                instantEntity.MDResetCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "0.1.0");
                instantEntity.ReadoutCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.7.1");// Story - 349654 - This case would be handled while adding in the entity
                instantEntity.ProgrammingCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.99");
                instantEntity.ABC = FormatterCommon.ParseDataFor1Phase(instantDic, "11.7.3");
                instantEntity.ABCType2Bill1 = FormatterCommon.ParseDataFor1Phase(instantDic, "4.1.00");
                instantEntity.ABCType2Bill2 = FormatterCommon.ParseDataFor1Phase(instantDic, "4.1.01");
                instantEntity.PowerfailCount = FormatterCommon.ParseDataFor1Phase(instantDic, "C.7.0"); // User Story 464096
                instantEntity.FraudEnergy = FormatterCommon.ParseDataFor1Phase(instantDic, "2.8.3"); // User Story 464096
                instantEntity.CumulativeActiveMDKWh = FormatterCommon.ParseDataFor1Phase(instantDic, "1.4.8");
                instantEntity.LegalEnergy = FormatterCommon.ParseDataFor1Phase(instantDic, "3.8.3");
                instantEntity.LineFrequency = FormatterCommon.ParseDataFor1Phase(instantDic, "1.6.2");

                #region Tamper counters
                instantEntity.SingleWireTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.7");
                instantEntity.EarthTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.2");
                instantEntity.MagnetTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.3");
                instantEntity.NeutralDisturbanceTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.5");
                instantEntity.TotalTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.8");
                instantEntity.ESDTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.3");
                instantEntity.CoverOpenTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.4");
                // User Story 464096 
                instantEntity.OverLoadTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.9");
                instantEntity.LowVoltageTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.10");
                instantEntity.LowPFTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.4");
                instantEntity.ReverseTamperCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.1");
                //instantEntity.TotalTransactionCounter = FormatterCommon.ParseDataFor1Phase(instantDic, "C.50.99"); // ProgrammingCounter is same as TotalTransactionCounter
                #endregion

                // Story - 365960 - More instant parameters for single phase non DLMS integration
                string[] arrPowerOffHours=null;
                if (!string.IsNullOrEmpty(FormatterCommon.ParseDataFor1Phase(instantDic, "0.9.3")))
                    arrPowerOffHours = FormatterCommon.ParseDataFor1Phase(instantDic, "0.9.3").Split(';');
                if (arrPowerOffHours != null && arrPowerOffHours.Length > 5)// checking 4th index as time is on 3rd index and date is on 4th
                {
                    // If manufacture time is not there then power off hours should also not be calculated
                    string manufacturedateTime = DateUtility.GetFormatedDateTme(string.Concat(arrPowerOffHours[4], ";", arrPowerOffHours[5]));
                    instantEntity.ManufactureDateTime = manufacturedateTime;
                    if (instantEntity.TotalPowerOnMinutes.ToUpper().Contains("M") && value.Length > 1)
                    {
                        DateTime meterDateTime = DateUtility.LongToDateTime(Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(value[0] + ";" + value[1])));// value is a array of meter date time
                        DateTime manufactureDate = DateUtility.LongToDateTime(Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(arrPowerOffHours[4] + ";" + arrPowerOffHours[5])));
                        TimeSpan ts = meterDateTime - manufactureDate;
                        long powerOffHours = (ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes) - Convert.ToInt64(instantEntity.TotalPowerOnMinutes.Substring(0, instantEntity.TotalPowerOnMinutes.ToUpper().IndexOf('M')));
                        instantEntity.TotalPowerOffMinutes = powerOffHours.ToString();// Need to calculate manufacture - pon
                    }
                    /* As discuss with Balgovind and Mohsin we are commneting the below code because if meter dosent provde obis code "C.8.0.1" or "C.8.0" then there is no need to show Cumulative Power-On Duration with 00:00:00;00:00:00 value. It is better to hide the Property from Instant Profile*/
                    //else
                    //    instantEntity.TotalPowerOffMinutes = "0";// in case meter RTC is not set or power on minutes doesnt come
                }
                else
                {
                    // if manufacture date time is not coming from the meter
                    instantEntity.ManufactureDateTime = string.Empty;
                    instantEntity.TotalPowerOffMinutes = string.Empty;
                }
                instantEntity.SingleWireTamperDuration = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.9.1");
                instantEntity.MagnetTamperDuration = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.9.2");
                instantEntity.NeutralDisturbanceTamperDuration = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.9.3");
                instantEntity.EarthTamperDuration = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.9.5");
                instantEntity.ESDTamperDuration = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.9.6");
                instantEntity.ReverseTamperDuration = FormatterCommon.ParseDataFor1Phase(instantDic, "C.51.9.4");
                string poweroffhours = FormatterCommon.ParseDataFor1Phase(instantDic, "C.8.1");
                if (poweroffhours != null && poweroffhours.Length > 0)
                {
                    poweroffhours = poweroffhours.Trim('h').Trim('m');
                    instantEntity.TotalPowerOffMinutes = poweroffhours;// For CSPDCL
                }
                return instantEntity;
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted Instant Power data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                instantEntity = null;
            }
            return instantEntity;
        }
        public string getFirmwareVersion(string[] data)
        {
            return FormatterCommon.ParseData(data[4]);
        }
        private GeneralEntity LoadGeneralData(string[] data)
        {
            GeneralEntity generalEntity = new GeneralEntity();
            try
            {
                //if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL|| UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.WBEXPORTVCL)
                //{
                int counter = 0;
                //check that whether it is a export meter
                generalEntity.CumulativeExportEnergyKVAH = FormatterCommon.ParseData(data[28]);
                //if (!generalEntity.CumulativeExportEnergyKVAH.ToLower().Contains("kvah"))
                //{
                //    isWBExportVCL = false;
                //}
                //else
                //{
                //    isWBExportVCL = true;
                //}
                generalEntity.MeterID = FormatterCommon.ParseData(data[counter]);
                generalEntity.MeterDateTime = FormatterCommon.ParseDate(data[++counter]);
                generalEntity.ErrorCode = FormatterCommon.ParseData(data[++counter]);
                generalEntity.MeterConstant = FormatterCommon.ParseData(data[++counter]);
                generalEntity.FirmwareVersion = FormatterCommon.ParseData(data[++counter]);
                counter += 2;
                generalEntity.VoltagePhaseSequence = FormatterCommon.ParseData(data[counter]);
                counter += 17;
                generalEntity.TotalActiveEnergy = FormatterCommon.ParseData(data[counter]);
                //if (isWBExportVCL)
                //{
                //    counter += 4;
                //    generalEntity.CumulativeExportEnergyKWH = FormatterCommon.ParseData(data[counter]);
                //    generalEntity.CumulativeExportEnergyKVAH = FormatterCommon.ParseData(data[++counter]);
                //    counter += 5;                  
                //}
                //else
                //{
                counter += 8;
                generalEntity.CumulativeExportEnergyKWH = null;
                generalEntity.CumulativeExportEnergyKVAH = null;
                //}
                generalEntity.CumulativeMD1 = FormatterCommon.ParseData(data[counter]);
                generalEntity.CumulativeMD2 = FormatterCommon.ParseData(data[++counter]);
                generalEntity.RisingDemandKW = FormatterCommon.ParseData(data[++counter]);
                generalEntity.ElapsedTimeKW = FormatterCommon.ParseData(data[++counter]) + "*MM:SS";
                generalEntity.RisingDemandKVA = FormatterCommon.ParseData(data[++counter]);
                generalEntity.ElapsedTimeKVA = FormatterCommon.ParseData(data[++counter]) + "*MM:SS";
                generalEntity.TotalPowerOnHours = FormatterCommon.ParseData(data[++counter]) + "*HH:MM";
                generalEntity.CurrentMonthPowerOnHours = FormatterCommon.ParseData(data[++counter]) + "*HH:MM";
                generalEntity.BateryModePowerOnHour = FormatterCommon.ParseData(data[++counter]) + "*HH:MM";
                counter += 2;
                generalEntity.MDResetCounter = FormatterCommon.ParseData(data[counter]);
                generalEntity.ReadoutCounter = FormatterCommon.ParseData(data[++counter]);
                generalEntity.ProgrammingCounter = FormatterCommon.ParseData(data[++counter]);
                //generalEntity.CTRatioProgrammingCounter = FormatterCommon.ParseData(data[44]);
                counter += 2;
                generalEntity.LatestTamperOccurrenceID = FormatterCommon.ParseData(data[counter]);
                generalEntity.OccurrenceTime = FormatterCommon.ParseDate(data[++counter]).ToString();
                generalEntity.LatestTamperRestorationID = FormatterCommon.ParseData(data[++counter]);
                generalEntity.RestorationTime = FormatterCommon.ParseDate(data[++counter]).ToString();
                // }
                //if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
                //{
                //    //index number 5 & 45 not in meter but file is giving the data
                //    generalEntity.MeterID = FormatterCommon.ParseData(data[0]);
                //    generalEntity.MeterDateTime = FormatterCommon.ParseDate(data[1]);
                //    generalEntity.ErrorCode = FormatterCommon.ParseData(data[2]);
                //    generalEntity.MeterConstant = FormatterCommon.ParseData(data[3]);
                //    generalEntity.FirmwareVersion = FormatterCommon.ParseData(data[4]);
                //    generalEntity.VoltagePhaseSequence = FormatterCommon.ParseData(data[6]);
                //    generalEntity.TotalActiveEnergy = FormatterCommon.ParseData(data[23]);
                //    generalEntity.CumulativeMD1 = FormatterCommon.ParseData(data[31]);
                //    generalEntity.CumulativeMD2 = FormatterCommon.ParseData(data[32]);
                //    generalEntity.RisingDemandKW = FormatterCommon.ParseData(data[33]);
                //    generalEntity.ElapsedTimeKW = FormatterCommon.ParseData(data[34]) + "*MM:SS";
                //    generalEntity.RisingDemandKVA = FormatterCommon.ParseData(data[35]);
                //    generalEntity.ElapsedTimeKVA = FormatterCommon.ParseData(data[36]) + "*MM:SS";
                //    generalEntity.TotalPowerOnHours = FormatterCommon.ParseData(data[37]) + "*HH:MM";
                //    generalEntity.CurrentMonthPowerOnHours = FormatterCommon.ParseData(data[38]) + "*HH:MM";
                //    generalEntity.BateryModePowerOnHour = FormatterCommon.ParseData(data[39]) + "*HH:MM";
                //    generalEntity.PowerOffDays = FormatterCommon.ParseData(data[40]);
                //    generalEntity.MDResetCounter = FormatterCommon.ParseData(data[42]);
                //    generalEntity.ReadoutCounter = FormatterCommon.ParseData(data[43]);
                //    generalEntity.ProgrammingCounter = FormatterCommon.ParseData(data[44]);
                //    generalEntity.LatestTamperOccurrenceID = FormatterCommon.ParseData(data[46]);
                //    generalEntity.OccurrenceTime = FormatterCommon.ParseDate(data[47]).ToString();
                //    generalEntity.LatestTamperRestorationID = FormatterCommon.ParseData(data[48]);
                //    generalEntity.RestorationTime = FormatterCommon.ParseDate(data[49]).ToString();
                //}
                return generalEntity;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception("File format not correct.");
            }
            catch (Exception)
            {
                generalEntity = null;
                // MessageBox.Show("Corrupted General data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return generalEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private GeneralEntity LoadGeneralDataFor1Phase(Dictionary<string, string> generalDic)
        {
            GeneralEntity generalEntity = new GeneralEntity();
            try
            {
                generalEntity.MeterID = FormatterCommon.ParseDataFor1Phase(generalDic, "C.1");
                generalEntity.FirmwareVersion = FormatterCommon.ParseDataFor1Phase(generalDic, "0.2.0");
                string[] data = FormatterCommon.ParseDataFor1Phase(generalDic, "0.9.3").Split(';');
                if (data.Length > 0)
                {
                    if (data[5].ToString() != string.Empty)
                        generalEntity.MeterManufacturing = string.Concat("20", data[5].Split('-')[2]);

                    //SarkarA code change 20180530 //Provide dash notation for zero value //start
                    //generalEntity.VoltageRating = string.Concat(data[3].ToString(), "V");
                    //generalEntity.CurrentRating = string.Concat(data[1], "-", data[2], "A");
                    generalEntity.VoltageRating = data[3].Equals("00") ? "-----" : string.Concat(data[3].ToString(), "V");
                    generalEntity.CurrentRating = data[2].Equals("00") ? "-----" : string.Concat(data[1], "-", data[2], "A");
                    //SarkarA code change 20180530 //end
                }
                // Story - 349654 - Meter Time is coming with 0.9.1 OBIS code
                //string[] value = FormatterCommon.ParseDataFor1Phase(generalDic, "0.9.3").Split(';');
                string[] value = FormatterCommon.ParseDataFor1Phase(generalDic, "0.9.1").Split(';');
                if (value.Length > 0)
                {
                    //string dateTime = DateUtility.GetFormatedDateTmeForSPhase(string.Concat(value[4], ";", value[5]));
                    string dateTime = DateUtility.GetFormatedDateTmeForSPhase(string.Concat(value[0], ";", value[1]));
                    generalEntity.MeterDateTime = Convert.ToInt64(dateTime);
                }
                // Now Meter Constant is avilable in Nameplate Profile
                ///generalEntity.MeterConstant = "3200";
                
                //generalEntity.AccuracyClass = FormatterCommon.ParseDataFor1Phase(generalDic, "0.9.3");
                generalEntity.AccuracyClass = "1";// Story - 365960 - More instant parameters for single phase non DLMS integration
                return generalEntity;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception("File format not correct.");
            }
            catch (Exception)
            {
                generalEntity = null;
                // MessageBox.Show("Corrupted General data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return generalEntity;
        }
        private IECBillingEntity LoadCurrentHistory(string[] data, int history)
        {
            IECBillingEntity billingEntity = new IECBillingEntity();
            int counter = 23;
            try
            {
                billingEntity.CumulativeEnergyKWH = FormatterCommon.ParseData(data[counter]);
                billingEntity.CumulativeEnergyKVARHLag = FormatterCommon.ParseData(data[++counter]);
                billingEntity.CumulativeEnergyKVARHLead = FormatterCommon.ParseData(data[++counter]);
                billingEntity.CumulativeEnergyKVAH = FormatterCommon.ParseData(data[++counter]);
                //if (isWBExportVCL)
                //{
                //    billingEntity.CumulativeExportEnergyKWH = FormatterCommon.ParseData(data[++counter]);
                //    billingEntity.CumulativeExportEnergyKVAH = FormatterCommon.ParseData(data[++counter]);
                //}
                billingEntity.CumulativeMD1 = FormatterCommon.ParseData(data[++counter]);
                billingEntity.CumulativeMD1TimeStamp = FormatterCommon.ParseDate(data[++counter]).ToString();
                billingEntity.CumulativeMD2 = FormatterCommon.ParseData(data[++counter]);
                billingEntity.CumulativeMD2TimeStamp = FormatterCommon.ParseDate(data[++counter]).ToString();
                //37
                billingEntity.PowerOnHours = FormatterCommon.ParseData(data[counter + 7]);
                //if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
                //{
                //41
                billingEntity.LoadFactor = FormatterCommon.ParseData(data[counter + 11]);
                //}
                //else
                //{
                //40
                billingEntity.LoadFactor = FormatterCommon.ParseData(data[counter + 10]);
                //}               
                billingEntity.History_ID = history;
                return billingEntity;
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted General data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                billingEntity = null;
            }
            return billingEntity;
        }
        private IECBillingEntity LoadCurrentHistoryForSPhase(Dictionary<string, string> cBillingDic, int history)
        {
            IECBillingEntity billingEntity = new IECBillingEntity();
            try
            {
                billingEntity.CumulativeEnergyKWH = (FormatterCommon.ParseDataFor1Phase(cBillingDic, "1.8.0")).Split(';')[0];
                billingEntity.CumulativeEnergyKVARHLag = null;
                billingEntity.CumulativeEnergyKVARHLead = null;
                billingEntity.CumulativeEnergyKVAH = (FormatterCommon.ParseDataFor1Phase(cBillingDic, "1.8.7")).Split(';')[0];

                string[] dateTimeMD1 = (FormatterCommon.ParseDataFor1Phase(cBillingDic, "1.4.0")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(cBillingDic, "1.4.0")).Split(';');
                string[] dateTimeMD2 = (FormatterCommon.ParseDataFor1Phase(cBillingDic, "1.4.7")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(cBillingDic, "1.4.7")).Split(';');

                billingEntity.CumulativeMD1 = dateTimeMD1 == null ? string.Empty : dateTimeMD1[0];
                billingEntity.CumulativeMD1TimeStamp = dateTimeMD1 == null ? "0" : DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2]));

                billingEntity.CumulativeMD2 = dateTimeMD2 == null ? string.Empty : dateTimeMD2[0];
                billingEntity.CumulativeMD2TimeStamp = dateTimeMD2 == null ? "0" : DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD2[1], ";", dateTimeMD2[2]));

                billingEntity.PowerOnHours = "0";

                //string currDate = (FormatterCommon.ParseDataFor1Phase(cBillingDic, "1.8.0")).Split(' ')[0];
                //currDate = string.Concat(currDate.Split(';')[2], ";", currDate.Split(';')[1]);
                //billingEntity.BillingDate = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(currDate));
                //billingEntity.LoadFactor = FormatterCommon.ParseDataFor1Phase(cBillingDic, "C.1");

                billingEntity.History_ID = history;
                return billingEntity;
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted General data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                billingEntity = null;
            }
            return billingEntity;
        }

        private IECBillingEntity LoadBillingHistory(string[] data, int history, int index)
        {
            IECBillingEntity billingEntity = new IECBillingEntity();
            try
            {
                billingEntity.BillingResetType = FormatterCommon.ParseData(data[++index]);
                billingEntity.CumulativeEnergyKWH = FormatterCommon.ParseData(data[++index]);
                billingEntity.CumulativeEnergyKVARHLag = FormatterCommon.ParseData(data[++index]);
                billingEntity.CumulativeEnergyKVARHLead = FormatterCommon.ParseData(data[++index]);
                billingEntity.CumulativeEnergyKVAH = FormatterCommon.ParseData(data[++index]);
                //if (isWBExportVCL)
                //{
                //    billingEntity.CumulativeExportEnergyKWH = FormatterCommon.ParseData(data[++index]);
                //    billingEntity.CumulativeExportEnergyKVAH = FormatterCommon.ParseData(data[++index]);
                //}

                billingEntity.CumulativeMD1 = FormatterCommon.ParseData(data[++index]);
                billingEntity.CumulativeMD1TimeStamp = FormatterCommon.ParseDate(data[++index]).ToString();
                billingEntity.CumulativeMD2 = FormatterCommon.ParseData(data[++index]);
                billingEntity.CumulativeMD2TimeStamp = FormatterCommon.ParseDate(data[++index]).ToString();
                billingEntity.AveragePowerFactor = FormatterCommon.ParseData(data[++index]);
                billingEntity.PowerOnHours = FormatterCommon.ParseData(data[++index]);
                billingEntity.LoadFactor = FormatterCommon.ParseData(data[++index]);
                billingEntity.History_ID = history;
            }
            catch (Exception)
            {
                //MessageBox.Show("Corrupted Billing data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                billingEntity = null;
            }
            return billingEntity;
        }

        private IECBillingEntity LoadBillingHistoryForSPhase(Dictionary<string, string> cBillingDic, int history)
        {
            IECBillingEntity billingEntity = new IECBillingEntity();
            try
            {
                string val = FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.8.0.", (history.ToString("00"))));
                if (string.IsNullOrWhiteSpace(val) && !val.Contains(";")) return null;
                string BType = ((val).Split(';')[2]);
                billingType = BType.Split('-').Length == 4 ? BType.Split('-')[3] : string.Empty;
                billingEntity.BillingResetType = billingType;
                billingEntity.CumulativeEnergyKWH = (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.8.0.", (history.ToString("00"))))).Split(';')[0];
                billingEntity.CumulativeEnergyKVARHLag = null;
                billingEntity.CumulativeEnergyKVARHLead = null;
                billingEntity.CumulativeEnergyKVAH = (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.8.9.", (history.ToString("00"))))).Split(';')[0];

                string[] dateTimeMD1 = (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.6.0.", (history.ToString("00"))))).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.6.0.", (history.ToString("00"))))).Split(';');
                string[] dateTimeMD2 = (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.6.8.", (history.ToString("00"))))).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.6.8.", (history.ToString("00"))))).Split(';');

                billingEntity.CumulativeMD1 = dateTimeMD1 == null ? string.Empty : dateTimeMD1[0];
                billingEntity.CumulativeMD1TimeStamp = dateTimeMD1 == null ? "0" : DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2]));
                billingEntity.CumulativeMD2 = dateTimeMD2 == null ? string.Empty : dateTimeMD2[0];
                billingEntity.CumulativeMD2TimeStamp = dateTimeMD2 == null ? "0" : DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD2[1], ";", dateTimeMD2[2]));
                // OBIS code was not correct of power factor
                billingEntity.AveragePowerFactor = (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("13.9.0.", (history.ToString("00"))))).Split(';')[0];


                //Userstory 505196,505198
                if (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("C.9.1.", (history.ToString("00")))) != string.Empty) // for PowerOnDuration in minute
                {
                    string totalmin = FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("C.9.1.", (history.ToString("00")))).Split('m')[0];
                    billingEntity.PowerOnHours = totalmin;
                }
                if (billingEntity.PowerOnHours == string.Empty || billingEntity.PowerOnHours == null)
                    billingEntity.PowerOnHours = string.IsNullOrEmpty(FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("C.8.0.", (history.ToString("00"))))) ? null : (Convert.ToInt32((FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("C.8.0.", (history.ToString("00"))))).Split('h')[0]) * 60).ToString();

                // Mohsin - 30-11-2016
                // CSPDCL
                //if (billingEntity.PowerOffHours == string.Empty || billingEntity.PowerOffHours == null)
                //    billingEntity.PowerOffHours = string.IsNullOrEmpty(FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("C.8.2.", (history.ToString("00"))))) ? null : (Convert.ToInt32((FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("C.8.2.", (history.ToString("00"))))).Split('m')[0])).ToString();

                if (string.IsNullOrEmpty(billingEntity.PowerOffHours))//SarkarA-start-20171116//CESC
                {
                    string tempString = (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("C.8.2.", (history.ToString("00")))));
                    billingEntity.PowerOffHours = string.IsNullOrEmpty(tempString) ? null :
                        tempString.Contains(":") ? (Convert.ToInt32(tempString.Split(':')[0])).ToString() : (Convert.ToInt32(tempString.Split('m')[0])).ToString();
                }



                //string meterDateTime = FormatterCommon.ParseDataFor1Phase(cBillingDic, "0.9.3");
                //string mdResetCount = FormatterCommon.ParseDataFor1Phase(cBillingDic, "0.1.0");
                string currDate = (FormatterCommon.ParseDataFor1Phase(cBillingDic, string.Concat("1.8.0.", (history.ToString("00"))))).Split(' ')[0];
                currDate = string.Concat(currDate.Split(';')[1], ";", currDate.Split(';')[2]);
                billingEntity.BillingDate = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(currDate));
                //billingEntity.PowerOffHours = GetHistoryMonthHrs(currDate, billingEntity.PowerOnHours, meterDateTime, mdResetCount);


                // billingEntity.LoadFactor = FormatterCommon.ParseData(data[++index]);
                billingEntity.History_ID = history;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Corrupted Billing data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                billingEntity = null;
            }
            return billingEntity;
        }

        //public string GetHistoryMonthHrs(string currentDate, string currentPON, string MetermfgDateTime, string MDResetCounter)
        //{

        //    StandardDateTime objsdt = new StandardDateTime();
        //    string mfgHrs = string.Empty;
        //    int objint;
        //    int MDResetCounterValue = 0;
        //    string[] totalHrs = new string[12];
        //    int historyCnt = 0;
        //    string hrTime = string.Empty;
        //    int DTMinutesCal = 0;
        //    //while (historyCnt < currentPON.Count)
        //    //{
        //        string billingDate = currentDate;
        //        if (billingDate == null) continue;
        //        hrTime = billingDate.Substring(billingDate.IndexOf(";") + 1, 5);

        //        if (hrTime == "24:00") hrTime = "23:59";
        //        string billingDate1 = billingDate.Substring(billingDate.LastIndexOf(";") + 1, 8) + " " + hrTime;
        //        DateTime dt1 = objsdt.MeterDateTimeToSystemDateTime(billingDate1);
        //        bool isDate1 = true;
        //        if (dt1 == new DateTime()) isDate1 = false;//DateTime.TryParse(billingDate1, out dt);

        //        billingDate = currentDate[historyCnt + 1];
        //        if (billingDate == null) continue;
        //        hrTime = billingDate.Substring(billingDate.IndexOf(";") + 1, 5);
        //        if (hrTime == "24:00") hrTime = "23:59";
        //        string billingDate2 = billingDate.Substring(billingDate.LastIndexOf(";") + 1, 8) + " " + hrTime;
        //        DateTime dt2 = objsdt.MeterDateTimeToSystemDateTime(billingDate2);
        //        bool isDate2 = true;
        //        if (dt2 == new DateTime()) isDate2 = false;
        //        // bool isDate2 = DateTime.TryParse(billingDate2, out dt);
        //        if (isDate1 && !isDate2 && billingDate2 == "00-00-00 00:00")
        //        {
        //            if (int.TryParse(MDResetCounter, out objint)) MDResetCounterValue = Convert.ToInt16(MDResetCounter);
        //            if (MDResetCounterValue > 13)
        //            {
        //                billingDate2 = "01" + billingDate1.Substring(billingDate1.IndexOf("-"), 6) + " 00:00"; isDate2 = true;
        //                dt2 = objsdt.MeterDateTimeToSystemDateTime(billingDate2);
        //            }
        //            //-----------IF Manufacturing DateTime is After Last Billing date Time then Assign Mfg Date Time to Last Billing
        //            DateTime mfgdt = objdtformat.StringDateTimeToSystemDateTime(MetermfgDateTime);
        //            if (mfgdt != new DateTime())
        //            {
        //                dt2 = mfgdt;
        //                isDate2 = true;
        //                if (DateTime.Compare(mfgdt, dt2) > 0) dt2 = mfgdt;

        //            }

        //        }
        //        if (isDate1 && isDate2)
        //        {
        //            TimeSpan TS = dt1 - dt2;
        //            int pooFF_hr = 0;
        //            DTMinutesCal = 0;
        //            if (TS.Minutes >= 30) DTMinutesCal = 1;
        //            pooFF_hr = ((TS.Minutes / 60) + TS.Hours + (TS.Days * 24)) + DTMinutesCal;
        //            pooFF_hr = pooFF_hr - Convert.ToInt32(currentPON[historyCnt].ToString().Substring(currentPON[historyCnt].ToString().IndexOf("(") + 1, 4));
        //            if (pooFF_hr <= 0) pooFF_hr = 0;
        //            totalHrs[historyCnt] = (pooFF_hr).ToString("0000");
        //        }
        //        else totalHrs[historyCnt] = "0000";
        //        historyCnt++;


        //    //}
        //    return totalHrs;
        //}

        private TariffEntity LoadTariff(string[] data, int history, int counter)
        {
            TariffEntity tariffEntity = new TariffEntity();
            try
            {
                tariffEntity.HistoryID = history;
                tariffEntity.Tariff1_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff1_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff1_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff1_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff1_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff1_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff1_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff1_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff1_Aver_PF = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff2_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff2_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff2_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff2_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff2_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff2_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff2_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff2_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff2_Aver_PF = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff3_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff3_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff3_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff3_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff3_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff3_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff3_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff3_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff3_Aver_PF = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff4_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff4_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff4_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff4_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff4_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff4_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff4_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff4_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff4_Aver_PF = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff5_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff5_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff5_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff5_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff5_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff5_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff5_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff5_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff5_Aver_PF = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff6_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff6_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff6_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff6_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff6_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff6_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff6_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff6_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff6_Aver_PF = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff7_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff7_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff7_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff7_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff7_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff7_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff7_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff7_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff7_Aver_PF = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff8_kWh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff8_kVARh_lag = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff8_kVARh_lead = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff8_kVAh = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff8_MD1 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff8_MD1_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff8_MD2 = FormatterCommon.ParseData(data[counter++]);
                tariffEntity.Tariff8_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff8_Aver_PF = FormatterCommon.ParseData(data[counter++]);
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted Tariff History data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tariffEntity = null;
            }
            return tariffEntity;
        }
        private TariffEntity LoadTariffOfCurrentForSPhase(Dictionary<string, string> tariffDic, int history)
        {
            TariffEntity tariffEntity = new TariffEntity();
            try
            {
                string[] dateTimeMD = null;
                string valueKvah = string.Empty;
                string valueKwh = string.Empty;
                tariffEntity.HistoryID = history;

                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.8.1")).Split(';')[0];
                tariffEntity.Tariff1_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff1_kVARh_lag = null;
                tariffEntity.Tariff1_kVARh_lead = null;

                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.1", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.9.1")).Split(';')[0];
                tariffEntity.Tariff1_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.1")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.1")).Split(';');
                tariffEntity.Tariff1_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff1_MD1_TimeStamp = dateTimeMD == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2])));
                //tariffEntity.Tariff1_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff1_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff1_Aver_PF = null;

                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.8.2")).Split(';')[0];
                tariffEntity.Tariff2_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff2_kVARh_lag = null;
                tariffEntity.Tariff2_kVARh_lead = null;

                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.2", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.9.2")).Split(';')[0];
                tariffEntity.Tariff2_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.2")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.2")).Split(';');
                tariffEntity.Tariff2_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff2_MD1_TimeStamp = dateTimeMD == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2])));
                //tariffEntity.Tariff2_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff2_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff2_Aver_PF = null;

                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.8.3")).Split(';')[0];
                tariffEntity.Tariff3_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff3_kVARh_lag = null;
                tariffEntity.Tariff3_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.3", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.9.3")).Split(';')[0];
                tariffEntity.Tariff3_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.3")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.3")).Split(';');
                tariffEntity.Tariff3_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff3_MD1_TimeStamp = dateTimeMD == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2])));
                //tariffEntity.Tariff3_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff3_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff3_Aver_PF = null;

                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.8.4")).Split(';')[0];
                tariffEntity.Tariff4_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff4_kVARh_lag = null;
                tariffEntity.Tariff4_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.4", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.9.4")).Split(';')[0];
                tariffEntity.Tariff4_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.4")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.4")).Split(';');
                tariffEntity.Tariff4_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff4_MD1_TimeStamp = dateTimeMD == null ? 0 : (Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2]))));
                //tariffEntity.Tariff4_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff4_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff4_Aver_PF = null;

                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.8.5")).Split(';')[0];
                tariffEntity.Tariff5_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff5_kVARh_lag = null;
                tariffEntity.Tariff5_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.5", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.9.5")).Split(';')[0];
                tariffEntity.Tariff5_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.5")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.5")).Split(';');
                tariffEntity.Tariff5_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff5_MD1_TimeStamp = dateTimeMD == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2])));
                //tariffEntity.Tariff5_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff5_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff5_Aver_PF = null;

                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.8.6")).Split(';')[0];
                tariffEntity.Tariff6_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff6_kVARh_lag = null;
                tariffEntity.Tariff6_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.6", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.9.6")).Split(';')[0];
                tariffEntity.Tariff6_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.6")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.4.6")).Split(';');
                tariffEntity.Tariff6_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff6_MD1_TimeStamp = dateTimeMD == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2])));
                //tariffEntity.Tariff6_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff6_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff6_Aver_PF = null;


                // New Tariff rate 7 added for Torrent Power requirement
                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.1")).Split(';')[0];
                tariffEntity.Tariff7_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff7_kVARh_lag = null;
                tariffEntity.Tariff7_kVARh_lead = null;

                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.1", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.5")).Split(';')[0];
                tariffEntity.Tariff7_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.3")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.3")).Split(';');
                tariffEntity.Tariff7_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff7_MD1_TimeStamp = dateTimeMD == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2])));
                //tariffEntity.Tariff1_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff1_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff7_Aver_PF = null;

                // New Tariff rate 8 added for Torrent Power requirement
                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.2")).Split(';')[0];
                tariffEntity.Tariff8_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff8_kVARh_lag = null;
                tariffEntity.Tariff8_kVARh_lead = null;

                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.1", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.6")).Split(';')[0];
                tariffEntity.Tariff8_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                dateTimeMD = (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.4")).Length == 0 ? null : (FormatterCommon.ParseDataFor1Phase(tariffDic, "1.10.4")).Split(';');
                tariffEntity.Tariff8_MD1 = dateTimeMD == null ? "00.00kW" : dateTimeMD[0];
                tariffEntity.Tariff8_MD1_TimeStamp = dateTimeMD == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD[1], ";", dateTimeMD[2])));
                //tariffEntity.Tariff1_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff1_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff8_Aver_PF = null;
            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted Tariff History data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tariffEntity = null;
            }
            return tariffEntity;
        }
        private TariffEntity LoadTariffForSPhase(Dictionary<string, string> tariffDic, int history)
        {
            TariffEntity tariffEntity = new TariffEntity();
            try
            {
                string[] dateTimeMD1 = null;
                string valueMD1 = string.Empty;
                string valueKvah = string.Empty;
                string valueKwh = string.Empty;
                tariffEntity.HistoryID = history;

                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.8.1.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff1_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff1_kVARh_lag = null;
                tariffEntity.Tariff1_kVARh_lead = null;

                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.1.", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.1.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff1_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.6.1.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff1_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff1_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff1_MD2 = dateTimeMD2[0];
                //tariffEntity.Tariff1_MD2_TimeStamp = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD2[1], ";", dateTimeMD2[2])));
                tariffEntity.Tariff1_Aver_PF = null;


                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.8.2.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff2_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff2_kVARh_lag = null;
                tariffEntity.Tariff2_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.2.", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.2.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff2_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.6.2.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff2_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff2_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff2_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff2_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff2_Aver_PF = null;


                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.8.3.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff3_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff3_kVARh_lag = null;
                tariffEntity.Tariff3_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.3.", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.3.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff3_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.6.3.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff3_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff3_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff3_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff3_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff3_Aver_PF = null;


                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.8.4.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff4_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff4_kVARh_lag = null;
                tariffEntity.Tariff4_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.4.", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.4.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff4_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.6.4.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff4_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff4_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff4_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff4_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff4_Aver_PF = null;


                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.8.5.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff5_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff5_kVARh_lag = null;
                tariffEntity.Tariff5_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.5.", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.5.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff5_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.6.5.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff5_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff5_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff5_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff5_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff5_Aver_PF = null;


                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.8.6.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff6_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff6_kVARh_lag = null;
                tariffEntity.Tariff6_kVARh_lead = null;
                //valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.6.", (history.ToString("00")))));
                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.9.6.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff6_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.6.6.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff6_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff6_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff6_MD2 = FormatterCommon.ParseData(data[counter++]);
                //tariffEntity.Tariff6_MD2_TimeStamp = FormatterCommon.ParseDate(data[counter++]);
                tariffEntity.Tariff6_Aver_PF = null;



                // New Tariff 7 rate added for Torrent Power requirement
                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.10.1.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff7_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff7_kVARh_lag = null;
                tariffEntity.Tariff7_kVARh_lead = null;

                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.10.5.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff7_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.10.3.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff7_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff7_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff1_MD2 = dateTimeMD2[0];
                //tariffEntity.Tariff1_MD2_TimeStamp = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD2[1], ";", dateTimeMD2[2])));
                tariffEntity.Tariff7_Aver_PF = null;


                // New Tariff 8 rate added for Torrent Power requirement
                valueKwh = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.10.2.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff8_kWh = (valueKwh == string.Empty ? "0.0kWh" : valueKwh);
                tariffEntity.Tariff8_kVARh_lag = null;
                tariffEntity.Tariff8_kVARh_lead = null;

                valueKvah = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.10.6.", (history.ToString("00"))))).Split(';')[0];
                tariffEntity.Tariff8_kVAh = valueKvah == string.Empty ? null : valueKvah.Split(';')[0];
                valueMD1 = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.10.4.", (history.ToString("00")))));
                dateTimeMD1 = valueMD1.Length == 0 ? null : valueMD1.Split(';');
                tariffEntity.Tariff8_MD1 = dateTimeMD1 == null ? "0.0kW" : dateTimeMD1[0];
                tariffEntity.Tariff8_MD1_TimeStamp = dateTimeMD1 == null ? 0 : Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD1[1], ";", dateTimeMD1[2])));
                //tariffEntity.Tariff1_MD2 = dateTimeMD2[0];
                //tariffEntity.Tariff1_MD2_TimeStamp = Convert.ToInt64(DateUtility.GetFormatedDateTmeForSPhase(string.Concat(dateTimeMD2[1], ";", dateTimeMD2[2])));
                tariffEntity.Tariff8_Aver_PF = null;

                //000000.0kWh;00:00;01-09-15 -AUTO     (16-02-12  00:00)
                string[] totalValue = (FormatterCommon.ParseDataFor1Phase(tariffDic, string.Concat("1.8.1.", (history.ToString("00"))))).Split(';');
                if (totalValue.Length < 3) return null;
                string[] date = totalValue[2].Split('-');
                string timeStamp = string.Concat("(", date[0], "-", date[1], "-", date[2], " ", totalValue[1], ")");
                if (history > 0)   //cosntionbilling timestamp will come in history 1 not in history 0
                {
                    if (billingType == "AUTO")
                    {
                        tariffEntity.BillingTimeStamp = FormatterCommon.ParseDate(ModifyDate(timeStamp));
                    }
                    else
                    {
                        tariffEntity.BillingTimeStamp = FormatterCommon.ParseDate(timeStamp);
                    }
                }

            }
            catch (Exception)
            {
                // MessageBox.Show("Corrupted Tariff History data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tariffEntity = null;
            }
            return tariffEntity;
        }

        private string ModifyDate(string date)//(16-02-12  00:00)
        {
            if (date == "(00-00-00  00:00)")
            {
                return date;
            }

            date = date.Replace("(", "");
            date = date.Replace(")", "");

            string[] t = date.Split(' ');

            string[] times;
            if (t[1].Contains(":"))
            {
                times = t[1].Split(':');
            }
            else
            {
                times = t[2].Split(':');
            }

            string hr = times[0].Trim();
            string min = times[1].Trim();

            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";


            DateTime d = Convert.ToDateTime(t[0], dateInfo);
            DateTime dt = new DateTime(d.Year, d.Month, d.Day);
            /* GKG 25/02/2013 Biling Type Issue */
            // Note:  This check is a patch work but not implemented correctly.
            // As per discussion with amitesh, In case of AUTO billing and FW 0.27 and 0.70
            // For Odd months , one month need to be added in report
            // As it is coming meter Display
            ////added on date 7th Aug-12 by Abhay
            if (d.Month != 1 && d.Month != 3 && d.Month != 5 && d.Month != 7 && d.Month != 9 && d.Month != 11)
            {
                dt = dt.AddMonths(1);
            }
            /* GKG 25/02/2013 Biling Type Issue */
            return "(" + dt.Day.ToString() + "-" + dt.Month + "-" + dt.Year + "  " + hr + ":" + min + ")";
        }
        private TamperCounterGeneralEntity LoadTamper(string[] data, int history, int counter, string relatedTo)
        {
            TamperCounterGeneralEntity tamperCounterGeneralEntity = new TamperCounterGeneralEntity();
            try
            {
                tamperCounterGeneralEntity.VoltageImbalanceRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.VoltageImbalanceYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.VoltageImbalanceBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.MissingPotentialRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.MissingPotentialYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.MissingPotentialBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CTShortTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CTOpenRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CTOpenYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CTOpenBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.OnePhaseNeutralAbsentTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.VoltagePhaseReversalTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CurrentImbalanceRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CurrentImbalanceYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CurrentImbalanceBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CurrentReversalRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CurrentReversalYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.CurrentReversalBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.MagneticInfluenceTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.NeutralDisturbanceTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.FrontCoverOpeningTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                tamperCounterGeneralEntity.TerminalCoverOpeningTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                if (history > 0)   //cosntionbilling timestamp will come in history 1 not in history 0
                {

                    if ((firmwareVersion == "00.27" || firmwareVersion == "00.70") & billingType == "AUTO")
                    {
                        tamperCounterGeneralEntity.BillingTimeStamp = FormatterCommon.ParseDate(ModifyDate(data[counter++]));
                    }
                    else
                    {
                        tamperCounterGeneralEntity.BillingTimeStamp = FormatterCommon.ParseDate(data[counter++]);
                    }

                    tamperCounterGeneralEntity.BillingCounter = FormatterCommon.ParseDate(data[counter++]);
                }
                tamperCounterGeneralEntity.History_ID = history;
                tamperCounterGeneralEntity.RelatedTo = relatedTo;
            }
            catch (Exception)
            {
                //MessageBox.Show("Corrupted Tamper History data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tamperCounterGeneralEntity = null;
            }
            return tamperCounterGeneralEntity;
        }
        private TamperCounterGeneralEntity LoadTamperForSPhase(Dictionary<string, string> tamperDic, int history, int counter, string relatedTo)
        {
            TamperCounterGeneralEntity tamperCounterGeneralEntity = new TamperCounterGeneralEntity();
            try
            {
                //tamperCounterGeneralEntity.VoltageImbalanceRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.VoltageImbalanceYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.VoltageImbalanceBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.MissingPotentialRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.MissingPotentialYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.MissingPotentialBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CTShortTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CTOpenRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CTOpenYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CTOpenBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.OnePhaseNeutralAbsentTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.VoltagePhaseReversalTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CurrentImbalanceRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CurrentImbalanceYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CurrentImbalanceBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CurrentReversalRPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CurrentReversalYPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.CurrentReversalBPhaseTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.MagneticInfluenceTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.NeutralDisturbanceTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.FrontCoverOpeningTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //tamperCounterGeneralEntity.TerminalCoverOpeningTamperCounter = FormatterCommon.ParseIntData(data[counter++]);
                //if (history > 0)   //cosntionbilling timestamp will come in history 1 not in history 0
                //{

                //    if ((firmwareVersion == "00.27" || firmwareVersion == "00.70") & billingType == "AUTO")
                //    {
                //        tamperCounterGeneralEntity.BillingTimeStamp = FormatterCommon.ParseDate(ModifyDate(data[counter++]));
                //    }
                //    else
                //    {
                //        tamperCounterGeneralEntity.BillingTimeStamp = FormatterCommon.ParseDate(data[counter++]);
                //    }

                //    tamperCounterGeneralEntity.BillingCounter = FormatterCommon.ParseDate(data[counter++]);
                //}
                //tamperCounterGeneralEntity.History_ID = history;
                //tamperCounterGeneralEntity.RelatedTo = relatedTo;
            }
            catch (Exception)
            {
                //MessageBox.Show("Corrupted Tamper History data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tamperCounterGeneralEntity = null;
            }
            return tamperCounterGeneralEntity;
        }
    }
}
