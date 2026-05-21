using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.Framework;
using CAB.Channel;
using CHANNEL;
using CHANNEL.Formatter;
using System.Linq;
namespace CAB.Channel.Formatter
{
    public class DLMS650FormatterBilling : ReadBase
    {
        private DLMS650StructureInfoBLL structureInfoBLL;
        private DLMS650StructureUnitInfoBLL structureUnitInfoBLL;
        private DLMS650FormatterCommon common;
        string[] BillingOBISCode, BillingScaleOBISCode, BillingScaleOBISCodeDataValue, BillingScaleOBISCodeDataUnitValue;
        string[,] BillingOBISCodeDataValue;
        int[] BillingScaleClassID, BillingScaleAttribute, BillingClassID, BillingAttribute, BillingDataIndex;
        int structure, structureLength, dataLength, datatype, totByte, pocketLength, pocketDataLength;
        bool IsCurrent = false;
        DLMS650FormatterInstant instantFormat;
        DLMS650FormatterGeneral generalFormat;
        DLMS650FormatterLoadSurvey lsFormat;
        DLMS650FormatterTamper tamperFormat;
        DLMS650FormatterAnomaly diagnosisFormat;
        //added for MVVNL
        DLMS650FormatterMidnightData midnightDataFormat;
        //added for MVVNL
        /* GKG JVVNL Current TOU Read */
        DLMS650FormatterTOUData touFormat;
        //added for CESC normal phasor data
        DLMS650FormatterPhasor normalFormatterPhasor = null;
        /* GKG JVVNL Current TOU Read */
        string utility = string.Empty;
        bool isPUMA = false;
        bool isMVVNL = false;
        bool isMPKWCL = false;
        bool isPGVCL = false;
        bool IsAPI = false;
        /* GKG JVVNL Current TOU Read */
        bool isJVVNL = false;
        /* GKG JVVNL Current TOU Read */
        bool showModelNo = false;
        //BhardwajG variable for holding month no       
        int billingMonths = 0;
        bool showCumPowerOffDetails = false;
        public DLMS650FormatterBilling()
        {
            if (UtilityEntity.Generic == UtilityDetails.Utility)
            {
                /* GKG JVVNL Current TOU Read */
                if (UtilityEntity.JVVNL.ToString() == UtilityDetails.PrimaryUtlityName)
                {
                    isJVVNL = true;
                }
                /* GKG JVVNL Current TOU Read */
                isPUMA = true;
            }
            else if (UtilityEntity.MVVNL == UtilityDetails.Utility)
            {
                isMVVNL = true;
            }
            else if (UtilityEntity.MPKWCL == UtilityDetails.Utility)
            {
                isMPKWCL = true;
            }
            else if (UtilityEntity.PGVCL == UtilityDetails.Utility)
            {
                isPGVCL = true;
            }
            else
            {
                /* GKG JVVNL Current TOU Read */
                isJVVNL = false;
                /* GKG JVVNL Current TOU Read */
                isPUMA = false;
                isMVVNL = false;
                isMPKWCL = false;
            }
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            showModelNo = true;
            instantFormat = new DLMS650FormatterInstant();
            instantFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            generalFormat = new DLMS650FormatterGeneral();
            generalFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            tamperFormat = new DLMS650FormatterTamper();
            tamperFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            //added for MVVNL
            midnightDataFormat = new DLMS650FormatterMidnightData();
            midnightDataFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);

            diagnosisFormat = new DLMS650FormatterAnomaly();
            diagnosisFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            //CESC normal phasor read
            normalFormatterPhasor = new DLMS650FormatterPhasor();
            normalFormatterPhasor.OnChannelStatusChanged += new ChannelStatusChanged(OnChannelStatusChange);
            /* GKG JVVNL Current TOU Read */
            touFormat = new DLMS650FormatterTOUData();
            touFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);

            //added for MVVNL
            //set the boolean if power off duration is enabled for utility 
            if (UtilityDetails.ShowPowerOffDurationInBilling)
            {
                showCumPowerOffDetails = true;

            }
        }

        public DLMS650FormatterBilling(UtilityEntity utilityEntity)
        {
            if (UtilityEntity.Generic == utilityEntity)
            {
                isPUMA = true;
            }
            else if (UtilityEntity.MVVNL == utilityEntity)
            {
                isMVVNL = true;
            }
            else if (UtilityEntity.MPKWCL == utilityEntity)
            {
                isMPKWCL = true;
            }
            else if (UtilityEntity.PGVCL == utilityEntity)
            {
                isPGVCL = true;
            }
            else
            {
                isPUMA = false;
                isMVVNL = false;
                isMPKWCL = false;
            }
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            IsAPI = true;

            //diagnosisFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
        }
        private void OnChannelStatusChange(string msg)
        {
            this.StatusMessage = msg;
            Application.DoEvents();
        }
        public DLMS650FormatterBilling(bool isAPI)
        {
            this.IsAPI = isAPI;
            //Setting power off details true for API.
            this.showCumPowerOffDetails = true;
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();

        }


        public void GetData(string data, BillingGeneralNFDLMSEntity master)
        {
            string[] allArrayData = data.Split('\n');
            string[] instantData = new string[8];
            string[] billingData = null;
            string[] loadSurveyData = new string[4];
            string[] tamperData = new string[24];
            string[] selfDiagno = new string[1];
            string[] phasorData = new string[1];
            //for normal DLMS phasor data
            string[] normalPhasorData = new string[4];
            //BhardwajG will be instantiated later from Billing List
            string[] generalData = null;
            /* GKG JVVNL Current TOU Read */
            string[] touData = new string[4];
            /* GKG JVVNL Current TOU Read */
            //BhardwajG : General list initialization : NDPLS MIOS
            IList<string> generalList = new List<string>();
            //BhardwajG : Billing list initialization : NDPL MIOS
            IList<string> billingList = new List<string>();
            string[] midnightData = new string[4];
            string demandIntegrationPeriod = string.Empty;
            //added for MVVNL
            string generalInfo = "";
            string dataInfo = "";
            int loopCounter, generalCounter, instantCounter, billingCounter, loadsurveyCounter, tamperCounter, midnightDataCounter, selfDiagnosis, phasorCounter;
            loopCounter = 0; generalCounter = instantCounter = billingCounter = loadsurveyCounter = tamperCounter = midnightDataCounter = selfDiagnosis = phasorCounter = 0;
            /* GKG JVVNL Current TOU Read */
            int touDataCounter = 0;
            //for normal phasor data
            int normalPhasorCounter = 0;
            int dIP;
            /* GKG JVVNL Current TOU Read */
            // To solve bug 94904.
            try
            {
                for (loopCounter = 0; loopCounter < allArrayData.Length - 2; loopCounter++)
                {
                    if (string.IsNullOrEmpty(allArrayData[loopCounter]))
                        continue;
                    string var = allArrayData[loopCounter].Substring(0, 2);
                    switch (common.GetParameterType(var))
                    {
                        case DLMSParameter.Current:
                            generalInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            break;
                        case DLMSParameter.Instant:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if ((dataInfo.Equals("\r")) || (dataInfo.Equals("0100\r")))
                                dataInfo = string.Empty;
                            instantData[instantCounter] = dataInfo;
                            instantCounter++;
                            break;
                        case DLMSParameter.Billing:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if ((dataInfo.Equals("\r")) || (dataInfo.Equals("0100\r")))
                                dataInfo = string.Empty;
                            billingList.Add(dataInfo);
                            billingCounter++;
                            break;
                        case DLMSParameter.LoadSurvey:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if ((dataInfo.Equals("\r")) || (dataInfo.Equals("0100\r")))
                                dataInfo = string.Empty;
                            loadSurveyData[loadsurveyCounter] = dataInfo;
                            loadsurveyCounter++;
                            break;
                        //added for MVVNL
                        case DLMSParameter.MidnightData:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if (dataInfo.Equals("\r"))
                                dataInfo = string.Empty;
                            midnightData[midnightDataCounter] = dataInfo;
                            midnightDataCounter++;
                            break;
                        //added for MVVNL
                        case DLMSParameter.Tamper:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if ((dataInfo.Equals("\r")) || (dataInfo.Equals("0100\r")))
                                dataInfo = string.Empty;
                            tamperData[tamperCounter] = dataInfo;
                            tamperCounter++;
                            break;
                        case DLMSParameter.General:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if (dataInfo.Equals("\r"))
                                dataInfo = string.Empty;
                            //generalData[generalCounter] = dataInfo;
                            generalList.Add(dataInfo);
                            generalCounter++;
                            break;
                        case DLMSParameter.SelfDiagnosis:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if (dataInfo.Equals("\r"))
                                dataInfo = string.Empty;
                            selfDiagno[selfDiagnosis] = dataInfo;
                            selfDiagnosis++;
                            break;
                        /* GKG JVVNL Current TOU Read */
                        case DLMSParameter.TOU:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if (dataInfo.Equals("\r"))
                                dataInfo = string.Empty;
                            touData[touDataCounter] = dataInfo;
                            touDataCounter++;
                            break;
                        /* GKG JVVNL Current TOU Read */
                        case DLMSParameter.Phasor:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if (dataInfo.Equals("\r"))
                                dataInfo = string.Empty;
                            phasorData[phasorCounter] = dataInfo;
                            phasorCounter++;
                            break;
                        //BhardwajG : NDPL : for Demand Integration period
                        case DLMSParameter.DemandIntegrationPeriod:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if (string.IsNullOrEmpty(dataInfo) && dataInfo.Equals("\r"))
                                dataInfo = string.Empty;
                            demandIntegrationPeriod = dataInfo;
                            break;
                        //CESC : Phasor download in normal mode for Ruby meter
                        case DLMSParameter.NormalPhasor:
                            dataInfo = allArrayData[loopCounter].Substring(2, allArrayData[loopCounter].Length - 2);
                            if (dataInfo.Equals("\r"))
                                dataInfo = string.Empty;
                            normalPhasorData[normalPhasorCounter] = dataInfo;
                            normalPhasorCounter++;
                            break;
                        case DLMSParameter.None:
                            break;
                    }

                }
                //BhardwajG - Parse demand integration perid if available
                if (!string.IsNullOrEmpty(demandIntegrationPeriod))
                {
                    if (int.TryParse(demandIntegrationPeriod, out dIP))
                    {
                        master.DemandIntegrationPeriod = dIP / 60;
                    }
                }
                //BhardwajG : Export to Array
                generalData = generalList.ToArray();
                billingData = billingList.ToArray();
                if (!IsAPI)
                {
                    /* GKG JVVNL Current TOU Read */
                    //FillMaster(master, generalInfo, instantData, billingData, loadSurveyData, tamperData, midnightData, generalData, selfDiagno);
                    FillMaster(master, generalInfo, instantData, billingData, loadSurveyData, tamperData, midnightData, generalData, selfDiagno, touData, phasorData, normalPhasorData);
                    /* GKG JVVNL Current TOU Read */
                }
                else
                {
                    FillMasterForMIOS(master, generalInfo, instantData, billingData, loadSurveyData, tamperData, midnightData, generalData, selfDiagno, touData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* GKG JVVNL Current TOU Read */
        // private void FillMaster(BillingGeneralNFDLMSEntity master, string generalInfo, string[] instantData, string[] billingData, string[] loadSurveyData, string[] tamperData, string[] midnightData, string[] generalData, string[] selfDiagno)
        private void FillMaster(BillingGeneralNFDLMSEntity master, string generalInfo, string[] instantData, string[] billingData,
            string[] loadSurveyData, string[] tamperData, string[] midnightData, string[] generalData,
            string[] selfDiagno, string[] touData, string[] phasorData, string[] normalPhasorData)
        /* GKG JVVNL Current TOU Read */
        {
            //set energy resolution 0
            DLMS650FormatterCommon.EnergyResolution = 0;
            common.LoadMeterData(generalInfo, master);
            instantFormat.LoadInstantData(instantData, master);
            this.LoadBillingData(billingData, master);
            tamperFormat.LoadTamperData(tamperData, master);
            //added for MVVNL
            midnightDataFormat.LoadMidnightData(midnightData, master);
            generalFormat.LoadGeneralData(generalData, master);
            //Set the parser for load survey
            //if (UtilityDetails.ShowMeterModelNo)
            //{
                if (master.General.MeterModelNo.Trim() == NamePlateConstants.RubyE250Value.ToString())
                {
                    lsFormat = new DLMS650FormatterLoadSurvey(false);

                }
                else
                {
                    lsFormat = new DLMS650FormatterLoadSurvey(true);
                }
            //}
            //else
            //{
            //    lsFormat = new DLMS650FormatterLoadSurvey();
            //}
            lsFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            lsFormat.LoadLSData(loadSurveyData, master);
            //diagnosisFormat.LoadDiagnosisData(selfDiagno, master);
            /* GKG JVVNL Current TOU Read */
            touFormat.LoadTOUData(touData, master);
            //CESC normal phasor read
            normalFormatterPhasor.LoadPhasorData(normalPhasorData, master);
            /* GKG JVVNL Current TOU Read */
            #region Special case parsing
            //BhardwajG : Check null and empty both
            if (!string.IsNullOrEmpty(phasorData[0]))
            {
                master.Phasor = new ParseFDLPhasorData(phasorData[0], "", 0, 0).FillPhasorEntity(phasorData[0],
                    Convert.ToInt32(master.General.InternalCTratio), Convert.ToInt32(master.General.InternalPTratio));
            }
            
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                    diagnosisFormat.LoadDiagnosisData(selfDiagno, master, Convert.ToInt32(master.General.MeterModelNo));
                //}
                //else
                //{
                //    diagnosisFormat.LoadDiagnosisData(selfDiagno, master);
                //}
            
            #endregion
            //added for MVVNL
        }

        private void FillMasterForMIOS(BillingGeneralNFDLMSEntity master, string generalInfo, string[] instantData, string[] billingData, string[] loadSurveyData, string[] tamperData, string[] midnightData, string[] generalData, string[] selfDiagno, string[] touData)
        {
            UtilityEntity tempUtility = UtilityEntity.JUSCO;
            isPUMA = true;
            if (isPUMA)
            {
                tempUtility = UtilityEntity.Generic;
            }
            else
            {
                tempUtility = UtilityEntity.JUSCO;
            }

            //lsFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            tamperFormat = new DLMS650FormatterTamper(tempUtility);
            //tamperFormat.OnChannelStatusChanged += new CAB.Channel.ReadBatse.ChannelStatusChanged(OnChannelStatusChange);
            //added for MVVNL

            //midnightDataFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);

            diagnosisFormat = new DLMS650FormatterAnomaly();
            //set energy resolution 0

            DLMS650FormatterCommon.EnergyResolution = 0;
            generalFormat = new DLMS650FormatterGeneral(true);
            common.LoadMeterData(generalInfo, master);
            generalFormat.LoadGeneralData(generalData, master);
            //if (master.General != null)
            //{
            //    //if (!string.IsNullOrEmpty(master.General.MeterModelNo))
            //    //{
            //    //    if (master.General.MeterModelNo.Equals("1"))
            //    //    {
            //    //        isPUMA = false;
            //    //    }
            //    //    else if (master.General.MeterModelNo.Equals("2"))
            //    //    {
            //    //        isPUMA = true;
            //    //    }
            //    //    else
            //    //    {
            //    //        isPUMA = false;
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    master.General.MeterModelNo = "1";
            //    //}
            //}
            instantFormat = new DLMS650FormatterInstant(isPUMA);
            //instantFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            instantFormat.LoadInstantData(instantData, master);
            this.LoadBillingData(billingData, master);
            //generalFormat.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(OnChannelStatusChange);
            //Set the parser for load survey
            if (master.General.MeterModelNo.Trim() == NamePlateConstants.RubyE250Value.ToString())
            {
                lsFormat = new DLMS650FormatterLoadSurvey(false);

            }
            else
            {
                lsFormat = new DLMS650FormatterLoadSurvey(true);
            }
            lsFormat.LoadLSData(loadSurveyData, master);
            tamperFormat.LoadTamperData(tamperData, master);
            //added for MVVNL
            midnightDataFormat = new DLMS650FormatterMidnightData(isPUMA);
            midnightDataFormat.LoadMidnightData(midnightData, master);
            //generalFormat.LoadGeneralData(generalData, master);

            //BhardwajG : parse the anomaly data
            if (master != null && master.General != null && !string.IsNullOrEmpty(master.General.MeterModelNo))
            {
                diagnosisFormat.LoadDiagnosisData(selfDiagno, master, Convert.ToInt32(master.General.MeterModelNo));
            }
            //added for MVVNL
            //BhardwajG : parse the tou data
            touFormat = new DLMS650FormatterTOUData();
            touFormat.LoadTOUData(touData, master);
        }
        public void LoadBillingData(string[] data, BillingGeneralNFDLMSEntity master)
        {
            int counter = 0;
            bool flag = false;

            for (counter = 0; counter < data.Length; counter++)
            {

                if (string.IsNullOrEmpty(data[counter]))
                    continue;
                else
                    flag = true;
            }
            if (!flag || string.IsNullOrEmpty(data[1]))
            {
                this.StatusMessage = "Billing data not found.";
                Application.DoEvents();
                return;
            }

            this.StatusMessage = "Uploading billing data......";
            Application.DoEvents();
            string captureObject = data[0];
            string captureObjectData = data[1];
            string captureObjectScalerUnit = data[2];
            string captureObjectDataScalerUnit = data[3];
            // BhardwajG : if parser called from API && billing month is logged
            if (data != null && data.Length > 4)
            {
                billingMonths = Convert.ToInt32(common.ConvertHexToDecimal(data[4]));
            }
            int index = 0;
            int array = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
            int arrayLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;

            BillingOBISCode = new string[arrayLength];
            BillingClassID = new int[arrayLength];
            BillingAttribute = new int[arrayLength];
            BillingDataIndex = new int[arrayLength];
            StructureInfoEntity structureInfoEntity = null;
            counter = 0;

            #region Reading Capture Objects
            while (counter < arrayLength)
            {
                structure = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                totByte = 0;
                //Class ID
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    BillingClassID[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                BillingOBISCode[counter] = common.GetOBISCode(captureObject.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    BillingAttribute[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                //DataIndex
                datatype = common.ConvertHexToDecimal(captureObject.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    BillingDataIndex[counter] = common.ConvertHexToDecimal(captureObject.Substring(index, totByte));
                    index += totByte;
                }
                counter++;
            }
            #endregion

            #region Reading Capture Data
            counter = 0;
            index = 0;
            array = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
            pocketLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;

            structure = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
            pocketDataLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
            BillingOBISCodeDataValue = new string[pocketLength, pocketDataLength];
            int pocketCounter = 0;
            while (counter < pocketLength)//arrayLength
            {
                int innerCounter = 0;
                while (innerCounter < pocketDataLength)
                {
                    datatype = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                    dataLength = 0;
                    totByte = 0;
                    if (datatype == 9 || datatype == 10)
                    {
                        dataLength = common.ConvertHexToDecimal(captureObjectData.Substring(index, 2)); index += 2;
                        if (dataLength == 0x0C && datatype == 9)
                        {
                            BillingOBISCodeDataValue[pocketCounter, innerCounter] = common.GetDateTimeString(captureObjectData.Substring(index, (dataLength * 2)));

                        }
                        else
                        {
                            BillingOBISCodeDataValue[pocketCounter, innerCounter] = captureObjectData.Substring(index, (dataLength * 2));
                        }
                        index += (dataLength * 2);
                    }
                    else
                    {
                        structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                        if (structureInfoEntity != null)
                        {
                            totByte = structureInfoEntity.ValueInByte * 2;
                            BillingOBISCodeDataValue[pocketCounter, innerCounter] = common.ConvertHexToDecimal(captureObjectData.Substring(index, totByte)).ToString();
                            index += totByte;
                        }
                    }
                    innerCounter++;
                }
                pocketCounter++;
                index += 4;
                counter++;
            }
            #endregion

            #region capture Scaler Object
            counter = 0;
            index = 0;
            array = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
            arrayLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;

            BillingScaleOBISCode = new string[arrayLength];
            BillingScaleClassID = new int[arrayLength];
            BillingScaleAttribute = new int[arrayLength];
            BillingScaleOBISCodeDataValue = new string[arrayLength];
            BillingScaleOBISCodeDataUnitValue = new string[arrayLength];
            while (counter < arrayLength)
            {
                structure = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;

                totByte = 0;

                //Class ID
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    BillingScaleClassID[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                    index += totByte;
                }
                //OBIS Code
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                dataLength = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                BillingScaleOBISCode[counter] = common.GetOBISCode(captureObjectScalerUnit.Substring(index, (dataLength * 2))); index += (dataLength * 2);
                //Attribute
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    BillingScaleAttribute[counter] = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, totByte));
                    index += totByte;
                }
                //DataIndex
                datatype = common.ConvertHexToDecimal(captureObjectScalerUnit.Substring(index, 2)); index += 2;
                structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                if (structureInfoEntity != null)
                {
                    totByte = structureInfoEntity.ValueInByte * 2;
                    index += totByte;
                }
                counter++;
            }
            #endregion

            #region Reading Capture Scalar Unit Data
            counter = 0;
            index = 0;
            array = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
            arrayLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2));
            if (arrayLength == 1)
            {
                arrayLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index + 4, 2));
                index += 4;
            }
            index += 6;
            while (counter < arrayLength)
            {
                datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                if (datatype == 9 || datatype == 10)
                {
                    dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    BillingScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        BillingScaleOBISCodeDataValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                datatype = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                if (datatype == 9 || datatype == 10)
                {
                    dataLength = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, 2)); index += 2;
                    BillingScaleOBISCodeDataValue[counter] = captureObjectDataScalerUnit.Substring(index, (dataLength * 2));
                    index += (dataLength * 2);
                }
                else
                {
                    structureInfoEntity = structureInfoBLL.GetDetailData(datatype) as StructureInfoEntity;
                    if (structureInfoEntity != null)
                    {
                        totByte = structureInfoEntity.ValueInByte * 2;
                        BillingScaleOBISCodeDataUnitValue[counter] = common.ConvertHexToDecimal(captureObjectDataScalerUnit.Substring(index, totByte)).ToString();
                        index += totByte;
                    }
                }
                index += 4;
                counter++;
            }
            #endregion

            ApplyScaleToBilling(master);
            master.Billing = GetBillingEntity();

        }

        private List<DLMS650BillingEntity> GetBillingEntity()
        {
            int history = 1;
            List<DLMS650BillingEntity> billing = new List<DLMS650BillingEntity>();
            for (int counter = 0; counter < pocketLength; counter++)
            {
                DLMS650BillingEntity billingEntity = new DLMS650BillingEntity();
                for (int innerCounter = 0; innerCounter < pocketDataLength; innerCounter++)
                {
                    switch (innerCounter)
                    {
                        case 0:
                            billingEntity.BillingDate = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 1:
                            billingEntity.SystemPowerFactorforBillingPeriod = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 2:
                            billingEntity.CumulativeEnergykWhTZ0 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 3:
                            billingEntity.CumulativeEnergykWhTZ1 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 4:
                            billingEntity.CumulativeEnergykWhTZ2 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 5:
                            billingEntity.CumulativeEnergykWhTZ3 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 6:
                            billingEntity.CumulativeEnergykWhTZ4 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 7:
                            billingEntity.CumulativeEnergykWhTZ5 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 8:
                            billingEntity.CumulativeEnergykWhTZ6 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 9:
                            billingEntity.CumulativeEnergykWhTZ7 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 10:
                            billingEntity.CumulativeEnergykWhTZ8 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 11:
                            billingEntity.CumulativeEnergykvarhLag = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 12:
                            billingEntity.CumulativeEnergykvarhLead = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 13:
                            billingEntity.CumulativeEnergykVAhTZ0 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 14:
                            billingEntity.CumulativeEnergykVAhTZ1 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 15:
                            billingEntity.CumulativeEnergykVAhTZ2 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 16:
                            billingEntity.CumulativeEnergykVAhTZ3 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 17:
                            billingEntity.CumulativeEnergykVAhTZ4 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 18:
                            billingEntity.CumulativeEnergykVAhTZ5 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 19:
                            billingEntity.CumulativeEnergykVAhTZ6 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 20:
                            billingEntity.CumulativeEnergykVAhTZ7 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 21:
                            billingEntity.CumulativeEnergykVAhTZ8 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 22:
                            billingEntity.MDkWTZ0 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 23:
                            billingEntity.MDkWDateTimeTZ0 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 24:
                            billingEntity.MDkWTZ1 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 25:
                            billingEntity.MDkWDateTimeTZ1 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 26:
                            billingEntity.MDkWTZ2 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 27:
                            billingEntity.MDkWDateTimeTZ2 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 28:
                            billingEntity.MDkWTZ3 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 29:
                            billingEntity.MDkWDateTimeTZ3 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 30:
                            billingEntity.MDkWTZ4 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 31:
                            billingEntity.MDkWDateTimeTZ4 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 32:
                            billingEntity.MDkWTZ5 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 33:
                            billingEntity.MDkWDateTimeTZ5 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 34:
                            billingEntity.MDkWTZ6 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 35:
                            billingEntity.MDkWDateTimeTZ6 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 36:
                            billingEntity.MDkWTZ7 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 37:
                            billingEntity.MDkWDateTimeTZ7 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 38:
                            billingEntity.MDkWTZ8 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 39:
                            billingEntity.MDkWDateTimeTZ8 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 40:
                            billingEntity.MDkVATZ0 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 41:
                            billingEntity.MDkVADateTimeTZ0 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 42:
                            billingEntity.MDkVATZ1 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 43:
                            billingEntity.MDkVADateTimeTZ1 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 44:
                            billingEntity.MDkVATZ2 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 45:
                            billingEntity.MDkVADateTimeTZ2 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 46:
                            billingEntity.MDkVATZ3 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 47:
                            billingEntity.MDkVADateTimeTZ3 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 48:
                            billingEntity.MDkVATZ4 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 49:
                            billingEntity.MDkVADateTimeTZ4 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 50:
                            billingEntity.MDkVATZ5 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 51:
                            billingEntity.MDkVADateTimeTZ5 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 52:
                            billingEntity.MDkVATZ6 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 53:
                            billingEntity.MDkVADateTimeTZ6 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 54:
                            billingEntity.MDkVATZ7 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 55:
                            billingEntity.MDkVADateTimeTZ7 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 56:
                            billingEntity.MDkVATZ8 = BillingOBISCodeDataValue[counter, innerCounter];
                            break;
                        case 57:
                            billingEntity.MDkVADateTimeTZ8 = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter]);
                            break;
                        case 58:
                            //if (isMPKWCL)
                            //{
                            //    //billingEntity.CumPowerOffDuration = common.GetCumPowerOffDuration(BillingOBISCodeDataValue[counter, innerCounter]);
                            //}
                            //else if (isPGVCL)
                            //{
                                billingEntity.BillingType = GetBillingType(BillingOBISCodeDataValue[counter, innerCounter]);
                            //}
                            //else if (showCumPowerOffDetails)
                            //{
                            //    //billingEntity.CumPowerOffDuration = BillingOBISCodeDataValue[counter, innerCounter].ToString();
                            //}
                            break;
                        case 59:
                            if (isMPKWCL)
                            {
                                billingEntity.CumTamperCount = Int64.Parse(BillingOBISCodeDataValue[counter, innerCounter].Remove(BillingOBISCodeDataValue[counter, innerCounter].Length - 1));
                            }
                            break;
                    }
                }
                if (IsCurrent)
                {
                    billingEntity.DataIndex = 0;
                    IsCurrent = false;

                }
                else
                {
                    billingEntity.DataIndex = history;
                    history++;
                }
                // BhardwajG : If called from API, then assign data index stored in file for exporting
                // exactly that billing which was read.
                if (IsAPI)
                {
                    if (billingMonths < 13)
                    {
                        billingEntity.DataIndex = billingMonths;
                        // BhardwajG : if billing is 0 then export only current billilng, as meter has done full read in case of current billing
                        if (billingMonths == 0)
                        {
                            billing.Add(billingEntity);
                            break;
                        }
                    }

                }
                billing.Add(billingEntity);



            }
            return billing;
        }

        private int GetScaleIndex(string obisCode, int classId, int attributeId)
        {
            for (int counter = 0; counter < pocketDataLength; counter++)
            {
                if (BillingScaleOBISCode[counter].Trim().ToUpper().Equals(obisCode.Trim().ToUpper()) && BillingScaleClassID[counter] == classId)
                    return counter;
            }
            return 0;
        }

        private void ApplyScaleToBilling(BillingGeneralNFDLMSEntity master)
        {
            DLMSObjectType dlmsObjectType;
            bool isHTCT = false;
            try
            {
                if (billingMonths == 13 || pocketLength > 12)
                    IsCurrent = true;
                else
                    IsCurrent = false;

                for (int counter = 0; counter < pocketLength; counter++)
                {
                    for (int innerCounter = 0; innerCounter < pocketDataLength; innerCounter++)
                    {
                        int scaleId = GetScaleIndex(BillingOBISCode[innerCounter], BillingClassID[innerCounter], BillingAttribute[innerCounter]);
                        dlmsObjectType = GetBillingObjectType(innerCounter);
                        if (dlmsObjectType == DLMSObjectType.DateTime)
                            continue;
                        StructureUnitEntity structureUnitEntity = structureUnitInfoBLL.GetDetailData(Int32.Parse(BillingScaleOBISCodeDataUnitValue[scaleId])) as StructureUnitEntity;
                        int scaleVale = Int32.Parse(BillingScaleOBISCodeDataValue[scaleId]);


                        if ((!(dlmsObjectType == DLMSObjectType.Power)) && (scaleVale == 0))
                        {
                            if (scaleVale == 0)
                            {
                                if (structureUnitEntity != null)//Bug Id:DLMS_0008
                                {
                                    string unit = common.GetUnit(structureUnitEntity.StructureUnit);
                                    if (unit.ToUpper().Contains("K"))
                                    {
                                        //int mode = Convert.ToInt32(BillingOBISCodeDataValue[counter, innerCounter]) % 1000;
                                        //int reminder = Convert.ToInt32(BillingOBISCodeDataValue[counter, innerCounter]) / 1000;
                                        //string billingOBISCodeValue = reminder.ToString() + "." + mode.ToString();

                                        // Added to fix bug DLMS_0040
                                        string billingOBISCodeValue = (Convert.ToDouble(BillingOBISCodeDataValue[counter, innerCounter]) / 1000).ToString("0.000");
                                        BillingOBISCodeDataValue[counter, innerCounter] = string.Concat(billingOBISCodeValue, "*", unit);
                                    }
                                    else
                                    {
                                        BillingOBISCodeDataValue[counter, innerCounter] = string.Concat(BillingOBISCodeDataValue[counter, innerCounter], "*", unit);
                                    }
                                }
                                continue;
                            }
                        }
                        string dataValue = BillingOBISCodeDataValue[counter, innerCounter];
                        if (scaleVale > 128)
                        {
                            int ScaleDiff = 256 - scaleVale;
                            //ScaleDiff = ScaleDiff * -1;
                            if (isPUMA)
                            {
                                dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, false).ToString();
                            }
                            else
                            {
                                dataValue = common.GetSignValue(Int32.Parse(dataValue), ScaleDiff, "-", structureUnitEntity.StructureUnit);
                            }
                        }
                        else
                        {
                            if (isPUMA)
                            {
                                dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit, dlmsObjectType, out isHTCT, false).ToString();
                            }
                            else
                            {
                                dataValue = common.GetSignValue(Int32.Parse(dataValue), scaleVale, "+", structureUnitEntity.StructureUnit);
                            }
                        }
                        if (isPUMA)
                        {
                            dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit, isHTCT);
                        }
                        else
                        {
                            dataValue = dataValue + "*" + common.GetUnit(structureUnitEntity.StructureUnit);
                        }
                        BillingOBISCodeDataValue[counter, innerCounter] = dataValue;
                    }
                }

                //Assign Meter Type to MeterDataType property 
                master.MeterDataType = (isHTCT) ? MeterDataTypes.HTCT : MeterDataTypes.LTCT;
            }
            catch (Exception)
            {

            }
        }



        /// <summary>
        /// Return the Billing Type. 
        /// If it is 0x40 then Auto 
        /// Else if it is 0x24 Then Manual
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetBillingType(string value)
        {
            string strBillingType = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                if (Convert.ToInt32(value.Substring(0, value.Length - 1)) == 64)
                {
                    return Constants.BTAuto;
                }
                else if (Convert.ToInt32(value.Substring(0, value.Length - 1)) == 36)
                {
                    return Constants.BTManual;
                }
                else
                { return Constants.BTDefault; }

            }
            return string.Empty;
        }

        private DLMSObjectType GetBillingObjectType(int counter)
        {
            DLMSObjectType dlmsobjectType = DLMSObjectType.None;
            switch (counter)
            {
                case 0:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 1:
                    dlmsobjectType = DLMSObjectType.None;
                    break;
                case 2:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 3:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 4:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 5:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 6:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 7:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 8:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 9:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 10:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 11:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 12:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 13:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 14:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 15:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 16:
                    dlmsobjectType = DLMSObjectType.Energy; ;
                    break;
                case 17:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 18:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 19:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 20:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 21:
                    dlmsobjectType = DLMSObjectType.Energy;
                    break;
                case 22:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 23:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 24:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 25:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 26:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 27:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 28:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 29:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 30:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 31:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 32:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 33:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 34:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 35:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 36:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 37:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 38:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 39:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 40:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 41:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 42:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 43:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 44:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 45:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 46:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 47:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 48:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 49:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 50:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 51:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 52:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 53:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 54:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 55:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 56:
                    dlmsobjectType = DLMSObjectType.Demand;
                    break;
                case 57:
                    dlmsobjectType = DLMSObjectType.DateTime;
                    break;
                case 58:
                    if (isMPKWCL)
                    {
                        dlmsobjectType = DLMSObjectType.None;
                    }
                    else if (isPGVCL)
                    {
                        dlmsobjectType = DLMSObjectType.None;
                    }
                    break;
                case 59:
                    if (isMPKWCL)
                    {
                        dlmsobjectType = DLMSObjectType.None;
                    }
                    break;
            }
            return dlmsobjectType;
        }
    }
}
