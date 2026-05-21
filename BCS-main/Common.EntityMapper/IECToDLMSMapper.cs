#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.Serialization;
using CABEntity;
using CAB.BLL;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps IEC Entity to dlms entity
    /// </summary>
    public class IECToDLMSMapper
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private ConfigurationParser meterConfigParser;
        private readonly Serializer lngSerialzer = null;
        private TAMPERMAPPER tamperMapper = null;
        private const string NotAvailable = "NA";
        private Dictionary<string, int> transactions = null;
        Tamper mapperTamper = new Tamper();
        private readonly bool isCDFConverterCall;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(IECToDLMSMapper).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public IECToDLMSMapper(bool isCDFConverter)
        {
            isCDFConverterCall = isCDFConverter;
            transactions = new Dictionary<string, int>() { 
            {"Real Time Clock - Date and Time",151},
            {"Maximum Demand",152},
            {"Load Survey IP",153},
            {"Billing Date & Time",154},
            {"Future TOU",155},
            {"CT Ratio",156}, 
            {"PT Ratio",157},
            {"kVAh Selection",158},
            {"MD Reset",159},
            {"Display Parameter - Push",160},
            {"Display Parameter - Scroll",161},
            {"Display Parameter - HR",162},
            {"Display Timeout",163},
            {"Auto Billing Lock",164},
            {"RS232 Lock",165},
            {"Daily Log Parameters",166},
            {"Software Billing Parameters",167},
            {"Manual Billing Parameters",168},
            {"Tamper Reset", 256} // Tamper Reset
            };
            lngSerialzer = new Serializer();
            tamperMapper = (TAMPERMAPPER)lngSerialzer.DeserializeToObject(AppDomain.CurrentDomain.BaseDirectory + "TamperDLMSToIECMapper.xml", typeof(TAMPERMAPPER));
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///  Used to convert IEC Entity to DLMS entity .
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="isUpload">to Decide wether function is called for uplaod or CDF conversion </param>
        /// <returns></returns>
        public BillingGeneralNFDLMSEntity ConvertIECEntityToDLMSEntity(IECBillingGeneralNFEntity inputEntity, bool isUpload)
        {
            BillingGeneralNFDLMSEntity outputEntity = new BillingGeneralNFDLMSEntity();
            outputEntity.MeterDataID = inputEntity.MeterDataID;
            //General , instant and Billing
            if (inputEntity.listGeneralData != null && inputEntity.listGeneralData.Count > 0)
            {
                if (inputEntity.listGeneralData[0].MeterData != null)
                {
                    outputEntity.MeterData = GetMeterDataEntity(inputEntity.listGeneralData[0].MeterData);
                }

                outputEntity.General = GetGeneralEntity(inputEntity.listGeneralData[0].CurrentGeneral);
                outputEntity.Instant = GetInstantEntity(inputEntity.listGeneralData[0].CurrentInstant, inputEntity.listGeneralData[0].CurrentGeneral
                    , inputEntity.listGeneralData[0].CurrentBilling);
                outputEntity.Billing = GetBillingEntity(inputEntity.listGeneralData[0]);


            }
            //Load Surevy
            if (inputEntity.listLoadSurveyData != null && inputEntity.listLoadSurveyData.Count > 0
                && inputEntity.listLoadSurveyData[0].LoadSurvey != null && inputEntity.listLoadSurveyData[0].LoadSurvey.Count > 0)
            {
                outputEntity.LoadSurvey = GetLoadSurveyEntity(inputEntity.listLoadSurveyData, isUpload);
                outputEntity.LSParameterColumns = GetLoadSurveyParameterEntity(inputEntity.listLoadSurveyData[0].LoadSurvey[0]);
            }

            //Phasor
            if (inputEntity.listPhasor != null && inputEntity.listPhasor.Count > 0)
            {
                outputEntity.Phasor = GetPhasorEntity(inputEntity.listPhasor[0]);
            }

            //Tamper and Transaction and RTC Update
            if (inputEntity.listTamper != null && inputEntity.listTransactionData != null
                                                 && inputEntity.listRTCUpdate != null)
            {
                outputEntity.Tamper = GetTamperEntity(inputEntity.listTamper, inputEntity.listTransactionData, inputEntity.listRTCUpdate);
                if (inputEntity.listTamper.Count > 0 && inputEntity.listTamper[0].Snapshot != null && inputEntity.listTamper[0].Snapshot.Count > 0)
                {
                    outputEntity.TamperParameterColumns = GetTamperParameterEntity(inputEntity.listTamper[0].Snapshot[0]);
                }
            }

            //Midnight
            if (inputEntity.listDTMDailyProfileData != null && inputEntity.listDTMDailyProfileData.Count > 0
                && inputEntity.listDTMDailyProfileData[0].DTMDailyProfile != null && inputEntity.listDTMDailyProfileData[0].DTMDailyProfile.Count > 0)
            {
                outputEntity.MidnightData = GetMidnightEntity(inputEntity.listDTMDailyProfileData);
                outputEntity.MidnightData = outputEntity.MidnightData.OrderBy(i => i.RealTimeClockDateandTime).ToList();
                outputEntity.MidnightParameterColumns = GetMidnightParameterEntity(inputEntity.listDTMDailyProfileData[0].DTMDailyProfile[0]);
            }

            if (inputEntity.listFraudEnergy != null && inputEntity.listFraudEnergy.Count > 0)
            {

                outputEntity.FraudEnergy = GetFraudEnergyEntity(inputEntity.listFraudEnergy[0]);
            }

            return outputEntity;

        }

        #endregion
        #region Public Methods
        /// <summary>
        ///  Used to convert IEC Entity to DLMS entity .
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="isUpload">to Decide wether function is called for uplaod or CDF conversion </param>
        /// <returns></returns>
        public BillingGeneralNFDLMSEntity ConvertIECEntityToDLMSEntityForSPhase(IECBillingGeneralNFEntity inputEntity, bool isUpload)
        {
            BillingGeneralNFDLMSEntity outputEntity = new BillingGeneralNFDLMSEntity();
            outputEntity.MeterDataID = inputEntity.MeterDataID;
            //General , instant and Billing
            if (inputEntity.listGeneralData != null && inputEntity.listGeneralData.Count > 0)
            {
                if (inputEntity.listGeneralData[0].MeterData != null)
                {
                    outputEntity.MeterData = GetMeterDataEntity(inputEntity.listGeneralData[0].MeterData);
                }

                outputEntity.General = GetGeneralEntityForSPhase(inputEntity.listGeneralData[0].CurrentGeneral);
                outputEntity.Instant = GetInstantEntityForSPhase(inputEntity.listGeneralData[0].CurrentInstant, inputEntity.listGeneralData[0].CurrentGeneral
                    , inputEntity.listGeneralData[0].CurrentBilling);
                outputEntity.Billing = GetBillingEntityForSPhase(inputEntity.listGeneralData[0]);
                outputEntity.Anomaly = GetAnomalyEntityForSPhase(inputEntity.listGeneralData[0]);


            }
            //Load Surevy
            if (inputEntity.listLoadSurveyData != null && inputEntity.listLoadSurveyData.Count > 0
                && inputEntity.listLoadSurveyData[0].LoadSurvey != null && inputEntity.listLoadSurveyData[0].LoadSurvey.Count > 0)
            {
                outputEntity.LoadSurvey = GetLoadSurveyEntitySPhase(inputEntity.listLoadSurveyData, isUpload);
                outputEntity.LSParameterColumns = GetLoadSurveyParameterEntitySPhase(inputEntity.listLoadSurveyData[0].LoadSurvey[0]);
            }

            //Phasor
            if (inputEntity.listPhasor != null && inputEntity.listPhasor.Count > 0)
            {
                outputEntity.Phasor = GetPhasorEntity(inputEntity.listPhasor[0]);
            }

            //Tamper and Transaction and RTC Update
            if (inputEntity.listTamper != null && inputEntity.listTransactionData != null
                                                 && inputEntity.listRTCUpdate != null)
            {
                outputEntity.Tamper = GetTamperEntityForSPhase(inputEntity.listTamper, inputEntity.listTransactionData, inputEntity.listRTCUpdate);
                if (inputEntity.listTamper.Count > 0 && inputEntity.listTamper[0].Snapshot != null && inputEntity.listTamper[0].Snapshot.Count > 0)
                {
                    outputEntity.TamperParameterColumns = GetTamperParameterEntitySPhase(inputEntity.listTamper[0].Snapshot[0]);
                }
            }

            //Midnight
            if (inputEntity.listDTMDailyProfileData != null && inputEntity.listDTMDailyProfileData.Count > 0
                && inputEntity.listDTMDailyProfileData[0].DTMDailyProfile != null && inputEntity.listDTMDailyProfileData[0].DTMDailyProfile.Count > 0)
            {
                outputEntity.MidnightData = GetMidnightEntity(inputEntity.listDTMDailyProfileData);
                outputEntity.MidnightData = outputEntity.MidnightData.OrderBy(i => i.RealTimeClockDateandTime).ToList();
                outputEntity.MidnightParameterColumns = GetMidnightParameterEntity(inputEntity.listDTMDailyProfileData[0].DTMDailyProfile[0]);
            }

            if (inputEntity.listFraudEnergy != null && inputEntity.listFraudEnergy.Count > 0)
            {

                outputEntity.FraudEnergy = GetFraudEnergyEntity(inputEntity.listFraudEnergy[0]);
            }

            // TOD IEC Implementation 
            // Addition of mapping TOU data for IEC
            if (inputEntity.meterConfigurationDetail != null && inputEntity.meterConfigurationDetail.Count > 0)
            {
                TODEntity _todentity = new TODEntity();
                MeterConfigurationsNFEntity _meterconfig = new MeterConfigurationsNFEntity();
                _todentity.MeterDataID = inputEntity.meterConfigurationDetail[0].TODEntity.MeterData_ID;
                _todentity.TODData = inputEntity.meterConfigurationDetail[0].TODEntity.TODData;
                _meterconfig.TODEntity = _todentity;
                outputEntity.MeterConfigurations = _meterconfig;

            }

            return outputEntity;

        }

        /// <summary>
        /// CDF-specific wrapper: returns CAB.Entity so Generic.CABApplication can use it without direct CAB.Entity reference.
        /// </summary>
        public LNG.Entity.BillingGeneralNFDLMSEntity ConvertIECEntityToDLMSEntityForCAB(IECBillingGeneralNFEntity inputEntity, bool isUpload)
            => MapToCABEntity(ConvertIECEntityToDLMSEntity(inputEntity, isUpload));

        /// <summary>
        /// CDF-specific wrapper: returns CAB.Entity so Generic.CABApplication can use it without direct CAB.Entity reference.
        /// </summary>
        public LNG.Entity.BillingGeneralNFDLMSEntity ConvertIECEntityToDLMSEntityForSPhaseCAB(IECBillingGeneralNFEntity inputEntity, bool isUpload)
            => MapToCABEntity(ConvertIECEntityToDLMSEntityForSPhase(inputEntity, isUpload));

        /// <summary>
        /// Maps CAB.Entity.BillingGeneralNFDLMSEntity to CAB.Entity.BillingGeneralNFDLMSEntity
        /// so callers in Generic.CABApplication (which has no direct CAB.Entity reference) can use it.
        /// </summary>
        public LNG.Entity.BillingGeneralNFDLMSEntity MapEntityToCAB(BillingGeneralNFDLMSEntity src) => MapToCABEntity(src);
        private LNG.Entity.BillingGeneralNFDLMSEntity MapToCABEntity(BillingGeneralNFDLMSEntity src)
        {
            if (src == null) return null;
            var dst = new LNG.Entity.BillingGeneralNFDLMSEntity();
            dst.MeterDataID        = src.MeterDataID;
            dst.ReadoutCounter     = src.ReadoutCounter;
            dst.FileType           = src.FileType;
            dst.DemandIntegrationPeriod = src.DemandIntegrationPeriod;
            if (src.MeterData != null)
            {
                dst.MeterData = new LNG.Entity.MeterDataEntity();
                dst.MeterData.MeterID           = src.MeterData.MeterID;
                dst.MeterData.ReadingDateTime   = src.MeterData.ReadingDateTime;
                dst.MeterData.UploadingDateTime = src.MeterData.UploadingDateTime;
                dst.MeterData.MeterData_ID      = src.MeterData.MeterData_ID;
                dst.MeterData.FileUpload_ID     = src.MeterData.FileUpload_ID;
                dst.MeterData.CMRIID            = src.MeterData.CMRIID;
            }
            if (src.General != null)
            {
                dst.General = new LNG.Entity.DLMS650NamePlateDetailsEntity();
                dst.General.MeterSerialNumber   = src.General.MeterSerialNumber;
                dst.General.BasicCurrentRating  = src.General.BasicCurrentRating;
                dst.General.CurrentRating       = src.General.CurrentRating;
                dst.General.InternalCTratio     = src.General.InternalCTratio;
                dst.General.InternalPTratio     = src.General.InternalPTratio;
                dst.General.MeterData_ID        = src.General.MeterData_ID;
                dst.General.Metertype           = src.General.Metertype;
                dst.General.Manufacturername    = src.General.Manufacturername;
                dst.General.FirmwareVersionformeter = src.General.FirmwareVersionformeter;
                dst.General.Category            = src.General.Category;
                dst.General.EnergyResolution    = src.General.EnergyResolution;
                dst.General.DemandResolution    = src.General.DemandResolution;
                dst.General.VoltageRating       = src.General.VoltageRating;
                dst.General.MeterDataType       = src.General.MeterDataType;
                dst.General.MeterModelNo        = src.General.MeterModelNo;
                dst.General.InternalFirmwareVersion = src.General.InternalFirmwareVersion;
                dst.General.MeterConstant       = src.General.MeterConstant;
                dst.General.MeterClass          = src.General.MeterClass;
                dst.General.LEDPulseRate        = src.General.LEDPulseRate;
                dst.General.AccuracyClass       = src.General.AccuracyClass;
                dst.General.NetMeterVariantInfo = src.General.NetMeterVariantInfo;
                dst.General.Meteryearofmanufacture = src.General.Meteryearofmanufacture;
                dst.General.MeterMonthOfManufacture = src.General.MeterMonthOfManufacture;
                dst.General.DisplayProgrammingType  = src.General.DisplayProgrammingType;
                dst.General.ReverseKWh          = src.General.ReverseKWh;
            }
            dst.Billing     = src.Billing      != null ? src.Billing.Select(b      => MapBilling(b)).ToList()      : null;
            dst.Instant     = src.Instant      != null ? src.Instant.Select(i      => MapInstant(i)).ToList()      : null;
            dst.LoadSurvey  = src.LoadSurvey   != null ? src.LoadSurvey.Select(l   => MapLoadSurvey(l)).ToList()  : null;
            dst.MidnightData= src.MidnightData != null ? src.MidnightData.Select(m => MapMidnight(m)).ToList()    : null;
            dst.Tamper      = src.Tamper       != null ? src.Tamper.Select(t       => MapTamper(t)).ToList()       : null;
            return dst;
        }
        private LNG.Entity.DLMS650BillingEntity MapBilling(DLMS650BillingEntity s)
        {
            if (s == null) return null;
            var d = new LNG.Entity.DLMS650BillingEntity();
            d.DataIndex = s.DataIndex; d.BillingType = s.BillingType;
            d.CumulativeEnergykWhTZ0 = s.CumulativeEnergykWhTZ0; d.CumulativeEnergykVAhTZ0 = s.CumulativeEnergykVAhTZ0;
            d.CumulativeEnergykvarhLag = s.CumulativeEnergykvarhLag; d.CumulativeEnergykvarhLead = s.CumulativeEnergykvarhLead;
            d.MDkWTZ0 = s.MDkWTZ0; d.MDkVATZ0 = s.MDkVATZ0;
            d.MDkWDateTimeTZ0 = s.MDkWDateTimeTZ0; d.MDkVADateTimeTZ0 = s.MDkVADateTimeTZ0;
            d.SystemPowerFactorforBillingPeriod = s.SystemPowerFactorforBillingPeriod;
            d.PowerOnDuration = s.PowerOnDuration; d.CumPowerOffDuration = s.CumPowerOffDuration;
            d.CumTamperCount = s.CumTamperCount; d.MechanismForCDF = s.MechanismForCDF;
            return d;
        }
        private LNG.Entity.DLMS650InstantaneousEntity MapInstant(DLMS650InstantaneousEntity s)
        {
            if (s == null) return null;
            return new LNG.Entity.DLMS650InstantaneousEntity();
        }
        private LNG.Entity.DLMS650LoadSurveyEntity MapLoadSurvey(DLMS650LoadSurveyEntity s)
        {
            if (s == null) return null;
            var d = new LNG.Entity.DLMS650LoadSurveyEntity();
            d.RealTimeClockDateandTime = s.RealTimeClockDateandTime;
            d.MDIntervalPeriod = s.MDIntervalPeriod;
            return d;
        }
        private LNG.Entity.DLMS650MidnightDataEntity MapMidnight(DLMS650MidnightDataEntity s)
        {
            if (s == null) return null;
            var d = new LNG.Entity.DLMS650MidnightDataEntity();
            d.RealTimeClockDateandTime = s.RealTimeClockDateandTime;
            return d;
        }
        private LNG.Entity.DLMS650TamperEntity MapTamper(DLMS650TamperEntity s)
        {
            if (s == null) return null;
            var d = new LNG.Entity.DLMS650TamperEntity();
            d.EventCode = s.EventCode; d.DateTimeEvent = s.DateTimeEvent;
            d.CompartmentNumber = s.CompartmentNumber;
            return d;
        }


        #endregion
        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DLMS650MidnightDataEntity> GetMidnightEntity(List<DTMDailyProfileData> midnightData)
        {
            List<DLMS650MidnightDataEntity> outputEntityList = new List<DLMS650MidnightDataEntity>();
            DLMS650MidnightDataEntity midnightEntity;
            try
            {
                foreach (DTMDailyProfileEntity inputEntity in midnightData[0].DTMDailyProfile)
                {
                    midnightEntity = new DLMS650MidnightDataEntity();

                    if (!string.IsNullOrEmpty(inputEntity.CumulativekWh))
                    {
                        midnightEntity.CumEnergykWh = inputEntity.CumulativekWh + "*kWh";
                    }
                    if (!string.IsNullOrEmpty(inputEntity.CumulativekVAh))
                    {
                        midnightEntity.CumEnergykVAh = inputEntity.CumulativekVAh + "*kVAh";
                    }
                    if (!string.IsNullOrEmpty(inputEntity.CumulativekVArh_lag))
                    {
                        midnightEntity.CumEnergykvarhlag = inputEntity.CumulativekVArh_lag + "*kvarh";
                    }
                    if (!string.IsNullOrEmpty(inputEntity.CumulativekVArh_lead))
                    {
                        midnightEntity.CumEnergykvarhlead = inputEntity.CumulativekVArh_lead + "*kvarh";
                    }
                    midnightEntity.RealTimeClockDateandTime = inputEntity.DailyProfileDate;
                    if (!string.IsNullOrEmpty(inputEntity.PowerOnHours))
                    {
                        midnightEntity.PowerOnDuration = inputEntity.PowerOnHours + "*Seconds";
                        //For IEC we do not have specific column in Report for Cumulative Power On
                        //midnightEntity.MDKW = inputEntity.PowerOnHours + "*Seconds";
                    }

                    outputEntityList.Add(midnightEntity);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMidnightEntity(List<DTMDailyProfileData> midnightData)", ex);
            }
            return outputEntityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DLMS650TamperEntity> GetTamperEntity(List<TamperData> inputData, List<TransactionData> transactionData, List<RTCUpdateEntity> rtcUpdateEntity)
        {
            List<DLMS650TamperEntity> outputEntityList = new List<DLMS650TamperEntity>();
            DLMS650TamperEntity DLMSTamperEntity;
            TAMPERMAPPERMAP mapper;
            try
            {
                if (inputData != null && inputData.Count > 0)
                {
                    foreach (TamperSnapshotEntity inputTamperEntity in inputData[0].Snapshot)
                    {
                        mapper = FindMappedTamper(inputTamperEntity.TamperCode.ToString());
                        if (mapper != null)
                        {
                            DLMSTamperEntity = new DLMS650TamperEntity();
                            //Occurence Info
                            DLMSTamperEntity.EventCode = Convert.ToInt64(mapper.DLMSCODE);
                            FillOccurenceInfo(inputTamperEntity, DLMSTamperEntity);
                            outputEntityList.Add(DLMSTamperEntity);
                            //Restoration info
                            if (mapper.DLMSRESCODE != NotAvailable && inputTamperEntity.TamperRestoredTime != 19000101000000)
                            {
                                DLMSTamperEntity = new DLMS650TamperEntity();
                                DLMSTamperEntity.EventCode = Convert.ToInt64(mapper.DLMSRESCODE);
                                FillRestorationInfo(inputTamperEntity, DLMSTamperEntity);
                                outputEntityList.Add(DLMSTamperEntity);
                            }
                        }

                    }
                }

                #region Transaction
                if (transactionData != null && transactionData.Count > 0)
                {
                    //Transactions
                    int eventCode = 0;
                    foreach (ProgrammingEntity programmingEntity in transactionData[0].programmingData)
                    {
                        if (!string.IsNullOrEmpty(programmingEntity.Description1))
                        {
                            if (transactions.TryGetValue(programmingEntity.Description1, out eventCode))
                            {
                                DLMSTamperEntity = new DLMS650TamperEntity();
                                DLMSTamperEntity.CompartmentNumber = 4;
                                DLMSTamperEntity.EventCode = eventCode;
                                DLMSTamperEntity.DateTimeEvent = DateUtility.DateTimeToLong(DateTime.Parse(programmingEntity.LastTimestamp, new System.Globalization.CultureInfo("hi-in")
                                                                                           , System.Globalization.DateTimeStyles.AssumeLocal));

                                outputEntityList.Add(DLMSTamperEntity);
                            }
                        }

                    }
                }
                #endregion

                #region RTCUPdate
                if (rtcUpdateEntity != null && rtcUpdateEntity.Count > 0)
                {
                    foreach (RTCUpdateEntity rtcEntity in rtcUpdateEntity)
                    {
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC1))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC1));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC2))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC2));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC3))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC3));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC4))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC4));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC5))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC5));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC6))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC6));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC7))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC7));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC8))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC8));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC9))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC9));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC10))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC10));
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperEntity(List<TamperData> inputData, List<TransactionData> transactionData, List<RTCUpdateEntity> rtcUpdateEntity)", ex);
            }
            return outputEntityList;
        }

        private List<DLMS650TamperEntity> GetTamperEntityForSPhase(List<TamperData> inputData, List<TransactionData> transactionData, List<RTCUpdateEntity> rtcUpdateEntity)
        {
            List<DLMS650TamperEntity> outputEntityList = new List<DLMS650TamperEntity>();
            DLMS650TamperEntity DLMSTamperEntity;
            TAMPERMAPPERMAP mapper;
            try
            {
                if (inputData != null && inputData.Count > 0)
                {
                    foreach (TamperSnapshotEntity inputTamperEntity in inputData[0].Snapshot)
                    {
                        mapper = FindMappedTamper(inputTamperEntity.TamperCode.ToString());
                        if (mapper != null)
                        {
                            DLMSTamperEntity = new DLMS650TamperEntity();
                            //Occurence Info
                            DLMSTamperEntity.EventCode = Convert.ToInt64(mapper.DLMSCODE);
                            FillOccurenceInfoForSPhase(inputTamperEntity, DLMSTamperEntity);
                            if (DLMSTamperEntity.DateTimeEvent != 19000101000000)
                                outputEntityList.Add(DLMSTamperEntity);
                            //Restoration info
                            if (mapper.DLMSRESCODE != NotAvailable && inputTamperEntity.TamperRestoredTime != 19000101000000)
                            {
                                DLMSTamperEntity = new DLMS650TamperEntity();
                                DLMSTamperEntity.EventCode = Convert.ToInt64(mapper.DLMSRESCODE);
                                FillRestorationInfoForSPhase(inputTamperEntity, DLMSTamperEntity);
                                if (DLMSTamperEntity.DateTimeEvent != 19000101000000)
                                    outputEntityList.Add(DLMSTamperEntity);
                            }
                        }

                    }
                }

                #region Transaction
                if (transactionData != null && transactionData.Count > 0)
                {
                    //Transactions
                    int eventCode = 0;
                    foreach (ProgrammingEntity programmingEntity in transactionData[0].programmingData)
                    {
                        if (!string.IsNullOrEmpty(programmingEntity.Description1))
                        {
                            
                            if (transactions.TryGetValue(programmingEntity.Description1, out eventCode))
                            {
                                DLMSTamperEntity = new DLMS650TamperEntity();
                                DLMSTamperEntity.CompartmentNumber = 4;
                                DLMSTamperEntity.EventCode = eventCode;
                                if (programmingEntity.LastTimestamp == string.Empty)
                                    continue;
                                DLMSTamperEntity.DateTimeEvent = DateUtility.DateTimeToLong(DateTime.Parse(programmingEntity.LastTimestamp, new System.Globalization.CultureInfo("hi-in")
                                                                                           , System.Globalization.DateTimeStyles.AssumeLocal));

                                outputEntityList.Add(DLMSTamperEntity);
                            }
                        }

                    }
                }
                #endregion

                #region RTCUPdate
                if (rtcUpdateEntity != null && rtcUpdateEntity.Count > 0)
                {
                    foreach (RTCUpdateEntity rtcEntity in rtcUpdateEntity)
                    {
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC1))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC1));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC2))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC2));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC3))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC3));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC4))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC4));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC5))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC5));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC6))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC6));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC7))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC7));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC8))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC8));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC9))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC9));
                        }
                        if (!string.IsNullOrEmpty(rtcEntity.CurrentRTC10))
                        {
                            outputEntityList.Add(AddRTCUpdateTransaction(rtcEntity.CurrentRTC10));
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperEntityForSPhase(List<TamperData> inputData, List<TransactionData> transactionData, List<RTCUpdateEntity> rtcUpdateEntity)", ex);
            }
            return outputEntityList;
        }
        /// <summary>
        /// Used to create a tamper entity from RTC Update .
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DLMS650TamperEntity AddRTCUpdateTransaction(string dateTime)
        {
            DLMS650TamperEntity DLMSTamperEntity = new DLMS650TamperEntity();
            DLMSTamperEntity.CompartmentNumber = 4;
            DLMSTamperEntity.EventCode = 151; //RTC Event code.
            DLMSTamperEntity.DateTimeEvent = DateUtility.DateTimeToLong(DateTime.Parse(dateTime, new System.Globalization.CultureInfo("hi-in")
                                                                               , System.Globalization.DateTimeStyles.AssumeLocal));
            return DLMSTamperEntity;
        }

        /// <summary>
        /// Fill occurence information in tamper snap shot entity
        /// </summary>
        /// <param name="tamperSnapShot"></param>
        /// <param name="resultEntity"></param>
        private void FillOccurenceInfo(TamperSnapshotEntity tamperSnapShot, DLMS650TamperEntity resultEntity)
        {
            resultEntity.DateTimeEvent = tamperSnapShot.TamperOccurredTime;
            resultEntity.CurrentIR = string.Concat(tamperSnapShot.RCurrentOccurred, "*A");
            resultEntity.CurrentIY = string.Concat(tamperSnapShot.YCurrentOccurred, "*A");
            resultEntity.CurrentIB = string.Concat(tamperSnapShot.BCurrentOccurred, "*A");
            resultEntity.VoltageVRN = string.Concat(tamperSnapShot.RVoltageOccurred, "*V");
            resultEntity.VoltageVYN = string.Concat(tamperSnapShot.YVoltageOccurred, "*V");
            resultEntity.VoltageVBN = string.Concat(tamperSnapShot.BVoltageOccurred, "*V");

            resultEntity.PowerFactorRphase = string.Concat(tamperSnapShot.RPFOccurred, "*");
            resultEntity.PowerFactorYphase = string.Concat(tamperSnapShot.YPFOccurred, "*");
            resultEntity.PowerFactorBphase = string.Concat(tamperSnapShot.BPFOccurred, "*");
            resultEntity.TotalPowerFactor = string.Concat(tamperSnapShot.TotalPFOccurred, "*"); ;
            resultEntity.CumulativeEnergykWh = string.Concat(tamperSnapShot.KWhOccurred, "*kWh");
            resultEntity.CumulativeEnergykVAh = string.Concat(tamperSnapShot.KVAhOccurred, "*kVAh");

        }
        /// <summary>
        /// Fill restoration information in tamper snap shot entity for Single Phase
        /// </summary>
        /// <param name="tamperSnapShot"></param>
        /// <param name="resultEntity"></param>
        private void FillRestorationInfo(TamperSnapshotEntity tamperSnapShot, DLMS650TamperEntity resultEntity)
        {

            resultEntity.DateTimeEvent = tamperSnapShot.TamperRestoredTime;
            resultEntity.CurrentIR = string.Concat(tamperSnapShot.RCurrentRestored, "*A");
            resultEntity.CurrentIY = string.Concat(tamperSnapShot.YCurrentRestored, "*A");
            resultEntity.CurrentIB = string.Concat(tamperSnapShot.BCurrentRestored, "*A");
            resultEntity.VoltageVRN = string.Concat(tamperSnapShot.RVoltageRestored, "*V");
            resultEntity.VoltageVYN = string.Concat(tamperSnapShot.YVoltageRestored, "*V");
            resultEntity.VoltageVBN = string.Concat(tamperSnapShot.BVoltageRestored, "*V");

            resultEntity.PowerFactorRphase = string.Concat(tamperSnapShot.RPFRestored, "*");
            resultEntity.PowerFactorYphase = string.Concat(tamperSnapShot.YPFRestored, "*");
            resultEntity.PowerFactorBphase = string.Concat(tamperSnapShot.BPFRestored, "*");
            resultEntity.TotalPowerFactor = string.Concat(tamperSnapShot.TotalPFRestored, "*");
            resultEntity.CumulativeEnergykWh = string.Concat(tamperSnapShot.KWhRestored, "*kWh");
            resultEntity.CumulativeEnergykVAh = string.Concat(tamperSnapShot.KVAhRestored, "*kVAh");
        }


        /// <summary>
        /// Fill occurence information in tamper snap shot entity
        /// </summary>
        /// <param name="tamperSnapShot"></param>
        /// <param name="resultEntity"></param>
        private void FillOccurenceInfoForSPhase(TamperSnapshotEntity tamperSnapShot, DLMS650TamperEntity resultEntity)
        {
            resultEntity.DateTimeEvent = tamperSnapShot.TamperOccurredTime;
            resultEntity.PhaseCurrentInstant = string.Concat(tamperSnapShot.PhaseCurrentOccured, "*A");
            resultEntity.NeutralCurrent = string.Concat(tamperSnapShot.OccuredNeutralCurrent, "*A");
            resultEntity.PhaseVoltage = string.Concat(tamperSnapShot.PhaseVoltageOccured, "*V");

            resultEntity.TotalPowerFactor = string.Concat(tamperSnapShot.TotalPFOccurred, "*");
            resultEntity.CumulativeEnergykWh = string.Concat(tamperSnapShot.KWhOccurred, "*kWh");
            resultEntity.CumulativeEnergykVAh = string.Concat(tamperSnapShot.KVAhOccurred, "*kVAh");
            resultEntity.Temprature = string.Concat(tamperSnapShot.TempratureOccured, Convert.ToChar(0176)+"C");           

        }
        /// <summary>
        /// Fill restoration information in tamper snap shot entity for Single Phase
        /// </summary>
        /// <param name="tamperSnapShot"></param>
        /// <param name="resultEntity"></param>
        private void FillRestorationInfoForSPhase(TamperSnapshotEntity tamperSnapShot, DLMS650TamperEntity resultEntity)
        {
            resultEntity.DateTimeEvent = tamperSnapShot.TamperRestoredTime;
            resultEntity.PhaseCurrentInstant = string.Concat(tamperSnapShot.PhaseCurrentRestore, "*A");
            resultEntity.NeutralCurrent = string.Concat(tamperSnapShot.RestoreNeutralCurrent, "*A");
            resultEntity.PhaseVoltage = string.Concat(tamperSnapShot.PhaseVoltageRestore, "*V");

            resultEntity.TotalPowerFactor = string.Concat(tamperSnapShot.TotalPFRestored, "*");
            resultEntity.CumulativeEnergykWh = string.Concat(tamperSnapShot.KWhRestored, "*kWh");
            resultEntity.CumulativeEnergykVAh = string.Concat(tamperSnapShot.KVAhRestored, "*kVAh");
        }

        /// <summary>
        /// Gets a mapper instance corresponding to IEc Event code .
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private TAMPERMAPPERMAP FindMappedTamper(string iECEventCode)
        {
            TAMPERMAPPERMAP mapper = null;
            foreach (TAMPERMAPPERMAP map in tamperMapper.MAP)
            {
                if (map.IECCODE.Trim() == iECEventCode.Trim())
                {
                    mapper = map;
                    break;
                }
            }
            return mapper;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iecFraudEnergyEntity"></param>
        /// <returns></returns>
        private FraudEnergyEntity GetFraudEnergyEntity(IECFraudEnergyEntity iecFraudEnergyEntity)
        {
            FraudEnergyEntity outPutEntity = new FraudEnergyEntity();
            try
            {
                outPutEntity.MagneticInfluenceKWh = iecFraudEnergyEntity.MagneticInfluenceKWh;
                outPutEntity.MagneticInflueneceKVAh = iecFraudEnergyEntity.MagneticInflueneceKVAh;
                outPutEntity.MagneticInflueneceKVARhLag = iecFraudEnergyEntity.MagneticInflueneceKVARhLag;
                outPutEntity.MagneticInflueneceKVARhLead = iecFraudEnergyEntity.MagneticInflueneceKVARhLead;

                outPutEntity.ReverseEnergyKWh = iecFraudEnergyEntity.ReverseEnergyKWh;
                outPutEntity.ReverseEnergyKVAh = iecFraudEnergyEntity.ReverseEnergyKVAh;
                outPutEntity.ReverseEnergyKVARhLag = iecFraudEnergyEntity.ReverseEnergyKVARhLag;
                outPutEntity.ReverseEnergyKVARhLead = iecFraudEnergyEntity.ReverseEnergyKVARhLead;

                outPutEntity.THDCurrentRPhase = iecFraudEnergyEntity.THDCurrentRPhase;
                outPutEntity.THDCurrentYPhase = iecFraudEnergyEntity.THDCurrentYPhase;
                outPutEntity.THDCurrentBPhase = iecFraudEnergyEntity.THDCurrentBPhase;
                outPutEntity.THDVoltageRPhase = iecFraudEnergyEntity.THDVoltageRPhase;
                outPutEntity.THDVoltageYPhase = iecFraudEnergyEntity.THDVoltageYPhase;
                outPutEntity.THDVoltageBPhase = iecFraudEnergyEntity.THDVoltageBPhase;

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetFraudEnergyEntity(IECFraudEnergyEntity iecFraudEnergyEntity)", ex);
                return null;
            }
            return outPutEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iECPhasorEntity"></param>
        /// <returns></returns>
        private PhasorEntity GetPhasorEntity(IECPhasorEntity iECPhasorEntity)
        {
            PhasorEntity outPutEntity = new PhasorEntity();

            try
            {
                outPutEntity.CurrentDateTime = iECPhasorEntity.ReadingDateTime;

                outPutEntity.RPhaseVoltage = iECPhasorEntity.RPhaseVoltage;
                outPutEntity.YPhaseVoltage = iECPhasorEntity.YPhaseVoltage;
                outPutEntity.BPhaseVoltage = iECPhasorEntity.BPhaseVoltage;

                outPutEntity.RPhaseCurrent = iECPhasorEntity.RPhaseCurrent;
                outPutEntity.YPhaseCurrent = iECPhasorEntity.YPhaseCurrent;
                outPutEntity.BPhaseCurrent = iECPhasorEntity.BPhaseCurrent;

                outPutEntity.RPhasePowerFactor = iECPhasorEntity.RPhasePF;
                outPutEntity.YPhasePowerFactor = iECPhasorEntity.YPhasePF;
                outPutEntity.BPhasePowerFactor = iECPhasorEntity.BPhasePF;
                outPutEntity.TotalPhasePowerFactor = iECPhasorEntity.TotalInstantaneousPF;

                outPutEntity.Frequency = iECPhasorEntity.Frequency;

                outPutEntity.ActivePower = iECPhasorEntity.TotalActivePower;
                outPutEntity.ApparentPower = iECPhasorEntity.TotalApparentPower;
                outPutEntity.ReActivePower = iECPhasorEntity.TotalInductivePower;

                outPutEntity.PhaseSequence = iECPhasorEntity.PhaseSequence;

                outPutEntity.RPhaseCapacitiveInductiveFlag = iECPhasorEntity.RPhaseLagLead;
                outPutEntity.YPhaseCapacitiveInductiveFlag = iECPhasorEntity.YPhaseLagLead;
                outPutEntity.BPhaseCapacitiveInductiveFlag = iECPhasorEntity.BPhaseLagLead;

                outPutEntity.RPhaseNegativePowerFlag = iECPhasorEntity.RPhasekWDirection;
                outPutEntity.YPhaseNegativePowerFlag = iECPhasorEntity.YPhasekWDirection;
                outPutEntity.BPhaseNegativePowerFlag = iECPhasorEntity.BPhasekWDirection;
                outPutEntity.TotalkWDirection = iECPhasorEntity.BPhasekWDirection;


                outPutEntity.AngleYR = iECPhasorEntity.YPhaseAngleWithRPhase;
                outPutEntity.AngleBR = iECPhasorEntity.BPhaseAngleWithRPhase;
                outPutEntity.AngleBetweenTwo = iECPhasorEntity.AngleBWAnyPhasePresent;
                outPutEntity.RPhaseChannel = iECPhasorEntity.RPhaseChannel;
                outPutEntity.YPhaseChannel = iECPhasorEntity.YPhaseChannel;
                outPutEntity.BPhaseChannel = iECPhasorEntity.BPhaseChannel;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetPhasorEntity(IECPhasorEntity iECPhasorEntity)", ex);
            }

            return outPutEntity;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generalData"></param>
        /// <returns></returns>
        private List<DLMS650BillingEntity> GetBillingEntity(GeneralData generalData)
        {
            List<DLMS650BillingEntity> outputEntityList = new List<DLMS650BillingEntity>();
            try
            {
                DLMS650BillingEntity outputEntity;
                //Current billing
                if (generalData.CurrentBilling != null)
                {
                    IECBillingEntity inputEntity = generalData.CurrentBilling;
                    outputEntity = new DLMS650BillingEntity();
                    outputEntity.CumulativeEnergykWhTZ0 = CommonMapper.FormatIECData(inputEntity.CumulativeEnergyKWH);
                    outputEntity.CumulativeEnergykVAhTZ0 = CommonMapper.FormatIECData(inputEntity.CumulativeEnergyKVAH);
                    outputEntity.CumulativeEnergykvarhLag = CommonMapper.FormatIECData(inputEntity.CumulativeEnergyKVARHLag);
                    outputEntity.CumulativeEnergykvarhLead = CommonMapper.FormatIECData(inputEntity.CumulativeEnergyKVARHLead);
                    outputEntity.MDkWTZ0 = CommonMapper.FormatIECMD(inputEntity.CumulativeMD1);
                    outputEntity.MDkVATZ0 = CommonMapper.FormatIECMD(inputEntity.CumulativeMD2);
                    outputEntity.MDkWDateTimeTZ0 = Convert.ToInt64(inputEntity.CumulativeMD1TimeStamp);
                    outputEntity.MDkVADateTimeTZ0 = Convert.ToInt64(inputEntity.CumulativeMD2TimeStamp);
                    //outputEntity.SystemPowerFactorforBillingPeriod = inputEntity.AveragePowerFactor == null ? "0" : inputEntity.AveragePowerFactor;
                    outputEntity.SystemPowerFactorforBillingPeriod = string.IsNullOrEmpty(inputEntity.AveragePowerFactor) ? string.Empty : inputEntity.AveragePowerFactor;
                    outputEntity.DataIndex = 0;
                    outputEntity.BillingType = inputEntity.BillingResetType;
                    outputEntity.PowerOnDuration = null; // Story - 358810 - This value is not already set for 3 Phase Non DLMS, so it should be null
                    outputEntity.CumPowerOffDuration = null; // Story - 358810 - This value is not already set for 3 Phase Non DLMS, so it should be null
                    outputEntity.CumTamperCount = -1;// Story - 358810 - This value is not already set for 3 Phase Non DLMS, so it should be -1 in case not coming from meter
                    outputEntityList.Add(outputEntity);

                }
                //Billing History
                if (generalData.listHistoryBilling != null && generalData.listHistoryBilling.Count > 0)
                {
                    foreach (IECBillingEntity historyBilling in generalData.listHistoryBilling)
                    {
                        if (isCDFConverterCall && historyBilling.BillingResetType.Replace(" ", "") == BillingResetType.NoBilling.ToString().ToUpper())
                        {
                            continue;
                        }
                        outputEntity = new DLMS650BillingEntity();
                        outputEntity.DataIndex = generalData.listHistoryBilling.IndexOf(historyBilling) + 1;
                        outputEntity.CumulativeEnergykWhTZ0 = CommonMapper.FormatIECData(historyBilling.CumulativeEnergyKWH);
                        outputEntity.CumulativeEnergykVAhTZ0 = CommonMapper.FormatIECData(historyBilling.CumulativeEnergyKVAH);
                        outputEntity.CumulativeEnergykvarhLag = CommonMapper.FormatIECData(historyBilling.CumulativeEnergyKVARHLag);
                        outputEntity.CumulativeEnergykvarhLead = CommonMapper.FormatIECData(historyBilling.CumulativeEnergyKVARHLead);
                        outputEntity.MDkWTZ0 = CommonMapper.FormatIECMD(historyBilling.CumulativeMD1);
                        outputEntity.MDkVATZ0 = CommonMapper.FormatIECMD(historyBilling.CumulativeMD2);
                        outputEntity.MDkWDateTimeTZ0 = Convert.ToInt64(historyBilling.CumulativeMD1TimeStamp);
                        outputEntity.MDkVADateTimeTZ0 = Convert.ToInt64(historyBilling.CumulativeMD2TimeStamp);
                        //outputEntity.SystemPowerFactorforBillingPeriod = historyBilling.AveragePowerFactor;
                        outputEntity.SystemPowerFactorforBillingPeriod = string.IsNullOrEmpty(historyBilling.AveragePowerFactor) ? string.Empty : historyBilling.AveragePowerFactor;
                        outputEntity.BillingType = historyBilling.BillingResetType;
                        outputEntity.PowerOnDuration = null; // Story - 358810 - This value is not already set for 3 Phase Non DLMS, so it should be null
                        outputEntity.CumPowerOffDuration = null; // Story - 358810 - This value is not already set for 3 Phase Non DLMS, so it should be null
                        outputEntity.CumTamperCount = -1;// Story - 358810 - This value is not already set for 3 Phase Non DLMS, so it should be -1 in case not coming from meter

                        #region DataForCDFConverter
                        try
                        {
                            if (isCDFConverterCall)
                            {
                                outputEntity.MechanismForCDF = CommonBLL.GetEnumDescription(BillingResetType.Auto).ToUpper();
                                if (historyBilling.BillingResetType.Replace(" ", "") == BillingResetType.SManual.ToString().ToUpper())
                                {
                                    outputEntity.MechanismForCDF = CommonBLL.GetEnumDescription(BillingResetType.SManual).ToUpper();
                                }
                                else if (historyBilling.BillingResetType.Replace(" ", "") == BillingResetType.Manual.ToString().ToUpper())
                                {
                                    outputEntity.MechanismForCDF = CommonBLL.GetEnumDescription(BillingResetType.Manual).ToUpper();
                                }

                            }
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "GetBillingEntity(GeneralData generalData)", ex);
                        }
                        #endregion

                        outputEntityList.Add(outputEntity);
                    }

                }
                //Add tarrif data 
                foreach (DLMS650BillingEntity billingEntity in outputEntityList)
                {
                    //Current tarrif
                    if (billingEntity.DataIndex == 0)
                    {
                        if (generalData.CurrentTariff != null)
                        {
                            FillTarrifDetail(billingEntity, generalData.CurrentTariff);
                        }

                    }
                    else if (generalData.listHistoryTariff != null) //history tarrif 
                    {
                        FillTarrifDetail(billingEntity, generalData.listHistoryTariff[(int)(billingEntity.DataIndex) - 1]);
                    }

                    //To Get Billing TimeStamp.
                    if (generalData.CurrentBilling != null && billingEntity.DataIndex == 0)
                    {
                        billingEntity.BillingDate = generalData.CurrentGeneral.MeterDateTime;
                    }

                    //To Get Billing TimeStamp.
                    if (generalData.listHistoryTamper != null && billingEntity.DataIndex != 0)
                    {
                        FillBillingTimeStamp(billingEntity, generalData.listHistoryTamper[(int)(billingEntity.DataIndex) - 1]);
                    }


                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingEntity(GeneralData generalData)", ex);
            }

            return outputEntityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generalData"></param>
        /// <returns></returns>
        private List<DLMS650BillingEntity> GetBillingEntityForSPhase(GeneralData generalData)
        {
            List<DLMS650BillingEntity> outputEntityList = new List<DLMS650BillingEntity>();
            try
            {
                DLMS650BillingEntity outputEntity;
                //Current billing
                if (generalData.CurrentBilling != null)
                {
                    IECBillingEntity inputEntity = generalData.CurrentBilling;
                    outputEntity = new DLMS650BillingEntity();
                    outputEntity.CumulativeEnergykWhTZ0 = CommonMapper.FormatIECDataForSinglePhase(inputEntity.CumulativeEnergyKWH);
                    outputEntity.CumulativeEnergykVAhTZ0 = CommonMapper.FormatIECDataForSinglePhase(inputEntity.CumulativeEnergyKVAH);
                    outputEntity.CumulativeEnergykvarhLag = CommonMapper.FormatIECDataForSinglePhase(inputEntity.CumulativeEnergyKVARHLag);
                    outputEntity.CumulativeEnergykvarhLead = CommonMapper.FormatIECDataForSinglePhase(inputEntity.CumulativeEnergyKVARHLead);
                    outputEntity.MDkWTZ0 = CommonMapper.FormatIECDataForSinglePhase(inputEntity.CumulativeMD1);
                    outputEntity.MDkVATZ0 = CommonMapper.FormatIECDataForSinglePhase(inputEntity.CumulativeMD2);
                    outputEntity.MDkWDateTimeTZ0 = Convert.ToInt64(inputEntity.CumulativeMD1TimeStamp);
                    outputEntity.MDkVADateTimeTZ0 = Convert.ToInt64(inputEntity.CumulativeMD2TimeStamp);
                    // Power factor was saved as 0 if data is not coming, eventhough 0 is a valid value
                    //outputEntity.SystemPowerFactorforBillingPeriod = inputEntity.AveragePowerFactor == null ? "0" : inputEntity.AveragePowerFactor;
                    outputEntity.SystemPowerFactorforBillingPeriod = string.IsNullOrEmpty(inputEntity.AveragePowerFactor) ? string.Empty : inputEntity.AveragePowerFactor;
                    outputEntity.DataIndex = 0;
                    outputEntity.BillingType = inputEntity.BillingResetType;
                    outputEntity.PowerOnDuration = inputEntity.PowerOnHours;
                    outputEntity.PowerOffDuration = inputEntity.PowerOffHours;
                    outputEntity.PowerOnDurationDisplay = 2;
                    outputEntity.CumPowerOffDuration = inputEntity.PowerOffHours;
                    if (inputEntity.PowerOffHours == null || inputEntity.PowerOffHours == "")
                    {                     
                        outputEntity.CumPowerOffDuration = null;
                    }
                    // Story - 358810 - This value is not already set for 1 Phase Non DLMS, so it should be null
                    outputEntity.CumTamperCount = -1;// Story - 358810 - This value is not already set for 1 Phase Non DLMS, so it should be -1 in case not coming from meter
                    
                    outputEntityList.Add(outputEntity);

                }
                //Billing History
                if (generalData.listHistoryBilling != null && generalData.listHistoryBilling.Count > 0)
                {
                        foreach (IECBillingEntity historyBilling in generalData.listHistoryBilling)
                        {
                            if (isCDFConverterCall && historyBilling.BillingResetType.Replace(" ", "") == BillingResetType.NoBilling.ToString().ToUpper())
                            {
                                continue;
                            }
                            outputEntity = new DLMS650BillingEntity();
                            //SarkarA-code-change-start-20171204 // CESC
                            if (historyBilling == null)     
                            {
                                continue;
                            }
                            //SarkarA-code-change-end-20171204
                            outputEntity.DataIndex = generalData.listHistoryBilling.IndexOf(historyBilling) + 1;
                            outputEntity.CumulativeEnergykWhTZ0 = CommonMapper.FormatIECDataForSinglePhase(historyBilling.CumulativeEnergyKWH);
                            outputEntity.CumulativeEnergykVAhTZ0 = CommonMapper.FormatIECDataForSinglePhase(historyBilling.CumulativeEnergyKVAH);
                            outputEntity.CumulativeEnergykvarhLag = CommonMapper.FormatIECDataForSinglePhase(historyBilling.CumulativeEnergyKVARHLag);
                            outputEntity.CumulativeEnergykvarhLead = CommonMapper.FormatIECDataForSinglePhase(historyBilling.CumulativeEnergyKVARHLead);
                            outputEntity.MDkWTZ0 = CommonMapper.FormatIECDataForSinglePhase(historyBilling.CumulativeMD1);
                            outputEntity.MDkVATZ0 = CommonMapper.FormatIECDataForSinglePhase(historyBilling.CumulativeMD2);
                            outputEntity.MDkWDateTimeTZ0 = Convert.ToInt64(historyBilling.CumulativeMD1TimeStamp);
                            outputEntity.MDkVADateTimeTZ0 = Convert.ToInt64(historyBilling.CumulativeMD2TimeStamp);
                            // Power factor was saved as 0 if data is not coming, eventhough 0 is a valid value
                            //outputEntity.SystemPowerFactorforBillingPeriod = historyBilling.AveragePowerFactor == string.Empty ? "0" : historyBilling.AveragePowerFactor;
                            outputEntity.SystemPowerFactorforBillingPeriod = string.IsNullOrEmpty(historyBilling.AveragePowerFactor)? string.Empty : historyBilling.AveragePowerFactor;
                            outputEntity.BillingType = historyBilling.BillingResetType;
                            //Bug ID 502789
                            outputEntity.BillingDate = historyBilling.BillingDate;
                            outputEntity.PowerOnDuration = historyBilling.PowerOnHours;
                            outputEntity.PowerOnDurationDisplay = 6;
                            outputEntity.CumPowerOffDuration = historyBilling.PowerOffHours;

                            if (historyBilling.PowerOffHours == null || historyBilling.PowerOffHours == "")
                            {
                                // Mohsin 
                                // For CSPDCL
                                outputEntity.PowerOnDurationDisplay = 2;
                                outputEntity.CumPowerOffDuration = null;
                            }// Story - 358810 - This value is not already set for 1 Phase Non DLMS, so it should be null
                            outputEntity.CumTamperCount = -1;// Story - 358810 - This value is not already set for 1 Phase Non DLMS, so it should be -1 in case not coming from meter

                            #region DataForCDFConverter
                            try
                            {
                                if (isCDFConverterCall)
                                {
                                    outputEntity.MechanismForCDF = CommonBLL.GetEnumDescription(BillingResetType.Auto).ToUpper();
                                    if (historyBilling.BillingResetType.Replace(" ", "") == BillingResetType.SManual.ToString().ToUpper())
                                    {
                                        outputEntity.MechanismForCDF = CommonBLL.GetEnumDescription(BillingResetType.SManual).ToUpper();
                                    }
                                    else if (historyBilling.BillingResetType.Replace(" ", "") == BillingResetType.Manual.ToString().ToUpper())
                                    {
                                        outputEntity.MechanismForCDF = CommonBLL.GetEnumDescription(BillingResetType.Manual).ToUpper();
                                    }

                                }
                            }
                            catch (Exception ex)    //Exception log for catch block
                            {
                                logger.Log(LOGLEVELS.Error, "GetBillingEntityForSPhase(GeneralData generalData)", ex);
                            }
                            #endregion

                            outputEntityList.Add(outputEntity);
                    }

                }
                //Add tarrif data 
                int counter = 0;
                foreach (DLMS650BillingEntity billingEntity in outputEntityList)
                {
                    //Current tarrif
                    if (billingEntity.DataIndex == 0)
                    {
                        if (generalData.CurrentTariff != null)
                        {
                            FillTarrifDetailForSPhase(billingEntity, generalData.CurrentTariff);
                        }

                    }
                    else if (generalData.listHistoryTariff != null) //history tarrif 
                    {
                        FillTarrifDetailForSPhase(billingEntity, generalData.listHistoryTariff[(int)(billingEntity.DataIndex) - 1]);
                    }

                    //To Get Billing TimeStamp.
                    if (generalData.CurrentBilling != null && billingEntity.DataIndex == 0)
                    {
                        billingEntity.BillingDate = generalData.CurrentGeneral.MeterDateTime;
                    }
                    //To Get Billing TimeStamp.
                    //Bug ID 502789
                    else if (generalData.listHistoryBilling != null && billingEntity.DataIndex != 0 && generalData.listHistoryBilling.Count > counter && generalData.listHistoryBilling[counter] != null)
                    {
                        billingEntity.BillingDate = generalData.listHistoryBilling[counter].BillingDate;
                        counter++;
                    }

                    //To Get Billing TimeStamp.
                    //if (generalData.listHistoryTamper != null && billingEntity.DataIndex != 0)
                    //{
                    //    FillBillingTimeStamp(billingEntity, generalData.listHistoryTamper[(int)(billingEntity.DataIndex) - 1]);
                    //}


                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetBillingEntityForSPhase(GeneralData generalData)", ex);
            }

             return outputEntityList;
        }
        /// <summary>
        /// Fill Billing TimeStamp
        /// </summary>
        /// <param name="billingEntity"></param>
        /// <param name="tamperCounterGeneralEntity"></param>
        private void FillBillingTimeStamp(DLMS650BillingEntity billingEntity, TamperCounterGeneralEntity tamperCounterGeneralEntity)
        {
            billingEntity.BillingDate = tamperCounterGeneralEntity.BillingTimeStamp;
        }
        /// <summary>
        /// Add tarrif values to billing record 
        /// </summary>
        /// <param name="billingEntity"></param>
        /// <param name="tariffEntity"></param>
        private void FillTarrifDetail(DLMS650BillingEntity billingEntity, TariffEntity tariffEntity)
        {
            try
            {
                billingEntity.CumulativeEnergykWhTZ1 = CommonMapper.FormatIECData(tariffEntity.Tariff1_kWh);
                billingEntity.CumulativeEnergykWhTZ2 = CommonMapper.FormatIECData(tariffEntity.Tariff2_kWh);
                billingEntity.CumulativeEnergykWhTZ3 = CommonMapper.FormatIECData(tariffEntity.Tariff3_kWh);
                billingEntity.CumulativeEnergykWhTZ4 = CommonMapper.FormatIECData(tariffEntity.Tariff4_kWh);
                billingEntity.CumulativeEnergykWhTZ5 = CommonMapper.FormatIECData(tariffEntity.Tariff5_kWh);
                billingEntity.CumulativeEnergykWhTZ6 = CommonMapper.FormatIECData(tariffEntity.Tariff6_kWh);
                billingEntity.CumulativeEnergykWhTZ7 = CommonMapper.FormatIECData(tariffEntity.Tariff7_kWh);
                billingEntity.CumulativeEnergykWhTZ8 = CommonMapper.FormatIECData(tariffEntity.Tariff8_kWh);

                billingEntity.CumulativeEnergykVAhTZ1 = CommonMapper.FormatIECData(tariffEntity.Tariff1_kVAh);
                billingEntity.CumulativeEnergykVAhTZ2 = CommonMapper.FormatIECData(tariffEntity.Tariff2_kVAh);
                billingEntity.CumulativeEnergykVAhTZ3 = CommonMapper.FormatIECData(tariffEntity.Tariff3_kVAh);
                billingEntity.CumulativeEnergykVAhTZ4 = CommonMapper.FormatIECData(tariffEntity.Tariff4_kVAh);
                billingEntity.CumulativeEnergykVAhTZ5 = CommonMapper.FormatIECData(tariffEntity.Tariff5_kVAh);
                billingEntity.CumulativeEnergykVAhTZ6 = CommonMapper.FormatIECData(tariffEntity.Tariff6_kVAh);
                billingEntity.CumulativeEnergykVAhTZ7 = CommonMapper.FormatIECData(tariffEntity.Tariff7_kVAh);
                billingEntity.CumulativeEnergykVAhTZ8 = CommonMapper.FormatIECData(tariffEntity.Tariff8_kVAh);


                billingEntity.MDkWTZ1 = CommonMapper.FormatIECMD(tariffEntity.Tariff1_MD1);
                billingEntity.MDkWTZ2 = CommonMapper.FormatIECMD(tariffEntity.Tariff2_MD1);
                billingEntity.MDkWTZ3 = CommonMapper.FormatIECMD(tariffEntity.Tariff3_MD1);
                billingEntity.MDkWTZ4 = CommonMapper.FormatIECMD(tariffEntity.Tariff4_MD1);
                billingEntity.MDkWTZ5 = CommonMapper.FormatIECMD(tariffEntity.Tariff5_MD1);
                billingEntity.MDkWTZ6 = CommonMapper.FormatIECMD(tariffEntity.Tariff6_MD1);
                billingEntity.MDkWTZ7 = CommonMapper.FormatIECMD(tariffEntity.Tariff7_MD1);
                billingEntity.MDkWTZ8 = CommonMapper.FormatIECMD(tariffEntity.Tariff8_MD1);

                billingEntity.MDkWDateTimeTZ1 = tariffEntity.Tariff1_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ2 = tariffEntity.Tariff2_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ3 = tariffEntity.Tariff3_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ4 = tariffEntity.Tariff4_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ5 = tariffEntity.Tariff5_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ6 = tariffEntity.Tariff6_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ7 = tariffEntity.Tariff7_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ8 = tariffEntity.Tariff8_MD1_TimeStamp;

                billingEntity.MDkVATZ1 = CommonMapper.FormatIECMD(tariffEntity.Tariff1_MD2);
                billingEntity.MDkVATZ2 = CommonMapper.FormatIECMD(tariffEntity.Tariff2_MD2);
                billingEntity.MDkVATZ3 = CommonMapper.FormatIECMD(tariffEntity.Tariff3_MD2);
                billingEntity.MDkVATZ4 = CommonMapper.FormatIECMD(tariffEntity.Tariff4_MD2);
                billingEntity.MDkVATZ5 = CommonMapper.FormatIECMD(tariffEntity.Tariff5_MD2);
                billingEntity.MDkVATZ6 = CommonMapper.FormatIECMD(tariffEntity.Tariff6_MD2);
                billingEntity.MDkVATZ7 = CommonMapper.FormatIECMD(tariffEntity.Tariff7_MD2);
                billingEntity.MDkVATZ8 = CommonMapper.FormatIECMD(tariffEntity.Tariff8_MD2);

                billingEntity.MDkVADateTimeTZ1 = tariffEntity.Tariff1_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ2 = tariffEntity.Tariff2_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ3 = tariffEntity.Tariff3_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ4 = tariffEntity.Tariff4_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ5 = tariffEntity.Tariff5_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ6 = tariffEntity.Tariff6_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ7 = tariffEntity.Tariff7_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ8 = tariffEntity.Tariff8_MD2_TimeStamp;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillTarrifDetail(DLMS650BillingEntity billingEntity, TariffEntity tariffEntity)", ex);
            }
        }

        /// <summary>
        /// Add tarrif values to billing record 
        /// </summary>
        /// <param name="billingEntity"></param>
        /// <param name="tariffEntity"></param>
        private void FillTarrifDetailForSPhase(DLMS650BillingEntity billingEntity, TariffEntity tariffEntity)
        {
            try
            {
                billingEntity.CumulativeEnergykWhTZ1 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff1_kWh);
                billingEntity.CumulativeEnergykWhTZ2 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff2_kWh);
                billingEntity.CumulativeEnergykWhTZ3 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff3_kWh);
                billingEntity.CumulativeEnergykWhTZ4 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff4_kWh);
                billingEntity.CumulativeEnergykWhTZ5 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff5_kWh);
                billingEntity.CumulativeEnergykWhTZ6 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff6_kWh);
                billingEntity.CumulativeEnergykWhTZ7 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff7_kWh);
                billingEntity.CumulativeEnergykWhTZ8 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff8_kWh);

                billingEntity.CumulativeEnergykVAhTZ1 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff1_kVAh);
                billingEntity.CumulativeEnergykVAhTZ2 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff2_kVAh);
                billingEntity.CumulativeEnergykVAhTZ3 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff3_kVAh);
                billingEntity.CumulativeEnergykVAhTZ4 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff4_kVAh);
                billingEntity.CumulativeEnergykVAhTZ5 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff5_kVAh);
                billingEntity.CumulativeEnergykVAhTZ6 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff6_kVAh);
                billingEntity.CumulativeEnergykVAhTZ7 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff7_kVAh);
                billingEntity.CumulativeEnergykVAhTZ8 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff8_kVAh);


                billingEntity.MDkWTZ1 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff1_MD1);
                billingEntity.MDkWTZ2 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff2_MD1);
                billingEntity.MDkWTZ3 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff3_MD1);
                billingEntity.MDkWTZ4 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff4_MD1);
                billingEntity.MDkWTZ5 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff5_MD1);
                billingEntity.MDkWTZ6 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff6_MD1);
                billingEntity.MDkWTZ7 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff7_MD1);
                billingEntity.MDkWTZ8 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff8_MD1);

                billingEntity.MDkWDateTimeTZ1 = tariffEntity.Tariff1_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ2 = tariffEntity.Tariff2_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ3 = tariffEntity.Tariff3_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ4 = tariffEntity.Tariff4_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ5 = tariffEntity.Tariff5_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ6 = tariffEntity.Tariff6_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ7 = tariffEntity.Tariff7_MD1_TimeStamp;
                billingEntity.MDkWDateTimeTZ8 = tariffEntity.Tariff8_MD1_TimeStamp;

                billingEntity.MDkVATZ1 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff1_MD2);
                billingEntity.MDkVATZ2 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff2_MD2);
                billingEntity.MDkVATZ3 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff3_MD2);
                billingEntity.MDkVATZ4 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff4_MD2);
                billingEntity.MDkVATZ5 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff5_MD2);
                billingEntity.MDkVATZ6 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff6_MD2);
                billingEntity.MDkVATZ7 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff7_MD2);
                billingEntity.MDkVATZ8 = CommonMapper.FormatIECDataForSinglePhase(tariffEntity.Tariff8_MD2);

                billingEntity.MDkVADateTimeTZ1 = tariffEntity.Tariff1_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ2 = tariffEntity.Tariff2_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ3 = tariffEntity.Tariff3_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ4 = tariffEntity.Tariff4_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ5 = tariffEntity.Tariff5_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ6 = tariffEntity.Tariff6_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ7 = tariffEntity.Tariff7_MD2_TimeStamp;
                billingEntity.MDkVADateTimeTZ8 = tariffEntity.Tariff8_MD2_TimeStamp;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillTarrifDetailForSPhase(DLMS650BillingEntity billingEntity, TariffEntity tariffEntity)", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iECLoadSurveyEntity"></param>
        /// <returns></returns>
        private LoadSurveyParameterEntity GetLoadSurveyParameterEntity(IECLoadSurveyEntity iECLoadSurveyEntity)
        {
            LoadSurveyParameterEntity loadSurevyParameterEntity = new LoadSurveyParameterEntity();

            loadSurevyParameterEntity.ColumnsNames += "realTimeClockDateandTime";
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.RPhaseCurrent))
            {
                loadSurevyParameterEntity.ColumnsNames += ",RPhaseCurrent";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.YPhaseCurrent))
            {
                loadSurevyParameterEntity.ColumnsNames += ",YPhaseCurrent";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.BPhaseCurrent))
            {
                loadSurevyParameterEntity.ColumnsNames += ",BPhaseCurrent";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.RPhaseVoltage))
            {
                loadSurevyParameterEntity.ColumnsNames += ",RPhaseVoltage";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.YPhaseVoltage))
            {
                loadSurevyParameterEntity.ColumnsNames += ",YPhaseVoltage";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.BPhaseVoltage))
            {
                loadSurevyParameterEntity.ColumnsNames += ",BPhaseVoltage";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKW))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykWh";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKVA))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykVAh";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKVARLag))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykvarhlag";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKVARLead))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykvarhlead";
            }

            return loadSurevyParameterEntity;
        }
        private LoadSurveyParameterEntity GetLoadSurveyParameterEntitySPhase(IECLoadSurveyEntity iECLoadSurveyEntity)
        {
            LoadSurveyParameterEntity loadSurevyParameterEntity = new LoadSurveyParameterEntity();

            loadSurevyParameterEntity.ColumnsNames += "realTimeClockDateandTime";
            //if (!string.IsNullOrEmpty(iECLoadSurveyEntity.RPhaseCurrent))
            //{
            //    loadSurevyParameterEntity.ColumnsNames += ",RPhaseCurrent";
            //}
            //if (!string.IsNullOrEmpty(iECLoadSurveyEntity.YPhaseCurrent))
            //{
            //    loadSurevyParameterEntity.ColumnsNames += ",YPhaseCurrent";
            //}
            //if (!string.IsNullOrEmpty(iECLoadSurveyEntity.BPhaseCurrent))
            //{
            //    loadSurevyParameterEntity.ColumnsNames += ",BPhaseCurrent";
            //}
            //if (!string.IsNullOrEmpty(iECLoadSurveyEntity.RPhaseVoltage))
            //{
            //    loadSurevyParameterEntity.ColumnsNames += ",RPhaseVoltage";
            //}
            //if (!string.IsNullOrEmpty(iECLoadSurveyEntity.YPhaseVoltage))
            //{
            //    loadSurevyParameterEntity.ColumnsNames += ",YPhaseVoltage";
            //}
            //if (!string.IsNullOrEmpty(iECLoadSurveyEntity.BPhaseVoltage))
            //{
            //    loadSurevyParameterEntity.ColumnsNames += ",BPhaseVoltage";
            //}

            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.AvgVoltage))
            {
                loadSurevyParameterEntity.ColumnsNames += ",averageVoltage";
            }

            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.AvgCurrent))
            {
                loadSurevyParameterEntity.ColumnsNames += ",averageCurrent";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.AvgNeutralCurrent))
            {
                loadSurevyParameterEntity.ColumnsNames += ",AvgNeutralCurrent";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKW))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykWh";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKVA))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykVAh";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKVARLag))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykvarhlag";
            }
            if (!string.IsNullOrEmpty(iECLoadSurveyEntity.DemandKVARLead))
            {
                loadSurevyParameterEntity.ColumnsNames += ",blockEnergykvarhlead";
            }

            return loadSurevyParameterEntity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DLMS650LoadSurveyEntity> GetLoadSurveyEntity(List<LoadSurveyData> inpuData, bool isUpload)
        {
            List<DLMS650LoadSurveyEntity> loadSurveyEntityList = new List<DLMS650LoadSurveyEntity>();
            try
            {
                LoadSurveyData loadSurveyData = inpuData[0];
                DLMS650LoadSurveyEntity dlmsLoadSurveyEntity;
                foreach (IECLoadSurveyEntity iecLoadSurveyEntity in loadSurveyData.LoadSurvey)
                {
                    dlmsLoadSurveyEntity = new DLMS650LoadSurveyEntity();

                    dlmsLoadSurveyEntity.MDIntervalPeriod = iecLoadSurveyEntity.MDIntervalPeriod;
                    dlmsLoadSurveyEntity.IsDLMS = 0;

                    dlmsLoadSurveyEntity.RPhaseVoltage = iecLoadSurveyEntity.RPhaseVoltage;
                    dlmsLoadSurveyEntity.YPhaseVoltage = iecLoadSurveyEntity.YPhaseVoltage;
                    dlmsLoadSurveyEntity.BPhaseVoltage = iecLoadSurveyEntity.BPhaseVoltage;

                    dlmsLoadSurveyEntity.RPhaseCurrent = iecLoadSurveyEntity.RPhaseCurrent;
                    dlmsLoadSurveyEntity.YPhaseCurrent = iecLoadSurveyEntity.YPhaseCurrent;
                    dlmsLoadSurveyEntity.BPhaseCurrent = iecLoadSurveyEntity.BPhaseCurrent;

                    dlmsLoadSurveyEntity.PowerFactor = iecLoadSurveyEntity.PowerFactor;

                    dlmsLoadSurveyEntity.RealTimeClockDateandTime = iecLoadSurveyEntity.LoadSurveyDateTime;
                    dlmsLoadSurveyEntity.TamperStatus = iecLoadSurveyEntity.TamperStatus;

                    //IN IEC Demand comes from meter and we need to calculate energy form demand.
                    //While in DLMS energy comes from meter and we calculate demand .
                    // Here we are putting demand value in energy column , later on while displaying data we will apply logic for demand to energy conversion .
                    int div = 1;
                    if (iecLoadSurveyEntity.MDIntervalPeriod == 15)
                        div = 4;
                    if (iecLoadSurveyEntity.MDIntervalPeriod == 30)
                        div = 2;
                    decimal num = 0;
                    if (isUpload)
                    {
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKW))
                        {
                            dlmsLoadSurveyEntity.BlockEnergykWh = iecLoadSurveyEntity.DemandKW + "*kWh";
                        }
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVA))
                        {
                            dlmsLoadSurveyEntity.BlockEnergykVAh = iecLoadSurveyEntity.DemandKVA + "*kVAh";
                        }
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLag))
                        {
                            dlmsLoadSurveyEntity.BlockEnergykvarhlag = iecLoadSurveyEntity.DemandKVARLag + "*kvarh";
                        }
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLead))
                        {
                            dlmsLoadSurveyEntity.BlockEnergykvarhlead = iecLoadSurveyEntity.DemandKVARLead + "*kvarh";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKW))
                        {
                            num = 0;
                            if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKW))
                                num = decimal.Parse(iecLoadSurveyEntity.DemandKW) / div;
                            dlmsLoadSurveyEntity.BlockEnergykWh = num.TruncateToPrecision(3) + "*kWh";
                        }
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVA))
                        {
                            num = 0;
                            if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVA))
                                num = decimal.Parse(iecLoadSurveyEntity.DemandKVA) / div;
                            dlmsLoadSurveyEntity.BlockEnergykVAh = num.TruncateToPrecision(3) + "*kVAh";
                        }
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLag))
                        {
                            num = 0;
                            if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLag))
                                num = decimal.Parse(iecLoadSurveyEntity.DemandKVARLag) / div;
                            dlmsLoadSurveyEntity.BlockEnergykvarhlag = num.TruncateToPrecision(3) + "*kvarh";
                        }
                        if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLead))
                        {
                            num = 0;
                            if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLead))
                                num = decimal.Parse(iecLoadSurveyEntity.DemandKVARLead) / div;
                            dlmsLoadSurveyEntity.BlockEnergykvarhlead = num.TruncateToPrecision(3) + "*kvarh";
                        }
                    }
                    loadSurveyEntityList.Add(dlmsLoadSurveyEntity);

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadSurveyEntity(List<LoadSurveyData> inpuData, bool isUpload)", ex);
            }

            return loadSurveyEntityList;
        }

        private List<DLMS650LoadSurveyEntity> GetLoadSurveyEntitySPhase(List<LoadSurveyData> inpuData, bool isUpload)
        {
            List<DLMS650LoadSurveyEntity> loadSurveyEntityList = new List<DLMS650LoadSurveyEntity>();
            try
            {
                LoadSurveyData loadSurveyData = inpuData[0];
                DLMS650LoadSurveyEntity dlmsLoadSurveyEntity;
                foreach (IECLoadSurveyEntity iecLoadSurveyEntity in loadSurveyData.LoadSurvey)
                {
                    dlmsLoadSurveyEntity = new DLMS650LoadSurveyEntity();

                    dlmsLoadSurveyEntity.MDIntervalPeriod = iecLoadSurveyEntity.MDIntervalPeriod;
                    dlmsLoadSurveyEntity.IsDLMS = 0;
                    dlmsLoadSurveyEntity.AverageVoltage = iecLoadSurveyEntity.AvgVoltage;
                    //dlmsLoadSurveyEntity.RPhaseVoltage = iecLoadSurveyEntity.RPhaseVoltage;
                    //dlmsLoadSurveyEntity.YPhaseVoltage = iecLoadSurveyEntity.YPhaseVoltage;
                    //dlmsLoadSurveyEntity.BPhaseVoltage = iecLoadSurveyEntity.BPhaseVoltage;

                    dlmsLoadSurveyEntity.AverageCurrent = iecLoadSurveyEntity.AvgCurrent;
                    //dlmsLoadSurveyEntity.RPhaseCurrent = iecLoadSurveyEntity.RPhaseCurrent;
                    //dlmsLoadSurveyEntity.YPhaseCurrent = iecLoadSurveyEntity.YPhaseCurrent;
                    //dlmsLoadSurveyEntity.BPhaseCurrent = iecLoadSurveyEntity.BPhaseCurrent;

                    dlmsLoadSurveyEntity.AvgNeuCurrent = iecLoadSurveyEntity.AvgNeutralCurrent;

                    dlmsLoadSurveyEntity.PowerFactor = iecLoadSurveyEntity.PowerFactor;

                    dlmsLoadSurveyEntity.RealTimeClockDateandTime = iecLoadSurveyEntity.LoadSurveyDateTime;
                    //dlmsLoadSurveyEntity.TamperStatus = iecLoadSurveyEntity.TamperStatus;

                    //IN IEC Demand comes from meter and we need to calculate energy form demand.
                    //While in DLMS energy comes from meter and we calculate demand .
                    // Here we are putting demand value in energy column , later on while displaying data we will apply logic for demand to energy conversion .

                    if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKW))
                    {
                        dlmsLoadSurveyEntity.BlockEnergykWh = iecLoadSurveyEntity.DemandKW + "*kWh";
                    }
                    if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVA))
                    {
                        dlmsLoadSurveyEntity.BlockEnergykVAh = iecLoadSurveyEntity.DemandKVA + "*kVAh";
                    }
                    if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLag))
                    {
                        dlmsLoadSurveyEntity.BlockEnergykvarhlag = iecLoadSurveyEntity.DemandKVARLag + "*kvarh";
                    }
                    if (!string.IsNullOrEmpty(iecLoadSurveyEntity.DemandKVARLead))
                    {
                        dlmsLoadSurveyEntity.BlockEnergykvarhlead = iecLoadSurveyEntity.DemandKVARLead + "*kvarh";
                    }


                    loadSurveyEntityList.Add(dlmsLoadSurveyEntity);

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetLoadSurveyEntitySPhase(List<LoadSurveyData> inpuData, bool isUpload)", ex);
            }

            return loadSurveyEntityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterDataEntity"></param>
        /// <returns></returns>
        private MeterDataEntity GetMeterDataEntity(IECMeterDataEntity meterDataEntity)
        {
            MeterDataEntity meterData = new MeterDataEntity();
            meterData.MeterID = meterDataEntity.MeterID;
            meterData.ReadingDateTime = meterDataEntity.ReadingDateTime;
            meterData.UploadingDateTime = meterDataEntity.UploadingDateTime;
            return meterData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="namePlateDetailEntity"></param>
        /// <param name="generalData"></param>
        /// <returns></returns>
        private DLMS650NamePlateDetailsEntity GetGeneralEntity(GeneralEntity generalEntity)
        {
            string defaultValue = "-----";
            DLMS650NamePlateDetailsEntity namePlateEntity = new DLMS650NamePlateDetailsEntity();
            namePlateEntity.Manufacturername = defaultValue;
            namePlateEntity.Meteryearofmanufacture = generalEntity.MeterManufacturing;
            namePlateEntity.MeterSerialNumber = generalEntity.MeterID;
            namePlateEntity.InternalCTratio = defaultValue;
            namePlateEntity.VoltageRating = defaultValue;
            namePlateEntity.BasicCurrentRating = defaultValue;
            namePlateEntity.Category = defaultValue;
            namePlateEntity.FirmwareVersionformeter = namePlateEntity.InternalFirmwareVersion = generalEntity.FirmwareVersion.TrimStart('0');
            namePlateEntity.MeterModelNo = NamePlateConstants.RubyE250Value.ToString();
            namePlateEntity.Metertype = MeterType.ThreePhaseFourWire;
            namePlateEntity.MeterDataType = "LTCT";
            namePlateEntity.InternalPTratio = defaultValue;
            return namePlateEntity;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="namePlateDetailEntity"></param>
        /// <param name="generalData"></param>
        /// <returns></returns>
        private DLMS650NamePlateDetailsEntity GetGeneralEntityForSPhase(GeneralEntity generalEntity)
        {
            string defaultValue = "-----";
            DLMS650NamePlateDetailsEntity namePlateEntity = new DLMS650NamePlateDetailsEntity();
            namePlateEntity.Meteryearofmanufacture = generalEntity.MeterManufacturing;
            namePlateEntity.MeterSerialNumber = generalEntity.MeterID;
            namePlateEntity.FirmwareVersionformeter = namePlateEntity.InternalFirmwareVersion = generalEntity.FirmwareVersion.TrimStart('0');
            namePlateEntity.VoltageRating = generalEntity.VoltageRating;
            namePlateEntity.BasicCurrentRating = generalEntity.CurrentRating;
            namePlateEntity.AccuracyClass = generalEntity.AccuracyClass;

            namePlateEntity.Manufacturername = "Cabcon";
            namePlateEntity.Meteryearofmanufacture = generalEntity.MeterManufacturing;
            namePlateEntity.MeterSerialNumber = generalEntity.MeterID;
            namePlateEntity.InternalCTratio = defaultValue;
            namePlateEntity.Category = defaultValue;
            namePlateEntity.MeterModelNo = NamePlateConstants.RubyE150Value.ToString(); // Story - 347720
            namePlateEntity.Metertype = MeterType.OnePhaseTwoWire;
            namePlateEntity.MeterDataType = "LTCT";
            namePlateEntity.InternalPTratio = defaultValue;
            namePlateEntity.MeterConstant = "3200 Impluse/kwh"; //Impluse/kwh unit PGVCL

            return namePlateEntity;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instantPowerEntity"></param>
        /// <param name="generalEntity"></param>
        /// <returns></returns>
        private List<DLMS650InstantaneousEntity> GetInstantEntity(InstantPowerEntity instantPowerEntity, GeneralEntity generalEntity
            , IECBillingEntity billingEntity)
        {
            List<DLMS650InstantaneousEntity> instantEntityList = new List<DLMS650InstantaneousEntity>();
            DLMS650InstantaneousEntity instantEntity = new DLMS650InstantaneousEntity();
            meterConfigParser = new ConfigurationParser(true);
            try
            {
                instantEntityList.Add(GetInstangtEntity(9, DateUtility.LongToStringDateTimeWithSecFormat(instantPowerEntity.MeterDateTime) + string.Concat("*" + ConfigInfo.DateFormat(), " HH:MM:SS"), 1));
                //Volatge
                instantEntityList.Add(GetInstangtEntity(10, CommonMapper.FormatIECData(instantPowerEntity.CurrentRPhase), 2));
                instantEntityList.Add(GetInstangtEntity(11, CommonMapper.FormatIECData(instantPowerEntity.CurrentYPhase), 3));
                instantEntityList.Add(GetInstangtEntity(12, CommonMapper.FormatIECData(instantPowerEntity.CurrentBPhase), 4));
                //Current
                instantEntityList.Add(GetInstangtEntity(13, CommonMapper.FormatIECData(instantPowerEntity.VoltageRPhase), 5));
                instantEntityList.Add(GetInstangtEntity(14, CommonMapper.FormatIECData(instantPowerEntity.VoltageYPhase), 6));
                instantEntityList.Add(GetInstangtEntity(15, CommonMapper.FormatIECData(instantPowerEntity.VoltageBPhase), 7));
                //Power factor
                // instantEntityList.Add(GetInstangtEntity(16, instantPowerEntity.PowerFactorRPhase.Replace("Pf",""), 8));
                //instantEntityList.Add(GetInstangtEntity(17, instantPowerEntity.PowerFactorYPhase.Replace("Pf",""), 9));
                //instantEntityList.Add(GetInstangtEntity(18, instantPowerEntity.PowerFactorBPhase.Replace("Pf",""), 10));
                //instantEntityList.Add(GetInstangtEntity(19, instantPowerEntity.TotalPowerFactor.Replace("Pf",""), 11));
                //Frequency
                instantEntityList.Add(GetInstangtEntity(20, CommonMapper.FormatIECData(instantPowerEntity.Frequency), 12));
                //Apparent , active , reactive power .
                instantEntityList.Add(GetInstangtEntity(21, CommonMapper.FormatIECData(instantPowerEntity.InstantApparentPower), 13));
                instantEntityList.Add(GetInstangtEntity(22, CommonMapper.FormatIECData(instantPowerEntity.InstantActivepower), 14));
                try
                {
                    if (instantPowerEntity.InstantReactiveLeadPower.Contains("*") && instantPowerEntity.InstantReactiveLagPower.Contains("*"))
                    {
                        if (Convert.ToDecimal(instantPowerEntity.InstantReactiveLeadPower.Split('*')[0]) > Convert.ToDecimal(instantPowerEntity.InstantReactiveLagPower.Split('*')[0]))
                        {
                            instantEntityList.Add(GetInstangtEntity(24, "-" + CommonMapper.FormatIECData(instantPowerEntity.InstantReactiveLeadPower), 16));
                        }
                        else
                        {
                            instantEntityList.Add(GetInstangtEntity(24, CommonMapper.FormatIECData(instantPowerEntity.InstantReactiveLagPower), 15));
                        }
                    }
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "GetInstantEntity(InstantPowerEntity instantPowerEntity, GeneralEntity generalEntity, IECBillingEntity billingEntity)", ex);
                }
                //DLMS650InstantaneousEntity instantEntityForLagLaed = GetInstangtEntity(24, CommonMapper.FormatIECData(instantPowerEntity.InstantReactiveLagPower), 15);
                //instantEntityForLagLaed.InstantPowerColumnName = "Signed Reactive Power kvar Lag";
                //instantEntityList.Add(instantEntityForLagLaed);

                //instantEntityForLagLaed = GetInstangtEntity(24, CommonMapper.FormatIECData(instantPowerEntity.InstantReactiveLeadPower), 16);
                //instantEntityForLagLaed.InstantPowerColumnName = "Signed Reactive Power kvar Lead";
                //instantEntityList.Add(instantEntityForLagLaed);

                //instantEntityList.Add(GetInstangtEntity(25, "0", 16));
                //instantEntityList.Add(GetInstangtEntity(26, "0", 17));
                //instantEntityList.Add(GetInstangtEntity(27, "0", 18)); 

                instantEntityList.Add(GetInstangtEntity(80, generalEntity.TotalPowerOnHours.TrimStart('0'), 37));

                //Billing programming and readout counter 
                instantEntityList.Add(GetInstangtEntity(28, Convert.ToInt32(generalEntity.MDResetCounter).ToString(), 19));
                instantEntityList.Add(GetInstangtEntity(29, Convert.ToInt32(generalEntity.ProgrammingCounter).ToString(), 20));
                instantEntityList.Add(GetInstangtEntity(82, Convert.ToInt32(generalEntity.ReadoutCounter).ToString(), 21));

                //billing date
                // instantEntityList.Add(GetInstangtEntity(30, "0", 22));

                //active reactive ,apparent energy .
                instantEntityList.Add(GetInstangtEntity(31, CommonMapper.FormatIECData(generalEntity.TotalActiveEnergy), 23));
                instantEntityList.Add(GetInstangtEntity(32, CommonMapper.FormatIECData(billingEntity.CumulativeEnergyKVARHLag), 24));
                instantEntityList.Add(GetInstangtEntity(33, CommonMapper.FormatIECData(billingEntity.CumulativeEnergyKVARHLead), 25));
                instantEntityList.Add(GetInstangtEntity(34, CommonMapper.FormatIECData(billingEntity.CumulativeEnergyKVAH), 26));

                //Current month MD1 and MD1 datetime
                instantEntityList.Add(GetInstangtEntity(35, CommonMapper.FormatIECMD(billingEntity.CumulativeMD1), 27));
                if (isCDFConverterCall)
                {
                    instantEntityList.Add(GetInstangtEntity(36, billingEntity.CumulativeMD1TimeStamp, 28));
                }
                else
                {
                    instantEntityList.Add(GetInstangtEntity(36, DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(billingEntity.CumulativeMD1TimeStamp))
                                                                + string.Concat("*" + ConfigInfo.DateFormat(), " HH:MM"), 28));
                }

                //Current month MD2 and MD2 datetime
                instantEntityList.Add(GetInstangtEntity(37, CommonMapper.FormatIECMD(billingEntity.CumulativeMD2), 29));
                if (isCDFConverterCall)
                {
                    instantEntityList.Add(GetInstangtEntity(38, billingEntity.CumulativeMD2TimeStamp, 30));
                }
                else
                {
                    instantEntityList.Add(GetInstangtEntity(38, DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(billingEntity.CumulativeMD2TimeStamp))
                                                                + string.Concat("*" + ConfigInfo.DateFormat(), " HH:MM"), 30));
                }

                //cumulative MD1 MD2
                instantEntityList.Add(GetInstangtEntity(39, CommonMapper.FormatIECMD(generalEntity.CumulativeMD1), 31));
                instantEntityList.Add(GetInstangtEntity(40, CommonMapper.FormatIECMD(generalEntity.CumulativeMD2), 32));


                //Rising demand AND elasped times 
                instantEntityList.Add(GetInstangtEntity(71, CommonMapper.FormatIECMD(generalEntity.RisingDemandKW), 33));
                instantEntityList.Add(GetInstangtEntity(72, generalEntity.ElapsedTimeKW.TrimStart('0'), 34));
                instantEntityList.Add(GetInstangtEntity(73, CommonMapper.FormatIECMD(generalEntity.RisingDemandKVA), 35));
                instantEntityList.Add(GetInstangtEntity(74, generalEntity.ElapsedTimeKVA.TrimStart('0'), 36));                

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstantEntity(InstantPowerEntity instantPowerEntity, GeneralEntity generalEntity, IECBillingEntity billingEntity)", ex);
            }

            return instantEntityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instantPowerEntity"></param>
        /// <param name="generalEntity"></param>
        /// <returns></returns>
        private List<DLMS650InstantaneousEntity> GetInstantEntityForSPhase(InstantPowerEntity instantPowerEntity, GeneralEntity generalEntity
            , IECBillingEntity billingEntity)
        {
            List<DLMS650InstantaneousEntity> instantEntityList = new List<DLMS650InstantaneousEntity>();
            DLMS650InstantaneousEntity instantEntity = new DLMS650InstantaneousEntity();
            meterConfigParser = new ConfigurationParser(true);
            try
            {
                if (instantPowerEntity.MeterDateAndTime != String.Empty)
                {
                    // Story - 349654 - format for the year is changed as it is coming in 2 digits, 2000 is added as meter can be valid for 5 year only
                    string datetime = instantPowerEntity.MeterDateAndTime;
                    string[] datetimeArr = datetime.Split('/');
                    if (datetimeArr.Length > 2)
                    {
                        datetimeArr[2] = (Convert.ToInt64(datetimeArr[2].Substring(0, 2)) + 2000).ToString() + " " + datetimeArr[2].Substring(3, 8);
                        datetime = datetimeArr[0] + "/" + datetimeArr[1] + "/" + datetimeArr[2];
                    }
                    instantEntityList.Add(GetInstangtEntity(9, datetime + string.Concat("*" + ConfigInfo.DateFormat(), " HH:MM:SS"), 1));
                }
                if (instantPowerEntity.PhaseVoltage != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(172, (instantPowerEntity.PhaseVoltage).Insert(((instantPowerEntity.PhaseVoltage.Length) - 1), "*"), 2));
                if (instantPowerEntity.PhaseCurrent != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(173, (instantPowerEntity.PhaseCurrent).Insert(((instantPowerEntity.PhaseCurrent.Length) - 1), "*"), 3));
                if (instantPowerEntity.NeutralCurrent != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(174, (instantPowerEntity.NeutralCurrent).Insert(((instantPowerEntity.NeutralCurrent.Length) - 1), "*"), 4));
                if (instantPowerEntity.InstantApparentPower != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(21, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.InstantApparentPower), 5));
                if (instantPowerEntity.ActivePowerKW != String.Empty)
                {
                    DLMS650InstantaneousEntity activePower = GetInstangtEntity(23, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.ActivePowerKW), 6);
                    if (activePower.InstantPowerColumnName == "Signed Active Power - kW (+Forward;-Reverse)")
                        activePower.InstantPowerColumnName = "Phase Power - kW"; // Story - 349654 - All description column value unit should start with small letters
                    instantEntityList.Add(activePower);
                }
                if (instantPowerEntity.NeutralPowerKW != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1033, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.NeutralPowerKW), 7));
                if (instantPowerEntity.TotalPowerOnMinutes != String.Empty)
                {
                    string powerOnHours = (instantPowerEntity.TotalPowerOnMinutes).Substring(0, ((instantPowerEntity.TotalPowerOnMinutes.Length) - 1));
                    TimeSpan span = TimeSpan.FromMinutes(Convert.ToDouble(powerOnHours));
                    powerOnHours = string.Concat(span.Days.ToString("00"), " : ", span.Hours.ToString("00"), " : ", span.Minutes.ToString("00"), "*dd : hh : mm"); //span.ToString(@"hh\:mm\:ss");
                    instantEntityList.Add(GetInstangtEntity(196, powerOnHours, 8));
                }
                //instantEntityList.Add(GetInstangtEntity(82, Convert.ToInt32(instantPowerEntity.ReadoutCounter).ToString(), 9));
                // Story - 349654 - OBIS code for Meter Readout Counter is not displaying correctly
                if (instantPowerEntity.ReadoutCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(226, Convert.ToInt32(instantPowerEntity.ReadoutCounter).ToString(), 9));// Story - 349654 - Meter Readout Counter OBIS code was not displaying, mapping has been corrected
                if (instantPowerEntity.MDResetCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(28, Convert.ToInt32(instantPowerEntity.MDResetCounter).ToString(), 10));
                if (instantPowerEntity.ProgrammingCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1029, Convert.ToInt32(instantPowerEntity.ProgrammingCounter).ToString(), 11));
                if (instantPowerEntity.TamperResetCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1030, Convert.ToInt32(instantPowerEntity.TamperResetCounter).ToString(), 12));
                if (instantPowerEntity.CumulativeEnergyKWh != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(31, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.CumulativeEnergyKWh), 13));
                if (instantPowerEntity.FraudEnergy != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1060, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.FraudEnergy), 43)); // User Story 464096  
                if (instantPowerEntity.LegalEnergy != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1070, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.LegalEnergy), 46));

                if (instantPowerEntity.LineFrequency != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1071, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.LineFrequency), 47));

                if (instantPowerEntity.CumulativeEnergyKVArh != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1031, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.CumulativeEnergyKVArh), 14));

                if (instantPowerEntity.CumulativeEnergyKVAh != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(34, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.CumulativeEnergyKVAh), 16));
                //instantEntityList.Add(GetInstangtEntity(1032, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.ABC), 15));
                if (instantPowerEntity.ABC != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1032, instantPowerEntity.ABC, 15)); // Story - 349654 - There is no need to call this function for ABC code as this can be only numeric              
               if (instantPowerEntity.TotalPowerFactor != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(302, instantPowerEntity.TotalPowerFactor, 17));

               if (instantPowerEntity.PowerFactor != String.Empty)
                   instantEntityList.Add(GetInstangtEntity(1054, instantPowerEntity.PowerFactor, 39));

               if (instantPowerEntity.PresentMonthAveragePF != String.Empty)
                   instantEntityList.Add(GetInstangtEntity(1055, instantPowerEntity.PresentMonthAveragePF, 40));

               if (instantPowerEntity.CumulativeActiveMDKWh != String.Empty)
                   instantEntityList.Add(GetInstangtEntity(1069, CommonMapper.FormatIECDataForSinglePhase(instantPowerEntity.CumulativeActiveMDKWh), 45));

                //tamper counts from instantaneous
                if (instantPowerEntity.SingleWireTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1034, instantPowerEntity.SingleWireTamperCounter, 19));
                if (instantPowerEntity.EarthTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1035, instantPowerEntity.EarthTamperCounter, 20));

                if (instantPowerEntity.MagnetTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1036, instantPowerEntity.MagnetTamperCounter, 22));

                if (instantPowerEntity.NeutralDisturbanceTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1037, instantPowerEntity.NeutralDisturbanceTamperCounter, 23));

                if (instantPowerEntity.TotalTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(27, instantPowerEntity.TotalTamperCounter, 24));

                if (instantPowerEntity.ESDTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1050, instantPowerEntity.ESDTamperCounter, 35));

                if (instantPowerEntity.CoverOpenTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1051, instantPowerEntity.CoverOpenTamperCounter, 36));

                if (instantPowerEntity.OverLoadTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1056, instantPowerEntity.OverLoadTamperCounter, 39)); // User Story 464096

                if (instantPowerEntity.LowVoltageTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1057, instantPowerEntity.LowVoltageTamperCounter, 40)); // User Story 464096

                if (instantPowerEntity.LowPFTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1058, instantPowerEntity.LowPFTamperCounter, 41)); // User Story 464096

                if (instantPowerEntity.ReverseTamperCounter != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1059, instantPowerEntity.ReverseTamperCounter, 42)); // User Story 464096

                //if (instantPowerEntity.TotalTransactionCounter != String.Empty)
                //    instantEntityList.Add(GetInstangtEntity(1072, instantPowerEntity.TotalTransactionCounter, 48));// ProgrammingCounter is same as TotalTransactionCounter

                if (instantPowerEntity.PowerfailCount != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1061, instantPowerEntity.PowerfailCount, 44)); // User Story 464096


                // Story - 365960 - More instant parameters for single phase non DLMS integration
                if (instantPowerEntity.ManufactureDateTime != String.Empty)
                {
                    string datetime = instantPowerEntity.ManufactureDateTime;
                    string[] datetimeArr = datetime.Split('/');
                    if (datetimeArr.Length > 2)
                    {
                        datetimeArr[2] = (Convert.ToInt64(datetimeArr[2].Substring(0, 2)) + 2000).ToString() + " " + datetimeArr[2].Substring(3, 8);
                        datetime = datetimeArr[0] + "/" + datetimeArr[1] + "/" + datetimeArr[2];
                    }
                    instantEntityList.Add(GetInstangtEntity(1038, datetime + string.Concat("*" + ConfigInfo.DateFormat(), " HH:MM:SS"), 25));
                }

                // User Story 464096
                if (instantPowerEntity.ProgrammedBillDayTime != String.Empty)
                {
                    instantEntityList.Add(GetInstangtEntity(1062, instantPowerEntity.ProgrammedBillDayTime + string.Concat("*" ," d ; hr : m"), 45));
                }

                if (instantPowerEntity.TotalPowerOffMinutes != String.Empty)
                {
                    string powerOffHours = instantPowerEntity.TotalPowerOffMinutes;
                    TimeSpan span = TimeSpan.FromMinutes(Convert.ToDouble(powerOffHours));
                    powerOffHours = string.Concat(span.Days.ToString("00"), " : ", span.Hours.ToString("00"), " : ", span.Minutes.ToString("00"), "*dd : hh : mm"); //span.ToString(@"hh\:mm\:ss");
                    instantEntityList.Add(GetInstangtEntity(1039, powerOffHours, 26));
                    
                }
                if (instantPowerEntity.SingleWireTamperDuration != String.Empty)
				{
					instantPowerEntity.SingleWireTamperDuration = instantPowerEntity.SingleWireTamperDuration + "*min";
                    instantEntityList.Add(GetInstangtEntity(1040, instantPowerEntity.SingleWireTamperDuration, 27));
				}

                if (instantPowerEntity.MagnetTamperDuration != String.Empty)
				{
					instantPowerEntity.MagnetTamperDuration = instantPowerEntity.MagnetTamperDuration + "*min";
                    instantEntityList.Add(GetInstangtEntity(1041, instantPowerEntity.MagnetTamperDuration, 28));
				}

                if (instantPowerEntity.NeutralDisturbanceTamperDuration != String.Empty)
				{
					instantPowerEntity.NeutralDisturbanceTamperDuration = instantPowerEntity.NeutralDisturbanceTamperDuration + "*min";
                    instantEntityList.Add(GetInstangtEntity(1042, instantPowerEntity.NeutralDisturbanceTamperDuration, 29));
				}
                if (instantPowerEntity.EarthTamperDuration != String.Empty)
				{
					instantPowerEntity.EarthTamperDuration = instantPowerEntity.EarthTamperDuration + "*min";
                    instantEntityList.Add(GetInstangtEntity(1043, instantPowerEntity.EarthTamperDuration, 30));
				}

                if (instantPowerEntity.ESDTamperDuration != String.Empty)
                {
                    instantPowerEntity.ESDTamperDuration = instantPowerEntity.ESDTamperDuration + "*min";
                    instantEntityList.Add(GetInstangtEntity(1049, instantPowerEntity.ESDTamperDuration, 37));
                }

                if (instantPowerEntity.ReverseTamperDuration != String.Empty)
                {
                    instantPowerEntity.ReverseTamperDuration = instantPowerEntity.ReverseTamperDuration + "*min";
                    instantEntityList.Add(GetInstangtEntity(1048, instantPowerEntity.ReverseTamperDuration, 38));
                }
					
                if (generalEntity.AccuracyClass != string.Empty)
                    instantEntityList.Add(GetInstangtEntity(1044,generalEntity.AccuracyClass,31));

                // Now Meter Constant is avilable in Nameplate Profile
                //if (generalEntity.MeterConstant != string.Empty)
                //{
                //    generalEntity.MeterConstant = generalEntity.MeterConstant + "*Impulse/kWh";
                //    instantEntityList.Add(GetInstangtEntity(1045, generalEntity.MeterConstant, 32));
                //}

                if (instantPowerEntity.ABCType2Bill1 != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1052, instantPowerEntity.ABCType2Bill1, 33));

                if (instantPowerEntity.ABCType2Bill2 != String.Empty)
                    instantEntityList.Add(GetInstangtEntity(1053, instantPowerEntity.ABCType2Bill2, 34));               
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstantEntity(InstantPowerEntity instantPowerEntity, GeneralEntity generalEntity, IECBillingEntity billingEntity)", ex);
            }

            return instantEntityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataDefId"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private DLMS650InstantaneousEntity GetInstangtEntity(int dataDefId, string value, int index)
        {
            DLMS650InstantaneousEntity instantEntity = new DLMS650InstantaneousEntity();
            try
            {
                DLMSCOMMAND obisInfo = meterConfigParser.GetObisInfoFromRepository(dataDefId);
                instantEntity.InstantPowerColumnValue = value;
                instantEntity.InstantPowerObisCode = obisInfo.OBISCODE;
                instantEntity.InstantPowerColumnName = obisInfo.CLASSNAME;
                instantEntity.InstantPowerClassID = obisInfo.CLASS;
                instantEntity.InstantPowerAttribute = obisInfo.ATTRIBUTE;
                instantEntity.InstantPowerDataIndex = index;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstangtEntity(int dataDefId, string value, int index)", ex);
            }
            return instantEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iECLoadSurveyEntity"></param>
        /// <returns></returns>
        private DTMDailyProfileParameterEntity GetMidnightParameterEntity(DTMDailyProfileEntity iECMidnightEntity)
        {
            DTMDailyProfileParameterEntity midnightParameterEntity = new DTMDailyProfileParameterEntity();

            midnightParameterEntity.ColumnsNames += "realTimeClockDateandTime";
            if (!string.IsNullOrEmpty(iECMidnightEntity.CumulativekWh))
            {
                midnightParameterEntity.ColumnsNames += ",cumEnergykWh";
            }
            if (!string.IsNullOrEmpty(iECMidnightEntity.CumulativekVArh_lag))
            {
                midnightParameterEntity.ColumnsNames += ",cumEnergykvarhlag";
            }
            if (!string.IsNullOrEmpty(iECMidnightEntity.CumulativekVArh_lead))
            {
                midnightParameterEntity.ColumnsNames += ",cumEnergykvarhlead";
            }
            if (!string.IsNullOrEmpty(iECMidnightEntity.CumulativekVAh))
            {
                midnightParameterEntity.ColumnsNames += ",cumEnergykVAh";
            }
            if (!string.IsNullOrEmpty(iECMidnightEntity.PowerOnHours))
            {
                midnightParameterEntity.ColumnsNames += ",PowerOnDuration";
                //midnightParameterEntity.ColumnsNames += ",MDKW";
            }
         
               
            
            return midnightParameterEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iECLoadSurveyEntity"></param>
        /// <returns></returns>
        private TamperParameterEntity GetTamperParameterEntity(TamperSnapshotEntity iECTamperEntity)
        {
            TamperParameterEntity tamperParameterEntity = new TamperParameterEntity();

            if (!string.IsNullOrEmpty(iECTamperEntity.RCurrentOccurred))
            {
                tamperParameterEntity.ColumnsNames += "CurrentIR,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.YCurrentOccurred))
            {
                tamperParameterEntity.ColumnsNames += "CurrentIY,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.BCurrentOccurred))
            {
                tamperParameterEntity.ColumnsNames += "CurrentIB,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.RVoltageOccurred))
            {
                tamperParameterEntity.ColumnsNames += "VoltageVRN,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.YVoltageOccurred))
            {
                tamperParameterEntity.ColumnsNames += "VoltageVYN,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.BVoltageOccurred))
            {
                tamperParameterEntity.ColumnsNames += "VoltageVBN,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.RPFOccurred))
            {
                tamperParameterEntity.ColumnsNames += "PowerFactorRphase,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.YPFOccurred))
            {
                tamperParameterEntity.ColumnsNames += "PowerFactorYphase,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.BPFOccurred))
            {
                tamperParameterEntity.ColumnsNames += "PowerFactorBphase,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.KWhOccurred))
            {
                tamperParameterEntity.ColumnsNames += "CumulativeEnergykWh,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.KVAhOccurred))
            {
                tamperParameterEntity.ColumnsNames += "CumulativeEnergykVAh,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.TotalPFOccurred))
            {
                tamperParameterEntity.ColumnsNames += "TotalPowerFactor,";
            }

           
            tamperParameterEntity.ColumnsNames = tamperParameterEntity.ColumnsNames.Remove(tamperParameterEntity.ColumnsNames.LastIndexOf(","));
            return tamperParameterEntity;
        }

        private TamperParameterEntity GetTamperParameterEntitySPhase(TamperSnapshotEntity iECTamperEntity)
        {
            TamperParameterEntity tamperParameterEntity = new TamperParameterEntity();

            if (!string.IsNullOrEmpty(iECTamperEntity.PhaseCurrentOccured))
            {
                tamperParameterEntity.ColumnsNames += "PhaseCurrentInstant,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.PhaseVoltageOccured))
            {
                tamperParameterEntity.ColumnsNames += "PhaseVoltage,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.KWhOccurred))
            {
                tamperParameterEntity.ColumnsNames += "CumulativeEnergykWh,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.KVAhOccurred))
            {
                tamperParameterEntity.ColumnsNames += "CumulativeEnergykVAh,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.TotalPFOccurred))
            {
                tamperParameterEntity.ColumnsNames += "TotalPowerFactor,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.OccuredNeutralCurrent)) // Story - 349654 - Neutral current in Tamper
            {
                tamperParameterEntity.ColumnsNames += "NeutralCurrent,";
            }
            if (!string.IsNullOrEmpty(iECTamperEntity.TempratureOccured))
            {
                tamperParameterEntity.ColumnsNames += "Temprature,";
            }

            tamperParameterEntity.ColumnsNames = tamperParameterEntity.ColumnsNames.Remove(tamperParameterEntity.ColumnsNames.LastIndexOf(","));
            return tamperParameterEntity;
        }

        /// <summary>
        /// Filling AnomalyEntity
        /// </summary>
        /// <param name="generalData"></param>
        /// <returns></returns>
        private AnomalyEntity GetAnomalyEntityForSPhase(GeneralData generalData)
        {
            AnomalyEntity anomalyEntity = new AnomalyEntity();
            //anomalyEntity.Rtc = generalData.Anomaly.Rtc;
            anomalyEntity.Rtc = generalData.Anomaly.RTCBattery; // For single phase meter RTC Battery Status is RTC status, refer story 361700
            anomalyEntity.EeProm = generalData.Anomaly.EeProm;
            anomalyEntity.Flash = generalData.Anomaly.Flash;
            anomalyEntity.MainBattery = generalData.Anomaly.MainBattery;
            anomalyEntity.MeterDataId = generalData.Anomaly.MeterDataId;
            anomalyEntity.RTCBattery = generalData.Anomaly.RTCBattery;
            anomalyEntity.Smps = generalData.Anomaly.Smps;
            anomalyEntity.AnomalyId = generalData.Anomaly.AnomalyId;
            return anomalyEntity;
        }

        #endregion
    }
}
