#region Namespaces
using System.Collections.Generic;
using System.Linq;

using CAB.Serialization;
using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using System;
#endregion

namespace CAB.Mapper
{
    /// <summary>
    /// This class is responsible for Mapping input entity to Tamper entity
    /// </summary>
    public class Tamper
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private readonly Serializer lngSerialzer = null;
        private TAMPERMAPPER tamperMapper = null;
        private const string NotAvailable = "NA";
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public Tamper()
        {
            lngSerialzer = new Serializer();
            tamperMapper = (TAMPERMAPPER)lngSerialzer.DeserializeToObject(AppDomain.CurrentDomain.BaseDirectory + "TamperDLMSToIECMapper.xml", typeof(TAMPERMAPPER));
        }
        #endregion

        #region Public Methods
        public List<TamperData> GetData(List<ProfileData> tamperProfileData)
        {
            List<TamperData> resultEntity = new List<TamperData>();
            TamperData tamperData = new TamperData();
            tamperData.Snapshot = new List<TamperSnapshotEntity>();
            tamperProfileData = GetTampers(tamperProfileData);
            foreach (MeterDataPacket meterDataPacket in tamperProfileData[0].ListMeterDataPacket)
            {
                TamperSnapshotEntity tamperSnapShotEntity = new TamperSnapshotEntity();
                DataElement dataElement = Common.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 76);
                TAMPERMAPPERMAP map = FindMappedTamper(dataElement.Value);
                if (map != null)
                {
                    tamperSnapShotEntity.TamperCode = Convert.ToInt32(map.IECCODE);
                    if (map.OCCURENCE)
                    {
                        tamperSnapShotEntity.TamperOccurredTime = Common.StringToLongDateTimeFormat(meterDataPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                        FillOccurenceInfo(meterDataPacket.ListDataElementValue, tamperSnapShotEntity);
                        // corresponding restoration
                        if (map.DLMSRESCODE != NotAvailable)
                        {
                            MeterDataPacket restoredPacket = GetCorrespondingRestoration(tamperProfileData[0], meterDataPacket, map);
                            if (restoredPacket != null)
                            {
                                tamperSnapShotEntity.TamperRestoredTime = Common.StringToLongDateTimeFormat(restoredPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                                FillRestorationInfo(restoredPacket.ListDataElementValue, tamperSnapShotEntity);
                            }
                        }
                        tamperData.Snapshot.Add(tamperSnapShotEntity);
                    }

                }
            }
            tamperData = GetTamperCounters(tamperData);
            resultEntity.Add(tamperData);            
            return resultEntity;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        /// Skips the transaction data and sorts the tamper data on date time
        /// </summary>
        /// <param name="tamperData"></param>
        /// <returns></returns>
        private List<ProfileData> GetTampers(List<ProfileData> tamperData)
        {
            List<ProfileData> tamperList = new List<ProfileData>();
            ProfileData tamperProfile = new ProfileData();
            tamperProfile.ProfileId = (int)ProfileId.Tamper;
            tamperProfile.ListMeterDataPacket = new List<MeterDataPacket>();
            for (int counter = 0; counter < tamperData.Count; counter++)
            {
                //skip for Transaction
                if (counter != 3 )
                {
                    tamperProfile.ListMeterDataPacket.AddRange(tamperData[counter].ListMeterDataPacket);
                }
            }
            var list = (from packet in tamperProfile.ListMeterDataPacket select packet).
                OrderBy<MeterDataPacket, DateTime>(item => item.ReadingDate);
            tamperProfile.ListMeterDataPacket = list.ToList<MeterDataPacket>();
            tamperList.Add(tamperProfile);
            return tamperList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileData"></param>
        /// <param name="meterDataPacket"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private MeterDataPacket GetCorrespondingRestoration(ProfileData profileData, MeterDataPacket meterDataPacket, TAMPERMAPPERMAP map)
        {
            MeterDataPacket restoredPacket = null;
            foreach (MeterDataPacket packet in profileData.ListMeterDataPacket)
            {
                if (packet.ReadingDate >= meterDataPacket.ReadingDate)
                {
                    DataElement dataElement = Common.GetDataElementByDataDefId(packet.ListDataElementValue, 76);
                    if (map.DLMSRESCODE.Trim() == dataElement.Value.Trim())
                    {
                        restoredPacket = packet;
                        break;
                    }
                }
            }
            return restoredPacket;
        }
        /// <summary>
        /// Gets a boolean indicating whether the record is of occurence
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private TAMPERMAPPERMAP FindMappedTamper(string dlmsEventCode)
        {
            TAMPERMAPPERMAP mapper = null;
            foreach (TAMPERMAPPERMAP map in tamperMapper.MAP)
            {
                if (map.DLMSCODE.Trim() == dlmsEventCode.Trim())
                {
                    mapper = map;
                    break;
                }
            }
            return mapper;
        }

        /// <summary>
        /// Fill occurence information in tamper snap shot entity
        /// </summary>
        /// <param name="dataElements"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private TamperSnapshotEntity FillOccurenceInfo(List<DataElement> dataElements, TamperSnapshotEntity entity)
        {
            entity.RCurrentOccurred = Common.GetDataElementByDataDefId(dataElements, 10).Value;
            entity.YCurrentOccurred = Common.GetDataElementByDataDefId(dataElements, 11).Value;
            entity.BCurrentOccurred = Common.GetDataElementByDataDefId(dataElements, 12).Value;
            entity.RVoltageOccurred = Common.GetDataElementByDataDefId(dataElements, 13).Value;
            entity.YVoltageOccurred = Common.GetDataElementByDataDefId(dataElements, 14).Value;
            entity.BVoltageOccurred = Common.GetDataElementByDataDefId(dataElements, 15).Value;
            entity.RPFOccurred = Common.GetDataElementByDataDefId(dataElements, 16).Value;
            entity.YPFOccurred = Common.GetDataElementByDataDefId(dataElements, 17).Value;
            entity.BPFOccurred = Common.GetDataElementByDataDefId(dataElements, 18).Value;
            entity.TotalPFOccurred = Common.GetDataElementByDataDefId(dataElements, 19).Value;
            entity.KWhOccurred = Common.FormatLoadSurveyData(Common.GetDataElementByDataDefId(dataElements, 31));
            entity.KVAhOccurred = Common.FormatLoadSurveyData(Common.GetDataElementByDataDefId(dataElements, 34));
            return entity;
        }
        /// <summary>
        /// Fill restoration information in tamper snap shot entity
        /// </summary>
        /// <param name="dataElements"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private TamperSnapshotEntity FillRestorationInfo(List<DataElement> dataElements, TamperSnapshotEntity entity)
        {
            
            entity.RCurrentRestored = Common.GetDataElementByDataDefId(dataElements, 10).Value;
            entity.YCurrentRestored = Common.GetDataElementByDataDefId(dataElements, 11).Value;
            entity.BCurrentRestored = Common.GetDataElementByDataDefId(dataElements, 12).Value;
            entity.RVoltageRestored = Common.GetDataElementByDataDefId(dataElements, 13).Value;
            entity.YVoltageRestored = Common.GetDataElementByDataDefId(dataElements, 14).Value;
            entity.BVoltageRestored = Common.GetDataElementByDataDefId(dataElements, 15).Value;
            entity.RPFRestored = Common.GetDataElementByDataDefId(dataElements, 16).Value;
            entity.YPFRestored = Common.GetDataElementByDataDefId(dataElements, 17).Value;
            entity.BPFRestored = Common.GetDataElementByDataDefId(dataElements, 18).Value;
            entity.TotalPFRestored = Common.GetDataElementByDataDefId(dataElements, 19).Value;            
            entity.KWhRestored = Common.FormatLoadSurveyData(Common.GetDataElementByDataDefId(dataElements, 31));            
            entity.KVAhRestored = Common.FormatLoadSurveyData(Common.GetDataElementByDataDefId(dataElements, 34));
            return entity;
        }
        /// <summary>
        /// Get counts of occurence of tamper counters
        /// </summary>
        /// <param name="tamper"></param>
        /// <returns></returns>
        private TamperData GetTamperCounters(TamperData tamper)
        {
            int totalTamperCounter = 0;
            int powerOnOffCounter = 0;
            TamperCounterEntity counterTotal = new TamperCounterEntity();
            TamperCounterGeneralEntity counter = new TamperCounterGeneralEntity();
            foreach (TamperSnapshotEntity entity in tamper.Snapshot)
            {
                switch (entity.TamperCode)
                {
                    case 164:
                        counter.MissingPotentialRPhaseTamperCounter = counter.MissingPotentialRPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 165:
                        counter.MissingPotentialYPhaseTamperCounter = counter.MissingPotentialYPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 166:
                        counter.MissingPotentialBPhaseTamperCounter = counter.MissingPotentialBPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 177:
                        counter.CTShortTamperCounter = counter.CTShortTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 178:
                        counter.CTOpenRPhaseTamperCounter = counter.CTOpenRPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 179:
                        counter.CTOpenYPhaseTamperCounter = counter.CTOpenYPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 180:
                        counter.CTOpenBPhaseTamperCounter = counter.CTOpenBPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 198:
                        counter.CurrentReversalRPhaseTamperCounter = counter.CurrentReversalRPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 199:
                        counter.CurrentReversalYPhaseTamperCounter = counter.CurrentReversalYPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 200:
                        counter.CurrentReversalBPhaseTamperCounter = counter.CurrentReversalBPhaseTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 209:
                        counter.MagneticInfluenceTamperCounter = counter.MagneticInfluenceTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 210:
                        counter.NeutralDisturbanceTamperCounter = counter.NeutralDisturbanceTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 211:
                        counter.FrontCoverOpeningTamperCounter = counter.FrontCoverOpeningTamperCounter + 1;
                        totalTamperCounter++;
                        break;
                    case 225:
                        powerOnOffCounter = powerOnOffCounter + 1;
                        break;
                }

            }
            counterTotal.PowerOnOffCounter = powerOnOffCounter;
            counterTotal.TotalTamperCounter = totalTamperCounter;
            counter.RelatedTo = "T";
            tamper.General = counter;
            tamper.Counter = counterTotal;
            return tamper;
        }
        #endregion
    }
}
